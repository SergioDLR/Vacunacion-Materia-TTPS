using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VacunacionApi.ModelsDataWareHouse;

namespace VacunacionApi.ControllersDataWareHouse
{
    [Route("api/[controller]")]
    [ApiController]
    public class HVencidasController : ControllerBase
    {
        private readonly DataWareHouseContext _context;

        public HVencidasController(DataWareHouseContext context)
        {
            _context = context;
        }

        // GET: api/HVencidas/GetAll
        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult<IEnumerable<HVencidas>>> GetHVencidas()
        {
            return await _context.HVencidas.ToListAsync();
        }

        // GET: api/HVencidas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HVencidas>> GetHVencidas(int id)
        {
            var hVencidas = await _context.HVencidas.FindAsync(id);

            if (hVencidas == null)
            {
                return NotFound();
            }

            return hVencidas;
        }

        // PUT: api/HVencidas/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHVencidas(int id, HVencidas hVencidas)
        {
            if (id != hVencidas.Id)
            {
                return BadRequest();
            }

            _context.Entry(hVencidas).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HVencidasExists(id))
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

        // POST: api/HVencidas
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<HVencidas>> PostHVencidas(HVencidas hVencidas)
        {
            _context.HVencidas.Add(hVencidas);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetHVencidas", new { id = hVencidas.Id }, hVencidas);
        }

        // DELETE: api/HVencidas/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<HVencidas>> DeleteHVencidas(int id)
        {
            var hVencidas = await _context.HVencidas.FindAsync(id);
            if (hVencidas == null)
            {
                return NotFound();
            }

            _context.HVencidas.Remove(hVencidas);
            await _context.SaveChangesAsync();

            return hVencidas;
        }

        private bool HVencidasExists(int id)
        {
            return _context.HVencidas.Any(e => e.Id == id);
        }
    }
}
