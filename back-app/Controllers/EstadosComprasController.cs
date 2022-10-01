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
    public class EstadosComprasController : ControllerBase
    {
        private readonly VacunasContext _context;

        public EstadosComprasController(VacunasContext context)
        {
            _context = context;
        }

        // GET: api/EstadosCompras/GetAll
        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult<IEnumerable<EstadoCompraDTO>>> GetAll()
        {
            List<EstadoCompraDTO> estadosComprasDTO = new List<EstadoCompraDTO>();

            try
            {
                List<EstadoCompra> estadosCompras = await _context.EstadoCompra.ToListAsync();
                foreach (EstadoCompra estadoCompra in estadosCompras) 
                {
                    EstadoCompraDTO estadoCompraDTO = new EstadoCompraDTO(estadoCompra.Id, estadoCompra.Descripcion);
                    estadosComprasDTO.Add(estadoCompraDTO);
                }
            }
            catch(Exception error)
            {
                return BadRequest(error.Message);
            }

            return Ok(estadosComprasDTO);
        }

        // GET: api/EstadosCompras/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EstadoCompra>> GetEstadoCompra(int id)
        {
            var estadoCompra = await _context.EstadoCompra.FindAsync(id);

            if (estadoCompra == null)
            {
                return NotFound();
            }

            return estadoCompra;
        }

        // PUT: api/EstadosCompras/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEstadoCompra(int id, EstadoCompra estadoCompra)
        {
            if (id != estadoCompra.Id)
            {
                return BadRequest();
            }

            _context.Entry(estadoCompra).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EstadoCompraExists(id))
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

        // POST: api/EstadosCompras
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<EstadoCompra>> PostEstadoCompra(EstadoCompra estadoCompra)
        {
            _context.EstadoCompra.Add(estadoCompra);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEstadoCompra", new { id = estadoCompra.Id }, estadoCompra);
        }

        // DELETE: api/EstadosCompras/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<EstadoCompra>> DeleteEstadoCompra(int id)
        {
            var estadoCompra = await _context.EstadoCompra.FindAsync(id);
            if (estadoCompra == null)
            {
                return NotFound();
            }

            _context.EstadoCompra.Remove(estadoCompra);
            await _context.SaveChangesAsync();

            return estadoCompra;
        }

        private bool EstadoCompraExists(int id)
        {
            return _context.EstadoCompra.Any(e => e.Id == id);
        }
    }
}
