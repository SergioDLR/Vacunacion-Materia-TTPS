using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VacunacionApi.DTO;
using VacunacionApi.Models;
using VacunacionApi.ModelsDataWareHouse;
using VacunacionApi.Services;

namespace VacunacionApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LotesController : ControllerBase
    {
        private readonly VacunasContext _context;
        private readonly DataWareHouseContext _context2;

        public LotesController(VacunasContext context, DataWareHouseContext context2)
        {
            _context = context;
            _context2 = context2;
        }

        // GET: api/Lotes/GetAll
        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult<IEnumerable<Lote>>> GetAll()
        {
            return await _context.Lote.Where(l => l.Lotes == 111 || l.Lotes == 222 || l.Lotes == 333 || l.Lotes == 444 || l.Lotes == 555 || l.Lotes == 666 || l.Lotes == 777 || l.Lotes == 888 || l.Lotes == 999) .ToListAsync();
        }

        // POST: api/Lotes/VencerLote?email=juan@gmail.com&idLote=777
        [HttpPost]
        [Route("VencerLote")]
        public async Task<ActionResult<ResponseCargarVacunaDTO>> VencerLote(string email = null, int idLote = 0)
        {
            ResponseCargarVacunaDTO response;
            
            try 
            {
                List<string> errores = new List<string>();

                if (email == null)
                {
                    errores.Add(string.Format("El email operador nacional es obligatorio"));
                }
                else
                {
                    errores = await VerificarCredencialesUsuarioOperadorNacionalVacunador(email, errores);
                }
                if(idLote == 0)
                    errores.Add(string.Format("El id de lote es incorrecto"));


                if (errores.Count > 0)
                    response = new ResponseCargarVacunaDTO("Rechazada", true, errores, email);
                else
                {
                    Lote lote = await _context.Lote.Where(l => l.Lotes == idLote).FirstOrDefaultAsync();
                    lote.FechaVencimiento = DateTime.Now;
                    lote.Disponible = false;
                    _context.Entry(lote).State = EntityState.Modified;

                    Compra compra = await _context.Compra.Where(c => c.IdLote == lote.Lotes).FirstOrDefaultAsync();

                    List<Distribucion> distribucionesLote = await _context.Distribucion
                        .Where(d => d.IdLote == lote.Lotes).ToListAsync();

                    foreach (Distribucion distribucion in distribucionesLote)
                    {
                        distribucion.Vencidas = distribucion.CantidadVacunas - distribucion.Aplicadas;
                        _context.Entry(distribucion).State = EntityState.Modified;
                    }

                    compra.Vencidas = compra.CantidadVacunas - compra.Distribuidas;
                    _context.Entry(compra).State = EntityState.Modified;
                    
                    await _context.SaveChangesAsync();
                    await new DataWareHouseService().CargarVencidasDataWareHouse(_context2, lote.Lotes);
                    response = new ResponseCargarVacunaDTO("Aceptada", false, errores, email);
                }
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }

            return response;
        }

        // GET: api/Lotes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Lote>> GetLote(int id)
        {
            var lote = await _context.Lote.FindAsync(id);

            if (lote == null)
            {
                return NotFound();
            }

            return lote;
        }

        // PUT: api/Lotes/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLote(int id, Lote lote)
        {
            if (id != lote.Id)
            {
                return BadRequest();
            }

            _context.Entry(lote).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LoteExists(id))
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

        // POST: api/Lotes
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Lote>> PostLote(Lote lote)
        {
            _context.Lote.Add(lote);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLote", new { id = lote.Id }, lote);
        }

        // DELETE: api/Lotes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Lote>> DeleteLote(int id)
        {
            var lote = await _context.Lote.FindAsync(id);
            if (lote == null)
            {
                return NotFound();
            }

            _context.Lote.Remove(lote);
            await _context.SaveChangesAsync();

            return lote;
        }

        private bool LoteExists(int id)
        {
            return _context.Lote.Any(e => e.Id == id);
        }

        private async Task<List<string>> VerificarCredencialesUsuarioOperadorNacionalVacunador(string emailOperadorNacional, List<string> errores)
        {
            try
            {
                Usuario usuarioSolicitante = await _context.Usuario.Where(u => u.Email == emailOperadorNacional).FirstOrDefaultAsync();
                if (usuarioSolicitante == null)
                    errores.Add(string.Format("El usuario {0} no está registrado en el sistema", emailOperadorNacional));
                else
                {
                    Rol rol = await _context.Rol
                        .Where(r => r.Id == usuarioSolicitante.IdRol
                            && (r.Descripcion == "Operador Nacional" || r.Descripcion == "Vacunador"))
                        .FirstOrDefaultAsync();
                    if (rol == null)
                        errores.Add(string.Format("El usuario {0} no tiene rol operador nacional o rol vacunador", emailOperadorNacional));
                }
            }
            catch
            {

            }

            return errores;
        }
    }
}
