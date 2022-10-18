using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
            catch(Exception error) 
            {
                return BadRequest(error.Message);
            }
        }

        // GET: api/MarcasComerciales/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MarcaComercial>> GetMarcaComercial(int id)
        {
            var marcaComercial = await _context.MarcaComercial.FindAsync(id);

            if (marcaComercial == null)
            {
                return NotFound();
            }

            return marcaComercial;
        }

        // PUT: api/MarcasComerciales/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMarcaComercial(int id, MarcaComercial marcaComercial)
        {
            if (id != marcaComercial.Id)
            {
                return BadRequest();
            }

            _context.Entry(marcaComercial).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MarcaComercialExists(id))
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
               
                if(errores.Count > 0)
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

        // DELETE: api/MarcasComerciales/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<MarcaComercial>> DeleteMarcaComercial(int id)
        {
            var marcaComercial = await _context.MarcaComercial.FindAsync(id);
            if (marcaComercial == null)
            {
                return NotFound();
            }

            _context.MarcaComercial.Remove(marcaComercial);
            await _context.SaveChangesAsync();

            return marcaComercial;
        }


        //metodos privados
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
    }
}
