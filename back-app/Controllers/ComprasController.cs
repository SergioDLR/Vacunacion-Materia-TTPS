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
    public class ComprasController : ControllerBase
    {
        private readonly VacunasContext _context;

        public ComprasController(VacunasContext context)
        {
            _context = context;
        }

        // GET: api/Compras/GetAll
        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult<IEnumerable<ResponseListaComprasDTO>>> GetAll(string emailOperadorNacional = null, int idVacunaDesarrollada = 0)
        {
            try
            {
                ResponseListaComprasDTO responseListaComprasDTO = new ResponseListaComprasDTO();
                List<CompraDTO> listaCompras = new List<CompraDTO>();
                List<string> errores = new List<string>();

                if (emailOperadorNacional == null)
                {
                    errores.Add("No se especificó el email operador nacional");
                }
                else
                {
                    errores = await VerificarCredencialesUsuarioOperadorNacional(emailOperadorNacional, errores);

                    if (errores.Count > 0)
                        responseListaComprasDTO = new ResponseListaComprasDTO("Rechazada", true, errores, listaCompras);
                    else
                    {
                        List<Lote> listaLotes = new List<Lote>();

                        if (idVacunaDesarrollada != 0)
                            listaLotes = await _context.Lote.Where(l => l.IdVacunaDesarrollada == idVacunaDesarrollada).ToListAsync();
                        else
                            listaLotes = await _context.Lote.ToListAsync();

                        foreach (Lote lote in listaLotes)
                        {
                            Compra compra = await _context.Compra.Where(c => c.IdLote == lote.Id).FirstOrDefaultAsync();

                            if (compra != null)
                            {
                                EstadoCompra estadoCompra = await _context.EstadoCompra.Where(ec => ec.Id == compra.IdEstadoCompra).FirstOrDefaultAsync();
                                if (estadoCompra != null)
                                {
                                    Vacuna vacuna = null;
                                    VacunaDesarrollada vd = await _context.VacunaDesarrollada.Where(v => v.Id == lote.IdVacunaDesarrollada).FirstOrDefaultAsync();

                                    if (vd != null)
                                    {
                                        vacuna = await _context.Vacuna.Where(v => v.Id == vd.IdVacuna).FirstOrDefaultAsync();

                                        if (vacuna != null)
                                        {
                                            MarcaComercial marcaComercial = await _context.MarcaComercial.Where(mc => mc.Id == vd.IdMarcaComercial).FirstOrDefaultAsync();
                                            if (marcaComercial != null)
                                            {
                                                CompraDTO compraDTO = new CompraDTO(compra.Id, compra.IdLote, lote.FechaVencimiento, lote.IdVacunaDesarrollada,
                                                    vacuna.Descripcion + " " + marcaComercial.Descripcion, compra.IdEstadoCompra, estadoCompra.Descripcion, compra.CantidadVacunas,
                                                    compra.Codigo, compra.FechaCompra, compra.FechaEntrega, compra.Distribuidas, compra.Vencidas, compra.CantidadVacunas * vd.PrecioVacuna);

                                                listaCompras.Add(compraDTO);
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        responseListaComprasDTO = new ResponseListaComprasDTO("Aceptada", false, errores, listaCompras);
                    }
                }

                return Ok(responseListaComprasDTO);
            }
            catch(Exception error)
            {
                return BadRequest(error.Message);
            }
        }

        // GET: api/Compras/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Compra>> GetCompra(int id)
        {
            var compra = await _context.Compra.FindAsync(id);

            if (compra == null)
            {
                return NotFound();
            }

            return compra;
        }

        // PUT: api/Compras/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCompra(int id, Compra compra)
        {
            if (id != compra.Id)
            {
                return BadRequest();
            }

            _context.Entry(compra).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompraExists(id))
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

        // POST: api/Compras/RegistrarCompra
        [HttpPost]
        [Route("RegistrarCompra")]
        public async Task<ActionResult<ResponseCompraDTO>> RegistrarCompra([FromBody] RequestCompraDTO model)
        {
            try
            {
                ResponseCompraDTO responseCompraDTO = null;
                List<string> errores = new List<string>();
                Vacuna vacuna = null;
                MarcaComercial marcaComercial = null;

                EstadoCompra estadoCompra = await _context.EstadoCompra.Where(ec => ec.Descripcion == "No Recibida").FirstOrDefaultAsync();
                if (estadoCompra == null)
                    errores.Add("Error de conexión al procesar los estados de compra");

                VacunaDesarrollada vacunaDesarrolladaExistente = await GetVacunaDesarrollada(model.IdVacunaDesarrollada);
                
                if (vacunaDesarrolladaExistente == null)
                    errores.Add(string.Format("La vacuna desarrollada con identificador {0} no está registrada en el sistema", model.IdVacunaDesarrollada));
                else
                {
                    marcaComercial = await _context.MarcaComercial.Where(mc => mc.Id == vacunaDesarrolladaExistente.IdMarcaComercial).FirstOrDefaultAsync();
                    if (marcaComercial == null)
                        errores.Add("Error de conexión al procesar marca comercial");

                    vacuna = await _context.Vacuna.Where(v => v.Id == vacunaDesarrolladaExistente.IdVacuna).FirstOrDefaultAsync();
                    
                    if(vacuna != null)
                    {
                        TipoVacuna tipoVacuna = await _context.TipoVacuna.Where(tv => tv.Id == vacuna.IdTipoVacuna).FirstOrDefaultAsync();

                        if (tipoVacuna != null)
                        {
                            DateTime fechaActual = DateTime.Now;
                            int anioActual = fechaActual.Year;
                            int mesActual = Convert.ToInt32(fechaActual.Month.ToString());

                            if (tipoVacuna.Descripcion == "Vacuna Anual")
                            {
                                EntidadVacunaDosis evd = await _context.EntidadVacunaDosis.Where(evd => evd.IdVacuna == vacuna.Id).FirstOrDefaultAsync();
                                if (evd != null)
                                {
                                    Dosis dosisVacuna = await _context.Dosis.Where(d => d.Id == evd.IdDosis).FirstOrDefaultAsync();

                                    if (dosisVacuna != null)
                                    {
                                        EntidadDosisRegla edr = await _context.EntidadDosisRegla.Where(e => e.Id == dosisVacuna.Id).FirstOrDefaultAsync();

                                        if (edr != null)
                                        {
                                            Regla regla = await _context.Regla.Where(r => r.Id == edr.IdRegla).FirstOrDefaultAsync();

                                            if (regla != null)
                                            {
                                                string[] meses = regla.MesesVacunacion.Split(",");
                                                List<int> listaMeses = new List<int>();

                                                foreach (string m in meses)
                                                {
                                                    listaMeses.Add(Convert.ToInt32(m));
                                                }

                                                if (fechaActual.AddDays(vacunaDesarrolladaExistente.DiasDemoraEntrega).Month > listaMeses.Last())
                                                {
                                                    errores.Add(string.Format("La vacuna {0} es del tipo {1}. El último mes de aplicacón es {2}: compra fuera de término",
                                                        vacuna.Descripcion, tipoVacuna.Descripcion, listaMeses.Last()));
                                                }
                                            }
                                            else
                                                errores.Add("Error de conexión al procesar regla");
                                        }
                                        else
                                            errores.Add("Error de conexión al procesar dosis y reglas");
                                    }
                                    else
                                        errores.Add("Error de conexión al procesar dosis");
                                }
                                else
                                    errores.Add("Error de conexión al procesar vacunas y dosis");
                            }
                            else if (tipoVacuna.Descripcion == "Vacuna de Pandemia")
                            {
                                Pandemia pandemia = await _context.Pandemia.Where(p => p.Id == vacuna.IdPandemia).FirstOrDefaultAsync();

                                if (pandemia != null)
                                {
                                    if (fechaActual > pandemia.FechaFin)
                                    {
                                        errores.Add(string.Format("La pandemia {0} finalizó", pandemia.Descripcion));
                                    }
                                    else if (fechaActual.AddDays(vacunaDesarrolladaExistente.DiasDemoraEntrega) > pandemia.FechaFin)
                                    {
                                        errores.Add(string.Format("La pandemia {0} finalizará el día {1}. Fecha recepción de la compra: {2}", pandemia.Descripcion, pandemia.FechaFin, fechaActual.AddDays(vacunaDesarrolladaExistente.DiasDemoraEntrega)));
                                    }   
                                }
                                else
                                    errores.Add("Error de conexión al procesar pandemia");
                            }
                        }
                        else
                            errores.Add("Error de conexión al procesar tipo vacuna");
                    }
                    else
                        errores.Add("Error de conexión al procesar vacuna");
                }

                errores = await VerificarCredencialesUsuarioOperadorNacional(model.EmailOperadorNacional, errores);
                              
                if (errores.Count > 0)
                    responseCompraDTO = new ResponseCompraDTO("Rechazada", true, errores, new CompraDTO(0, 0, null, model.IdVacunaDesarrollada, null, 0, null, model.CantidadVacunas, 0, null, null, 0, 0, 0));
                else
                {
                    Random randomCodigoCompra = new Random();
                    int codigoCompra = randomCodigoCompra.Next(1, 100000000);

                    while (await GetCompraExistente(codigoCompra) != null)
                    {
                        codigoCompra = randomCodigoCompra.Next(1, 100000000);
                    }

                    DateTime start = DateTime.Now; 
                    Random gen = new Random();
                    int dias = gen.Next(1, 1000);
                    
                    while(start.AddDays(dias) < DateTime.Now.AddDays(vacunaDesarrolladaExistente.DiasDemoraEntrega))
                    {
                        dias = gen.Next(1, 1000);
                    }

                    DateTime fechaVencimiento = start.AddDays(dias);

                    Lote lote = new Lote(vacunaDesarrolladaExistente.Id, fechaVencimiento);
                    _context.Lote.Add(lote);
                    await _context.SaveChangesAsync();
                                       
                    Compra compra = new Compra(lote.Id, estadoCompra.Id, model.CantidadVacunas, codigoCompra, DateTime.Now.AddDays(vacunaDesarrolladaExistente.DiasDemoraEntrega));
                    _context.Compra.Add(compra);
                    await _context.SaveChangesAsync();

                    CompraDTO compraDTO = new CompraDTO(compra.Id, compra.IdLote, lote.FechaVencimiento, lote.IdVacunaDesarrollada, 
                        vacuna.Descripcion + " " + marcaComercial.Descripcion, compra.IdEstadoCompra, estadoCompra.Descripcion, compra.CantidadVacunas, 
                        compra.Codigo, compra.FechaCompra, compra.FechaEntrega, compra.Distribuidas, compra.Vencidas, compra.CantidadVacunas * vacunaDesarrolladaExistente.PrecioVacuna);

                    responseCompraDTO = new ResponseCompraDTO("Aceptada", false, errores, compraDTO);
                }

                return Ok(responseCompraDTO);

            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }

        // DELETE: api/Compras/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Compra>> DeleteCompra(int id)
        {
            var compra = await _context.Compra.FindAsync(id);
            if (compra == null)
            {
                return NotFound();
            }

            _context.Compra.Remove(compra);
            await _context.SaveChangesAsync();

            return compra;
        }



        //Métodos privados de ayuda
        private bool CompraExists(int id)
        {
            return _context.Compra.Any(e => e.Id == id);
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

        private async Task<Compra> GetCompraExistente(int codigoCompra)
        {
            Compra compraExistente = null;

            try
            {
                compraExistente = await _context.Compra
                    .Where(c => c.Codigo == codigoCompra).FirstOrDefaultAsync();
            }
            catch
            {

            }

            return compraExistente;
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

        private async Task<List<string>> VerificarCredencialesUsuarioOperadorNacional(string emailOperadorNacional, List<string> errores)
        {
            try
            {
                Usuario usuarioSolicitante = await GetUsuario(emailOperadorNacional);
                if (usuarioSolicitante == null)
                    errores.Add(string.Format("El usuario {0} no está registrado en el sistema", emailOperadorNacional));
                else
                {
                    bool tieneRolOperadorNacional = await TieneRolOperadorNacional(usuarioSolicitante);
                    if (!tieneRolOperadorNacional)
                        errores.Add(string.Format("El usuario {0} no tiene rol operador nacional", emailOperadorNacional));
                }
            }
            catch
            {

            }

            return errores;
        }
    }
}
