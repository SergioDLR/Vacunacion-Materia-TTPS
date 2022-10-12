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
    public class UsuariosController : ControllerBase
    {
        private readonly VacunasContext _context;

        public UsuariosController(VacunasContext context)
        {
            _context = context;
        }

        // GET: api/Usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuario()
        {
            return await _context.Usuario.ToListAsync();
        }

        // GET: api/Usuarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            var usuario = await _context.Usuario.FindAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }

            return usuario;
        }

        // PUT: api/Usuarios/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(int id, Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return BadRequest();
            }

            _context.Entry(usuario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(id))
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

        // POST: api/Usuarios
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        [Route("CrearUsuario")]
        public async Task<ActionResult<ResponseUsuarioDTO>> CrearUsuario([FromBody] RequestUsuarioDTO model)
        {
            try
            {
                ResponseUsuarioDTO responseUsuarioDTO = new ResponseUsuarioDTO();
                List<string> errores = new List<string>();
                string descripcionJurisdiccion = null;
                string descripcionRol = null;

                Usuario usuarioSolicitante = await CuentaUsuarioExists(model.EmailAdministrador, "", "GetAccountAdministrador");
                if (usuarioSolicitante == null)
                    errores.Add(string.Format("El usuario {0} no existe", model.EmailAdministrador));
                else
                {
                    bool tieneRolAdministrador = await TieneRolAdministrador(usuarioSolicitante);
                    if(!tieneRolAdministrador)
                        errores.Add(string.Format("El usuario {0} no tiene rol administrador", model.EmailAdministrador));
                }

                Usuario usuarioExistente = await CuentaUsuarioExists(model.Email, model.Password, "Create");
                if (usuarioExistente != null)
                    errores.Add(string.Format("El email {0} ya existe", model.Email));
                
                Jurisdiccion jurisdiccionExistente = await JurisdiccionExists(model.IdJurisdiccion);
                if (jurisdiccionExistente == null)
                    errores.Add(string.Format("La jurisdicción con identificador {0} no existe", model.IdJurisdiccion));
                else
                    descripcionJurisdiccion = jurisdiccionExistente.Descripcion;
                
                Rol rolExistente = await RolExists(model.IdRol);
                if (rolExistente == null)
                    errores.Add(string.Format("El rol con identificador {0} no existe", model.IdRol));
                else
                    descripcionRol = rolExistente.Descripcion;

                if(errores.Count > 0)
                    responseUsuarioDTO = LoadResponseUsuarioDTO("Rechazada", true, errores, model.EmailAdministrador,
                        model.Email, model.Password, model.IdJurisdiccion, model.IdRol, descripcionJurisdiccion, descripcionRol);
                else
                {
                    responseUsuarioDTO = LoadResponseUsuarioDTO("Aceptada", false, errores, model.EmailAdministrador,
                        model.Email, model.Password, model.IdJurisdiccion, model.IdRol, descripcionJurisdiccion, descripcionRol);
               
                    Usuario usuario = new Usuario(model);
                    _context.Usuario.Add(usuario);
                    await _context.SaveChangesAsync();
                }   
                
                return Ok(responseUsuarioDTO);
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }

        // DELETE: api/Usuarios/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Usuario>> DeleteUsuario(int id)
        {
            var usuario = await _context.Usuario.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            _context.Usuario.Remove(usuario);
            await _context.SaveChangesAsync();

            return usuario;
        }



        // Métodos privados de ayuda
        
        private bool UsuarioExists(int id)
        {
            return _context.Usuario.Any(e => e.Id == id);
        }

        private ResponseUsuarioDTO LoadResponseUsuarioDTO(string estadoTransaccion, bool existenciaErrores, List<string> errores,
            string emailAdministrador, string email, string password, int idJurisdiccion, int idRol, string descripcionJurisdiccion, string descripcionRol)
        {
            ResponseUsuarioDTO responseUsuarioDTO = new ResponseUsuarioDTO(estadoTransaccion, existenciaErrores,
                errores, emailAdministrador, email, password, idJurisdiccion, idRol, descripcionJurisdiccion, descripcionRol);

            return responseUsuarioDTO;
        }

        private async Task<bool> TieneRolAdministrador(Usuario usuario)
        {
            try
            {
                Rol rolAdministrador = await _context.Rol
                    .Where(rol => rol.Descripcion == "Administrador").FirstOrDefaultAsync();

                if (rolAdministrador.Id == usuario.IdRol) 
                {
                    return true;
                }
            }
            catch 
            { 
            
            }

            return false;
        }

        private async Task<Usuario> CuentaUsuarioExists(string email, string password, string operacion)
        {
            Usuario cuentaExistente = null;

            try
            {
                if (operacion == "GetAccount")
                {
                    cuentaExistente = await _context.Usuario
                        .Where(user => user.Email == email && user.Password == password).FirstOrDefaultAsync();
                }
                if (operacion == "Create" || operacion == "GetAccountAdministrador")
                {
                    cuentaExistente = await _context.Usuario
                        .Where(user => user.Email == email).FirstOrDefaultAsync();
                }
            }
            catch
            {

            }

            return cuentaExistente;
        }

        private async Task<Jurisdiccion> JurisdiccionExists(int idJurisdiccion)
        {
            Jurisdiccion jurisdiccionExistente = null;

            try
            {
                jurisdiccionExistente = await _context.Jurisdiccion
                    .Where(juris => juris.Id == idJurisdiccion).FirstOrDefaultAsync();
            }
            catch 
            { 
            
            }

            return jurisdiccionExistente;
        }

        private async Task<Rol> RolExists(int idRol)
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
