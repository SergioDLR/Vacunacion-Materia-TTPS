using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VacunacionApi.DTO;
using VacunacionApi.Models;
using VacunacionApi.Services;

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

                if (emailOperadorNacional != null && RolService.TieneRol(_context, UsuarioService.GetUsuario(_context, emailOperadorNacional), "Operador Nacional") && idVacuna != 0)
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

                        VacunaDesarrollada vd = VacunaDesarrolladaService.GetVacunaDesarrollada(_context, lote.IdVacunaDesarrollada);
                        MarcaComercial mc = MarcaComercialService.GetMarcaComercial(_context, vd.IdMarcaComercial);
                        Vacuna vac = VacunaService.GetVacuna(_context, idVacuna);

                        VacunaStockDTO vs = new VacunaStockDTO(vac.Id, vd.Id, vac.Descripcion + " " + mc.Descripcion + " - Cantidad Disponible: " + disponibles);

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

        // GET: api/Distribuciones/GetStockOperadorNacionalAllVacunas?emailOperadorNacional=maria@gmail.com
        [HttpGet]
        [Route("GetStockOperadorNacionalAllVacunas")]
        public async Task<ActionResult<List<VacunaStockDTO>>> GetStockOperadorNacionalAllVacunas(string emailOperadorNacional = null)
        {
            try
            {
                List<VacunaStockDTO> vacunasStock = new List<VacunaStockDTO>();

                if (emailOperadorNacional != null && RolService.TieneRol(_context, UsuarioService.GetUsuario(_context, emailOperadorNacional), "Operador Nacional"))
                {
                    List<Compra> compras = await _context.Compra
                       .Where(l => l.IdLoteNavigation.Disponible == true
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

                        VacunaDesarrollada vd = VacunaDesarrolladaService.GetVacunaDesarrollada(_context, lote.IdVacunaDesarrollada);
                        MarcaComercial mc = MarcaComercialService.GetMarcaComercial(_context, vd.IdMarcaComercial);
                        Vacuna vac = VacunaService.GetVacuna(_context, vd.IdVacuna);

                        VacunaStockDTO vs = new VacunaStockDTO(vac.Id, vd.Id, vac.Descripcion + " " + mc.Descripcion + " - Cantidad Disponible: " + disponibles);

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

        // GET: api/Distribuciones/GetStockAnalistaProvincialAllVacunas?emailAnalistaProvincial=analista@gmail.com
        [HttpGet]
        [Route("GetStockAnalistaProvincialAllVacunas")]
        public async Task<ActionResult<List<VacunaStockAnalistaProvincialDTO>>> GetStockAnalistaProvincialAllVacunas(string emailAnalistaProvincial = null)
        {
            try
            {
                List<VacunaStockAnalistaProvincialDTO> vacunasAnalistaStockDTO = new List<VacunaStockAnalistaProvincialDTO>();
                Usuario usuarioAnalista = UsuarioService.GetUsuario(_context, emailAnalistaProvincial);

                if (emailAnalistaProvincial != null && RolService.TieneRol(_context, UsuarioService.GetUsuario(_context, emailAnalistaProvincial), "Analista Provincial"))
                {
                    List<Compra> compras = await _context.Compra
                       .Where(l => l.IdLoteNavigation.Disponible == true
                           && l.IdLoteNavigation.FechaVencimiento > DateTime.Now)
                       .OrderBy(l => l.IdLoteNavigation.FechaVencimiento)
                       .ToListAsync();

                    List<Distribucion> distribuciones = await _context.Distribucion
                        .Where(d => d.IdJurisdiccion == usuarioAnalista.IdJurisdiccion
                            && d.Vencidas == 0).ToListAsync();

                    foreach (Distribucion dist in distribuciones)
                    {
                        int disponibles = dist.CantidadVacunas - dist.Aplicadas;

                        Lote loteDist = await _context.Lote.Where(l => l.Id == dist.IdLote).FirstAsync();

                        VacunaDesarrollada vd = VacunaDesarrolladaService.GetVacunaDesarrollada(_context, loteDist.IdVacunaDesarrollada);
                        MarcaComercial mc = MarcaComercialService.GetMarcaComercial(_context, vd.IdMarcaComercial);
                        Vacuna vac = VacunaService.GetVacuna(_context, vd.IdVacuna);

                        VacunaStockAnalistaProvincialDTO vs = new VacunaStockAnalistaProvincialDTO(loteDist.Id, loteDist.FechaVencimiento, vac.Id, vd.Id, vac.Descripcion + " " + mc.Descripcion + " - Cantidad Disponible: " + disponibles);

                        vacunasAnalistaStockDTO.Add(vs);
                    }
                }

                return Ok(vacunasAnalistaStockDTO);
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }

        //------------------------

        // GET: api/Distribuciones/GetStockOperadorNacionalAllVacunasDisponibles?emailOperadorNacional=maria@gmail.com
        [HttpGet]
        [Route("GetStockOperadorNacionalAllVacunasDisponibles")]
        public async Task<ActionResult<List<VacunaStockNacionDTO>>> GetStockOperadorNacionalAllVacunasDisponibles(string emailOperadorNacional = null)
        {
            try
            {
                List<VacunaStockOperadorNacionalDTO> vacunasStockOperadorNacionalDTO = new List<VacunaStockOperadorNacionalDTO>();
                VacunaStockNacionDTO vacunaStockNacionDTO = new VacunaStockNacionDTO(); 
                Usuario usuarioOperador = UsuarioService.GetUsuario(_context, emailOperadorNacional);
                int totalNacion = 0;

                if (emailOperadorNacional != null && RolService.TieneRol(_context, UsuarioService.GetUsuario(_context, emailOperadorNacional), "Operador Nacional"))
                {
                    List<Compra> compras = await _context.Compra
                       .Where(l => l.IdLoteNavigation.Disponible == true
                           && l.IdLoteNavigation.FechaVencimiento > DateTime.Now)
                       .OrderBy(l => l.IdLoteNavigation.FechaVencimiento)
                       .ToListAsync();


                    List<Jurisdiccion> jurisdicciones = await _context.Jurisdiccion.ToListAsync();

                    foreach (Jurisdiccion juris in jurisdicciones)
                    {
                        List<VacunaStockAnalistaProvincialDTO> vacunasAnalistaStockDTO = new List<VacunaStockAnalistaProvincialDTO>();
                        int totalJurisdiccion = 0;
                       
                        //distribuciones de esa jurisdiccion
                        List<Distribucion> distribuciones = await _context.Distribucion
                            .Where(d => d.IdJurisdiccion == juris.Id && d.Vencidas == 0)
                            .ToListAsync();

                        if(distribuciones.Count == 0) {
                            VacunaStockOperadorNacionalDTO vacunaStockOperadorNacionalDTO = new VacunaStockOperadorNacionalDTO(juris.Id, juris.Descripcion, vacunasAnalistaStockDTO, totalJurisdiccion);
                            vacunasStockOperadorNacionalDTO.Add(vacunaStockOperadorNacionalDTO);    
                        }
                        else
                        {
                            foreach (Distribucion dist in distribuciones)
                            {
                                int disponibles = dist.CantidadVacunas - dist.Aplicadas;
                                totalJurisdiccion += disponibles;
                                
                                Lote loteDist = await _context.Lote.Where(l => l.Id == dist.IdLote).FirstAsync();

                                VacunaDesarrollada vd = VacunaDesarrolladaService.GetVacunaDesarrollada(_context, loteDist.IdVacunaDesarrollada);
                                MarcaComercial mc = MarcaComercialService.GetMarcaComercial(_context, vd.IdMarcaComercial);
                                Vacuna vac = VacunaService.GetVacuna(_context, vd.IdVacuna);

                                VacunaStockAnalistaProvincialDTO vs = new VacunaStockAnalistaProvincialDTO(loteDist.Id, loteDist.FechaVencimiento, vac.Id, vd.Id, vac.Descripcion + " " + mc.Descripcion + " - Cantidad Disponible: " + disponibles);                                
                                vacunasAnalistaStockDTO.Add(vs);
                            }
                            totalNacion += totalJurisdiccion;
                            VacunaStockOperadorNacionalDTO vacunaStockOperadorNacionalDTO = new VacunaStockOperadorNacionalDTO(juris.Id, juris.Descripcion, vacunasAnalistaStockDTO, totalJurisdiccion);
                            vacunasStockOperadorNacionalDTO.Add(vacunaStockOperadorNacionalDTO);
                        }
                    }
                }
                vacunaStockNacionDTO.VacunasStockOperadorNacionalDTO = vacunasStockOperadorNacionalDTO;
                vacunaStockNacionDTO.TotalVacunasDisponibleNacion = totalNacion;
                return Ok(vacunaStockNacionDTO);
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }
        //------------------------

        // GET: api/Distribuciones/GetAll?emailOperadorNacional&idJurisdiccion=1
        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult<ResponseListaDistribucionesDTO>> GetAll(string emailOperadorNacional = null, int idJurisdiccion = 0)
        {
            try
            {
                ResponseListaDistribucionesDTO responseListaDistribucionesDTO = new ResponseListaDistribucionesDTO();
                List<DistribucionDTO> listaDistribuciones = new List<DistribucionDTO>();
                List<string> errores = new List<string>();

                if (emailOperadorNacional == null)
                {
                    errores.Add("No se especificó el email operador nacional");
                }
                else
                    errores = RolService.VerificarCredencialesUsuario(_context, emailOperadorNacional, errores, "Operador Nacional");

                if (errores.Count > 0)
                    responseListaDistribucionesDTO = new ResponseListaDistribucionesDTO("Rechazada", true, errores, listaDistribuciones);
                else
                {
                    List<Distribucion> distribuciones = new List<Distribucion>();

                    if (idJurisdiccion != 0)
                        distribuciones = await _context.Distribucion.Where(d => d.IdJurisdiccion == idJurisdiccion).ToListAsync();
                    else
                        distribuciones = await _context.Distribucion.ToListAsync();

                    foreach (Distribucion distribucion in distribuciones)
                    {
                        Jurisdiccion juris = JurisdiccionService.GetJurisdiccion(_context, distribucion.IdJurisdiccion);

                        DistribucionDTO distribucionDTO = new DistribucionDTO(distribucion.Id, distribucion.Codigo, distribucion.IdJurisdiccion, juris.Descripcion,
                            distribucion.IdLote, distribucion.FechaEntrega, distribucion.CantidadVacunas, distribucion.Aplicadas, distribucion.Vencidas);

                        listaDistribuciones.Add(distribucionDTO);
                    }

                    responseListaDistribucionesDTO = new ResponseListaDistribucionesDTO("Aceptada", false, errores, listaDistribuciones);
                }

                return Ok(responseListaDistribucionesDTO);
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
                if (!DistribucionService.DistribucionExists(_context, id))
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

                errores = RolService.VerificarCredencialesUsuario(_context, model.EmailOperadorNacional, errores, "Operador Nacional");

                Jurisdiccion jurisdiccionExistente = JurisdiccionService.GetJurisdiccion(_context, model.IdJurisdiccion);
                if (jurisdiccionExistente == null)
                    errores.Add(string.Format("La jurisdicción con identificador {0} no está registrada en el sistema", model.IdJurisdiccion));

                foreach (EnvioVacunaDTO envio in model.ListaEnviosVacunas)
                {
                    Vacuna vacunaExistente = VacunaService.GetVacuna(_context, envio.IdVacuna);
                    if (vacunaExistente == null)
                        errores.Add(string.Format("La vacuna con identificador {0} no está registrada en el sistema", envio.IdVacuna));

                    if (envio.IdVacunaDesarrollada != null && envio.IdVacunaDesarrollada != 0)
                    {
                        VacunaDesarrollada vacunaDesarrolladaExistente = VacunaDesarrolladaService.GetVacunaDesarrollada(_context, envio.IdVacunaDesarrollada.Value);
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
                        Vacuna vac = VacunaService.GetVacuna(_context, envio.IdVacuna);
                        int cantidadEntrega = 0;

                        listaDetallesEntregas = DistribucionService.GetListaDetallesEntregas(_context, envio);

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
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }

        // POST: api/Distribuciones/RegistrarDistribucion
        [HttpPost]
        [Route("RegistrarDistribucion")]
        public async Task<ActionResult<ResponseRegistrarDistribucionDTO>> RegistrarDistribucion([FromBody] RequestRegistrarDistribucionDTO model)
        {
            try
            {
                ResponseRegistrarDistribucionDTO responseRegistrarDistribucionDTO = null;
                List<string> errores = new List<string>();
                List<DistribucionDTO> listaDistribuciones = new List<DistribucionDTO>();

                errores = RolService.VerificarCredencialesUsuario(_context, model.EmailOperadorNacional, errores, "Operador Nacional");

                Jurisdiccion jurisdiccionExistente = JurisdiccionService.GetJurisdiccion(_context, model.IdJurisdiccion);
                if (jurisdiccionExistente == null)
                    errores.Add(string.Format("La jurisdicción con identificador {0} no está registrada en el sistema", model.IdJurisdiccion));

                foreach (DetalleEntregaDTO entrega in model.ListaDetallesEntregas)
                {
                    Vacuna vacunaExistente = VacunaService.GetVacuna(_context, entrega.IdVacuna);
                    if (vacunaExistente == null)
                        errores.Add(string.Format("La vacuna con identificador {0} no está registrada en el sistema", entrega.IdVacuna));

                    Lote lote = await _context.Lote.Where(l => l.Id == entrega.IdLote).FirstOrDefaultAsync();
                    if (lote == null)
                        errores.Add(string.Format("El lote con identificador {0} no está registrado en el sistema", entrega.IdLote));

                    if (entrega.IdVacunaDesarrollada != null && entrega.IdVacunaDesarrollada != 0)
                    {
                        VacunaDesarrollada vacunaDesarrolladaExistente = VacunaDesarrolladaService.GetVacunaDesarrollada(_context, entrega.IdVacunaDesarrollada.Value);
                        if (vacunaDesarrolladaExistente == null)
                            errores.Add(string.Format("La vacuna desarrollada con identificador {0} no está registrada en el sistema", entrega.IdVacunaDesarrollada));
                    }

                    if (entrega.FechaVencimientoLote < DateTime.Now)
                        errores.Add(string.Format("El lote con identificador {0} está vencido", entrega.IdLote));
                }

                if (errores.Count > 0)
                {
                    responseRegistrarDistribucionDTO = new ResponseRegistrarDistribucionDTO("Rechazada", true,
                        errores, model.EmailOperadorNacional, model.IdJurisdiccion, listaDistribuciones);
                }
                else
                {
                    foreach (DetalleEntregaDTO entrega in model.ListaDetallesEntregas)
                    {
                        int cantidadTotalDistribuidas = 0;
                        int disponibles = 0;
                        int otorgadas = 0;

                        List<Distribucion> distribucionesLote = await _context.Distribucion
                            .Where(d => d.IdLote == entrega.IdLote)
                            .ToListAsync();

                        foreach (Distribucion dist in distribucionesLote)
                        {
                            cantidadTotalDistribuidas += dist.CantidadVacunas;
                        }

                        Compra compra = await _context.Compra.Where(c => c.IdLote == entrega.IdLote).FirstAsync();

                        disponibles = compra.CantidadVacunas - cantidadTotalDistribuidas;
                        if (disponibles >= entrega.CantidadVacunas)
                            otorgadas = entrega.CantidadVacunas;
                        else if (disponibles != 0)
                            otorgadas = disponibles;

                        entrega.CantidadVacunas = otorgadas;

                        if (entrega.CantidadVacunas != 0)
                        {
                            Random randomCodigoDistribucion = new Random();
                            int codigoDistribucion = randomCodigoDistribucion.Next(1, 100000000);

                            while (DistribucionService.GetDistribucionExistente(_context, codigoDistribucion) != null)
                            {
                                codigoDistribucion = randomCodigoDistribucion.Next(1, 100000000);
                            }

                            Distribucion distribucion = new Distribucion(codigoDistribucion, model.IdJurisdiccion,
                                entrega.IdLote, DateTime.Now, entrega.CantidadVacunas, 0, 0);

                            _context.Distribucion.Add(distribucion);
                            await _context.SaveChangesAsync();

                            Jurisdiccion juris = JurisdiccionService.GetJurisdiccion(_context, distribucion.IdJurisdiccion);

                            DistribucionDTO distribucionDTO = new DistribucionDTO(distribucion.Id, distribucion.Codigo, distribucion.IdJurisdiccion,
                                juris.Descripcion, distribucion.IdLote, distribucion.FechaEntrega, distribucion.CantidadVacunas, 0, 0);

                            listaDistribuciones.Add(distribucionDTO);
                        }
                    }

                    responseRegistrarDistribucionDTO = new ResponseRegistrarDistribucionDTO("Aceptada", false, errores,
                        model.EmailOperadorNacional, model.IdJurisdiccion, listaDistribuciones);
                }

                return Ok(responseRegistrarDistribucionDTO);
            }
            catch (Exception error)
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
    }
}
