using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VacunacionApi.DTO;
using VacunacionApi.Models;

namespace VacunacionApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VacunasController : ControllerBase
    {
        private readonly VacunasContext _context;

        public VacunasController(VacunasContext context)
        {
            _context = context;
        }

        // GET: api/Vacunas/GetAll
        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult<IEnumerable<Vacuna>>> GetAll()
        {
            return await _context.Vacuna.ToListAsync();
        }

        // GET: api/Vacunas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Vacuna>> GetVacuna(int id)
        {
            var vacuna = await _context.Vacuna.FindAsync(id);

            if (vacuna == null)
            {
                return NotFound();
            }

            return vacuna;
        }

        // PUT: api/Vacunas/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVacuna(int id, Vacuna vacuna)
        {
            if (id != vacuna.Id)
            {
                return BadRequest();
            }

            _context.Entry(vacuna).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VacunaExists(id))
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

        // POST: api/Vacunas/CrearVacuna
        [HttpPost]
        [Route("CrearVacuna")]
        public async Task<ActionResult<ResponseVacunaDTO>> CrearVacuna([FromBody] RequestVacunaDTO model)
        {
            try
            {
                ResponseVacunaDTO responseVacunaDTO = new ResponseVacunaDTO();

                return Ok(responseVacunaDTO);
            }
            catch(Exception error)
            {
                return BadRequest(error.Message);
            }
        }

        // DELETE: api/Vacunas/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Vacuna>> DeleteVacuna(int id)
        {
            var vacuna = await _context.Vacuna.FindAsync(id);
            if (vacuna == null)
            {
                return NotFound();
            }

            _context.Vacuna.Remove(vacuna);
            await _context.SaveChangesAsync();

            return vacuna;
        }

        private bool VacunaExists(int id)
        {
            return _context.Vacuna.Any(e => e.Id == id);
        }
    }
}
