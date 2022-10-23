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

        // GET: api/VacunasDesarrolladas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VacunaDesarrollada>>> GetVacunaDesarrollada()
        {
            return await _context.VacunaDesarrollada.ToListAsync();
        }

        // GET: api/VacunasDesarrolladas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<VacunaDesarrollada>> GetVacunaDesarrollada(int id)
        {
            var vacunaDesarrollada = await _context.VacunaDesarrollada.FindAsync(id);

            if (vacunaDesarrollada == null)
            {
                return NotFound();
            }

            return vacunaDesarrollada;
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

            ResponseVacunaDesarrolladaDTO responseVacunaDesarrolladaDTO = null;
            
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
                responseVacunaDesarrolladaDTO.VacunaDesarrolladaDTO = new VacunaDesarrolladaDTO(0, model.IdVacuna, model.IdMarcaComercial, model.DiasDemoraEntrega, model.PrecioVacunaDesarrollada, model.FechaHasta.Value);
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
                vacunaDesarrollada.FechaHasta = model.FechaHasta;

                //guardo en la base de datos
                _context.VacunaDesarrollada.Add(vacunaDesarrollada);
                await _context.SaveChangesAsync();

                responseVacunaDesarrolladaDTO.EmailOperadorNacional = model.EmailOperadorNacional;
                responseVacunaDesarrolladaDTO.Errores = errores;
                responseVacunaDesarrolladaDTO.ExistenciaErrores = false;
                responseVacunaDesarrolladaDTO.EstadoTransaccion = "Aceptada";
                responseVacunaDesarrolladaDTO.VacunaDesarrolladaDTO = new VacunaDesarrolladaDTO(vacunaDesarrollada.Id, model.IdVacuna, model.IdMarcaComercial, model.DiasDemoraEntrega, model.PrecioVacunaDesarrollada, model.FechaHasta.Value);
            }
            return Ok(responseVacunaDesarrolladaDTO);
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