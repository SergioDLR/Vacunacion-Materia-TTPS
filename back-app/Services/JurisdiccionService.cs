using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VacunacionApi.Models;

namespace VacunacionApi.Services
{
    public static class JurisdiccionService
    {
        public static Jurisdiccion GetJurisdiccion(VacunasContext _context, int idJurisdiccion)
        {
            return _context.Jurisdiccion
                .Where(j => j.Id == idJurisdiccion).FirstOrDefault();
        }

        public static Jurisdiccion GetJurisdiccionByDescripcion(VacunasContext _context, string descripcionJurisdiccion)
        {
            return _context.Jurisdiccion
                .Where(j => j.Descripcion == descripcionJurisdiccion).FirstOrDefault();
        }
    }
}
