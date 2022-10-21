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

        // GET: api/Vacunas/GetDescripcionesVacunasCalendario
        [HttpGet]
        [Route("GetDescripcionesVacunasCalendario")]
        public ActionResult<List<DescripcionVacunaCalendarioDTO>> GetDescripcionesVacunasCalendario()
        {
            List<DescripcionVacunaCalendarioDTO> descripcionesVacunasCalendario = new List<DescripcionVacunaCalendarioDTO>();

            try
            {
                descripcionesVacunasCalendario.Add(new DescripcionVacunaCalendarioDTO(1, "Hepatitis B (HB)"));
                descripcionesVacunasCalendario.Add(new DescripcionVacunaCalendarioDTO(2, "BCG"));
                descripcionesVacunasCalendario.Add(new DescripcionVacunaCalendarioDTO(3, "Rotavirus"));
            }
            catch
            {

            }

            return descripcionesVacunasCalendario;
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
                        errores.Add("Se debe especificar un tipo de vacuna distinto al de pandemia");
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
                        ReglaDTO reglaDTO = new ReglaDTO(0, "Aplicar en meses de vacunación anual", "4,5,6,7,8,9", 0, 0, null, true, true);
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
                            ReglaDTO reglaDTO = new ReglaDTO(0, string.Format("Aplicar dosis cada {0} días", pandemia.IntervaloMinimoDias), null, pandemia.IntervaloMinimoDias, 0, "Anterior", true, true);
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

                        _context.Entry(vacuna).State = EntityState.Modified;
                        await _context.SaveChangesAsync();
                    }
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
                    new VacunaCalendarioAnualPandemiaDTO(3, "Rotavirus", ArmarListaDosisDTO("Rotavirus"))
                    //new VacunaCalendarioDTO(4, "Neumococo Conjugada"),
                    //new VacunaCalendarioDTO(5, "Quíntuple Pentavalente"),
                    //new VacunaCalendarioDTO(6, "Salk IPV"),
                    //new VacunaCalendarioDTO(7, "Meningocócica Conjugada"),
                    //new VacunaCalendarioDTO(8, "Antigripal"),
                    //new VacunaCalendarioDTO(9, "Triple Viral (SRP)"),
                    //new VacunaCalendarioDTO(10, "Hepatitis A (HA)"),
                    //new VacunaCalendarioDTO(11, "Varicela"),
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
    }
}
