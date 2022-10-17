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
    public class JurisdiccionesController : ControllerBase
    {
        private readonly VacunasContext _context;

        public JurisdiccionesController(VacunasContext context)
        {
            _context = context;
        }

        // GET: api/Jurisdicciones/GetAll?idRol=3
        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult<IEnumerable<JurisdiccionDTO>>> GetAll(int idRol = 0)
        {
            try
            {
                List<Jurisdiccion> jurisdicciones = new List<Jurisdiccion>();

                if (idRol != 0)
                {
                    Rol rol = await _context.Rol.Where(r => r.Id == idRol).FirstOrDefaultAsync();
                    if (rol != null) 
                    {
                        if (rol.Descripcion == "Operador Nacional")
                        {
                            jurisdicciones = await _context.Jurisdiccion.Where(jur => jur.Descripcion == "Nación").ToListAsync();
                        }
                        else
                            jurisdicciones = await _context.Jurisdiccion.Where(jur => jur.Descripcion != "Nación").ToListAsync();
                    }
                }
                else
                    jurisdicciones = await _context.Jurisdiccion.ToListAsync();

                List<JurisdiccionDTO> jurisdiccionesDTO = new List<JurisdiccionDTO>();

                foreach (Jurisdiccion juris in jurisdicciones)
                {
                    JurisdiccionDTO jurisdiccionDTO = new JurisdiccionDTO(juris.Id, juris.Descripcion);
                    jurisdiccionesDTO.Add(jurisdiccionDTO);
                }

                return Ok(jurisdiccionesDTO);
            }
            catch(Exception error)
            {
                return BadRequest(error.Message);
            }
        }

        // GET: api/Jurisdicciones/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Jurisdiccion>> GetJurisdiccion(int id)
        {
            var jurisdiccion = await _context.Jurisdiccion.FindAsync(id);

            if (jurisdiccion == null)
            {
                return NotFound();
            }

            return jurisdiccion;
        }

        // PUT: api/Jurisdicciones/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutJurisdiccion(int id, Jurisdiccion jurisdiccion)
        {
            if (id != jurisdiccion.Id)
            {
                return BadRequest();
            }

            _context.Entry(jurisdiccion).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JurisdiccionExists(id))
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

        // POST: api/Jurisdicciones
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Jurisdiccion>> PostJurisdiccion(Jurisdiccion jurisdiccion)
        {
            _context.Jurisdiccion.Add(jurisdiccion);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetJurisdiccion", new { id = jurisdiccion.Id }, jurisdiccion);
        }

        // DELETE: api/Jurisdicciones/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Jurisdiccion>> DeleteJurisdiccion(int id)
        {
            var jurisdiccion = await _context.Jurisdiccion.FindAsync(id);
            if (jurisdiccion == null)
            {
                return NotFound();
            }

            _context.Jurisdiccion.Remove(jurisdiccion);
            await _context.SaveChangesAsync();

            return jurisdiccion;
        }

        private bool JurisdiccionExists(int id)
        {
            return _context.Jurisdiccion.Any(e => e.Id == id);
        }
    }
}
