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
    public class DosisController : ControllerBase
    {
        private readonly VacunasContext _context;

        public DosisController(VacunasContext context)
        {
            _context = context;
        }

        // GET: api/Dosis
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Dosis>>> GetDosis()
        {
            return await _context.Dosis.ToListAsync();
        }

        // GET: api/Dosis/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Dosis>> GetDosis(int id)
        {
            var dosis = await _context.Dosis.FindAsync(id);

            if (dosis == null)
            {
                return NotFound();
            }

            return dosis;
        }

        // PUT: api/Dosis/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDosis(int id, Dosis dosis)
        {
            if (id != dosis.Id)
            {
                return BadRequest();
            }

            _context.Entry(dosis).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DosisExists(id))
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

        // POST: api/Dosis
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Dosis>> PostDosis(Dosis dosis)
        {
            _context.Dosis.Add(dosis);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDosis", new { id = dosis.Id }, dosis);
        }

        // DELETE: api/Dosis/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Dosis>> DeleteDosis(int id)
        {
            var dosis = await _context.Dosis.FindAsync(id);
            if (dosis == null)
            {
                return NotFound();
            }

            _context.Dosis.Remove(dosis);
            await _context.SaveChangesAsync();

            return dosis;
        }

        private bool DosisExists(int id)
        {
            return _context.Dosis.Any(e => e.Id == id);
        }
    }
}
