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
    public class VacunasController : ControllerBase
    {
        private readonly VacunasContext _context;

        public VacunasController(VacunasContext context)
        {
            _context = context;
        }

        // GET: api/Vacunas/EjecutarCron?emailOperadorNacional=maria@gmail.com
        [HttpGet]
        [Route("EjecutarCron")]
        public async Task<ActionResult<ResponseCronDTO>> EjecutarCron(string emailOperadorNacional)
        {
            List<CompraCronDTO> comprasRecibidasHoy = new List<CompraCronDTO>();
            List<LoteCronDTO> lotesVencidosHoy = new List<LoteCronDTO>();
            ResponseCronDTO responseCronDTO = null;

            try
            {
                Usuario usu = await GetUsuario(emailOperadorNacional);

                if (usu != null && await (TieneRolOperadorNacional(usu)))
                {
                    EstadoCompra estadoCompraRecibida = await _context.EstadoCompra.Where(e => e.Descripcion == "Recibida").FirstOrDefaultAsync();
                    EstadoCompra estadoCompraNoRecibida = await _context.EstadoCompra.Where(e => e.Descripcion == "No Recibida").FirstOrDefaultAsync();
                    DateTime fechaHoy = DateTime.Now;
                    int anioHoy = fechaHoy.Year;
                    int mesHoy = fechaHoy.Month;
                    int diaHoy = fechaHoy.Day;

                    //Recepciones
                    List<Compra> comprasNoRecibidas = await _context.Compra
                       .Where(l => l.IdLoteNavigation.Disponible == false
                           && l.Distribuidas == 0
                           && l.IdEstadoCompra == estadoCompraNoRecibida.Id
                           && l.IdLoteNavigation.FechaVencimiento > DateTime.Now)
                       .OrderBy(l => l.IdLoteNavigation.FechaVencimiento)
                       .ToListAsync();

                    foreach (Compra compra in comprasNoRecibidas)
                    {
                        int anioFechaEntregaCompra = compra.FechaEntrega.Year;
                        int mesFechaEntregaCompra = compra.FechaEntrega.Month;
                        int diaFechaEntregaCompra = compra.FechaEntrega.Day;

                        if (anioHoy == anioFechaEntregaCompra && mesHoy == mesFechaEntregaCompra && diaHoy == diaFechaEntregaCompra)
                        {
                            Lote loteCompra = await _context.Lote.Where(l => l.Id == compra.IdLote).FirstOrDefaultAsync();

                            compra.IdEstadoCompra = estadoCompraRecibida.Id;
                            _context.Entry(compra).State = EntityState.Modified;
                            loteCompra.Disponible = true;
                            _context.Entry(loteCompra).State = EntityState.Modified;
                            await _context.SaveChangesAsync();

                            CompraCronDTO compraCronDTO = new CompraCronDTO(compra.Id, loteCompra.Id, compra.FechaCompra,
                                compra.FechaEntrega, compra.CantidadVacunas, compra.IdEstadoCompra, estadoCompraRecibida.Descripcion);

                            comprasRecibidasHoy.Add(compraCronDTO);
                        }
                    }

                    //Vencimientos
                    List<Lote> lotes = await _context.Lote.ToListAsync();

                    foreach (Lote lote in lotes)
                    {
                        int anioFechaVencimientoLote = lote.FechaVencimiento.Year;
                        int mesFechaVencimientoLote = lote.FechaVencimiento.Month;
                        int diaFechaVencimientoLote = lote.FechaVencimiento.Day;

                        if (anioHoy == anioFechaVencimientoLote && mesHoy == mesFechaVencimientoLote && diaHoy == diaFechaVencimientoLote)
                        {
                            Compra compra = await _context.Compra.Where(c => c.IdLote == lote.Id).FirstOrDefaultAsync();

                            lote.Disponible = false;
                            _context.Entry(lote).State = EntityState.Modified;

                            List<Distribucion> distribucionesLote = await _context.Distribucion
                                .Where(d => d.IdLote == lote.Id).ToListAsync();

                            foreach (Distribucion distribucion in distribucionesLote)
                            {
                                distribucion.Vencidas = distribucion.CantidadVacunas - distribucion.Aplicadas;
                                _context.Entry(distribucion).State = EntityState.Modified;
                            }

                            compra.Vencidas = compra.CantidadVacunas - compra.Distribuidas;
                            _context.Entry(compra).State = EntityState.Modified;

                            await _context.SaveChangesAsync();

                            VacunaDesarrollada vd = await _context.VacunaDesarrollada.Where(v => v.Id == lote.IdVacunaDesarrollada).FirstOrDefaultAsync();
                            Vacuna vac = await _context.Vacuna.Where(v => v.Id == vd.IdVacuna).FirstOrDefaultAsync();

                            LoteCronDTO loteCronDTO = new LoteCronDTO(lote.Id, lote.FechaVencimiento, vac.Id, vac.Descripcion, compra.CantidadVacunas);

                            lotesVencidosHoy.Add(loteCronDTO);
                        }
                    }
                }

                responseCronDTO = new ResponseCronDTO(comprasRecibidasHoy, lotesVencidosHoy);
            }
            catch
            {

            }

            return Ok(responseCronDTO);
        }

        // GET: api/Vacunas/GetDescripcionesVacunasCalendario
        [HttpGet]
        [Route("GetDescripcionesVacunasCalendario")]
        public ActionResult<List<DescripcionVacunaCalendarioAnualDTO>> GetDescripcionesVacunasCalendario()
        {
            List<DescripcionVacunaCalendarioAnualDTO> descripcionesVacunasCalendario = new List<DescripcionVacunaCalendarioAnualDTO>();

            try
            {
                descripcionesVacunasCalendario.Add(new DescripcionVacunaCalendarioAnualDTO(1, "Hepatitis B (HB)"));
                descripcionesVacunasCalendario.Add(new DescripcionVacunaCalendarioAnualDTO(2, "BCG"));
                descripcionesVacunasCalendario.Add(new DescripcionVacunaCalendarioAnualDTO(3, "Rotavirus"));
                descripcionesVacunasCalendario.Add(new DescripcionVacunaCalendarioAnualDTO(4, "Neumococo Conjugada"));
                descripcionesVacunasCalendario.Add(new DescripcionVacunaCalendarioAnualDTO(5, "Quíntuple Pentavalente"));
                descripcionesVacunasCalendario.Add(new DescripcionVacunaCalendarioAnualDTO(6, "Salk IPV"));
                descripcionesVacunasCalendario.Add(new DescripcionVacunaCalendarioAnualDTO(7, "Meningocócica Conjugada"));
                descripcionesVacunasCalendario.Add(new DescripcionVacunaCalendarioAnualDTO(8, "Triple Viral (SRP)"));
                descripcionesVacunasCalendario.Add(new DescripcionVacunaCalendarioAnualDTO(9, "Hepatitis A (HA)"));
                descripcionesVacunasCalendario.Add(new DescripcionVacunaCalendarioAnualDTO(10, "Varicela"));
            }
            catch
            {

            }

            return descripcionesVacunasCalendario;
        }

        // GET: api/Vacunas/GetDescripcionesVacunasAnuales
        [HttpGet]
        [Route("GetDescripcionesVacunasAnuales")]
        public ActionResult<List<DescripcionVacunaCalendarioAnualDTO>> GetDescripcionesVacunasAnuales()
        {
            List<DescripcionVacunaCalendarioAnualDTO> descripcionesVacunasAnuales = new List<DescripcionVacunaCalendarioAnualDTO>();

            try
            {
                descripcionesVacunasAnuales = GetVacunasAnuales();
            }
            catch
            {

            }

            return descripcionesVacunasAnuales;
        }

        // GET: api/Vacunas/GetAll?emailOperadorNacional=juan@gmail.com&idTipoVacuna=2
        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult<ResponseListaVacunasDTO>> GetAll(string emailOperadorNacional = null, int idTipoVacuna = 0)
        {
            try
            {
                ResponseListaVacunasDTO responseListaVacunasDTO = null;
                List<string> errores = new List<string>();
                List<VacunaDTO> listaVacunasDTO = new List<VacunaDTO>();
                List<Vacuna> vacunas = new List<Vacuna>();

                if (idTipoVacuna != 0)
                {
                    List<List<string>> listaVerificacionTipoVacuna = await VerificarTipoVacuna(errores, idTipoVacuna);
                    errores = listaVerificacionTipoVacuna[0];
                }
                if (emailOperadorNacional == null)
                {
                    errores.Add(string.Format("El email operador nacional es obligatorio"));
                }
                else
                {
                    errores = await VerificarCredencialesUsuarioOperadorNacionalVacunador(emailOperadorNacional, errores);
                }

                if (errores.Count > 0)
                {
                    responseListaVacunasDTO = new ResponseListaVacunasDTO("Rechazada", true, errores, emailOperadorNacional, listaVacunasDTO);
                }
                else
                {
                    if (idTipoVacuna == 0)
                    {
                        vacunas = await _context.Vacuna.ToListAsync();
                    }
                    else
                    {
                        vacunas = await _context.Vacuna.Where(vac => vac.IdTipoVacuna == idTipoVacuna).ToListAsync();
                    }

                    foreach (Vacuna vac in vacunas)
                    {
                        TipoVacuna tipoVacuna = await GetTipoVacuna(vac.IdTipoVacuna);
                        Pandemia pandemia = await GetPandemia(vac.IdPandemia);
                        List<EntidadVacunaDosis> listaEntidadVacunaDosis = await _context.EntidadVacunaDosis.Where(evd => evd.IdVacuna == vac.Id).ToListAsync();
                        List<DosisDTO> dosisDTO = new List<DosisDTO>();
                        
                        foreach (EntidadVacunaDosis evd in listaEntidadVacunaDosis)
                        {
                            Dosis dosis = await _context.Dosis.Where(d => d.Id == evd.IdDosis).FirstOrDefaultAsync();
                            List<EntidadDosisRegla> listaEntidadDosisRegla = await _context.EntidadDosisRegla.Where(edr => edr.IdDosis == evd.IdDosis).ToListAsync();
                            List<ReglaDTO> reglasDTO = new List<ReglaDTO>();
                            foreach (EntidadDosisRegla edr in listaEntidadDosisRegla)
                            {
                                Regla reg = await _context.Regla.Where(r => r.Id == edr.IdRegla).FirstOrDefaultAsync();
                                if (reg != null)
                                {
                                    ReglaDTO reglaDTO = new ReglaDTO(reg.Id, reg.Descripcion, reg.MesesVacunacion, 
                                        reg.LapsoMinimoDias, reg.LapsoMaximoDias, reg.Otros, reg.Embarazada, reg.PersonalSalud);
                                    
                                    reglasDTO.Add(reglaDTO);
                                }
                            }

                            DosisDTO d = new DosisDTO(dosis.Id, evd.Orden.Value, dosis.Descripcion, reglasDTO);
                            dosisDTO.Add(d);
                        }

                        VacunaDTO vacunaDTO = new VacunaDTO(vac.Id, vac.Descripcion, vac.IdTipoVacuna, tipoVacuna.Descripcion,
                            vac.IdPandemia, pandemia.Descripcion, vac.CantidadDosis, dosisDTO);

                        listaVacunasDTO.Add(vacunaDTO);
                    }

                    responseListaVacunasDTO = new ResponseListaVacunasDTO("Aceptada", false, errores, emailOperadorNacional, listaVacunasDTO);
                }

                return Ok(responseListaVacunasDTO);
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }

        // GET: api/Vacunas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Vacuna>> GetVacuna(int id)
        {
            var vacuna = await _context.Vacuna.FindAsync(id);

            if (vacuna == null)
            {
                return NotFound();
            }

            return vacuna;
        }

        // PUT: api/Vacunas/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVacuna(int id, Vacuna vacuna)
        {
            if (id != vacuna.Id)
            {
                return BadRequest();
            }

            _context.Entry(vacuna).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VacunaExists(id))
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

        // POST: api/Vacunas/CrearVacuna
        [HttpPost]
        [Route("CrearVacuna")]
        public async Task<ActionResult<ResponseVacunaDTO>> CrearVacuna([FromBody] RequestVacunaDTO model)
        {
            try
            {
                ResponseVacunaDTO responseVacunaDTO = new ResponseVacunaDTO();
                List<string> errores = new List<string>();
                List<List<string>> listaVerificacionTipoVacuna = await VerificarTipoVacuna(errores, model.IdTipoVacuna);
                List<VacunaCalendarioAnualPandemiaDTO> vacunasCalendario = new List<VacunaCalendarioAnualPandemiaDTO>();
                errores = listaVerificacionTipoVacuna[0];

                if (listaVerificacionTipoVacuna[1][0] == "Vacuna de Calendario")
                {
                    vacunasCalendario = GetVacunasCalendario();
                    if (!vacunasCalendario.Any(v => v.Descripcion == model.Descripcion))
                        errores.Add(string.Format("La descripción {0} no corresponde a una vacuna de calendario", model.Descripcion));
                }

                if (model.IdPandemia > 1)
                {
                    List<List<string>> listaVerificacionPandemia = await VerificarPandemia(errores, model.IdPandemia);
                    errores = listaVerificacionPandemia[0];

                    if ((listaVerificacionTipoVacuna[1][0] == "Vacuna de Calendario" || listaVerificacionTipoVacuna[1][0] == "Vacuna Anual") && (listaVerificacionPandemia[1][0] != null))
                    {
                        errores.Add(string.Format("La {0} no puede ser asociada a la pandemia {1}", listaVerificacionTipoVacuna[1][0], listaVerificacionPandemia[1][0]));
                    }
                    if (listaVerificacionTipoVacuna[1][0] == "Vacuna Anual")
                    {
                        List<DescripcionVacunaCalendarioAnualDTO> vacunasAnuales = GetVacunasAnuales();
                        DescripcionVacunaCalendarioAnualDTO descripcion = vacunasAnuales.Where(x => x.Descripcion == model.Descripcion).FirstOrDefault();

                        if (descripcion == null)
                            errores.Add(string.Format("La descripción {0} no corresponde a una vacuna anual", model.Descripcion));
                    }
                    if (listaVerificacionPandemia[1][0] != model.Descripcion)
                    {
                        errores.Add(string.Format("La descripción de vacuna {0} no coincide con la pandemia {1}", model.Descripcion, listaVerificacionPandemia[1][0]));
                    }
                }
                else
                {
                    Pandemia noPandemia = await _context.Pandemia.Where(p => p.Descripcion == "No Pandemia").FirstOrDefaultAsync();
                    model.IdPandemia = noPandemia.Id;

                    if (listaVerificacionTipoVacuna[1][0] == "Vacuna de Pandemia")
                        errores.Add("El tipo de vacuna no corresponde");
                }

                errores = await VerificarCredencialesUsuarioOperadorNacional(model.EmailOperadorNacional, errores);

                Vacuna vacunaExistente = await GetVacuna(model.Descripcion);
                if (vacunaExistente != null)
                    errores.Add(string.Format("La descripción de vacuna {0} está registrada en el sistema", model.Descripcion));

                if (errores.Count > 0)
                    responseVacunaDTO = new ResponseVacunaDTO("Rechazada", true, errores, model.EmailOperadorNacional, new VacunaDTO(0, model.Descripcion, model.IdTipoVacuna, null, model.IdPandemia, null, 0, null));
                else
                {
                    Vacuna vacuna = new Vacuna();
                    vacuna.Descripcion = model.Descripcion;
                    vacuna.IdPandemia = model.IdPandemia;
                    vacuna.IdTipoVacuna = model.IdTipoVacuna;
                    vacuna.CantidadDosis = 0;

                    _context.Vacuna.Add(vacuna);
                    await _context.SaveChangesAsync();

                    if (listaVerificacionTipoVacuna[1][0] == "Vacuna de Calendario")
                    {
                        VacunaCalendarioAnualPandemiaDTO vacunaCalendarioDTO = null;

                        foreach (VacunaCalendarioAnualPandemiaDTO vac in vacunasCalendario)
                        {
                            if (vac.Descripcion == model.Descripcion)
                                vacunaCalendarioDTO = vac;
                        }
                        
                        vacunaCalendarioDTO.Id = vacuna.Id;
                        VacunaCalendarioAnualPandemiaDTO vacCal = await RespaldarDosisReglasByVacuna (vacunaCalendarioDTO);
                        if (vacCal != null)
                        {
                            vacuna.CantidadDosis = vacCal.Dosis.Count;
                            responseVacunaDTO = new ResponseVacunaDTO("Aceptada", false, errores, model.EmailOperadorNacional, 
                                new VacunaDTO(vacCal.Id, vacCal.Descripcion, model.IdTipoVacuna, listaVerificacionTipoVacuna[1][0], model.IdPandemia, null, vacCal.Dosis.Count, vacCal.Dosis));
                        }
                        else
                        {
                            errores.Add("Error de conexión con la base de datos");
                            responseVacunaDTO = new ResponseVacunaDTO("Rechazada", true, errores, model.EmailOperadorNacional, new VacunaDTO(0, model.Descripcion, model.IdTipoVacuna, null, model.IdPandemia, null, 0, null));
                        }
                    }
                    if (listaVerificacionTipoVacuna[1][0] == "Vacuna Anual")
                    {
                        List<ReglaDTO> listaReglas = new List<ReglaDTO>();
                        ReglaDTO reglaDTO = new ReglaDTO(0, model.Descripcion + " - Aplicar en meses de vacunación anual", "4,5,6,7,8,9", 0, 0, null, true, true);
                        listaReglas.Add(reglaDTO);

                        List<DosisDTO> listaDosis = new List<DosisDTO>();
                        DosisDTO dosisDTO = new DosisDTO(0, 0, "Dosis Anual", listaReglas);
                        listaDosis.Add(dosisDTO);

                        VacunaCalendarioAnualPandemiaDTO vacunaAnual = new VacunaCalendarioAnualPandemiaDTO(vacuna.Id, model.Descripcion, listaDosis);
                        VacunaCalendarioAnualPandemiaDTO vacAnu = await RespaldarDosisReglasByVacuna(vacunaAnual);
                        if (vacAnu != null)
                        {
                            vacuna.CantidadDosis = vacAnu.Dosis.Count;
                            responseVacunaDTO = new ResponseVacunaDTO("Aceptada", false, errores, model.EmailOperadorNacional,
                                new VacunaDTO(vacAnu.Id, vacAnu.Descripcion, model.IdTipoVacuna, listaVerificacionTipoVacuna[1][0], model.IdPandemia, null, vacAnu.Dosis.Count, vacAnu.Dosis));
                        }
                        else
                        {
                            errores.Add("Error de conexión con la base de datos");
                            responseVacunaDTO = new ResponseVacunaDTO("Rechazada", true, errores, model.EmailOperadorNacional, new VacunaDTO(0, model.Descripcion, model.IdTipoVacuna, null, model.IdPandemia, null, 0, null));
                        }
                    }
                    if (listaVerificacionTipoVacuna[1][0] == "Vacuna de Pandemia")
                    {
                        Pandemia pandemia = await _context.Pandemia.Where(p => p.Descripcion == model.Descripcion).FirstOrDefaultAsync();
                        if (pandemia != null)
                        {
                            int cantidadDosis = CalcularCantidadDosisPandemia(pandemia);
                            List<DosisDTO> listaDosis = new List<DosisDTO>();
                            List<ReglaDTO> listaReglas = new List<ReglaDTO>();
                            ReglaDTO reglaDTO = new ReglaDTO(0, string.Format("{0} - Aplicar dosis cada {1} días", model.Descripcion, pandemia.IntervaloMinimoDias), null, pandemia.IntervaloMinimoDias, 0, "Anterior", true, true);
                            listaReglas.Add(reglaDTO);
                            
                            for (int i=0; i < cantidadDosis; i++)
                            {
                                DosisDTO dosisDTO = new DosisDTO(0, i, "Dosis " + (i + 1), listaReglas);
                                listaDosis.Add(dosisDTO);
                            }

                            VacunaCalendarioAnualPandemiaDTO vacunaPandemia = new VacunaCalendarioAnualPandemiaDTO(vacuna.Id, model.Descripcion, listaDosis);
                            VacunaCalendarioAnualPandemiaDTO vacPan = await RespaldarDosisReglasByVacuna(vacunaPandemia);
                            if (vacPan != null)
                            {
                                vacuna.CantidadDosis = vacPan.Dosis.Count;
                                responseVacunaDTO = new ResponseVacunaDTO("Aceptada", false, errores, model.EmailOperadorNacional,
                                    new VacunaDTO(vacPan.Id, vacPan.Descripcion, model.IdTipoVacuna, listaVerificacionTipoVacuna[1][0], model.IdPandemia, pandemia.Descripcion, vacPan.Dosis.Count, vacPan.Dosis));
                            }
                            else
                            {
                                errores.Add("Error de conexión con la base de datos");
                                responseVacunaDTO = new ResponseVacunaDTO("Rechazada", true, errores, model.EmailOperadorNacional, new VacunaDTO(0, model.Descripcion, model.IdTipoVacuna, null, model.IdPandemia, null, 0, null));
                            }
                        }
                    }

                    _context.Entry(vacuna).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }

                return Ok(responseVacunaDTO);
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }

        // DELETE: api/Vacunas/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Vacuna>> DeleteVacuna(int id)
        {
            var vacuna = await _context.Vacuna.FindAsync(id);
            if (vacuna == null)
            {
                return NotFound();
            }

            _context.Vacuna.Remove(vacuna);
            await _context.SaveChangesAsync();

            return vacuna;
        }



        //Métodos privados de ayuda
        private int CalcularCantidadDosisPandemia(Pandemia pandemia)
        {
            int cantidadDosis = 0;
            DateTime fechaInicio = pandemia.FechaInicio;

            try
            {
                fechaInicio = fechaInicio.AddDays(pandemia.IntervaloMinimoDias);

                while (fechaInicio <= pandemia.FechaFin)
                {
                    cantidadDosis++;
                    fechaInicio = fechaInicio.AddDays(pandemia.IntervaloMinimoDias);
                }
            }
            catch
            {

            }

            return cantidadDosis;
        }

        private bool VacunaExists(int id)
        {
            return _context.Vacuna.Any(e => e.Id == id);
        }

        private async Task<TipoVacuna> GetTipoVacuna(int idTipoVacuna)
        {
            TipoVacuna tipoVacunaExistente = null;

            try
            {
                tipoVacunaExistente = await _context.TipoVacuna
                    .Where(tv => tv.Id == idTipoVacuna).FirstOrDefaultAsync();
            }
            catch
            {

            }

            return tipoVacunaExistente;
        }

        private async Task<Pandemia> GetPandemia(int idPandemia)
        {
            Pandemia pandemiaExistente = null;

            try
            {
                pandemiaExistente = await _context.Pandemia
                    .Where(p => p.Id == idPandemia).FirstOrDefaultAsync();
            }
            catch
            {

            }

            return pandemiaExistente;
        }

        private async Task<List<List<string>>> VerificarPandemia(List<string> errores, int idPandemia)
        {
            List<List<string>> erroresConcatDescripciones = new List<List<string>>();

            try
            {
                List<string> descripciones = new List<string>();
                Pandemia pandemiaExistente = await GetPandemia(idPandemia);

                if (pandemiaExistente == null)
                {
                    errores.Add(string.Format("La pandemia con identificador {0} no está registrada en el sistema", idPandemia));
                    descripciones.Add(null);
                }
                else
                {
                    descripciones.Add(pandemiaExistente.Descripcion);
                }

                erroresConcatDescripciones.Add(errores);
                erroresConcatDescripciones.Add(descripciones);
            }
            catch
            {

            }

            return erroresConcatDescripciones;
        }

        private async Task<List<List<string>>> VerificarTipoVacuna(List<string> errores, int idTipoVacuna)
        {
            List<List<string>> erroresConcatDescripciones = new List<List<string>>();

            try
            {
                List<string> descripciones = new List<string>();
                TipoVacuna tipoVacunaExistente = await GetTipoVacuna(idTipoVacuna);

                if (tipoVacunaExistente == null)
                {
                    errores.Add(string.Format("El tipo de vacuna con identificador {0} no está registrado en el sistema", idTipoVacuna));
                    descripciones.Add(null);
                }
                else
                {
                    descripciones.Add(tipoVacunaExistente.Descripcion);
                }

                erroresConcatDescripciones.Add(errores);
                erroresConcatDescripciones.Add(descripciones);
            }
            catch
            {

            }

            return erroresConcatDescripciones;
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

        private async Task<Vacuna> GetVacuna(string descripcion)
        {
            Vacuna vacunaExistente = null;

            try
            {
                vacunaExistente = await _context.Vacuna
                    .Where(vac => vac.Descripcion == descripcion).FirstOrDefaultAsync();
            }
            catch
            {

            }

            return vacunaExistente;
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

        private async Task<List<string>> VerificarCredencialesUsuarioOperadorNacionalVacunador(string emailOperadorNacional, List<string> errores)
        {
            try
            {
                Usuario usuarioSolicitante = await _context.Usuario.Where(u => u.Email == emailOperadorNacional).FirstOrDefaultAsync();
                if (usuarioSolicitante == null)
                    errores.Add(string.Format("El usuario {0} no está registrado en el sistema", emailOperadorNacional));
                else
                {
                    Rol rol = await _context.Rol
                        .Where(r => r.Id == usuarioSolicitante.IdRol
                            && (r.Descripcion == "Operador Nacional" || r.Descripcion == "Vacunador"))
                        .FirstOrDefaultAsync();
                    if (rol == null)
                        errores.Add(string.Format("El usuario {0} no tiene rol operador nacional o rol vacunador", emailOperadorNacional));
                }
            }
            catch
            {

            }

            return errores;
        }

        private List<DosisDTO> ArmarListaDosisDTO(string descripcionVacuna)
        {
            List<DosisDTO> listaDosis = new List<DosisDTO>();
            VacunaService vacunaService = new VacunaService();

            try
            {
                switch (descripcionVacuna)
                {
                    case "Hepatitis B (HB)":
                        listaDosis = vacunaService.ArmarListaDosisDTOHepatitisBHB();
                        break;
                    case "BCG":
                        listaDosis = vacunaService.ArmarListaDosisDTOBCG();
                        break;
                    case "Rotavirus":
                        listaDosis = vacunaService.ArmarListaDosisDTORotavirus();
                        break;
                    case "Neumococo Conjugada":
                        listaDosis = vacunaService.ArmarListaDosisNeumococoConjugada();
                        break;
                    case "Quíntuple Pentavalente":
                        listaDosis = vacunaService.ArmarListaDosisQuintuplePentavalente();
                        break;
                    case "Salk IPV":
                        listaDosis = vacunaService.ArmarListaDosisSalkIPV();
                        break;
                    case "Meningocócica Conjugada":
                        listaDosis = vacunaService.ArmarListaDosisMeningococicaConjugada();
                        break;
                    case "Triple Viral (SRP)":
                        listaDosis = vacunaService.ArmarListaDosisTripleViralSRP();
                        break;
                    case "Hepatitis A (HA)":
                        listaDosis = vacunaService.ArmarListaDosisHepatitisAHA();
                        break;
                    case "Varicela":
                        listaDosis = vacunaService.ArmarListaDosisVaricela();
                        break;
                    default:
                        break;
                }
            }
            catch
            { 
            
            }

            return listaDosis;
        }

        private List<VacunaCalendarioAnualPandemiaDTO> GetVacunasCalendario()
        {
            List<VacunaCalendarioAnualPandemiaDTO> vacunasCalendario = null;

            try
            {
                vacunasCalendario = new List<VacunaCalendarioAnualPandemiaDTO>()
                {
                    new VacunaCalendarioAnualPandemiaDTO(1, "Hepatitis B (HB)", ArmarListaDosisDTO("Hepatitis B (HB)")),
                    new VacunaCalendarioAnualPandemiaDTO(2, "BCG", ArmarListaDosisDTO("BCG")),
                    new VacunaCalendarioAnualPandemiaDTO(3, "Rotavirus", ArmarListaDosisDTO("Rotavirus")),
                    new VacunaCalendarioAnualPandemiaDTO(4, "Neumococo Conjugada", ArmarListaDosisDTO("Neumococo Conjugada")),
                    new VacunaCalendarioAnualPandemiaDTO(5, "Quíntuple Pentavalente", ArmarListaDosisDTO("Quíntuple Pentavalente")),
                    new VacunaCalendarioAnualPandemiaDTO(6, "Salk IPV", ArmarListaDosisDTO("Salk IPV")),
                    new VacunaCalendarioAnualPandemiaDTO(7, "Meningocócica Conjugada", ArmarListaDosisDTO("Meningocócica Conjugada")),
                    new VacunaCalendarioAnualPandemiaDTO(8, "Triple Viral (SRP)", ArmarListaDosisDTO("Triple Viral (SRP)")),
                    new VacunaCalendarioAnualPandemiaDTO(9, "Hepatitis A (HA)", ArmarListaDosisDTO("Hepatitis A (HA)")),
                    new VacunaCalendarioAnualPandemiaDTO(10, "Varicela", ArmarListaDosisDTO("Varicela")),
                    //new VacunaCalendarioDTO(12, "Triple Bacteriana (DTP)"),
                    //new VacunaCalendarioDTO(13, "Triple Bacteriana Acelular (DTPA)"),
                    //new VacunaCalendarioDTO(14, "VPH"),
                    //new VacunaCalendarioDTO(15, "Doble Bacteriana (DT)"),
                    //new VacunaCalendarioDTO(16, "Doble Viral (SR)"),
                    //new VacunaCalendarioDTO(17, "Fiebre Amarilla (FA)"),
                    //new VacunaCalendarioDTO(18, "Fiebre Hemorrágica Argentina (FHA)")
                };
            }
            catch
            {

            }

            return vacunasCalendario;
        }

        private async Task<VacunaCalendarioAnualPandemiaDTO> RespaldarDosisReglasByVacuna(VacunaCalendarioAnualPandemiaDTO vacunaCalendario)
        {
            try
            {
                foreach (DosisDTO dosisDTO in vacunaCalendario.Dosis)
                {
                    Dosis dosis = new Dosis(0, dosisDTO.Descripcion);
                    _context.Dosis.Add(dosis);
                    await _context.SaveChangesAsync();
                    EntidadVacunaDosis entidadVacunaDosis = new EntidadVacunaDosis(vacunaCalendario.Id, dosis.Id, dosisDTO.Orden);
                    _context.EntidadVacunaDosis.Add(entidadVacunaDosis);
                    await _context.SaveChangesAsync();
                    dosisDTO.Id = dosis.Id;

                    foreach (ReglaDTO reglaDTO in dosisDTO.Reglas)
                    {
                        Regla reglaExistente = await _context.Regla.Where(r => r.Descripcion == reglaDTO.Descripcion).FirstOrDefaultAsync();
                        int idRegla = 0;

                        if (reglaExistente == null)
                        {
                            Regla regla = new Regla(reglaDTO.Descripcion, reglaDTO.MesesVacunacion, reglaDTO.LapsoMinimoDias, reglaDTO.LapsoMaximoDias, reglaDTO.Otros, reglaDTO.Embarazada, reglaDTO.PersonalSalud);
                            _context.Regla.Add(regla);
                            await _context.SaveChangesAsync();
                            idRegla = regla.Id;
                        }
                        else
                            idRegla = reglaExistente.Id;

                        reglaDTO.Id = idRegla;

                        EntidadDosisRegla entidadDosisRegla = new EntidadDosisRegla(dosis.Id, idRegla);
                        _context.EntidadDosisRegla.Add(entidadDosisRegla);
                        await _context.SaveChangesAsync();
                    }
                }

                return vacunaCalendario;
            }
            catch 
            { 
            
            }

            return null;
        }

        private List<DescripcionVacunaCalendarioAnualDTO> GetVacunasAnuales()
        {
            List<DescripcionVacunaCalendarioAnualDTO> descripcionesVacunasAnuales = new List<DescripcionVacunaCalendarioAnualDTO>();

            try
            {
                descripcionesVacunasAnuales.Add(new DescripcionVacunaCalendarioAnualDTO(1, "Antigripal"));
                descripcionesVacunasAnuales.Add(new DescripcionVacunaCalendarioAnualDTO(2, "Neumonía"));
                descripcionesVacunasAnuales.Add(new DescripcionVacunaCalendarioAnualDTO(3, "Sarampión"));
                descripcionesVacunasAnuales.Add(new DescripcionVacunaCalendarioAnualDTO(4, "Tos Ferina"));
            }
            catch
            {

            }

            return descripcionesVacunasAnuales;
        }
    }
}
