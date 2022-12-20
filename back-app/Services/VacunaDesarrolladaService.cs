using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VacunacionApi.Models;

namespace VacunacionApi.Services
{
    public static class VacunaDesarrolladaService
    {
        public static VacunaDesarrollada GetVacunaDesarrollada(VacunasContext _context, int idVacunaDesarrollada)
        {
            return _context.VacunaDesarrollada
                .Where(vac => vac.Id == idVacunaDesarrollada).FirstOrDefault();
        }

        public static bool VacunaDesarrolladaExists(VacunasContext _context, int id)
        {
            return _context.VacunaDesarrollada.Any(e => e.Id == id);
        }
    }
}
