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
    public class MarcasComercialesController : ControllerBase
    {
        private readonly VacunasContext _context;

        public MarcasComercialesController(VacunasContext context)
        {
            _context = context;
        }

        // GET: api/MarcasComerciales
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MarcaComercial>>> GetMarcaComercial()
        {
            return await _context.MarcaComercial.ToListAsync();
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

        // POST: api/MarcasComerciales
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<MarcaComercial>> PostMarcaComercial(MarcaComercial marcaComercial)
        {
            _context.MarcaComercial.Add(marcaComercial);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMarcaComercial", new { id = marcaComercial.Id }, marcaComercial);
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

        private bool MarcaComercialExists(int id)
        {
            return _context.MarcaComercial.Any(e => e.Id == id);
        }
    }
}
