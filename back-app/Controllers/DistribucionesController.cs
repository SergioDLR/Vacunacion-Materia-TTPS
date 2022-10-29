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

        // GET: api/Distribuciones/GetStockOperadorNacionalAllVacunas?emailOperadorNacional=maria@gmail.com
        [HttpGet]
        [Route("GetStockOperadorNacionalAllVacunas")]
        public async Task<ActionResult<List<VacunaStockDTO>>> GetStockOperadorNacionalAllVacunas(string emailOperadorNacional)
        {
            try
            {
                List<VacunaStockDTO> vacunasStock = new List<VacunaStockDTO>();

                if (emailOperadorNacional != null && (await TieneRolOperadorNacional(await GetUsuario(emailOperadorNacional))))
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

                        VacunaDesarrollada vd = await GetVacunaDesarrollada(lote.IdVacunaDesarrollada);
                        MarcaComercial mc = await GetMarcaComercial(vd.IdMarcaComercial);
                        Vacuna vac = await GetVacuna(vd.IdVacuna);

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
        public async Task<ActionResult<List<VacunaStockDTO>>> GetStockOperadorNacionalAllVacunas(string emailOperadorNacional)
        {
            try
            {
                List<VacunaStockDTO> vacunasStock = new List<VacunaStockDTO>();

                if (emailOperadorNacional != null && (await TieneRolOperadorNacional(await GetUsuario(emailOperadorNacional))))
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

                        VacunaDesarrollada vd = await GetVacunaDesarrollada(lote.IdVacunaDesarrollada);
                        MarcaComercial mc = await GetMarcaComercial(vd.IdMarcaComercial);
                        Vacuna vac = await GetVacuna(vd.IdVacuna);

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

        // GET: api/Distribuciones/GetStockAnalista?emailAnalistaProvincial=juanAnalista@gmail.com
        [HttpGet]
        [Route("GetStockAnalista")]
        public async Task<ActionResult<IEnumerable<ResponseStockAnalistaProvincialDTO>>> GetStockAnalista(string emailAnalistaProvincial = null)
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
                    List<VacunaDesarrollada> vacunasDesarrolladas = null;
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

                        List<Vacuna> vacunasTipo = await _context.Vacuna.Where(v => v.IdTipoVacuna == tipoVacuna.Id).ToListAsync();
                        List<VacunaDesarrollada> vacunasDesarrolladasTipo = new List<VacunaDesarrollada>();

                        foreach (Vacuna vac in vacunasTipo)
                        {
                            VacunaDesarrollada vdesa = await _context.VacunaDesarrollada.Where(v => v.IdVacuna == vac.Id).FirstAsync();
                            vacunasDesarrolladasTipo.Add(vdesa);
                        }

                        vacunasDesarrolladas = vacunasDesarrolladasTipo;

                        foreach (VacunaDesarrollada vd in vacunasDesarrolladas)
                        {
                            int totalLotes = 0;
                            int totalLotesVencido = 0;
                            int totalLotesDisponible = 0;

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
                                                              
                                //----
                                MarcaComercial marcaComercialExistente = await _context.MarcaComercial.Where(mc => mc.Id == vd.IdMarcaComercial).FirstOrDefaultAsync();
                                Vacuna vacuna = await _context.Vacuna.Where(v => v.Id == vd.IdVacuna).FirstOrDefaultAsync();
                                string descripcionVacunaMarca = vacuna.Descripcion + " " + marcaComercialExistente.Descripcion;

                                VacunaDesarrolladaStockDTO vacunaDesarrolladaStockDTO = new VacunaDesarrolladaStockDTO(vd.Id, vd.IdVacuna, vd.IdMarcaComercial, descripcionVacunaMarca, lotesDTO, totalLotes, totalLotesVencido, totalLotesDisponible);
                                vacunasDesarrolladasStockDTO.Add(vacunaDesarrolladaStockDTO);
                                //----

                                totalVacunaDesarrollada += totalLotes;
                                totalVacunaDesarrolladaVencido += totalLotesVencido;
                                totalVacunaDesarrolladaDisponible += totalLotesDisponible;
                            }
                            

                            //----
                            TipoVacunaStockDTO tipoVacunaStockDTO = new TipoVacunaStockDTO(tipoVacuna.Id, tipoVacuna.Descripcion, vacunasDesarrolladasStockDTO, totalVacunaDesarrollada, totalVacunaDesarrolladaVencido, totalVacunaDesarrolladaDisponible);
                            tiposVacunasStockDTO.Add(tipoVacunaStockDTO);
                            //----
                        }
                                             
                        total += totalVacunaDesarrollada;
                        totalVencido += totalVacunaDesarrolladaVencido;
                        totalDisponible += totalVacunaDesarrolladaDisponible;
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


        // GET: api/Distribuciones/GetStockOperador?emailOperadorNacional=maria@gmail.com
        [HttpGet]
        [Route("GetStockOperador")]
        public async Task<ActionResult<IEnumerable<ResponseStockNacionalDTO>>> GetStockOperador(string emailOperadorNacional = null)
        {
            try
            {
                ResponseStockNacionalDTO responseStockNacionalDTO = new ResponseStockNacionalDTO();

                //lista vacia para los errores
                List<string> errores = new List<string>();

                errores = await VerificarCredencialesUsuarioOperadorNacional(emailOperadorNacional, errores);

                List<StockJurisdiccionDTO> listaStockJurisdiccionesDTO = new List<StockJurisdiccionDTO>();
                
                if (errores.Count == 0)
                {
                    Usuario usuario = await GetUsuario(emailOperadorNacional);

                    List<Jurisdiccion> listaJurisdicciones = await _context.Jurisdiccion.ToListAsync(); 
                    
                    foreach(Jurisdiccion jurisdiccion in listaJurisdicciones)
                    {
                        StockJurisdiccionDTO stockJurisdiccionDTO = await GetStockJurisdiccionDTO(jurisdiccion);
                        listaStockJurisdiccionesDTO.Add(stockJurisdiccionDTO);    
                    }

                    responseStockNacionalDTO.EmailOperadorNacional = emailOperadorNacional;
                    responseStockNacionalDTO.EstadoTransaccion = "Aceptada";
                    responseStockNacionalDTO.Errores = errores;
                    responseStockNacionalDTO.ExistenciaErrores = false;
                    responseStockNacionalDTO.StockJurisdicciones = listaStockJurisdiccionesDTO;
                }
                else
                {
                    //response errores
                    responseStockNacionalDTO.EmailOperadorNacional = emailOperadorNacional;
                    responseStockNacionalDTO.EstadoTransaccion = "Rechazada";
                    responseStockNacionalDTO.Errores = errores;
                    responseStockNacionalDTO.ExistenciaErrores = true;
                    responseStockNacionalDTO.StockJurisdicciones = listaStockJurisdiccionesDTO;
                }
                return Ok(responseStockNacionalDTO);
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }

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
                    errores = await VerificarCredencialesUsuarioOperadorNacional(emailOperadorNacional, errores);

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
                        Jurisdiccion juris = await GetJurisdiccion(distribucion.IdJurisdiccion);

                        DistribucionDTO distribucionDTO = new DistribucionDTO(distribucion.Id, distribucion.Codigo, distribucion.IdJurisdiccion, juris.Descripcion,
                            distribucion.IdLote, distribucion.FechaEntrega, distribucion.CantidadVacunas, distribucion.Aplicadas, distribucion.Vencidas);

                        listaDistribuciones.Add(distribucionDTO);
                    }

                    responseListaDistribucionesDTO = new ResponseListaDistribucionesDTO("Aceptada", false, errores, listaDistribuciones);
                }

                return Ok(responseListaDistribucionesDTO);
            }
            catch(Exception error)
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

                        listaDetallesEntregas = await GetListaDetallesEntregas(envio);

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

                errores = await VerificarCredencialesUsuarioOperadorNacional(model.EmailOperadorNacional, errores);

                Jurisdiccion jurisdiccionExistente = await GetJurisdiccion(model.IdJurisdiccion);
                if (jurisdiccionExistente == null)
                    errores.Add(string.Format("La jurisdicción con identificador {0} no está registrada en el sistema", model.IdJurisdiccion));

                foreach (DetalleEntregaDTO entrega in model.ListaDetallesEntregas)
                {
                    Vacuna vacunaExistente = await GetVacuna(entrega.IdVacuna);
                    if (vacunaExistente == null)
                        errores.Add(string.Format("La vacuna con identificador {0} no está registrada en el sistema", entrega.IdVacuna));

                    Lote lote = await _context.Lote.Where(l => l.Id == entrega.IdLote).FirstOrDefaultAsync();
                    if (lote == null)
                        errores.Add(string.Format("El lote con identificador {0} no está registrado en el sistema", entrega.IdLote));

                    if (entrega.IdVacunaDesarrollada != null && entrega.IdVacunaDesarrollada != 0)
                    {
                        VacunaDesarrollada vacunaDesarrolladaExistente = await GetVacunaDesarrollada(entrega.IdVacunaDesarrollada.Value);
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

                            while (await GetDistribucionExistente(codigoDistribucion) != null)
                            {
                                codigoDistribucion = randomCodigoDistribucion.Next(1, 100000000);
                            }

                            Distribucion distribucion = new Distribucion(codigoDistribucion, model.IdJurisdiccion,
                                entrega.IdLote, DateTime.Now, entrega.CantidadVacunas, 0, 0);
                                                       
                            _context.Distribucion.Add(distribucion);
                            await _context.SaveChangesAsync();

                            compra.Distribuidas += entrega.CantidadVacunas;
                            _context.Entry(compra).State = EntityState.Modified;
                            await _context.SaveChangesAsync();

                            Jurisdiccion juris = await GetJurisdiccion(distribucion.IdJurisdiccion);

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

        private async Task<List<DetalleEntregaDTO>> GetListaByListaCompras(List<Compra> listaCompras, int cantidadVacunasDemanda, EnvioVacunaDTO envio)
        {
            List<DetalleEntregaDTO> listaDetallesEntregasDTO = new List<DetalleEntregaDTO>();

            try
            {
                int cantidadTotalDistribuidasLotes = 0;

                foreach (Compra com in listaCompras)
                {
                    Lote lote = await _context.Lote.Where(l => l.Id == com.IdLote).FirstOrDefaultAsync();

                    List<Distribucion> distribucionesLote = await _context.Distribucion
                        .Where(d => d.IdLote == lote.Id).ToListAsync();

                    int cantidadTotalDistribuidas = 0;
                    int disponibles = 0;
                    int otorgadas = 0;

                    foreach (Distribucion distribucion in distribucionesLote)
                    {
                        cantidadTotalDistribuidas += distribucion.CantidadVacunas;
                    }

                    disponibles = com.CantidadVacunas - cantidadTotalDistribuidas;
                    
                    if (disponibles >= cantidadVacunasDemanda)
                    {
                        otorgadas = cantidadVacunasDemanda - cantidadTotalDistribuidasLotes;
                        cantidadTotalDistribuidasLotes += otorgadas;
                    }
                    else
                    {
                        if (cantidadTotalDistribuidasLotes + disponibles < cantidadVacunasDemanda)
                        {
                            cantidadTotalDistribuidasLotes += disponibles;
                            otorgadas = disponibles;
                        }
                        else
                        {
                            otorgadas = cantidadVacunasDemanda - cantidadTotalDistribuidasLotes;
                            if (otorgadas < 0)
                                otorgadas *= -1;
                            cantidadTotalDistribuidasLotes += otorgadas;
                        }
                    }
                                                            
                    if (otorgadas != 0)
                    {
                        Vacuna vacunaLote = await GetVacuna(envio.IdVacuna);
                        VacunaDesarrollada vacunaDesarrolladaLote = await GetVacunaDesarrollada(lote.IdVacunaDesarrollada);
                        MarcaComercial marcaComercial = await GetMarcaComercial(vacunaDesarrolladaLote.IdMarcaComercial);
                        string descripcionVacunaDesarrollada = vacunaLote.Descripcion + " " + marcaComercial.Descripcion;
                                               
                        DetalleEntregaDTO detalleEntregaDTO = new DetalleEntregaDTO(vacunaLote.Id, vacunaDesarrolladaLote.Id, 
                            vacunaLote.Descripcion, descripcionVacunaDesarrollada, otorgadas, lote.Id, lote.FechaVencimiento);

                        listaDetallesEntregasDTO.Add(detalleEntregaDTO);
                    }
                   
                    if (cantidadTotalDistribuidasLotes == cantidadVacunasDemanda)
                        break;
                }
            }
            catch
            {

            }

            return listaDetallesEntregasDTO;
        }

        private async Task<List<DetalleEntregaDTO>> GetListaDetallesEntregas(EnvioVacunaDTO envio)
        {
            List<DetalleEntregaDTO> listaDetallesEntregasDTO = new List<DetalleEntregaDTO>();

            try
            {
                List<Compra> compras = new List<Compra>();

                if (envio.IdVacunaDesarrollada == null || envio.IdVacunaDesarrollada == 0)
                {
                    compras = await _context.Compra
                        .Where(l => l.IdLoteNavigation.Disponible == true
                            && l.IdLoteNavigation.IdVacunaDesarrolladaNavigation.IdVacuna == envio.IdVacuna
                            && l.IdLoteNavigation.FechaVencimiento > DateTime.Now)
                        .OrderBy(l => l.IdLoteNavigation.FechaVencimiento)
                        .ToListAsync();
                }
                else
                {
                    compras = await _context.Compra
                        .Where(l => l.IdLoteNavigation.Disponible == true
                            && l.IdLoteNavigation.IdVacunaDesarrollada == envio.IdVacunaDesarrollada
                            && l.IdLoteNavigation.IdVacunaDesarrolladaNavigation.IdVacuna == envio.IdVacuna
                            && l.IdLoteNavigation.FechaVencimiento > DateTime.Now)
                        .OrderBy(l => l.IdLoteNavigation.FechaVencimiento)
                        .ToListAsync();
                }

                listaDetallesEntregasDTO = await GetListaByListaCompras(compras, envio.CantidadVacunas, envio);

                if (envio.IdVacunaDesarrollada != null && envio.IdVacunaDesarrollada != 0)
                {
                    int cantidadEntrega = 0;

                    foreach (DetalleEntregaDTO detalle in listaDetallesEntregasDTO)
                    {
                        cantidadEntrega += detalle.CantidadVacunas;
                    }
                    if (cantidadEntrega < envio.CantidadVacunas)
                    {
                        List<Compra> comprasAdicional = await _context.Compra
                            .Where(l => l.IdLoteNavigation.Disponible == true
                                && l.IdLoteNavigation.IdVacunaDesarrolladaNavigation.IdVacuna == envio.IdVacuna
                                && l.IdLoteNavigation.FechaVencimiento > DateTime.Now)
                            .OrderBy(l => l.IdLoteNavigation.FechaVencimiento)
                            .ToListAsync();

                        List<DetalleEntregaDTO> listaDetallesAdicional = await GetListaByListaCompras(comprasAdicional, (envio.CantidadVacunas - cantidadEntrega), envio);

                        foreach (DetalleEntregaDTO detDTO in listaDetallesAdicional)
                        {
                            if(!listaDetallesEntregasDTO.Any(x => x.IdLote == detDTO.IdLote))
                                listaDetallesEntregasDTO.Add(detDTO);
                        }
                    }   
                }
            }
            catch
            {

            }

            return listaDetallesEntregasDTO;
        }

        private async Task<Distribucion> GetDistribucionExistente(int codigoDistribucion)
        {
            Distribucion distribucionExistente = null;

            try
            {
                distribucionExistente = await _context.Distribucion
                    .Where(d => d.Codigo == codigoDistribucion).FirstOrDefaultAsync();
            }
            catch
            {

            }

            return distribucionExistente;
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

        private async Task<StockJurisdiccionDTO> GetStockJurisdiccionDTO(Jurisdiccion jurisdiccion)
        {
            //me quedo con la jurisdiccion del usuario
            //Jurisdiccion jurisdiccionUsuario = await _context.Jurisdiccion.Where(jurisdiccion => jurisdiccion.Id == usuario.IdJurisdiccion).FirstOrDefaultAsync();

            //me quedo con las distribuciones de la jurisdicción del usuario
            List<Distribucion> listaDistribuciones = await _context.Distribucion
                .Where(d => d.IdJurisdiccion == jurisdiccion.Id)
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
                            && d.IdJurisdiccion == jurisdiccion.Id
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
                    totalVencido += totalVacunaDesarrolladaVencido;
                    totalDisponible += totalVacunaDesarrolladaDisponible;

                    //----
                    TipoVacunaStockDTO tipoVacunaStockDTO = new TipoVacunaStockDTO(tipoVacuna.Id, tipoVacuna.Descripcion, vacunasDesarrolladasStockDTO, totalVacunaDesarrollada, totalVacunaDesarrolladaVencido, totalVacunaDesarrolladaDisponible);
                    tiposVacunasStockDTO.Add(tipoVacunaStockDTO);
                    //----
                }
            }
            //----
            StockDTO stockDTO = new StockDTO(tiposVacunasStockDTO, total, totalVencido, totalDisponible);
            StockJurisdiccionDTO stockJurisdiccionDTO = new StockJurisdiccionDTO(jurisdiccion.Id, jurisdiccion.Descripcion, stockDTO);
            //----

            return stockJurisdiccionDTO;
        }
    }
}
