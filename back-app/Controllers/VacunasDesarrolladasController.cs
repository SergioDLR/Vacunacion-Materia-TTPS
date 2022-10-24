using System;
using System.Collections.Generic;
using System.Linq;
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
    public class VacunasDesarrolladasController : ControllerBase
    {
        private readonly VacunasContext _context;

        public VacunasDesarrolladasController(VacunasContext context)
        {
            _context = context;
        }

        // GET: api/VacunasDesarrolladas/GetAll?emailOperadorNacional=juan@gmail.com
        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult<IEnumerable<ResponseListaVacunasDesarrolladasDTO>>> GetAll(string emailOperadorNacional = null)
        {
            try
            {
                ResponseListaVacunasDesarrolladasDTO responseListaVacunasDesarrolladasDTO = new ResponseListaVacunasDesarrolladasDTO();
                //lista vacia para los errores
                List<string> errores = new List<string>();
                List<VacunaDesarrolladaDTO> vacunasDesarrolladasDTO = new List<VacunaDesarrolladaDTO>();
                bool existenciaErrores = true;
                string transaccion = "";

                errores = await VerificarCredencialesOperadorNacional(emailOperadorNacional, errores);

                if(errores.Count == 0)
                {
                    existenciaErrores = false;
                    transaccion = "Aceptada";
                    List<VacunaDesarrollada> vacunasDesarrolladas = await _context.VacunaDesarrollada.ToListAsync();

                    foreach(VacunaDesarrollada item in vacunasDesarrolladas)
                    {
                        MarcaComercial marcaComercial = await _context.MarcaComercial.Where(mc => mc.Id == item.IdMarcaComercial).FirstOrDefaultAsync();
                        Vacuna vacuna = await _context.Vacuna.Where(v => v.Id == item.IdVacuna).FirstOrDefaultAsync();    
                        VacunaDesarrolladaDTO vacunaDesarrolladaDTO = new VacunaDesarrolladaDTO(item.Id, item.IdVacuna, vacuna.Descripcion, item.IdMarcaComercial, marcaComercial.Descripcion, item.DiasDemoraEntrega, item.PrecioVacuna);
                        vacunasDesarrolladasDTO.Add(vacunaDesarrolladaDTO);
                    }

                }
                else
                {
                    transaccion = "Rechazada";
                }

                responseListaVacunasDesarrolladasDTO.EmailOperadorNacional = emailOperadorNacional;
                responseListaVacunasDesarrolladasDTO.Errores = errores;
                responseListaVacunasDesarrolladasDTO.ExistenciaErrores = existenciaErrores;
                responseListaVacunasDesarrolladasDTO.EstadoTransaccion = transaccion;
                responseListaVacunasDesarrolladasDTO.ListaVacunasDesarrolladasDTO = vacunasDesarrolladasDTO;
                return Ok(responseListaVacunasDesarrolladasDTO);

            }
            catch(Exception error)
            {
                return BadRequest(error.Message);  
            }
        }

        // GET: api/VacunasDesarrolladas/GetVacunaDesarrollada?emailOperadorNacional=juan@gmail.com&idVacunaDesarrollada=5
        [HttpGet]
        [Route("GetVacunaDesarrollada")]
        public async Task<ActionResult<ResponseVacunaDesarrolladaDTO>> GetVacunaDesarrollada(string emailOperadorNacional = null, int idVacunaDesarrollada = 0)
        {
            try
            {
                ResponseVacunaDesarrolladaDTO responseVacunaDesarrolladaDTO = new ResponseVacunaDesarrolladaDTO();

                //lista vacia para los errores
                List<string> errores = new List<string>();

                errores = await VerificarCredencialesOperadorNacional(emailOperadorNacional, errores);

                VacunaDesarrollada vacunaDesarrolladaExistente = await _context.VacunaDesarrollada.Where(vd => vd.Id == idVacunaDesarrollada).FirstOrDefaultAsync();
                if (vacunaDesarrolladaExistente == null)
                {
                    errores.Add(String.Format("La vacuna desarrollada con identificador {0} no esta registrada", idVacunaDesarrollada));
                }
                
                if(errores.Count > 0)
                {
                    responseVacunaDesarrolladaDTO.EmailOperadorNacional = emailOperadorNacional;
                    responseVacunaDesarrolladaDTO.EstadoTransaccion = "Rechazada";
                    responseVacunaDesarrolladaDTO.ExistenciaErrores = true;
                    responseVacunaDesarrolladaDTO.Errores = errores;
                    responseVacunaDesarrolladaDTO.VacunaDesarrolladaDTO = new VacunaDesarrolladaDTO();
                }
                else
                {
                    Vacuna vacuna = await _context.Vacuna.Where(v => v.Id == vacunaDesarrolladaExistente.IdVacuna).FirstOrDefaultAsync();
                    MarcaComercial marcaComercial = await _context.MarcaComercial.Where(mc => mc.Id == vacunaDesarrolladaExistente.IdMarcaComercial).FirstOrDefaultAsync();

                    responseVacunaDesarrolladaDTO.EmailOperadorNacional = emailOperadorNacional;
                    responseVacunaDesarrolladaDTO.EstadoTransaccion = "Aceptada";
                    responseVacunaDesarrolladaDTO.ExistenciaErrores = false;
                    responseVacunaDesarrolladaDTO.Errores = errores;
                    responseVacunaDesarrolladaDTO.VacunaDesarrolladaDTO = new VacunaDesarrolladaDTO(vacunaDesarrolladaExistente.Id, vacunaDesarrolladaExistente.IdVacuna, vacuna.Descripcion, vacunaDesarrolladaExistente.IdMarcaComercial, marcaComercial.Descripcion, vacunaDesarrolladaExistente.DiasDemoraEntrega, vacunaDesarrolladaExistente.PrecioVacuna);
                }
                return Ok(responseVacunaDesarrolladaDTO);
            }
            catch (Exception ex)    
            {
                return BadRequest(ex.Message);  
            }
        }

        // PUT: api/VacunasDesarrolladas/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVacunaDesarrollada(int id, VacunaDesarrollada vacunaDesarrollada)
        {
            if (id != vacunaDesarrollada.Id)
            {
                return BadRequest();
            }

            _context.Entry(vacunaDesarrollada).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VacunaDesarrolladaExists(id))
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

        // POST: api/VacunasDesarrolladas/CrearVacunaDesarrollada
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        [Route("CrearVacunaDesarrollada")]
        public async Task<ActionResult<ResponseVacunaDesarrolladaDTO>> CrearVacunaDesarrollada([FromBody] RequestVacunaDesarrolladaDTO model)
        {
            try
            {
                ResponseVacunaDesarrolladaDTO responseVacunaDesarrolladaDTO = new ResponseVacunaDesarrolladaDTO();
            
                //lista vacia para los errores
                List<string> errores = new List<string>();

                errores = await VerificarCredencialesOperadorNacional(model.EmailOperadorNacional, errores);

                //verifico si existe la vacuna en la lista
                Vacuna vacunaExistente = await _context.Vacuna.Where(vd => vd.Id == model.IdVacuna).FirstOrDefaultAsync();
                if (vacunaExistente == null)
                {
                    errores.Add(String.Format("La vacuna {0} no está registrada en el sistema", model.IdVacuna));
                }

                //verifico si la marca comercial existe
                MarcaComercial marcaComercialExistente = await _context.MarcaComercial.Where(mc => mc.Id == model.IdMarcaComercial).FirstOrDefaultAsync();
                if(marcaComercialExistente == null)
                {
                    errores.Add(String.Format("La marca comercial {0} no esta registrada en el sistema", model.IdMarcaComercial));
                }     

                if (errores.Count() > 0)
                {
                    responseVacunaDesarrolladaDTO.EmailOperadorNacional = model.EmailOperadorNacional;
                    responseVacunaDesarrolladaDTO.Errores = errores;
                    responseVacunaDesarrolladaDTO.ExistenciaErrores = true;
                    responseVacunaDesarrolladaDTO.EstadoTransaccion = "Rechazada";
                    responseVacunaDesarrolladaDTO.VacunaDesarrolladaDTO = new VacunaDesarrolladaDTO();
                }
                else
                {
                    //creo la instancia del objeto original. Del model
                    VacunaDesarrollada vacunaDesarrollada = new VacunaDesarrollada();
                    vacunaDesarrollada.IdVacuna = model.IdVacuna;
                    vacunaDesarrollada.IdMarcaComercial = model.IdMarcaComercial;
                    vacunaDesarrollada.DiasDemoraEntrega = model.DiasDemoraEntrega;
                    vacunaDesarrollada.PrecioVacuna = model.PrecioVacunaDesarrollada;
                    vacunaDesarrollada.FechaDesde = DateTime.Now;   
                    vacunaDesarrollada.FechaHasta = null;

                    //guardo en la base de datos
                    _context.VacunaDesarrollada.Add(vacunaDesarrollada);
                    await _context.SaveChangesAsync();

                    Vacuna vacuna = await _context.Vacuna.Where(v => v.Id == vacunaDesarrollada.IdVacuna).FirstOrDefaultAsync();
                    MarcaComercial marcaComercial = await _context.MarcaComercial.Where(mc => mc.Id == vacunaDesarrollada.IdMarcaComercial).FirstOrDefaultAsync();

                    responseVacunaDesarrolladaDTO.EmailOperadorNacional = model.EmailOperadorNacional;
                    responseVacunaDesarrolladaDTO.Errores = errores;
                    responseVacunaDesarrolladaDTO.ExistenciaErrores = false;
                    responseVacunaDesarrolladaDTO.EstadoTransaccion = "Aceptada";
                    responseVacunaDesarrolladaDTO.VacunaDesarrolladaDTO = new VacunaDesarrolladaDTO(vacunaDesarrollada.Id, model.IdVacuna, vacuna.Descripcion, model.IdMarcaComercial, marcaComercial.Descripcion, model.DiasDemoraEntrega, model.PrecioVacunaDesarrollada);
                }
                return Ok(responseVacunaDesarrolladaDTO);
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }

        // DELETE: api/VacunasDesarrolladas/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<VacunaDesarrollada>> DeleteVacunaDesarrollada(int id)
        {
            var vacunaDesarrollada = await _context.VacunaDesarrollada.FindAsync(id);
            if (vacunaDesarrollada == null)
            {
                return NotFound();
            }

            _context.VacunaDesarrollada.Remove(vacunaDesarrollada);
            await _context.SaveChangesAsync();

            return vacunaDesarrollada;
        }

        //metodos privados ------------------------
        private bool VacunaDesarrolladaExists(int id)
        {
            return _context.VacunaDesarrollada.Any(e => e.Id == id);
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
    }
}