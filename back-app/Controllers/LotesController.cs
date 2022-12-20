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
        public IEnumerable<int> GetAll()
        {
            return new List<int>() { 111, 222, 333, 444, 555, 666, 777, 888, 999 };
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
                    errores = RolService.VerificarCredencialesUsuario(_context, email, errores, "Operador Nacional");
                }
                if (idLote == 0)
                    errores.Add(string.Format("El id de lote es incorrecto"));

                if (errores.Count > 0)
                    response = new ResponseCargarVacunaDTO("Rechazada", true, errores, email);
                else
                {
                    await new DataWareHouseService().CargarVencidasDataWareHouse(_context2, idLote);
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
    }
}
