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

                if (idJurisdiccion != 0)
                {
                    List<List<string>> listaVerificacionJurisdiccion = await VerificarJurisdiccion(errores, idJurisdiccion);
                    errores = listaVerificacionJurisdiccion[0];
                }
                if (idRol != 0)
                {
                    List<List<string>> listaVerificacionRol = await VerificarRol(errores, idRol);
                    errores = listaVerificacionRol[0];
                }

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
                    if(TieneFormatoEmail(email))
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
                    Jurisdiccion jurisdiccionExistente = await GetJurisdiccion(usuario.IdJurisdiccion.Value);
                    Rol rolExistente = await GetRol(usuario.IdRol);

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
                    if (!TieneFormatoEmail(emailAdministrador))
                        errores.Add(string.Format("El email {0} no tiene un formato válido", emailAdministrador));
                    else
                        errores = await VerificarCredencialesUsuarioAdministrador(emailAdministrador, errores);

                    if (idUsuario != 0)
                        usuarioExistente = await _context.Usuario.Where(usuario => usuario.Id == idUsuario).FirstOrDefaultAsync();

                    if(usuarioExistente == null)
                    {
                        if (emailUsuario != null)
                        {
                            if (!TieneFormatoEmail(emailUsuario))
                                errores.Add(string.Format("El email {0} no tiene un formato válido", emailUsuario));
                            else
                                usuarioExistente = await GetUsuario(emailUsuario);
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
                    listaVerificacionJurisdiccion = await VerificarJurisdiccion(errores, model.IdJurisdiccionNuevo);
                    errores = listaVerificacionJurisdiccion[0];
                }
                if (model.IdRolNuevo != 0)
                {
                    listaVerificacionRol = await VerificarRol(errores, model.IdRolNuevo);
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

                Usuario usuarioExistente = await GetUsuario(model.Email);
                if (usuarioExistente == null)
                    errores.Add(string.Format("El email {0} no está registrado en el sistema", model.Email));
                else if (errores.Count == 0)
                {
                    if (model.IdJurisdiccionNuevo != 0 && model.IdRolNuevo == 0)
                    {
                        Rol rolUsuario = await GetRol(usuarioExistente.IdRol);
                        if (rolUsuario.Descripcion == "Operador Nacional" && listaVerificacionJurisdiccion[1][0] != "Nación")
                            errores.Add(string.Format("El rol {0} no puede ser asociado a la jurisdicción {1}", rolUsuario.Descripcion, listaVerificacionJurisdiccion[1][0]));
                    }
                    if (model.IdRolNuevo != 0 && model.IdJurisdiccionNuevo == 0)
                    {
                        Jurisdiccion jurisUsuario = await GetJurisdiccion(usuarioExistente.IdJurisdiccion.Value);
                        if (jurisUsuario.Descripcion == "Nación" && listaVerificacionRol[1][0] != "Operador Nacional")
                            errores.Add(string.Format("El rol {0} no puede ser asociado a la jurisdicción {1}", listaVerificacionRol[1][0], jurisUsuario.Descripcion));
                    }
                }

                errores = await VerificarCredencialesUsuarioAdministrador(model.EmailAdministrador, errores);

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

                    Jurisdiccion juris = await GetJurisdiccion(usuarioExistente.IdJurisdiccion.Value);
                    Rol rol = await GetRol(usuarioExistente.IdRol);

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
               
                List<List<string>> listaVerificacionJurisdiccion = await VerificarJurisdiccion(errores, model.IdJurisdiccion);
                errores = listaVerificacionJurisdiccion[0];
                List<List<string>> listaVerificacionRol = await VerificarRol(errores, model.IdRol);
                errores = listaVerificacionRol[0];

                if (errores.Count == 0)
                {
                    Jurisdiccion juris = await GetJurisdiccion(model.IdJurisdiccion);
                    Rol rol = await GetRol(model.IdRol);

                    if ((rol.Descripcion == "Operador Nacional" && juris.Descripcion != "Nación") || (rol.Descripcion != "Operador Nacional" && juris.Descripcion == "Nación"))
                    {
                        errores.Add(string.Format("El rol {0} no puede ser asociado a la jurisdicción {1}", rol.Descripcion, juris.Descripcion));
                    }
                }

                errores = await VerificarCredencialesUsuarioAdministrador(model.EmailAdministrador, errores);

                Usuario usuarioExistente = await GetUsuario(model.Email);
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
                Usuario usuarioSolicitante = await GetUsuario(emailAdministrador);
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

        private bool TieneFormatoEmail(string email)
        {
            try
            {
                Regex regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$"); 
                Match matchEmail = regex.Match(email);
                
                if (matchEmail.Success) 
                    return true;
            }
            catch
            {

            }

            return false;
        }
    }
}
