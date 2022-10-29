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

        // GET: api/Distribuciones/GetStockOperadorNacionalByVacuna?emailOperadorNacional=maria@gmail.com&idVacuna=1
        [HttpGet]
        [Route("GetStockOperadorNacionalByVacuna")]
        public async Task<ActionResult<List<VacunaStockDTO>>> GetStockOperadorNacionalByVacuna(string emailOperadorNacional = null, int idVacuna = 0)
        {
            try
            {
                List<VacunaStockDTO> vacunasStock = new List<VacunaStockDTO>();

                if (emailOperadorNacional != null && (await TieneRolOperadorNacional(await GetUsuario(emailOperadorNacional))) && idVacuna != 0)
                {
                    List<Compra> compras = await _context.Compra
                       .Where(l => l.IdLoteNavigation.Disponible == true
                           && l.IdLoteNavigation.IdVacunaDesarrolladaNavigation.IdVacuna == idVacuna
                           && l.IdLoteNavigation.FechaVencimiento > DateTime.Now)
                       .OrderBy(l => l.IdLoteNavigation.FechaVencimiento)
                       .ToListAsync();

                    foreach (Compra com in compras)
                    {
                        Lote lote = await _context.Lote.Where(l => l.Id == com.IdLote).FirstOrDefaultAsync();

                        List<Distribucion> distribucionesLote = await _context.Distribucion
                            .Where(d => d.IdLote == lote.Id).ToListAsync();

                        int cantidadTotalDistribuidas = 0;
                        int disponibles = 0;

                        foreach (Distribucion distribucion in distribucionesLote)
                        {
                            cantidadTotalDistribuidas += distribucion.CantidadVacunas;
                        }

                        disponibles = com.CantidadVacunas - cantidadTotalDistribuidas;

                        VacunaDesarrollada vd = await GetVacunaDesarrollada(lote.IdVacunaDesarrollada);
                        MarcaComercial mc = await GetMarcaComercial(vd.IdMarcaComercial);
                        Vacuna vac = await GetVacuna(idVacuna);

                        VacunaStockDTO vs = new VacunaStockDTO(vd.Id, vac.Descripcion + " " + mc.Descripcion + " - Cantidad Disponible: " + disponibles);

                        vacunasStock.Add(vs);
                    }
                }

                return Ok(vacunasStock);
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }

        
        //Falta CHEQUEAR EN POSTMAN
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

                    //me quedo con la jurisdiccion del usuario
                    Jurisdiccion jurisdiccionUsuario = await _context.Jurisdiccion.Where(jurisdiccion => jurisdiccion.Id == usuario.IdJurisdiccion).FirstOrDefaultAsync();

                    //me quedo con las distribuciones de la jurisdicción del usuario
                    List<Distribucion> listaDistribuciones = await _context.Distribucion
                        .Where(d => d.IdJurisdiccion == usuario.IdJurisdiccion)
                        .ToListAsync();

                    List<TipoVacuna> tiposVacuna = await _context.TipoVacuna.ToListAsync();
                    List<VacunaDesarrollada> vacunasDesarrolladas = await _context.VacunaDesarrollada.ToListAsync();
                    List<TipoVacunaStockDTO> tiposVacunasStockDTO = new List<TipoVacunaStockDTO>();

                    int total = 0;
                    int totalVencido = 0;
                    int totalDisponible = 0;

                    foreach (TipoVacuna tipoVacuna in tiposVacuna)
                    {
                        int totalVacunaDesarrollada = 0;
                        int totalVacunaDesarrolladaVencido = 0;
                        int totalVacunaDesarrolladaDisponible = 0;

                        //------
                        List<VacunaDesarrolladaStockDTO> vacunasDesarrolladasStockDTO = new List<VacunaDesarrolladaStockDTO>();
                        //------

                        foreach (VacunaDesarrollada vd in vacunasDesarrolladas)
                        {
                            List<Distribucion> distribucionesVacunaDesarrollada = await _context.Distribucion
                                .Where(d => d.IdLoteNavigation.IdVacunaDesarrollada == vd.Id
                                    && d.IdJurisdiccion == usuario.IdJurisdiccion
                                    && d.IdLoteNavigation.IdVacunaDesarrolladaNavigation.IdVacunaNavigation.IdTipoVacuna == tipoVacuna.Id)
                                .ToListAsync();

                            if (distribucionesVacunaDesarrollada.Count == 0)
                            {
                                TipoVacunaStockDTO tipoVacunaStockVacioDTO = new TipoVacunaStockDTO(tipoVacuna.Id, tipoVacuna.Descripcion,
                                    new List<VacunaDesarrolladaStockDTO>(), totalVacunaDesarrollada, totalVacunaDesarrolladaVencido, totalVacunaDesarrolladaDisponible);
                                tiposVacunasStockDTO.Add(tipoVacunaStockVacioDTO);
                            }
                            else
                            {
                                List<Lote> lotes = null;

                                foreach (Distribucion distribucion in distribucionesVacunaDesarrollada)
                                {
                                    lotes = await _context.Lote.Where(l => l.Id == distribucion.IdLote)
                                        .OrderBy(l => l.IdVacunaDesarrollada)
                                        .ToListAsync();
                                }

                                int totalLotes = 0;
                                int totalLotesVencido = 0;
                                int totalLotesDisponible = 0;

                                //------
                                List<LoteStockDTO> lotesDTO = new List<LoteStockDTO>();
                                //------

                                foreach (Lote lote in lotes)
                                {
                                    Distribucion distribucion = await _context.Distribucion.Where(d => d.IdLote == lote.Id).FirstOrDefaultAsync();

                                    int cantidadRestanteLote = (distribucion.CantidadVacunas - distribucion.Aplicadas);
                                    totalLotes += distribucion.CantidadVacunas;
                                    totalLotesVencido += distribucion.Vencidas;
                                    totalLotesDisponible += cantidadRestanteLote - distribucion.Vencidas;
                                    LoteStockDTO loteStockDTO = new LoteStockDTO(lote.Id, distribucion.CantidadVacunas, lote.Disponible, lote.FechaVencimiento, cantidadRestanteLote);
                                    //------
                                    lotesDTO.Add(loteStockDTO);
                                    //------
                                }

                                totalVacunaDesarrollada += totalLotes;
                                totalVacunaDesarrolladaVencido += totalLotesVencido;
                                totalVacunaDesarrolladaDisponible += totalLotesDisponible;

                                //----
                                MarcaComercial marcaComercialExistente = await _context.MarcaComercial.Where(mc => mc.Id == vd.IdMarcaComercial).FirstOrDefaultAsync();
                                Vacuna vacuna = await _context.Vacuna.Where(v => v.Id == vd.IdVacuna).FirstOrDefaultAsync();
                                string descripcionVacunaMarca = vacuna.Descripcion + " " + marcaComercialExistente.Descripcion;

                                VacunaDesarrolladaStockDTO vacunaDesarrolladaStockDTO = new VacunaDesarrolladaStockDTO(vd.Id, vd.IdVacuna, vd.IdMarcaComercial, descripcionVacunaMarca, lotesDTO, totalLotes, totalLotesVencido, totalLotesDisponible);
                                vacunasDesarrolladasStockDTO.Add(vacunaDesarrolladaStockDTO);
                                //----
                            }
                            total += totalVacunaDesarrollada;
                            totalVencido += totalVacunaDesarrollada;
                            totalDisponible += totalVacunaDesarrolladaDisponible;

                            //----
                            TipoVacunaStockDTO tipoVacunaStockDTO = new TipoVacunaStockDTO(tipoVacuna.Id, tipoVacuna.Descripcion, vacunasDesarrolladasStockDTO, totalVacunaDesarrollada, totalVacunaDesarrolladaVencido, totalVacunaDesarrolladaDisponible);
                            tiposVacunasStockDTO.Add(tipoVacunaStockDTO);
                            //----
                        }
                    }

                    //----
                    StockDTO stockDTO = new StockDTO(tiposVacunasStockDTO, total, totalVencido, totalDisponible);
                    StockJurisdiccionDTO stockJurisdiccionDTO = new StockJurisdiccionDTO(jurisdiccionUsuario.Id, jurisdiccionUsuario.Descripcion, stockDTO);
                    //----

                    responseStockAnalistaProvincial.EmailAnalistaProvincial = emailAnalistaProvincial;
                    responseStockAnalistaProvincial.EstadoTransaccion = "Aceptada";
                    responseStockAnalistaProvincial.Errores = errores;
                    responseStockAnalistaProvincial.ExistenciaErrores = false;
                    responseStockAnalistaProvincial.StockJurisdiccion = stockJurisdiccionDTO;
                } 
                else
                {
                    //response errores
                    responseStockAnalistaProvincial.EmailAnalistaProvincial = emailAnalistaProvincial;
                    responseStockAnalistaProvincial.EstadoTransaccion = "Rechazada";
                    responseStockAnalistaProvincial.Errores = errores;
                    responseStockAnalistaProvincial.ExistenciaErrores = true;
                    responseStockAnalistaProvincial.StockJurisdiccion = new StockJurisdiccionDTO();
                }
                return Ok(responseStockAnalistaProvincial);
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
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

        private async Task<Jurisdiccion> GetJurisdiccion(int idJurisdiccion)
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

        private async Task<VacunaDesarrollada> GetVacunaDesarrollada(int idVacunaDesarrollada)
        {
            VacunaDesarrollada vacunaDesarrolladaExistente = null;

            try
            {
                vacunaDesarrolladaExistente = await _context.VacunaDesarrollada
                    .Where(vac => vac.Id == idVacunaDesarrollada).FirstOrDefaultAsync();
            }
            catch
            {

            }

            return vacunaDesarrolladaExistente;
        }

        private async Task<MarcaComercial> GetMarcaComercial(int idMarcaComercial)
        {
            MarcaComercial marcaComercialExistente = null;

            try
            {
                marcaComercialExistente = await _context.MarcaComercial
                    .Where(mc => mc.Id == idMarcaComercial).FirstOrDefaultAsync();
            }
            catch
            {

            }

            return marcaComercialExistente;
        }

        private async Task<Vacuna> GetVacuna(int idVacuna)
        {
            Vacuna vacunaExistente = null;

            try
            {
                vacunaExistente = await _context.Vacuna
                    .Where(vac => vac.Id == idVacuna).FirstOrDefaultAsync();
            }
            catch
            {

            }

            return vacunaExistente;
        }
    }

}