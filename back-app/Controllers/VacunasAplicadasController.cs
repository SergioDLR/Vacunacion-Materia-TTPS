using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VacunacionApi.DTO;
using VacunacionApi.Models;

namespace VacunacionApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VacunasAplicadasController : ControllerBase
    {
        private readonly VacunasContext _context;

        public VacunasAplicadasController(VacunasContext context)
        {
            _context = context;
        }

        // GET: api/VacunasAplicadas/GetAll?emailUsuario=juan@gmail.com
        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult<IEnumerable<ResponseListaVacunasAplicadasDTO>>> GetAll(string emailUsuario = null)
        {
            try
            {
                ResponseListaVacunasAplicadasDTO responseListaVacunasAplicadas = new ResponseListaVacunasAplicadasDTO();

                //lista vacia para los errores
                List<string> errores = new List<string>();
                List<VacunaAplicadaDTO> vacunasAplicadasDTO = new List<VacunaAplicadaDTO>();
                bool existenciaErrores = true;
                string transaccion = "Rechazada";
                Rol rolUsuario = null;

                Usuario usuarioExistente = await CuentaUsuarioExists(emailUsuario);
                if (usuarioExistente == null)
                {
                    errores.Add(String.Format("El usuario {0} no existe en el sistema", emailUsuario));
                }
                else
                {
                    rolUsuario = await GetRol(usuarioExistente.IdRol);                
                }

                //obtengo la lista
                List<VacunaAplicada> vacunasAplicadas = await _context.VacunaAplicada.ToListAsync();

                if(vacunasAplicadas.Count == 0)
                {
                    errores.Add("No existen vacunados en la base de datos");
                }

                if (rolUsuario != null)
                {
                    if (rolUsuario.Descripcion == "Administrador" | rolUsuario.Descripcion == "Vacunador")
                    {
                        errores.Add(String.Format("El rol {0} no tiene permisos para visualizar esta sección", rolUsuario.Descripcion));
                    }
                }

                if (errores.Count == 0)
                {
                    transaccion = "Aceptada";
                    existenciaErrores = false;
                                        
                    if (rolUsuario.Descripcion == "Analista Provincial")
                    {
                        foreach(VacunaAplicada item in vacunasAplicadas)
                        {
                            Jurisdiccion jurisdiccion = await getDescripcionJurisdiccion(item.IdJurisdiccion);
                            Lote lote = await getLote(item.IdLote);
                            VacunaDesarrollada vacunaDesarrollada = await getVacunaDesarrollada(lote.IdVacunaDesarrollada);
                            Vacuna vacuna = await getVacuna(vacunaDesarrollada.IdVacuna);
                            MarcaComercial marcaComercial = await getMarcaComercial(vacunaDesarrollada.IdMarcaComercial);
                            Dosis dosis = await getDosis(item.IdDosis);

                            if (jurisdiccion.Id == usuarioExistente.IdJurisdiccion)
                            {
                                VacunaAplicadaDTO vacunaAplicadaDTO = new VacunaAplicadaDTO(item.Dni, item.Apellido, item.Nombre, item.FechaVacunacion, jurisdiccion.Descripcion, item.IdLote, lote.IdVacunaDesarrollada, vacuna.Descripcion, marcaComercial.Descripcion, dosis.Descripcion);
                                vacunasAplicadasDTO.Add(vacunaAplicadaDTO);    
                            }   
                        }
                    }
                    if (rolUsuario.Descripcion == "Operador Nacional")
                    {
                        foreach (VacunaAplicada item in vacunasAplicadas)
                        {
                            Jurisdiccion jurisdiccion = await getDescripcionJurisdiccion(item.IdJurisdiccion);
                            Lote lote = await getLote(item.IdLote);
                            VacunaDesarrollada vacunaDesarrollada = await getVacunaDesarrollada(lote.IdVacunaDesarrollada);
                            Vacuna vacuna = await getVacuna(vacunaDesarrollada.IdVacuna);
                            MarcaComercial marcaComercial = await getMarcaComercial(vacunaDesarrollada.IdMarcaComercial);
                            Dosis dosis = await getDosis(item.IdDosis);
                            
                            VacunaAplicadaDTO vacunaAplicadaDTO = new VacunaAplicadaDTO(item.Dni, item.Apellido, item.Nombre, item.FechaVacunacion, jurisdiccion.Descripcion, item.IdLote, lote.IdVacunaDesarrollada, vacuna.Descripcion, marcaComercial.Descripcion, dosis.Descripcion);
                            vacunasAplicadasDTO.Add(vacunaAplicadaDTO);
                        }
                    }
                }

                responseListaVacunasAplicadas.EstadoTransaccion = transaccion;
                responseListaVacunasAplicadas.Errores = errores;
                responseListaVacunasAplicadas.ExistenciaErrores = existenciaErrores;
                responseListaVacunasAplicadas.EmailUsuario = emailUsuario;
                responseListaVacunasAplicadas.ListaVacunasAplicadasDTO = vacunasAplicadasDTO;

                return Ok(responseListaVacunasAplicadas);
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }

        // GET: api/VacunasAplicadas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<VacunaAplicada>> GetVacunaAplicada(int id)
        {
            var vacunaAplicada = await _context.VacunaAplicada.FindAsync(id);

            if (vacunaAplicada == null)
            {
                return NotFound();
            }

            return vacunaAplicada;
        }

        // PUT: api/VacunasAplicadas/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVacunaAplicada(int id, VacunaAplicada vacunaAplicada)
        {
            if (id != vacunaAplicada.Id)
            {
                return BadRequest();
            }

            _context.Entry(vacunaAplicada).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VacunaAplicadaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/VacunasAplicadas
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<VacunaAplicada>> PostVacunaAplicada(VacunaAplicada vacunaAplicada)
        {
            _context.VacunaAplicada.Add(vacunaAplicada);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVacunaAplicada", new { id = vacunaAplicada.Id }, vacunaAplicada);
        }

        // DELETE: api/VacunasAplicadas/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<VacunaAplicada>> DeleteVacunaAplicada(int id)
        {
            var vacunaAplicada = await _context.VacunaAplicada.FindAsync(id);
            if (vacunaAplicada == null)
            {
                return NotFound();
            }

            _context.VacunaAplicada.Remove(vacunaAplicada);
            await _context.SaveChangesAsync();

            return vacunaAplicada;
        }

        //metodos privados----------------------
        private bool VacunaAplicadaExists(int id)
        {
            return _context.VacunaAplicada.Any(e => e.Id == id);
        }

        private async Task<bool> TieneRolOperadorNacional(Usuario usuario)
        {
            try
            {
                Rol rolOperadorNacional = await _context.Rol
                    .Where(rol => rol.Descripcion == "Operador Nacional").FirstOrDefaultAsync();

                if (rolOperadorNacional.Id == usuario.IdRol)
                {
                    return true;
                }
            }
            catch
            {

            }
            return false;
        }

        private async Task<Usuario> CuentaUsuarioExists(string email)
        {
            Usuario cuentaExistente = null;
            try
            {
                cuentaExistente = await _context.Usuario
                    .Where(user => user.Email == email).FirstOrDefaultAsync();
            }
            catch
            {

            }
            return cuentaExistente;
        }

        private async Task<List<string>> VerificarCredencialesOperadorNacional(string emailOperadorNacional, List<string> errores)
        {
            try
            {
                Usuario usuarioSolicitante = await CuentaUsuarioExists(emailOperadorNacional);
                if (usuarioSolicitante != null)
                {
                    if (!await TieneRolOperadorNacional(usuarioSolicitante))
                    {
                        errores.Add(String.Format("El usuario {0} no tiene el rol de operador nacional", emailOperadorNacional));
                    }
                }
                else
                {
                    errores.Add(string.Format("El usuario {0} no existe en el sistema", emailOperadorNacional));
                }
            }
            catch
            {

            }

            return errores;
        }

        private async Task<Usuario> GetUsuario(string email)
        {
            try
            {
                Usuario cuentaExistente = new Usuario();
                List<Usuario> listaUsuarios = await _context.Usuario.ToListAsync();

                foreach (Usuario item in listaUsuarios)
                {
                    if (item.Email == email)
                    {
                        cuentaExistente.Id = item.Id;
                        cuentaExistente.Email = item.Email;
                        cuentaExistente.Password = item.Password;
                        cuentaExistente.IdJurisdiccion = item.IdJurisdiccion.Value;
                        cuentaExistente.IdRol = item.IdRol;
                    }
                }

                if (cuentaExistente.Email != null)
                    return cuentaExistente;
            }
            catch
            {

            }

            return null;
        }

        private async Task<Rol> GetRol(int idRol)
        {
            Rol rolExistente = null;

            try
            {
                rolExistente = await _context.Rol
                    .Where(rol => rol.Id == idRol).FirstOrDefaultAsync();
            }
            catch
            {

            }

            return rolExistente;
        }

        private async Task<Jurisdiccion> getDescripcionJurisdiccion(int idJurisdiccion)
        {
            Jurisdiccion jurisdiccionExistente = null;

            try
            {
                jurisdiccionExistente = await _context.Jurisdiccion.Where(jurisdiccion => jurisdiccion.Id == idJurisdiccion).FirstOrDefaultAsync();
            }
            catch
            {

            }
            return jurisdiccionExistente;
        }
        private async Task<Lote> getLote(int idLote)
        {
            Lote loteExistente = null;

            try
            {
                loteExistente = await _context.Lote.Where(lote => lote.Id == idLote).FirstOrDefaultAsync();
            }
            catch
            {

            }
            return loteExistente;
        }

        private async Task<VacunaDesarrollada> getVacunaDesarrollada(int idVacunaDesarrollada)
        {
            VacunaDesarrollada vacunaDesarrolladaExistente = null;

            try
            {
                vacunaDesarrolladaExistente = await _context.VacunaDesarrollada.Where(vd => vd.Id == idVacunaDesarrollada).FirstOrDefaultAsync();
            }
            catch
            {

            }
            return vacunaDesarrolladaExistente;
        }
        private async Task<Dosis> getDosis(int idDosis)
        {
            Dosis dosisExistente = null;

            try
            {
                dosisExistente = await _context.Dosis.Where(d => d.Id == idDosis).FirstOrDefaultAsync();
            }
            catch
            {

            }
            return dosisExistente;
        }

        private async Task<Vacuna> getVacuna(int idVacuna)
        {
            Vacuna vacunaExistente = null;

            try
            {
                vacunaExistente = await _context.Vacuna.Where(v => v.Id == idVacuna).FirstOrDefaultAsync();
            }
            catch
            {

            }
            return vacunaExistente;
        }
        private async Task<MarcaComercial> getMarcaComercial(int idMarcaComercial)
        {
            MarcaComercial marcaComercialExistente = null;

            try
            {
                marcaComercialExistente = await _context.MarcaComercial.Where(mc => mc.Id == idMarcaComercial).FirstOrDefaultAsync();
            }
            catch
            {

            }
            return marcaComercialExistente;
        }
    }
}