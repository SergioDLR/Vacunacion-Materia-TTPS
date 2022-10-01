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
    public class EntidadesVacunasDosisController : ControllerBase
    {
        private readonly VacunasContext _context;

        public EntidadesVacunasDosisController(VacunasContext context)
        {
            _context = context;
        }

        // GET: api/EntidadesVacunasDosis
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EntidadVacunaDosis>>> GetEntidadVacunaDosis()
        {
            return await _context.EntidadVacunaDosis.ToListAsync();
        }

        // GET: api/EntidadesVacunasDosis/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EntidadVacunaDosis>> GetEntidadVacunaDosis(int id)
        {
            var entidadVacunaDosis = await _context.EntidadVacunaDosis.FindAsync(id);

            if (entidadVacunaDosis == null)
            {
                return NotFound();
            }

            return entidadVacunaDosis;
        }

        // PUT: api/EntidadesVacunasDosis/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEntidadVacunaDosis(int id, EntidadVacunaDosis entidadVacunaDosis)
        {
            if (id != entidadVacunaDosis.Id)
            {
                return BadRequest();
            }

            _context.Entry(entidadVacunaDosis).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EntidadVacunaDosisExists(id))
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

        // POST: api/EntidadesVacunasDosis
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<EntidadVacunaDosis>> PostEntidadVacunaDosis(EntidadVacunaDosis entidadVacunaDosis)
        {
            _context.EntidadVacunaDosis.Add(entidadVacunaDosis);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEntidadVacunaDosis", new { id = entidadVacunaDosis.Id }, entidadVacunaDosis);
        }

        // DELETE: api/EntidadesVacunasDosis/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<EntidadVacunaDosis>> DeleteEntidadVacunaDosis(int id)
        {
            var entidadVacunaDosis = await _context.EntidadVacunaDosis.FindAsync(id);
            if (entidadVacunaDosis == null)
            {
                return NotFound();
            }

            _context.EntidadVacunaDosis.Remove(entidadVacunaDosis);
            await _context.SaveChangesAsync();

            return entidadVacunaDosis;
        }

        private bool EntidadVacunaDosisExists(int id)
        {
            return _context.EntidadVacunaDosis.Any(e => e.Id == id);
        }
    }
}
