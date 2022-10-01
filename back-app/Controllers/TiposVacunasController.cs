using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VacunacionApi.Models;

namespace VacunacionApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TiposVacunasController : ControllerBase
    {
        private readonly VacunasContext _context;

        public TiposVacunasController(VacunasContext context)
        {
            _context = context;
        }

        // GET: api/TiposVacunas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TipoVacuna>>> GetTipoVacuna()
        {
            return await _context.TipoVacuna.ToListAsync();
        }

        // GET: api/TiposVacunas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TipoVacuna>> GetTipoVacuna(int id)
        {
            var tipoVacuna = await _context.TipoVacuna.FindAsync(id);

            if (tipoVacuna == null)
            {
                return NotFound();
            }

            return tipoVacuna;
        }

        // PUT: api/TiposVacunas/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTipoVacuna(int id, TipoVacuna tipoVacuna)
        {
            if (id != tipoVacuna.Id)
            {
                return BadRequest();
            }

            _context.Entry(tipoVacuna).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TipoVacunaExists(id))
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

        // POST: api/TiposVacunas
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<TipoVacuna>> PostTipoVacuna(TipoVacuna tipoVacuna)
        {
            _context.TipoVacuna.Add(tipoVacuna);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTipoVacuna", new { id = tipoVacuna.Id }, tipoVacuna);
        }

        // DELETE: api/TiposVacunas/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TipoVacuna>> DeleteTipoVacuna(int id)
        {
            var tipoVacuna = await _context.TipoVacuna.FindAsync(id);
            if (tipoVacuna == null)
            {
                return NotFound();
            }

            _context.TipoVacuna.Remove(tipoVacuna);
            await _context.SaveChangesAsync();

            return tipoVacuna;
        }

        private bool TipoVacunaExists(int id)
        {
            return _context.TipoVacuna.Any(e => e.Id == id);
        }
    }
}
