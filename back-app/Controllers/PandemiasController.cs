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
    public class PandemiasController : ControllerBase
    {
        private readonly VacunasContext _context;

        public PandemiasController(VacunasContext context)
        {
            _context = context;
        }

        // GET: api/Pandemias
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pandemia>>> GetPandemia()
        {
            return await _context.Pandemia.ToListAsync();
        }

        // GET: api/Pandemias/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Pandemia>> GetPandemia(int id)
        {
            var pandemia = await _context.Pandemia.FindAsync(id);

            if (pandemia == null)
            {
                return NotFound();
            }

            return pandemia;
        }

        // PUT: api/Pandemias/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPandemia(int id, Pandemia pandemia)
        {
            if (id != pandemia.Id)
            {
                return BadRequest();
            }

            _context.Entry(pandemia).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PandemiaExists(id))
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

        // POST: api/Pandemias
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Pandemia>> PostPandemia(Pandemia pandemia)
        {
            _context.Pandemia.Add(pandemia);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPandemia", new { id = pandemia.Id }, pandemia);
        }

        // DELETE: api/Pandemias/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Pandemia>> DeletePandemia(int id)
        {
            var pandemia = await _context.Pandemia.FindAsync(id);
            if (pandemia == null)
            {
                return NotFound();
            }

            _context.Pandemia.Remove(pandemia);
            await _context.SaveChangesAsync();

            return pandemia;
        }

        private bool PandemiaExists(int id)
        {
            return _context.Pandemia.Any(e => e.Id == id);
        }
    }
}
