using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VacunacionApi.Models;

namespace VacunacionApi.Services
{
    public static class ReglaService
    {
        public static Regla GetRegla(VacunasContext _context, int idRegla)
        {
            return _context.Regla
                .Where(r => r.Id == idRegla).FirstOrDefault();
        }
    }
}
