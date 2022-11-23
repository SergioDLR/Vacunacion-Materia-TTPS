using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VacunacionApi.ModelsDataWareHouse;
using VacunacionApi.Services;

namespace VacunacionApi.ControllersDataWareHouse
{
    [Route("api/[controller]")]
    [ApiController]
    public class HVacunadosController : ControllerBase
    {
        private readonly DataWareHouseContext _context;

        public HVacunadosController(DataWareHouseContext context)
        {
            _context = context;
        }

        // GET: api/HVacunados/GetAll
        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult<IEnumerable<HVacunados>>> GetHVacunados()
        {
            return await _context.HVacunados.ToListAsync();
        }

        // GET: api/HVacunados/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HVacunados>> GetHVacunados(int id)
        {
            var hVacunados = await _context.HVacunados.FindAsync(id);

            if (hVacunados == null)
            {
                return NotFound();
            }

            return hVacunados;
        }

        // PUT: api/HVacunados/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHVacunados(int id, HVacunados hVacunados)
        {
            if (id != hVacunados.Id)
            {
                return BadRequest();
            }

            _context.Entry(hVacunados).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HVacunadosExists(id))
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

        // POST: api/HVacunados/SaveHVacunadosDataWareHouse
        [HttpPost]
        [Route("SaveHVacunadosDataWareHouse")]
        public async Task<ActionResult<bool>> SaveHVacunadosDataWareHouse()
        {
            try
            {
                DataWareHouseService service = new DataWareHouseService();
                await service.CargarDataWareHouse(_context);
            }
            catch(Exception error)
            {
                return BadRequest(error.Message);
            }

            return true;
        }

        // DELETE: api/HVacunados/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<HVacunados>> DeleteHVacunados(int id)
        {
            var hVacunados = await _context.HVacunados.FindAsync(id);
            if (hVacunados == null)
            {
                return NotFound();
            }

            _context.HVacunados.Remove(hVacunados);
            await _context.SaveChangesAsync();

            return hVacunados;
        }

        private bool HVacunadosExists(int id)
        {
            return _context.HVacunados.Any(e => e.Id == id);
        }
    }
}
