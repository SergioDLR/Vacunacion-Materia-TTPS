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
    public class PendientesEnviosDwController : ControllerBase
    {
        private readonly VacunasContext _context;

        public PendientesEnviosDwController(VacunasContext context)
        {
            _context = context;
        }

        // GET: api/PendientesEnviosDw
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PendienteEnvioDw>>> GetPendienteEnvioDw()
        {
            return await _context.PendienteEnvioDw.ToListAsync();
        }

        // GET: api/PendientesEnviosDw/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PendienteEnvioDw>> GetPendienteEnvioDw(int id)
        {
            var pendienteEnvioDw = await _context.PendienteEnvioDw.FindAsync(id);

            if (pendienteEnvioDw == null)
            {
                return NotFound();
            }

            return pendienteEnvioDw;
        }

        // PUT: api/PendientesEnviosDw/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPendienteEnvioDw(int id, PendienteEnvioDw pendienteEnvioDw)
        {
            if (id != pendienteEnvioDw.Id)
            {
                return BadRequest();
            }

            _context.Entry(pendienteEnvioDw).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PendienteEnvioDwExists(id))
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

        // POST: api/PendientesEnviosDw
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<PendienteEnvioDw>> PostPendienteEnvioDw(PendienteEnvioDw pendienteEnvioDw)
        {
            _context.PendienteEnvioDw.Add(pendienteEnvioDw);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPendienteEnvioDw", new { id = pendienteEnvioDw.Id }, pendienteEnvioDw);
        }

        // DELETE: api/PendientesEnviosDw/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<PendienteEnvioDw>> DeletePendienteEnvioDw(int id)
        {
            var pendienteEnvioDw = await _context.PendienteEnvioDw.FindAsync(id);
            if (pendienteEnvioDw == null)
            {
                return NotFound();
            }

            _context.PendienteEnvioDw.Remove(pendienteEnvioDw);
            await _context.SaveChangesAsync();

            return pendienteEnvioDw;
        }

        private bool PendienteEnvioDwExists(int id)
        {
            return _context.PendienteEnvioDw.Any(e => e.Id == id);
        }
    }
}
