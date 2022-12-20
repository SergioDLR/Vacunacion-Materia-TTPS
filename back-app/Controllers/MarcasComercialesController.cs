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
using VacunacionApi.Services;

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

                errores = RolService.VerificarCredencialesUsuario(_context, emailOperadorNacional, errores, "Operador Nacional");

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

                errores = RolService.VerificarCredencialesUsuario(_context, emailOperadorNacional, errores, "Operador Nacional");
                
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
                Usuario usuarioExistente = UsuarioService.GetUsuario(_context, model.EmailOperadorNacional);

                if (usuarioExistente != null)
                {
                    Rol rol = RolService.GetRol(_context, usuarioExistente.IdRol);
                    
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

                errores = RolService.VerificarCredencialesUsuario(_context, requestMarcaComercialDTO.EmailOperadorNacional, errores, "Operador Nacional");

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
    }
}
