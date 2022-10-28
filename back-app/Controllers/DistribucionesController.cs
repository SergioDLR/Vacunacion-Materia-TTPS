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

        // GET: api/Distribuciones
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Distribucion>>> GetDistribucion()
        {
            return await _context.Distribucion.ToListAsync();
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

        // POST: api/Distribuciones/ConsultarDistribucion
        [HttpPost]
        [Route("ConsultarDistribucion")]
        public async Task<ActionResult<ResponseDistribucionDTO>> ConsultarDistribucion([FromBody] RequestDistribucionDTO model)
        {
            try
            {
                ResponseDistribucionDTO responseDistribucionDTO = null;
                List<string> errores = new List<string>();
                List<SolicitudEntregaDTO> listaSolicitudesEntregas = new List<SolicitudEntregaDTO>();

                errores = await VerificarCredencialesUsuarioOperadorNacional(model.EmailOperadorNacional, errores);

                Jurisdiccion jurisdiccionExistente = await GetJurisdiccion(model.IdJurisdiccion);
                if (jurisdiccionExistente == null)
                    errores.Add(string.Format("La jurisdicción con identificador {0} no está registrada en el sistema", model.IdJurisdiccion));

                foreach (EnvioVacunaDTO envio in model.ListaEnviosVacunas)
                {
                    Vacuna vacunaExistente = await GetVacuna(envio.IdVacuna);
                    if (vacunaExistente == null)
                        errores.Add(string.Format("La vacuna con identificador {0} no está registrada en el sistema", envio.IdVacuna));

                    if (envio.IdVacunaDesarrollada != null && envio.IdVacunaDesarrollada != 0)
                    {
                        VacunaDesarrollada vacunaDesarrolladaExistente = await GetVacunaDesarrollada(envio.IdVacunaDesarrollada.Value);
                        if (vacunaDesarrolladaExistente == null)
                            errores.Add(string.Format("La vacuna desarrollada con identificador {0} no está registrada en el sistema", envio.IdVacunaDesarrollada));
                    }
                }

                if (errores.Count > 0)
                    responseDistribucionDTO = new ResponseDistribucionDTO("Rechazada", true, errores, 
                        model.EmailOperadorNacional, model.IdJurisdiccion, listaSolicitudesEntregas);
                else
                {
                    foreach (EnvioVacunaDTO envio in model.ListaEnviosVacunas)
                    {
                        List<DetalleEntregaDTO> listaDetallesEntregas = new List<DetalleEntregaDTO>();
                        List<string> alertas = new List<string>();
                        Vacuna vac = await GetVacuna(envio.IdVacuna);
                        int cantidadEntrega = 0;

                        if (envio.IdVacunaDesarrollada == null || envio.IdVacunaDesarrollada == 0)
                            listaDetallesEntregas = await GetListaSolicitudesEntregas(envio);

                        if (listaDetallesEntregas.Count == 0)
                            alertas.Add(string.Format("No hay stock nacional para distribuir la vacuna {0}", vac.Descripcion));
                        else
                        {
                            foreach (DetalleEntregaDTO detalle in listaDetallesEntregas)
                            {
                                cantidadEntrega += detalle.CantidadVacunas;
                            }
                            if (cantidadEntrega < envio.CantidadVacunas)
                                alertas.Add(string.Format("Hay stock nacional de {0} unidades para distribuir la vacuna {1}", cantidadEntrega, vac.Descripcion));
                        }

                        SolicitudEntregaDTO solicitudEntregaDTO = new SolicitudEntregaDTO(envio, alertas, cantidadEntrega, listaDetallesEntregas);

                        listaSolicitudesEntregas.Add(solicitudEntregaDTO);
                    }

                    responseDistribucionDTO = new ResponseDistribucionDTO("Aceptada", false, errores, 
                        model.EmailOperadorNacional, model.IdJurisdiccion, listaSolicitudesEntregas);
                }

                return Ok(responseDistribucionDTO);
            }
            catch(Exception error)
            {
                return BadRequest(error.Message);
            }
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



        //Métodos de ayuda
        private bool DistribucionExists(int id)
        {
            return _context.Distribucion.Any(e => e.Id == id);
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

        private async Task<Jurisdiccion> GetJurisdiccion(int idJurisdiccion)
        {
            Jurisdiccion jurisdiccionExistente = null;

            try
            {
                jurisdiccionExistente = await _context.Jurisdiccion
                    .Where(j => j.Id == idJurisdiccion).FirstOrDefaultAsync();
            }
            catch
            {

            }

            return jurisdiccionExistente;
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

        private async Task<List<DetalleEntregaDTO>> GetListaSolicitudesEntregas(EnvioVacunaDTO envio)
        {
            List<DetalleEntregaDTO> listaDetallesEntregasDTO = new List<DetalleEntregaDTO>();

            try
            {
                List<Compra> compras = await _context.Compra
                    .Where(l => l.IdLoteNavigation.Disponible == true
                        && l.IdLoteNavigation.IdVacunaDesarrolladaNavigation.IdVacuna == envio.IdVacuna
                        && l.IdLoteNavigation.FechaVencimiento > DateTime.Now)
                    .OrderBy(l => l.IdLoteNavigation.FechaVencimiento)
                    .ToListAsync();
                              
                foreach (Compra com in compras)
                {
                    Lote lote = await _context.Lote.Where(l => l.Id == com.IdLote).FirstOrDefaultAsync();

                    List<Distribucion> distribucionesLote = await _context.Distribucion
                        .Where(d => d.IdLote == lote.Id).ToListAsync();

                    int cantidadTotalAplicadas = 0;
                    int disponibles = 0;
                    int otorgadas = 0;

                    foreach (Distribucion distribucion in distribucionesLote)
                    {
                        cantidadTotalAplicadas += distribucion.Aplicadas;
                    }

                    disponibles = com.CantidadVacunas - cantidadTotalAplicadas;
                    
                    if (disponibles >= envio.CantidadVacunas)
                        otorgadas = envio.CantidadVacunas;
                    else if (disponibles != 0)
                        otorgadas = disponibles;

                    Vacuna vacunaLote = await GetVacuna(lote.IdVacunaDesarrolladaNavigation.IdVacuna);
                    VacunaDesarrollada vacunaDesarrolladaLote = await GetVacunaDesarrollada(lote.IdVacunaDesarrollada);
                    MarcaComercial marcaComercial = await GetMarcaComercial(vacunaDesarrolladaLote.IdMarcaComercial);

                    DetalleEntregaDTO detalleEntregaDTO = new DetalleEntregaDTO(vacunaLote.Id, vacunaDesarrolladaLote.Id, vacunaLote.Descripcion,
                        vacunaLote.Descripcion + " " + marcaComercial.Descripcion, otorgadas, lote.Id, lote.FechaVencimiento);

                    listaDetallesEntregasDTO.Add(detalleEntregaDTO);

                    if (otorgadas == envio.CantidadVacunas)
                        break;
                }
            }
            catch
            {

            }

            return listaDetallesEntregasDTO;
        }
    }
}
