using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using VacunacionApi.DTO;
using VacunacionApi.Models;

namespace VacunacionApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MarcasComercialesController : ControllerBase
    {
        private readonly VacunasContext _context;

        public MarcasComercialesController(VacunasContext context)
        {
            _context = context;
        }

        // GET: api/MarcasComerciales/GetAll?emailOperadorNacional=juan@gmail.com
        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult<IEnumerable<ResponseListaMarcasComercialesDTO>>> GetAll(string emailOperadorNacional = null)
        {
            try
            {
                ResponseListaMarcasComercialesDTO responseListaMarcasComercialesDTO = new ResponseListaMarcasComercialesDTO();

                //lista vacia para los errores
                List<string> errores = new List<string>();
                List<MarcaComercialDTO> marcasComercialesDTO = new List<MarcaComercialDTO>();
                bool existenciaErrores = true;
                string transaccion = "";

                errores = await VerificarCredencialesOperadorNacional(emailOperadorNacional, errores);

                if (errores.Count == 0)
                {
                    existenciaErrores = false;
                    transaccion = "Aceptada";
                    List<MarcaComercial> marcasComerciales = await _context.MarcaComercial.ToListAsync();

                    foreach (MarcaComercial item in marcasComerciales)
                    {
                        MarcaComercialDTO marcaComercialDTO = new MarcaComercialDTO(item.Id, item.Descripcion);
                        marcasComercialesDTO.Add(marcaComercialDTO);
                    }
                }
                else
                {
                    transaccion = "Rechazada";
                }
                responseListaMarcasComercialesDTO.Errores = errores;
                responseListaMarcasComercialesDTO.ExistenciaErrores = existenciaErrores;
                responseListaMarcasComercialesDTO.EstadoTransaccion = transaccion;
                responseListaMarcasComercialesDTO.EmailOperadorNacional = emailOperadorNacional;
                responseListaMarcasComercialesDTO.ListasMarcasComercialesDTO = marcasComercialesDTO;

                return Ok(responseListaMarcasComercialesDTO);
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }

        // GET: api/MarcasComerciales/GetMarcaComercial?emailOperadorNacional=pedro@hotmail.com&idMarcaComercial=5
        [HttpGet]
        [Route("GetMarcaComercial")]
        public async Task<ActionResult<ResponseMarcaComercialDTO>> GetMarcaComercial(string emailOperadorNacional = null, int idMarcaComercial = 0)
        {
            try
            {
                ResponseMarcaComercialDTO responseMarcaComercialDTO = new ResponseMarcaComercialDTO();

                //lista vacia para los errores
                List<string> errores = new List<string>();
                bool existenciaErrores = true;
                string transaccion = "";

                errores = await VerificarCredencialesOperadorNacional(emailOperadorNacional, errores);
                
                MarcaComercial marcaExistente = await _context.MarcaComercial.Where(mc => mc.Id == idMarcaComercial).FirstOrDefaultAsync();



                if (marcaExistente == null)
                    errores.Add(String.Format("La marca comercial con identificador {0} no está registrada en el sistema", idMarcaComercial));
                if (errores.Count == 0)
                {
                    existenciaErrores = false;
                    transaccion = "Aceptada";
                    responseMarcaComercialDTO.EmailOperadorNacional = emailOperadorNacional;
                    responseMarcaComercialDTO.Errores = errores;
                    responseMarcaComercialDTO.ExistenciaErrores = existenciaErrores;
                    responseMarcaComercialDTO.EstadoTransaccion = transaccion;
                    responseMarcaComercialDTO.MarcaComercialDTO = new MarcaComercialDTO(marcaExistente.Id, marcaExistente.Descripcion);
                }
                else
                {
                    transaccion = "Rechazada";
                    responseMarcaComercialDTO.EmailOperadorNacional = emailOperadorNacional;
                    responseMarcaComercialDTO.Errores = errores;
                    responseMarcaComercialDTO.ExistenciaErrores = existenciaErrores;
                    responseMarcaComercialDTO.EstadoTransaccion = transaccion;
                    responseMarcaComercialDTO.MarcaComercialDTO = new MarcaComercialDTO(idMarcaComercial, null);
                }
                return Ok(responseMarcaComercialDTO);   
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }

        // PUT: api/MarcasComerciales/ModificarMarcaComercial/
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut]
        [Route("ModificarMarcaComercial")]
        public async Task<ActionResult<ResponseMarcaComercialDTO>> ModificarMarcaComercial([FromBody] RequestMarcaComercialUpdateDTO model)
        {
            try
            {
                ResponseMarcaComercialDTO responseMarcaComercialDTO = new ResponseMarcaComercialDTO();
                List<string> errores = new List<string>();
                Usuario usuarioExistente = await GetUsuario(model.EmailOperadorNacional);

                if (usuarioExistente != null)
                {
                    Rol rol = await GetRol(usuarioExistente.IdRol);
                    
                    if (rol != null)
                    {
                        if (rol.Descripcion != "Operador Nacional")
                        {
                            errores.Add(String.Format("El Usuario {0} no tiene rol operador nacional", model.EmailOperadorNacional));
                        }
                    }
                    else
                        errores.Add(String.Format("El rol con identificador {0} no está registrado en el sistema", usuarioExistente.IdRol));
                }
                else
                    errores.Add(String.Format("El mail {0} no está registrado en el sistema", model.EmailOperadorNacional));

                MarcaComercial marcaComercialExistente = await _context.MarcaComercial.Where(mc => mc.Descripcion == model.DescripcionMarcaComercial).FirstOrDefaultAsync();
                if (marcaComercialExistente != null)
                {
                    MarcaComercial marcaNueva = await _context.MarcaComercial.Where(mc => mc.Descripcion == model.DescripcionMarcaComercialNueva).FirstOrDefaultAsync();
                    if (marcaNueva != null)
                        errores.Add(String.Format("La marca comercial {0} está registrada en el sistema", model.DescripcionMarcaComercialNueva));
                }
                else
                    errores.Add(String.Format("La marca comercial {0} no está registrada en el sistema", model.DescripcionMarcaComercial));

                if (errores.Count > 0)
                {
                    responseMarcaComercialDTO.EmailOperadorNacional = model.EmailOperadorNacional;
                    responseMarcaComercialDTO.Errores = errores;
                    responseMarcaComercialDTO.ExistenciaErrores = true;
                    responseMarcaComercialDTO.EstadoTransaccion = "Rechazada";
                    responseMarcaComercialDTO.MarcaComercialDTO = new MarcaComercialDTO(0, model.DescripcionMarcaComercialNueva);
                }
                else
                {
                    marcaComercialExistente.Descripcion = model.DescripcionMarcaComercialNueva;
                    _context.Entry(marcaComercialExistente).State = EntityState.Modified;
                    await _context.SaveChangesAsync();

                    responseMarcaComercialDTO.EmailOperadorNacional = model.EmailOperadorNacional;
                    responseMarcaComercialDTO.Errores = errores;
                    responseMarcaComercialDTO.ExistenciaErrores = false;
                    responseMarcaComercialDTO.EstadoTransaccion = "Aceptada";
                    responseMarcaComercialDTO.MarcaComercialDTO = new MarcaComercialDTO(marcaComercialExistente.Id, model.DescripcionMarcaComercialNueva);
                }
                return Ok(responseMarcaComercialDTO);
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }

        // POST: api/MarcasComerciales/CrearMarcaComercial
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        [Route("CrearMarcaComercial")]
        //el requestMarcaComercial posee toda la informacion que viene del formulario
        public async Task<ActionResult<ResponseMarcaComercialDTO>> CrearMarcaComercial([FromBody] RequestMarcaComercialDTO requestMarcaComercialDTO)
        {
            try
            {
                ResponseMarcaComercialDTO responseMarcaComercialDTO = null;
                MarcaComercialDTO marcaComercialDTO = new MarcaComercialDTO();

                //lista vacia para los errores
                List<string> errores = new List<string>();

                errores = await VerificarCredencialesOperadorNacional(requestMarcaComercialDTO.EmailOperadorNacional, errores);

                MarcaComercial marcaExistente = await _context.MarcaComercial.Where(mc => mc.Descripcion == requestMarcaComercialDTO.DescripcionMarcaComercial).FirstOrDefaultAsync();
                if (marcaExistente != null)
                    errores.Add(String.Format("La marca comercial {0} está registrada en el sistema", requestMarcaComercialDTO.DescripcionMarcaComercial));

                if (errores.Count > 0)
                {
                    marcaComercialDTO.Descripcion = requestMarcaComercialDTO.DescripcionMarcaComercial;
                    responseMarcaComercialDTO = new ResponseMarcaComercialDTO(requestMarcaComercialDTO.EmailOperadorNacional, "Rechazada", true, errores, marcaComercialDTO);
                } 
                else
                {                    
                    //creo la instancia del objeto original. Del model
                    MarcaComercial marcaComercial = new MarcaComercial();
                    marcaComercial.Descripcion = requestMarcaComercialDTO.DescripcionMarcaComercial;

                    //guardo en la base de datos
                    _context.MarcaComercial.Add(marcaComercial);
                    await _context.SaveChangesAsync();

                    marcaComercialDTO.Descripcion = marcaComercial.Descripcion;
                    marcaComercialDTO.Id = marcaComercial.Id;
                    responseMarcaComercialDTO = new ResponseMarcaComercialDTO(requestMarcaComercialDTO.EmailOperadorNacional, "Aceptada", false, errores, marcaComercialDTO);
                }

                return Ok(responseMarcaComercialDTO);
            }   
            catch(Exception error)
            {
                return BadRequest(error.Message);
            }
        }

        //metodos privados-----------------------------------
        private bool MarcaComercialExists(int id)
        {
            return _context.MarcaComercial.Any(e => e.Id == id);
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

        private async Task<List<string>> VerificarCredencialesOperadorNacional(string emailOperadorNacional, List<string> errores)
        {
            try
            {
                Usuario usuarioSolicitante = await CuentaUsuarioExists(emailOperadorNacional);
                if (usuarioSolicitante != null)
                {
                    if (!await TieneRolOperadorNacional(usuarioSolicitante))
                    {
                        errores.Add(String.Format("El usuario {0} no tiene el rol de operador nacional", emailOperadorNacional));
                    }
                }
                else
                {
                    errores.Add(string.Format("El usuario {0} no existe en el sistema", emailOperadorNacional));
                }
            }  
            catch
            {

            }

            return errores;
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
    }
}