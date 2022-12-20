using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VacunacionApi.Models;

namespace VacunacionApi.Services
{
    public static class EntidadVacunaDosisService
    {
        public static EntidadVacunaDosis GetEntidadVacunaDosis(VacunasContext _context, int idVacuna, int idDosis)
        {
            return _context.EntidadVacunaDosis
                .Where(evd => evd.IdVacuna == idVacuna && evd.IdDosis == idDosis).FirstOrDefault();
        }
    }
}
