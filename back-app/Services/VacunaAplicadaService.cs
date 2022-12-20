using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VacunacionApi.Models;

namespace VacunacionApi.Services
{
    public static class VacunaAplicadaService
    {
        public static bool VacunaAplicadaExists(VacunasContext _context, int id)
        {
            return _context.VacunaAplicada.Any(e => e.Id == id);
        }
    }
}
