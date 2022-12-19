using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.EntityFrameworkCore;
using VacunacionApi.DTO;
using VacunacionApi.Models;

namespace VacunacionApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VacunasDesarrolladasController : ControllerBase
    {
        private readonly VacunasContext _context;

        public VacunasDesarrolladasController(VacunasContext context)
        {
            _context = context;
        }

        // GET: api/VacunasDesarrolladas/GetAll?emailOperadorNacional=juan@gmail.com
        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult<IEnumerable<ResponseListaVacunasDesarrolladasDTO>>> GetAll(string emailOperadorNacional = null)
        {
            try
            {
                ResponseListaVacunasDesarrolladasDTO responseListaVacunasDesarrolladasDTO = new ResponseListaVacunasDesarrolladasDTO();
                //lista vacia para los errores
                List<string> errores = new List<string>();
                List<VacunaDesarrolladaDTO> vacunasDesarrolladasDTO = new List<VacunaDesarrolladaDTO>();
                bool existenciaErrores = true;
                string transaccion = "";

                errores = await VerificarCredencialesOperadorNacional(emailOperadorNacional, errores);

                if(errores.Count == 0)
                {
                    existenciaErrores = false;
                    transaccion = "Aceptada";
                    List<VacunaDesarrollada> vacunasDesarrolladas = await _context.VacunaDesarrollada.ToListAsync();

                    foreach(VacunaDesarrollada item in vacunasDesarrolladas)
                    {
                        MarcaComercial marcaComercial = await _context.MarcaComercial.Where(mc => mc.Id == item.IdMarcaComercial).FirstOrDefaultAsync();
                        Vacuna vacuna = await _context.Vacuna.Where(v => v.Id == item.IdVacuna).FirstOrDefaultAsync();    
                        VacunaDesarrolladaDTO vacunaDesarrolladaDTO = new VacunaDesarrolladaDTO(item.Id, item.IdVacuna, vacuna.Descripcion, item.IdMarcaComercial, marcaComercial.Descripcion, item.DiasDemoraEntrega, item.PrecioVacuna, item.FechaHasta);
                        vacunasDesarrolladasDTO.Add(vacunaDesarrolladaDTO);
                    }

                }
                else
                {
                    transaccion = "Rechazada";
                }

                responseListaVacunasDesarrolladasDTO.EmailOperadorNacional = emailOperadorNacional;
                responseListaVacunasDesarrolladasDTO.Errores = errores;
                responseListaVacunasDesarrolladasDTO.ExistenciaErrores = existenciaErrores;
                responseListaVacunasDesarrolladasDTO.EstadoTransaccion = transaccion;
                responseListaVacunasDesarrolladasDTO.ListaVacunasDesarrolladasDTO = vacunasDesarrolladasDTO;
                return Ok(responseListaVacunasDesarrolladasDTO);

            }
            catch(Exception error)
            {
                return BadRequest(error.Message);  
            }
        }

        // GET: api/VacunasDesarrolladas/GetAllEliminados?emailOperadorNacional=juan@gmail.com
        [HttpGet]
        [Route("GetAllEliminados")]
        public async Task<ActionResult<IEnumerable<ResponseListaVacunasDesarrolladasDTO>>> GetAllEliminados(string emailOperadorNacional = null)
        {
            try
            {
                ResponseListaVacunasDesarrolladasDTO responseListaVacunasDesarrolladasDTO = new ResponseListaVacunasDesarrolladasDTO();
                //lista vacia para los errores
                List<string> errores = new List<string>();
                List<VacunaDesarrolladaDTO> vacunasDesarrolladasDTO = new List<VacunaDesarrolladaDTO>();
                bool existenciaErrores = true;
                string transaccion = "";

                errores = await VerificarCredencialesOperadorNacional(emailOperadorNacional, errores);

                if (errores.Count == 0)
                {
                    existenciaErrores = false;
                    transaccion = "Aceptada";
                    List<VacunaDesarrollada> vacunasDesarrolladas = await _context.VacunaDesarrollada.ToListAsync();

                    foreach (VacunaDesarrollada item in vacunasDesarrolladas)
                    {
                        if (item.FechaHasta != null)
                        {
                            MarcaComercial marcaComercial = await _context.MarcaComercial.Where(mc => mc.Id == item.IdMarcaComercial).FirstOrDefaultAsync();
                            Vacuna vacuna = await _context.Vacuna.Where(v => v.Id == item.IdVacuna).FirstOrDefaultAsync();
                            VacunaDesarrolladaDTO vacunaDesarrolladaDTO = new VacunaDesarrolladaDTO(item.Id, item.IdVacuna, vacuna.Descripcion, item.IdMarcaComercial, marcaComercial.Descripcion, item.DiasDemoraEntrega, item.PrecioVacuna, item.FechaHasta.Value);
                            vacunasDesarrolladasDTO.Add(vacunaDesarrolladaDTO);
                        }
                    }
                }
                else
                {
                    transaccion = "Rechazada";
                }

                responseListaVacunasDesarrolladasDTO.EmailOperadorNacional = emailOperadorNacional;
                responseListaVacunasDesarrolladasDTO.Errores = errores;
                responseListaVacunasDesarrolladasDTO.ExistenciaErrores = existenciaErrores;
                responseListaVacunasDesarrolladasDTO.EstadoTransaccion = transaccion;
                responseListaVacunasDesarrolladasDTO.ListaVacunasDesarrolladasDTO = vacunasDesarrolladasDTO;
                return Ok(responseListaVacunasDesarrolladasDTO);

            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }


        // GET: api/VacunasDesarrolladas/GetAllActivas?emailOperadorNacional=juan@gmail.com
        [HttpGet]
        [Route("GetAllActivas")]
        public async Task<ActionResult<IEnumerable<ResponseListaVacunasDesarrolladasDTO>>> GetAllActivas(string emailOperadorNacional = null)
        {
            try
            {
                ResponseListaVacunasDesarrolladasDTO responseListaVacunasDesarrolladasDTO = new ResponseListaVacunasDesarrolladasDTO();
                //lista vacia para los errores
                List<string> errores = new List<string>();
                List<VacunaDesarrolladaDTO> vacunasDesarrolladasDTO = new List<VacunaDesarrolladaDTO>();
                bool existenciaErrores = true;
                string transaccion = "";

                errores = await VerificarCredencialesOperadorNacional(emailOperadorNacional, errores);

                if (errores.Count == 0)
                {
                    existenciaErrores = false;
                    transaccion = "Aceptada";
                    List<VacunaDesarrollada> vacunasDesarrolladas = await _context.VacunaDesarrollada.ToListAsync();

                    foreach (VacunaDesarrollada item in vacunasDesarrolladas)
                    {
                        if (item.FechaHasta == null)
                        {
                            MarcaComercial marcaComercial = await _context.MarcaComercial.Where(mc => mc.Id == item.IdMarcaComercial).FirstOrDefaultAsync();
                            Vacuna vacuna = await _context.Vacuna.Where(v => v.Id == item.IdVacuna).FirstOrDefaultAsync();
                            VacunaDesarrolladaDTO vacunaDesarrolladaDTO = new VacunaDesarrolladaDTO(item.Id, item.IdVacuna, vacuna.Descripcion, item.IdMarcaComercial, marcaComercial.Descripcion, item.DiasDemoraEntrega, item.PrecioVacuna, item.FechaHasta);
                            vacunasDesarrolladasDTO.Add(vacunaDesarrolladaDTO);
                        }
                    }
                }
                else
                {
                    transaccion = "Rechazada";
                }

                responseListaVacunasDesarrolladasDTO.EmailOperadorNacional = emailOperadorNacional;
                responseListaVacunasDesarrolladasDTO.Errores = errores;
                responseListaVacunasDesarrolladasDTO.ExistenciaErrores = existenciaErrores;
                responseListaVacunasDesarrolladasDTO.EstadoTransaccion = transaccion;
                responseListaVacunasDesarrolladasDTO.ListaVacunasDesarrolladasDTO = vacunasDesarrolladasDTO;
                return Ok(responseListaVacunasDesarrolladasDTO);

            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }


        // GET: api/VacunasDesarrolladas/GetVacunaDesarrollada?emailOperadorNacional=juan@gmail.com&idVacunaDesarrollada=5
        [HttpGet]
        [Route("GetVacunaDesarrollada")]
        public async Task<ActionResult<ResponseVacunaDesarrolladaDTO>> GetVacunaDesarrollada(string emailOperadorNacional = null, int idVacunaDesarrollada = 0)
        {
            try
            {
                ResponseVacunaDesarrolladaDTO responseVacunaDesarrolladaDTO = new ResponseVacunaDesarrolladaDTO();

                //lista vacia para los errores
                List<string> errores = new List<string>();

                errores = await VerificarCredencialesOperadorNacional(emailOperadorNacional, errores);

                VacunaDesarrollada vacunaDesarrolladaExistente = await _context.VacunaDesarrollada.Where(vd => vd.Id == idVacunaDesarrollada).FirstOrDefaultAsync();
                if (vacunaDesarrolladaExistente == null)
                {
                    errores.Add(String.Format("La vacuna desarrollada con identificador {0} no esta registrada", idVacunaDesarrollada));
                }
                
                if(errores.Count > 0)
                {
                    responseVacunaDesarrolladaDTO.EmailOperadorNacional = emailOperadorNacional;
                    responseVacunaDesarrolladaDTO.EstadoTransaccion = "Rechazada";
                    responseVacunaDesarrolladaDTO.ExistenciaErrores = true;
                    responseVacunaDesarrolladaDTO.Errores = errores;
                    responseVacunaDesarrolladaDTO.VacunaDesarrolladaDTO = new VacunaDesarrolladaDTO();
                }
                else
                {
                    Vacuna vacuna = await _context.Vacuna.Where(v => v.Id == vacunaDesarrolladaExistente.IdVacuna).FirstOrDefaultAsync();
                    MarcaComercial marcaComercial = await _context.MarcaComercial.Where(mc => mc.Id == vacunaDesarrolladaExistente.IdMarcaComercial).FirstOrDefaultAsync();

                    responseVacunaDesarrolladaDTO.EmailOperadorNacional = emailOperadorNacional;
                    responseVacunaDesarrolladaDTO.EstadoTransaccion = "Aceptada";
                    responseVacunaDesarrolladaDTO.ExistenciaErrores = false;
                    responseVacunaDesarrolladaDTO.Errores = errores;
                    responseVacunaDesarrolladaDTO.VacunaDesarrolladaDTO = new VacunaDesarrolladaDTO(vacunaDesarrolladaExistente.Id, vacunaDesarrolladaExistente.IdVacuna, vacuna.Descripcion, vacunaDesarrolladaExistente.IdMarcaComercial, marcaComercial.Descripcion, vacunaDesarrolladaExistente.DiasDemoraEntrega, vacunaDesarrolladaExistente.PrecioVacuna, vacunaDesarrolladaExistente.FechaHasta.Value);
                }
                return Ok(responseVacunaDesarrolladaDTO);
            }
            catch (Exception ex)    
            {
                return BadRequest(ex.Message);  
            }
        }


        // POST: api/VacunasDesarrolladas/CrearVacunaDesarrollada
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        [Route("CrearVacunaDesarrollada")]
        public async Task<ActionResult<ResponseVacunaDesarrolladaDTO>> CrearVacunaDesarrollada([FromBody] RequestVacunaDesarrolladaDTO model)
        {
            try
            {
                ResponseVacunaDesarrolladaDTO responseVacunaDesarrolladaDTO = new ResponseVacunaDesarrolladaDTO();
            
                //lista vacia para los errores
                List<string> errores = new List<string>();

                errores = await VerificarCredencialesOperadorNacional(model.EmailOperadorNacional, errores);

                //verifico si existe la vacuna en la lista
                Vacuna vacunaExistente = await _context.Vacuna.Where(vd => vd.Id == model.IdVacuna).FirstOrDefaultAsync();
                if (vacunaExistente == null)
                {
                    errores.Add(String.Format("La vacuna {0} no está registrada en el sistema", model.IdVacuna));
                }

                //verifico si la marca comercial existe
                MarcaComercial marcaComercialExistente = await _context.MarcaComercial.Where(mc => mc.Descripcion == model.MarcaComercial).FirstOrDefaultAsync();
                if(marcaComercialExistente == null)
                {
                    MarcaComercial mc = new MarcaComercial();
                    mc.Descripcion = model.MarcaComercial;
                    _context.MarcaComercial.Add(mc);
                    await _context.SaveChangesAsync();
                }

                marcaComercialExistente = await _context.MarcaComercial.Where(mc => mc.Descripcion == model.MarcaComercial).FirstOrDefaultAsync();

                //chequeo que no exista la marca ya creada
                List<VacunaDesarrollada> vacunasDesarrolladas = await _context.VacunaDesarrollada.ToListAsync();
                foreach (VacunaDesarrollada item in vacunasDesarrolladas)
                {
                    if (item.IdVacuna == model.IdVacuna && item.IdMarcaComercial == marcaComercialExistente.Id && item.FechaHasta == null)
                    {
                        errores.Add(String.Format("La vacuna desarrollada con identificador de vacuna {0} con la marca comercial con identificador {1} ya esta registrada", item.IdVacuna, item.IdMarcaComercial));
                    }
                }

                if (errores.Count() > 0)
                {
                    responseVacunaDesarrolladaDTO.EmailOperadorNacional = model.EmailOperadorNacional;
                    responseVacunaDesarrolladaDTO.Errores = errores;
                    responseVacunaDesarrolladaDTO.ExistenciaErrores = true;
                    responseVacunaDesarrolladaDTO.EstadoTransaccion = "Rechazada";
                    responseVacunaDesarrolladaDTO.VacunaDesarrolladaDTO = new VacunaDesarrolladaDTO();
                }
                else
                {
                    //creo la instancia del objeto original. Del model
                    VacunaDesarrollada vacunaDesarrollada = new VacunaDesarrollada();
                    vacunaDesarrollada.IdVacuna = model.IdVacuna;
                    vacunaDesarrollada.IdMarcaComercial = marcaComercialExistente.Id;
                    vacunaDesarrollada.DiasDemoraEntrega = model.DiasDemoraEntrega;
                    vacunaDesarrollada.PrecioVacuna = model.PrecioVacunaDesarrollada;
                    vacunaDesarrollada.FechaDesde = DateTime.Now;   
                    vacunaDesarrollada.FechaHasta = null;

                    //guardo en la base de datos
                    _context.VacunaDesarrollada.Add(vacunaDesarrollada);
                    await _context.SaveChangesAsync();

                    Vacuna vacuna = await _context.Vacuna.Where(v => v.Id == vacunaDesarrollada.IdVacuna).FirstOrDefaultAsync();
                    MarcaComercial marcaComercial = await _context.MarcaComercial.Where(mc => mc.Id == vacunaDesarrollada.IdMarcaComercial).FirstOrDefaultAsync();

                    responseVacunaDesarrolladaDTO.EmailOperadorNacional = model.EmailOperadorNacional;
                    responseVacunaDesarrolladaDTO.Errores = errores;
                    responseVacunaDesarrolladaDTO.ExistenciaErrores = false;
                    responseVacunaDesarrolladaDTO.EstadoTransaccion = "Aceptada";
                    responseVacunaDesarrolladaDTO.VacunaDesarrolladaDTO = new VacunaDesarrolladaDTO(vacunaDesarrollada.Id, model.IdVacuna, vacuna.Descripcion, marcaComercialExistente.Id, marcaComercial.Descripcion, model.DiasDemoraEntrega, model.PrecioVacunaDesarrollada, null);
                }
                return Ok(responseVacunaDesarrolladaDTO);
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }

        // DELETE: api/VacunasDesarrolladas/DeleteVacunaDesarrollada
        [HttpDelete]
        [Route("DeleteVacunaDesarrollada")]
        public async Task<ActionResult<ResponseVacunaDesarrolladaDTO>> DeleteVacunaDesarrollada(string emailOperadorNacional = null, int idVacunaDesarrollada = 0)
        {
            try
            {
                ResponseVacunaDesarrolladaDTO responseVacunaDesarrolladaDTO = new ResponseVacunaDesarrolladaDTO();

                //lista vacia para los errores
                List<string> errores = new List<string>();

                errores = await VerificarCredencialesOperadorNacional(emailOperadorNacional, errores);
                VacunaDesarrollada vacunaDesarrollada = await _context.VacunaDesarrollada.Where(vd => vd.Id == idVacunaDesarrollada).FirstOrDefaultAsync();
                
                if (vacunaDesarrollada == null)
                {
                    errores.Add(String.Format("La vacuna desarrollada con identificador {0} no esta registrada", idVacunaDesarrollada));
                }

                if (errores.Count() > 0)
                {
                    responseVacunaDesarrolladaDTO.Errores = errores;
                    responseVacunaDesarrolladaDTO.ExistenciaErrores = true;
                    responseVacunaDesarrolladaDTO.EstadoTransaccion = "Rechazada";
                    responseVacunaDesarrolladaDTO.EmailOperadorNacional = emailOperadorNacional;
                    responseVacunaDesarrolladaDTO.VacunaDesarrolladaDTO = new VacunaDesarrolladaDTO();
                }   
                else
                {
                    vacunaDesarrollada.FechaHasta = DateTime.Now;
                    //guardo en la base de datos
                    _context.Entry(vacunaDesarrollada).State = EntityState.Modified;
                    await _context.SaveChangesAsync();

                    //obtengo datos para la respuesta
                    Vacuna vacuna = await _context.Vacuna.Where(v => v.Id == vacunaDesarrollada.IdVacuna).FirstOrDefaultAsync();
                    MarcaComercial marcaComercial = await _context.MarcaComercial.Where(mc => mc.Id == vacunaDesarrollada.IdMarcaComercial).FirstOrDefaultAsync();


                    //armo respuesta
                    responseVacunaDesarrolladaDTO.Errores = errores;
                    responseVacunaDesarrolladaDTO.ExistenciaErrores = false;
                    responseVacunaDesarrolladaDTO.EstadoTransaccion = "Aceptada";
                    responseVacunaDesarrolladaDTO.EmailOperadorNacional = emailOperadorNacional;
                    responseVacunaDesarrolladaDTO.VacunaDesarrolladaDTO = new VacunaDesarrolladaDTO(vacunaDesarrollada.Id, vacunaDesarrollada.IdVacuna, vacuna.Descripcion, vacunaDesarrollada.IdMarcaComercial, marcaComercial.Descripcion, vacunaDesarrollada.DiasDemoraEntrega, vacunaDesarrollada.PrecioVacuna, vacunaDesarrollada.FechaHasta.Value);
                }
                return Ok(responseVacunaDesarrolladaDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);  
            }
        }

        //metodos privados ------------------------
        private bool VacunaDesarrolladaExists(int id)
        {
            return _context.VacunaDesarrollada.Any(e => e.Id == id);
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