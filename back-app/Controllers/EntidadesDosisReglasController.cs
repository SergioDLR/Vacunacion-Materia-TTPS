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
    public class EntidadesDosisReglasController : ControllerBase
    {
        private readonly VacunasContext _context;

        public EntidadesDosisReglasController(VacunasContext context)
        {
            _context = context;
        }

        // GET: api/EntidadesDosisReglas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EntidadDosisRegla>>> GetEntidadDosisRegla()
        {
            return await _context.EntidadDosisRegla.ToListAsync();
        }

        // GET: api/EntidadesDosisReglas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EntidadDosisRegla>> GetEntidadDosisRegla(int id)
        {
            var entidadDosisRegla = await _context.EntidadDosisRegla.FindAsync(id);

            if (entidadDosisRegla == null)
            {
                return NotFound();
            }

            return entidadDosisRegla;
        }

        // PUT: api/EntidadesDosisReglas/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEntidadDosisRegla(int id, EntidadDosisRegla entidadDosisRegla)
        {
            if (id != entidadDosisRegla.Id)
            {
                return BadRequest();
            }

            _context.Entry(entidadDosisRegla).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EntidadDosisReglaExists(id))
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

        // POST: api/EntidadesDosisReglas
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<EntidadDosisRegla>> PostEntidadDosisRegla(EntidadDosisRegla entidadDosisRegla)
        {
            _context.EntidadDosisRegla.Add(entidadDosisRegla);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEntidadDosisRegla", new { id = entidadDosisRegla.Id }, entidadDosisRegla);
        }

        // DELETE: api/EntidadesDosisReglas/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<EntidadDosisRegla>> DeleteEntidadDosisRegla(int id)
        {
            var entidadDosisRegla = await _context.EntidadDosisRegla.FindAsync(id);
            if (entidadDosisRegla == null)
            {
                return NotFound();
            }

            _context.EntidadDosisRegla.Remove(entidadDosisRegla);
            await _context.SaveChangesAsync();

            return entidadDosisRegla;
        }

        private bool EntidadDosisReglaExists(int id)
        {
            return _context.EntidadDosisRegla.Any(e => e.Id == id);
        }
    }
}
