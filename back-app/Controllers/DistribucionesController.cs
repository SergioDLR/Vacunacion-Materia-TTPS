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
    public class DistribucionesController : ControllerBase
    {
        private readonly VacunasContext _context;

        public DistribucionesController(VacunasContext context)
        {
            _context = context;
        }

        // GET: api/Distribuciones/GetStockAnalista?emailAnalistaProvincial=juanAnalista@gmail.com
        [HttpGet]
        [Route("GetStockAnalista")]
        public async Task<ActionResult<IEnumerable<ResponseStockAnalistaProvincialDTO>>> GetStockAnalista (string emailAnalistaProvincial = null)
        {
            try
            {
                ResponseStockAnalistaProvincialDTO responseStockAnalistaProvincial = new ResponseStockAnalistaProvincialDTO();
                
                //lista vacia para los errores
                List<string> errores = new List<string>();

                errores = await VerificarCredencialesAnalistaProvincial(emailAnalistaProvincial, errores);


                if (errores.Count == 0)
                {
                    Usuario usuario = await GetUsuario(emailAnalistaProvincial);
                    
                    //me quedo con las distribuciones de la jurisdicción del usuario
                    List<Distribucion> listaDistribuciones = await _context.Distribucion
                        .Where(d => d.IdJurisdiccion == usuario.IdJurisdiccion)
                        .ToListAsync();  
                    
                    List<TipoVacuna> tiposVacuna = await _context.TipoVacuna.ToListAsync();


                    foreach (TipoVacuna tipoVacuna in tiposVacuna)
                    {
                        foreach (Distribucion distribucion in listaDistribuciones.Where(d => d.IdLoteNavigation.IdVacunaDesarrolladaNavigation.IdVacunaNavigation.IdTipoVacuna == tipoVacuna.Id))
                        {
                            List<Lote> lotes = await _context.Lote.Where(l => l.Id == distribucion.IdLote)
                                .OrderBy(l => l.IdVacunaDesarrollada)
                                .ToListAsync();

                            List<VacunaDesarrollada> vacunasDesarrolladas = await _context.VacunaDesarrollada.ToListAsync();

                            int totalVacunaDesarrollada = 0;
                            int totalVacunaDesarrolladaVencido = 0;
                            int totalVacunaDesarrolladaDisponible = 0;

                            foreach (VacunaDesarrollada vd in vacunasDesarrolladas)
                            {
                                List<Lote> lotesVacunaDesarrollada = lotes.Where(l => l.IdVacunaDesarrolladaNavigation.Id == vd.Id).ToList();

                                int totalLotes = 0;
                                int totalLotesVencido = 0;
                                int totalLotesDisponible = 0;

                                foreach (Lote lote in lotesVacunaDesarrollada)
                                {
                                    int cantidadRestanteLote = (distribucion.CantidadVacunas - distribucion.Aplicadas);
                                    totalLotes += distribucion.CantidadVacunas;
                                    totalLotesVencido += distribucion.Vencidas;
                                    totalLotesDisponible += cantidadRestanteLote - distribucion.Vencidas;
                                    LoteStockDTO loteStockDTO = new LoteStockDTO(lote.Id, distribucion.CantidadVacunas, lote.Disponible, lote.FechaVencimiento, cantidadRestanteLote);
                                }

                                totalVacunaDesarrollada += totalLotes;
                                totalVacunaDesarrolladaVencido += totalLotesVencido;
                                totalVacunaDesarrolladaDisponible += totalLotesDisponible;
                            }




                        }  
                    }

                }

                return Ok(responseStockAnalistaProvincial);
            }
            catch
            {

            }
            
        }

        // GET: api/Distribuciones/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Distribucion>> GetDistribucion(int id)
        {
            var distribucion = await _context.Distribucion.FindAsync(id);

            if (distribucion == null)
            {
                return NotFound();
            }

            return distribucion;
        }

        // PUT: api/Distribuciones/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDistribucion(int id, Distribucion distribucion)
        {
            if (id != distribucion.Id)
            {
                return BadRequest();
            }

            _context.Entry(distribucion).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DistribucionExists(id))
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

        // POST: api/Distribuciones
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Distribucion>> PostDistribucion(Distribucion distribucion)
        {
            _context.Distribucion.Add(distribucion);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDistribucion", new { id = distribucion.Id }, distribucion);
        }

        // DELETE: api/Distribuciones/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Distribucion>> DeleteDistribucion(int id)
        {
            var distribucion = await _context.Distribucion.FindAsync(id);
            if (distribucion == null)
            {
                return NotFound();
            }

            _context.Distribucion.Remove(distribucion);
            await _context.SaveChangesAsync();

            return distribucion;
        }

        //privados
        private bool DistribucionExists(int id)
        {
            return _context.Distribucion.Any(e => e.Id == id);
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

        private async Task<bool> TieneRolAnalistaProvincial(Usuario usuario)
        {
            try
            {
                Rol rolAnalistaProvincial = await _context.Rol
                    .Where(rol => rol.Descripcion == "Analista Provincial").FirstOrDefaultAsync();

                if (rolAnalistaProvincial.Id == usuario.IdRol)
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

        private async Task<List<string>> VerificarCredencialesAnalistaProvincial(string emailAnalistaProvincial, List<string> errores)
        {
            try
            {
                Usuario usuarioSolicitante = await CuentaUsuarioExists(emailAnalistaProvincial);
                if (usuarioSolicitante != null)
                {
                    if (!await TieneRolAnalistaProvincial(usuarioSolicitante))
                    {
                        errores.Add(String.Format("El usuario {0} no tiene el rol de Analista Provincial", emailAnalistaProvincial));
                    }
                }
                else
                {
                    errores.Add(string.Format("El usuario {0} no existe en el sistema", emailAnalistaProvincial));
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
    }

}