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
    public class VacunasDesarrolladasController : ControllerBase
    {
        private readonly VacunasContext _context;

        public VacunasDesarrolladasController(VacunasContext context)
        {
            _context = context;
        }

        // GET: api/VacunasDesarrolladas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VacunaDesarrollada>>> GetVacunaDesarrollada()
        {
            return await _context.VacunaDesarrollada.ToListAsync();
        }

        // GET: api/VacunasDesarrolladas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<VacunaDesarrollada>> GetVacunaDesarrollada(int id)
        {
            var vacunaDesarrollada = await _context.VacunaDesarrollada.FindAsync(id);

            if (vacunaDesarrollada == null)
            {
                return NotFound();
            }

            return vacunaDesarrollada;
        }

        // PUT: api/VacunasDesarrolladas/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVacunaDesarrollada(int id, VacunaDesarrollada vacunaDesarrollada)
        {
            if (id != vacunaDesarrollada.Id)
            {
                return BadRequest();
            }

            _context.Entry(vacunaDesarrollada).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VacunaDesarrolladaExists(id))
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

        // POST: api/VacunasDesarrolladas
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<VacunaDesarrollada>> PostVacunaDesarrollada(VacunaDesarrollada vacunaDesarrollada)
        {
            _context.VacunaDesarrollada.Add(vacunaDesarrollada);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVacunaDesarrollada", new { id = vacunaDesarrollada.Id }, vacunaDesarrollada);
        }

        // DELETE: api/VacunasDesarrolladas/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<VacunaDesarrollada>> DeleteVacunaDesarrollada(int id)
        {
            var vacunaDesarrollada = await _context.VacunaDesarrollada.FindAsync(id);
            if (vacunaDesarrollada == null)
            {
                return NotFound();
            }

            _context.VacunaDesarrollada.Remove(vacunaDesarrollada);
            await _context.SaveChangesAsync();

            return vacunaDesarrollada;
        }

        private bool VacunaDesarrolladaExists(int id)
        {
            return _context.VacunaDesarrollada.Any(e => e.Id == id);
        }
    }
}
