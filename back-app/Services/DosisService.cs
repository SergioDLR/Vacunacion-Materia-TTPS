using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VacunacionApi.Models;

namespace VacunacionApi.Services
{
    public static class DosisService
    {
        public static Dosis GetDosis(VacunasContext _context, int idDosis)
        {
            return _context.Dosis
                .Where(d => d.Id == idDosis).FirstOrDefault();
        }
    }
}
