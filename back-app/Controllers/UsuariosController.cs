using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VacunacionApi.DTO;
using VacunacionApi.Models;
using VacunacionApi.Services;

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

                if (idJurisdiccion != 0)
                {
                    List<List<string>> listaVerificacionJurisdiccion = JurisdiccionService.VerificarJurisdiccion(_context, errores, idJurisdiccion);
                    errores = listaVerificacionJurisdiccion[0];
                }
                if (idRol != 0)
                {
                    List<List<string>> listaVerificacionRol = RolService.VerificarRol(_context, errores, idRol);
                    errores = listaVerificacionRol[0];
                }

                if (emailAdministrador == null)
                {
                    errores.Add(string.Format("El email administrador es obligatorio"));
                }
                else
                {
                    errores = RolService.VerificarCredencialesUsuario(_context, emailAdministrador, errores, "Administrador");
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
                        Jurisdiccion jurisdiccion = JurisdiccionService.GetJurisdiccion(_context, usuario.IdJurisdiccion.Value);
                        Rol rol = RolService.GetRol(_context, usuario.IdRol);
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

        // GET: api/Usuarios/Login?email=maria@gmail.com&password=1234
        [HttpGet]
        [Route("Login")]
        public async Task<ActionResult<ResponseLoginDTO>> Login(string email = null, string password = null)
        {
            try
            {
                List<string> errores = new List<string>();
                Usuario usuario = null;
                ResponseLoginDTO responseLoginDTO = null;

                if (email == null || password == null)
                    errores.Add("Las credenciales de usuario son incorrectas");
                else
                {
                    if(UsuarioService.TieneFormatoEmail(email))
                    {
                        usuario = await _context.Usuario.Where(usu => usu.Email == email && usu.Password == password).FirstOrDefaultAsync();
                        if (usuario == null)
                            errores.Add("Las credenciales de usuario son incorrectas");
                    }
                    else
                        errores.Add(string.Format("El email {0} no tiene un formato válido", email));
                }

                if (errores.Count > 0)
                    responseLoginDTO = new ResponseLoginDTO("Rechazada", true, errores, new UsuarioDTO(0, email, password, 0, 0, null, null));
                else
                {
                    Jurisdiccion jurisdiccionExistente = JurisdiccionService.GetJurisdiccion(_context, usuario.IdJurisdiccion.Value);
                    Rol rolExistente = RolService.GetRol(_context, usuario.IdRol);

                    responseLoginDTO = new ResponseLoginDTO("Aceptada", false, errores, new UsuarioDTO(usuario.Id, usuario.Email,
                        usuario.Password, usuario.IdJurisdiccion.Value, usuario.IdRol, jurisdiccionExistente.Descripcion, rolExistente.Descripcion));
                }

                return Ok(responseLoginDTO);
            }
            catch(Exception error)
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
                bool credencialesUsuario = true;

                if (emailAdministrador == null)
                    errores.Add(string.Format("El email administrador es obligatorio"));
                if (emailUsuario == null && idUsuario == 0)
                { 
                    errores.Add(string.Format("Se debe especificar email o identificador del usuario a consultar"));
                    credencialesUsuario = false;
                }
                else
                {
                    if (!UsuarioService.TieneFormatoEmail(emailAdministrador))
                        errores.Add(string.Format("El email {0} no tiene un formato válido", emailAdministrador));
                    else
                        errores = RolService.VerificarCredencialesUsuario(_context, emailAdministrador, errores, "Administrador");

                    if (idUsuario != 0)
                        usuarioExistente = await _context.Usuario.Where(usuario => usuario.Id == idUsuario).FirstOrDefaultAsync();

                    if(usuarioExistente == null)
                    {
                        if (emailUsuario != null)
                        {
                            if (!UsuarioService.TieneFormatoEmail(emailUsuario))
                                errores.Add(string.Format("El email {0} no tiene un formato válido", emailUsuario));
                            else
                                usuarioExistente = UsuarioService.GetUsuario(_context, emailUsuario);
                        }
                    }   
                }

                if (usuarioExistente == null && credencialesUsuario)
                {
                    errores.Add("El usuario a consultar no está registrado en el sistema");
                }

                if (errores.Count > 0)
                    responseUsuarioDTO = LoadResponseUsuarioDTO("Rechazada", true, errores, emailAdministrador, 
                        new Usuario(0, emailUsuario, null, 0, 0), null, null);
                else
                {
                    Jurisdiccion jurisdiccionExistente = JurisdiccionService.GetJurisdiccion(_context, usuarioExistente.IdJurisdiccion.Value);
                    Rol rolExistente = RolService.GetRol(_context, usuarioExistente.IdRol);
                    
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
        [HttpPut]
        [Route("ModificarUsuario")]
        public async Task<ActionResult<ResponseUsuarioDTO>> ModificarUsuario([FromBody] RequestUsuarioUpdateDTO model)
        {
            try
            {
                ResponseUsuarioDTO responseUsuarioDTO = null;
                List<string> errores = new List<string>();
                List<List<string>> listaVerificacionJurisdiccion = new List<List<string>>();
                List<List<string>> listaVerificacionRol = new List<List<string>>();

                //Verificación de nuevos datos
                if (model.IdJurisdiccionNuevo != 0)
                {
                    listaVerificacionJurisdiccion = JurisdiccionService.VerificarJurisdiccion(_context, errores, model.IdJurisdiccionNuevo);
                    errores = listaVerificacionJurisdiccion[0];
                }
                if (model.IdRolNuevo != 0)
                {
                    listaVerificacionRol = RolService.VerificarRol(_context, errores, model.IdRolNuevo);
                    errores = listaVerificacionRol[0];
                }
                if (errores.Count == 0 && model.IdJurisdiccionNuevo != 0 && model.IdRolNuevo != 0)
                {
                    if ((listaVerificacionRol[1][0] == "Operador Nacional" && listaVerificacionJurisdiccion[1][0] != "Nación") ||
                        (listaVerificacionRol[1][0] != "Operador Nacional" && listaVerificacionJurisdiccion[1][0] == "Nación"))
                    {
                        errores.Add(string.Format("El rol {0} no puede ser asociado a la jurisdicción {1}", listaVerificacionRol[1][0], listaVerificacionJurisdiccion[1][0]));
                    }
                }
                else if (errores.Count == 0 && model.IdJurisdiccionNuevo != 0)
                {
                    if (listaVerificacionJurisdiccion[1][0] == "Nación")
                    { 
                        Rol rolResultante = await _context.Rol.Where(rol => rol.Descripcion == "Operador Nacional").FirstOrDefaultAsync();
                        if (rolResultante != null)
                            model.IdRolNuevo = rolResultante.Id;
                    }
                }
                else if (errores.Count == 0 && model.IdRolNuevo != 0)
                {
                    if (listaVerificacionRol[1][0] == "Operador Nacional")
                    {
                        Jurisdiccion jurisResultante = await _context.Jurisdiccion.Where(jur => jur.Descripcion == "Nación").FirstOrDefaultAsync();
                        if (jurisResultante != null)
                            model.IdJurisdiccionNuevo = jurisResultante.Id;
                    }
                }

                Usuario usuarioExistente = UsuarioService.GetUsuario(_context, model.Email);
                if (usuarioExistente == null)
                    errores.Add(string.Format("El email {0} no está registrado en el sistema", model.Email));
                else if (errores.Count == 0)
                {
                    if (model.IdJurisdiccionNuevo != 0 && model.IdRolNuevo == 0)
                    {
                        Rol rolUsuario = RolService.GetRol(_context, usuarioExistente.IdRol);
                        if (rolUsuario.Descripcion == "Operador Nacional" && listaVerificacionJurisdiccion[1][0] != "Nación")
                            errores.Add(string.Format("El rol {0} no puede ser asociado a la jurisdicción {1}", rolUsuario.Descripcion, listaVerificacionJurisdiccion[1][0]));
                    }
                    if (model.IdRolNuevo != 0 && model.IdJurisdiccionNuevo == 0)
                    {
                        Jurisdiccion jurisUsuario = JurisdiccionService.GetJurisdiccion(_context, usuarioExistente.IdJurisdiccion.Value);
                        if (jurisUsuario.Descripcion == "Nación" && listaVerificacionRol[1][0] != "Operador Nacional")
                            errores.Add(string.Format("El rol {0} no puede ser asociado a la jurisdicción {1}", listaVerificacionRol[1][0], jurisUsuario.Descripcion));
                    }
                }

                errores = RolService.VerificarCredencialesUsuario(_context, model.EmailAdministrador, errores, "Administrador");

                if (model.Email == model.EmailAdministrador)
                    errores.Add(string.Format("El usuario {0} no tiene permisos para editar sus propias credenciales", model.EmailAdministrador));

                if (errores.Count > 0)
                    responseUsuarioDTO = LoadResponseUsuarioDTO("Rechazada", true, errores, model.EmailAdministrador, 
                        new Usuario(0, model.Email, null, 0, 0), null, null);
                else
                {
                    if (model.IdJurisdiccionNuevo != 0)
                        usuarioExistente.IdJurisdiccion = model.IdJurisdiccionNuevo;

                    if (model.IdRolNuevo != 0)
                        usuarioExistente.IdRol = model.IdRolNuevo;

                    if (model.PasswordNuevo != null && model.PasswordNuevo != "")
                        usuarioExistente.Password = model.PasswordNuevo;

                    _context.Entry(usuarioExistente).State = EntityState.Modified;
                    await _context.SaveChangesAsync();

                    Jurisdiccion juris = JurisdiccionService.GetJurisdiccion(_context, usuarioExistente.IdJurisdiccion.Value);
                    Rol rol = RolService.GetRol(_context, usuarioExistente.IdRol);

                    responseUsuarioDTO = LoadResponseUsuarioDTO("Aceptada", false, errores, model.EmailAdministrador,
                        usuarioExistente, juris.Descripcion, rol.Descripcion);
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
               
                List<List<string>> listaVerificacionJurisdiccion = JurisdiccionService.VerificarJurisdiccion(_context, errores, model.IdJurisdiccion);
                errores = listaVerificacionJurisdiccion[0];
                List<List<string>> listaVerificacionRol = RolService.VerificarRol(_context, errores, model.IdRol);
                errores = listaVerificacionRol[0];

                if (errores.Count == 0)
                {
                    Jurisdiccion juris = JurisdiccionService.GetJurisdiccion(_context, model.IdJurisdiccion);
                    Rol rol = RolService.GetRol(_context, model.IdRol);

                    if ((rol.Descripcion == "Operador Nacional" && juris.Descripcion != "Nación") || (rol.Descripcion != "Operador Nacional" && juris.Descripcion == "Nación"))
                    {
                        errores.Add(string.Format("El rol {0} no puede ser asociado a la jurisdicción {1}", rol.Descripcion, juris.Descripcion));
                    }
                }

                errores = RolService.VerificarCredencialesUsuario(_context, model.EmailAdministrador, errores, "Administrador");

                Usuario usuarioExistente = UsuarioService.GetUsuario(_context, model.Email);
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

        // Método privado de ayuda
        private ResponseUsuarioDTO LoadResponseUsuarioDTO(string estadoTransaccion, bool existenciaErrores, List<string> errores, string emailAdministrador,
            Usuario usuario, string descripcionJurisdiccion, string descripcionRol)
        {
            UsuarioDTO usuarioDTO = new UsuarioDTO(usuario.Id, usuario.Email, usuario.Password, usuario.IdJurisdiccion.Value, usuario.IdRol, descripcionJurisdiccion, descripcionRol);
            ResponseUsuarioDTO responseUsuarioDTO = new ResponseUsuarioDTO(estadoTransaccion, existenciaErrores, errores, emailAdministrador, usuarioDTO);

            return responseUsuarioDTO;
        }
    }
}
