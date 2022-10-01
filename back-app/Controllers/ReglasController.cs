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
    public class ReglasController : ControllerBase
    {
        private readonly VacunasContext _context;

        public ReglasController(VacunasContext context)
        {
            _context = context;
        }

        // GET: api/Reglas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Regla>>> GetRegla()
        {
            return await _context.Regla.ToListAsync();
        }

        // GET: api/Reglas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Regla>> GetRegla(int id)
        {
            var regla = await _context.Regla.FindAsync(id);

            if (regla == null)
            {
                return NotFound();
            }

            return regla;
        }

        // PUT: api/Reglas/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRegla(int id, Regla regla)
        {
            if (id != regla.Id)
            {
                return BadRequest();
            }

            _context.Entry(regla).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReglaExists(id))
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

        // POST: api/Reglas
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Regla>> PostRegla(Regla regla)
        {
            _context.Regla.Add(regla);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRegla", new { id = regla.Id }, regla);
        }

        // DELETE: api/Reglas/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Regla>> DeleteRegla(int id)
        {
            var regla = await _context.Regla.FindAsync(id);
            if (regla == null)
            {
                return NotFound();
            }

            _context.Regla.Remove(regla);
            await _context.SaveChangesAsync();

            return regla;
        }

        private bool ReglaExists(int id)
        {
            return _context.Regla.Any(e => e.Id == id);
        }
    }
}
