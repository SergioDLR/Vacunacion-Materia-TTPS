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

        // GET: api/Vacunas/GetAll
        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult<IEnumerable<Vacuna>>> GetAll()
        {
            return await _context.Vacuna.ToListAsync();
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
                errores = listaVerificacionTipoVacuna[0];

                if (listaVerificacionTipoVacuna[1][0] == "Vacuna de Calendario")
                {
                    List<VacunaCalendarioDTO> vacunasCalendario = GetVacunasCalendario();
                    if (!vacunasCalendario.Any(v => v.Descripcion == model.Descripcion))
                        errores.Add(string.Format("La descripción {0} no corresponde a una vacuna de calendario", model.Descripcion));
                }

                if (model.IdPandemia != 0)
                {
                    List<List<string>> listaVerificacionPandemia = await VerificarPandemia(errores, model.IdPandemia.Value);
                    errores = listaVerificacionPandemia[0];

                    if ((listaVerificacionTipoVacuna[1][0] == "Vacuna de Calendario" || listaVerificacionTipoVacuna[1][0] == "Vacuna Anual") && (listaVerificacionPandemia[1][0] != null))
                    {
                        errores.Add(string.Format("La {0} no puede ser asociada a la pandemia {1}", listaVerificacionTipoVacuna[1][0], listaVerificacionPandemia[1][0]));
                    }
                }

                errores = await VerificarCredencialesUsuarioOperadorNacional(model.EmailOperadorNacional, errores);

                Vacuna vacunaExistente = await GetVacuna(model.Descripcion);
                if (vacunaExistente != null)
                    errores.Add(string.Format("La descripción {0} está registrada en el sistema", model.Descripcion));

                if (errores.Count > 0)
                    responseVacunaDTO = new ResponseVacunaDTO("Rechazada", true, errores, model.EmailOperadorNacional, new VacunaDTO(0, model.Descripcion, model.IdTipoVacuna, null, model.IdPandemia.Value, null, 0, null));
                else
                {
                    Vacuna vacuna = new Vacuna();
                    vacuna.Descripcion = model.Descripcion;
                    vacuna.IdPandemia = model.IdPandemia.Value;
                    vacuna.IdTipoVacuna = model.IdTipoVacuna;

                    if (listaVerificacionTipoVacuna[1][0] == "Vacuna de Calendario")
                    {
                        List<VacunaCalendarioDTO> vacunasCalendario = GetVacunasCalendario();
                        _context.Vacuna.Add(vacuna);
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

        private List<VacunaCalendarioDTO> GetVacunasCalendario()
        {
            List<VacunaCalendarioDTO> vacunasCalendario = null;

            try
            {
                vacunasCalendario = new List<VacunaCalendarioDTO>()
                {
                    new VacunaCalendarioDTO(1, "Hepatitis B (HB)", ArmarListaDosisDTO("Hepatitis B (HB)")),
                    new VacunaCalendarioDTO(2, "BCG", ArmarListaDosisDTO("BCG")),
                    new VacunaCalendarioDTO(3, "Rotavirus", ArmarListaDosisDTO("Rotavirus"))
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
    }
}
