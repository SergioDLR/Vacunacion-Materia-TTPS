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
    public class RolesController : ControllerBase
    {
        private readonly VacunasContext _context;

        public RolesController(VacunasContext context)
        {
            _context = context;
        }

        // GET: api/Roles/GetAll?idJurisdiccion=3
        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult<IEnumerable<RolDTO>>> GetAll(int idJurisdiccion = 0)
        {
            try
            {
                List<Rol> roles = new List<Rol>();

                if (idJurisdiccion != 0)
                {
                    Jurisdiccion jurisdiccion = await _context.Jurisdiccion.Where(jur => jur.Id == idJurisdiccion).FirstOrDefaultAsync();
                    if (jurisdiccion != null)
                    {
                        if (jurisdiccion.Descripcion == "Nación")
                        {
                            roles = await _context.Rol.Where(r => r.Descripcion == "Operador Nacional").ToListAsync();
                        }
                        else
                            roles = await _context.Rol.Where(r => r.Descripcion != "Operador Nacional").ToListAsync();
                    }
                }
                else
                    roles = await _context.Rol.ToListAsync();

                List<RolDTO> rolesDTO = new List<RolDTO>();

                foreach (Rol rol in roles)
                {
                    RolDTO rolDTO = new RolDTO(rol.Id, rol.Descripcion);
                    rolesDTO.Add(rolDTO);
                }

                return Ok(rolesDTO);
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }

        // GET: api/Roles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Rol>> GetRol(int id)
        {
            var rol = await _context.Rol.FindAsync(id);

            if (rol == null)
            {
                return NotFound();
            }

            return rol;
        }

        // PUT: api/Roles/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRol(int id, Rol rol)
        {
            if (id != rol.Id)
            {
                return BadRequest();
            }

            _context.Entry(rol).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RolExists(id))
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

        // POST: api/Roles
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Rol>> PostRol(Rol rol)
        {
            _context.Rol.Add(rol);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRol", new { id = rol.Id }, rol);
        }

        // DELETE: api/Roles/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Rol>> DeleteRol(int id)
        {
            var rol = await _context.Rol.FindAsync(id);
            if (rol == null)
            {
                return NotFound();
            }

            _context.Rol.Remove(rol);
            await _context.SaveChangesAsync();

            return rol;
        }

        private bool RolExists(int id)
        {
            return _context.Rol.Any(e => e.Id == id);
        }
    }
}
