using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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

        // GET: api/Usuarios/GetAll?emailAdministrador=juan@gmail.com&idJurisdiccion=2&idRol=3
        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult<ResponseListaUsuariosDTO>> GetAll(string emailAdministrador = null, int idJurisdiccion = 0, int idRol = 0)
        {
            try
            {
                ResponseListaUsuariosDTO responseListaUsuariosDTO = null;
                List<string> errores = new List<string>();
                List<UsuarioDTO> listaUsuariosDTO = new List<UsuarioDTO>();
                List<Usuario> usuarios = new List<Usuario>();

                if (emailAdministrador == null)
                {
                    errores.Add(string.Format("El email administrador es obligatorio"));
                }
                else
                {
                    errores = await VerificarCredencialesUsuarioAdministrador(emailAdministrador, errores);
                }

                if (errores.Count > 0)
                { 
                    responseListaUsuariosDTO = new ResponseListaUsuariosDTO("Rechazada", true, errores, emailAdministrador, listaUsuariosDTO);
                }
                else
                {
                    if (idJurisdiccion == 0 && idRol == 0)
                    {
                        usuarios = await _context.Usuario.ToListAsync();
                    }
                    else if (idJurisdiccion == 0)
                    {
                        usuarios = await _context.Usuario.Where(usuario => usuario.IdRol == idRol).ToListAsync();
                    }
                    else if (idRol == 0)
                    {
                        usuarios = await _context.Usuario.Where(usuario => usuario.IdJurisdiccion == idJurisdiccion).ToListAsync();
                    }
                    else
                    {
                        usuarios = await _context.Usuario.Where(usuario => usuario.IdJurisdiccion == idJurisdiccion && usuario.IdRol == idRol).ToListAsync();
                    }

                    foreach (Usuario usuario in usuarios)
                    {
                        Jurisdiccion jurisdiccion = await GetJurisdiccion(usuario.IdJurisdiccion.Value);
                        Rol rol = await GetRol(usuario.IdRol);
                        UsuarioDTO usuarioDTO = new UsuarioDTO(usuario.Id, usuario.Email, usuario.Password, 
                            usuario.IdJurisdiccion.Value, usuario.IdRol, jurisdiccion.Descripcion, rol.Descripcion);
                        listaUsuariosDTO.Add(usuarioDTO);
                    }
                 
                    responseListaUsuariosDTO = new ResponseListaUsuariosDTO("Aceptada", false, errores, emailAdministrador, listaUsuariosDTO);
                }

                return Ok(responseListaUsuariosDTO);
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }

        // GET: api/Usuarios/GetUsuario?emailAdministrador=juan@gmail.com&emailUsuario=maria@gmail.com&idUsuario=34
        [HttpGet]
        [Route("GetUsuario")]
        public async Task<ActionResult<ResponseUsuarioDTO>> GetUsuario(string emailAdministrador = null, string emailUsuario = null, int idUsuario = 0)
        {
            try
            {
                ResponseUsuarioDTO responseUsuarioDTO = null;
                Usuario usuarioExistente = null;
                List<string> errores = new List<string>();

                if (emailAdministrador == null)
                {
                    errores.Add(string.Format("El email administrador es obligatorio"));
                }
                else if (emailUsuario == null && idUsuario == 0)
                {
                    errores.Add(string.Format("Se debe especificar email o identificador del usuario a consultar"));
                }
                else
                {
                    errores = VerificarFormatoEmailsAdministradorUsuario(emailAdministrador, emailUsuario, errores);
                    errores = await VerificarCredencialesUsuarioAdministrador(emailAdministrador, errores);

                    if (emailAdministrador != null)
                    {
                        usuarioExistente = await GetUsuario(emailUsuario, "", "GetAccountAdministrador");
                    }
                    else
                    {
                        usuarioExistente = await _context.Usuario.Where(usuario => usuario.Id == idUsuario).FirstOrDefaultAsync();
                    }
                    if (usuarioExistente == null)
                        errores.Add(string.Format("El email {0} no está registrado en el sistema", emailUsuario));
                }

                if (errores.Count > 0)
                    responseUsuarioDTO = LoadResponseUsuarioDTO("Rechazada", true, errores, emailAdministrador, 
                        new Usuario(0, emailUsuario, null, 0, 0), null, null);
                else
                {
                    Jurisdiccion jurisdiccionExistente = await GetJurisdiccion(usuarioExistente.IdJurisdiccion.Value);
                    Rol rolExistente = await GetRol(usuarioExistente.IdRol);
                    
                    responseUsuarioDTO = LoadResponseUsuarioDTO("Aceptada", false, errores, emailAdministrador, usuarioExistente, jurisdiccionExistente.Descripcion, rolExistente.Descripcion);
                }

                return Ok(responseUsuarioDTO);
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }

        // PUT: api/Usuarios/ModificarUsuario
        [HttpPut("ModificarUsuario")]
        public async Task<ActionResult<ResponseUsuarioDTO>> ModificarUsuario([FromBody] RequestUsuarioDTO model)
        {
            try
            {
                ResponseUsuarioDTO responseUsuarioDTO = null;
                List<string> errores = new List<string>();

                errores = await VerificarCredencialesUsuarioAdministrador(model.EmailAdministrador, errores);
                List<List<string>> listaVerificacionJurisdiccion = await VerificarJurisdiccion(errores, model.IdJurisdiccion);
                errores = listaVerificacionJurisdiccion[0];
                List<List<string>> listaVerificacionRol = await VerificarRol(errores, model.IdRol);
                errores = listaVerificacionRol[0];

                Usuario usuarioExistente = await GetUsuario(model.Email, model.Password, "GetAccount");
                if (usuarioExistente == null)
                    errores.Add(string.Format("El email {0} no está registrado en el sistema", model.Email));

                if (errores.Count > 0)
                    responseUsuarioDTO = LoadResponseUsuarioDTO("Rechazada", true, errores, model.EmailAdministrador, 
                        new Usuario(0, model.Email, model.Password, model.IdJurisdiccion, model.IdRol), 
                        listaVerificacionJurisdiccion[1][0], listaVerificacionRol[1][0]);
                else
                {
                    usuarioExistente.Email = model.Email;
                    usuarioExistente.Password = model.Password;
                    usuarioExistente.IdJurisdiccion = model.IdJurisdiccion;
                    usuarioExistente.IdRol = model.IdRol;
                    _context.Entry(usuarioExistente).State = EntityState.Modified;
                    await _context.SaveChangesAsync();

                    responseUsuarioDTO = LoadResponseUsuarioDTO("Aceptada", false, errores, model.EmailAdministrador, 
                        usuarioExistente, listaVerificacionJurisdiccion[1][0], listaVerificacionRol[1][0]);
                }

                return Ok(responseUsuarioDTO);
            }
            catch (DbUpdateConcurrencyException error)
            {
                return BadRequest(error.Message);
            }
        }

        // POST: api/Usuarios/CrearUsuario
        [HttpPost]
        [Route("CrearUsuario")]
        public async Task<ActionResult<ResponseUsuarioDTO>> CrearUsuario([FromBody] RequestUsuarioDTO model)
        {
            try
            {
                ResponseUsuarioDTO responseUsuarioDTO = null;
                List<string> errores = new List<string>();
               
                errores = await VerificarCredencialesUsuarioAdministrador(model.EmailAdministrador, errores);
                List<List<string>> listaVerificacionJurisdiccion = await VerificarJurisdiccion(errores, model.IdJurisdiccion);
                errores = listaVerificacionJurisdiccion[0];
                List<List<string>> listaVerificacionRol = await VerificarRol(errores, model.IdRol);
                errores = listaVerificacionRol[0];

                Usuario usuarioExistente = await GetUsuario(model.Email, model.Password, "Create");
                if (usuarioExistente != null)
                    errores.Add(string.Format("El email {0} está registrado en el sistema", model.Email));
                              
                if(errores.Count > 0)
                    responseUsuarioDTO = LoadResponseUsuarioDTO("Rechazada", true, errores, model.EmailAdministrador, 
                        new Usuario(0, model.Email, model.Password, model.IdJurisdiccion, model.IdRol), 
                        listaVerificacionJurisdiccion[1][0], listaVerificacionRol[1][0]);
                else
                {
                    Usuario usuario = new Usuario(model);
                    _context.Usuario.Add(usuario);
                    await _context.SaveChangesAsync();
                                        
                    responseUsuarioDTO = LoadResponseUsuarioDTO("Aceptada", false, errores, model.EmailAdministrador, 
                        usuario, listaVerificacionJurisdiccion[1][0], listaVerificacionRol[1][0]);
                }   
                
                return Ok(responseUsuarioDTO);
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }



        // Métodos privados de ayuda
        private ResponseUsuarioDTO LoadResponseUsuarioDTO(string estadoTransaccion, bool existenciaErrores, List<string> errores, string emailAdministrador,
            Usuario usuario, string descripcionJurisdiccion, string descripcionRol)
        {
            UsuarioDTO usuarioDTO = new UsuarioDTO(usuario.Id, usuario.Email, usuario.Password, usuario.IdJurisdiccion.Value, usuario.IdRol, descripcionJurisdiccion, descripcionRol);
            ResponseUsuarioDTO responseUsuarioDTO = new ResponseUsuarioDTO(estadoTransaccion, existenciaErrores, errores, emailAdministrador, usuarioDTO);

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

        private async Task<Usuario> GetUsuario(string email, string password, string operacion)
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

        private async Task<Jurisdiccion> GetJurisdiccion(int idJurisdiccion)
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

        private async Task<List<string>> VerificarCredencialesUsuarioAdministrador(string emailAdministrador, List<string> errores)
        {
            try
            {
                Usuario usuarioSolicitante = await GetUsuario(emailAdministrador, "", "GetAccountAdministrador");
                if (usuarioSolicitante == null)
                    errores.Add(string.Format("El usuario {0} no está registrado en el sistema", emailAdministrador));
                else
                {
                    bool tieneRolAdministrador = await TieneRolAdministrador(usuarioSolicitante);
                    if (!tieneRolAdministrador)
                        errores.Add(string.Format("El usuario {0} no tiene rol administrador", emailAdministrador));
                }
            }
            catch
            { 
            
            }

            return errores;
        }

        private async Task<List<List<string>>> VerificarJurisdiccion(List<string> errores, int idJurisdiccion)
        {
            List<List<string>> erroresConcatDescripciones = new List<List<string>>();
            
            try
            {
                List<string> descripciones = new List<string>();
                Jurisdiccion jurisdiccionExistente = await GetJurisdiccion(idJurisdiccion);
                
                if (jurisdiccionExistente == null)
                {
                    errores.Add(string.Format("La jurisdicción con identificador {0} no está registrada en el sistema", idJurisdiccion));
                    descripciones.Add(null);
                }
                else
                    descripciones.Add(jurisdiccionExistente.Descripcion);

                erroresConcatDescripciones.Add(errores);
                erroresConcatDescripciones.Add(descripciones);
            }
            catch
            {
                
            }

            return erroresConcatDescripciones;
        }

        private async Task<List<List<string>>> VerificarRol(List<string> errores, int idRol)
        {
            List<List<string>> erroresConcatDescripciones = new List<List<string>>();

            try
            {
                List<string> descripciones = new List<string>();
                Rol rolExistente = await GetRol(idRol);

                if (rolExistente == null)
                {
                    errores.Add(string.Format("El rol con identificador {0} no está registrado en el sistema", idRol));
                    descripciones.Add(null);
                }
                else
                    descripciones.Add(rolExistente.Descripcion);

                erroresConcatDescripciones.Add(errores);
                erroresConcatDescripciones.Add(descripciones);
            }
            catch
            {

            }

            return erroresConcatDescripciones;
        }

        private List<string> VerificarFormatoEmailsAdministradorUsuario(string emailAdministrador, string emailUsuario, List<string> errores)
        {
            try
            {
                Regex regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$"); 
                Match matchEmailAdministrador = regex.Match(emailAdministrador);
                Match matchEmailUsuario = regex.Match(emailUsuario);

                if (!matchEmailAdministrador.Success) 
                    errores.Add("El email administrador tiene un formato inválido");

                if (!matchEmailUsuario.Success)
                    errores.Add("El email usuario tiene un formato inválido");
            }
            catch
            {

            }

            return errores;
        }
    }
}
