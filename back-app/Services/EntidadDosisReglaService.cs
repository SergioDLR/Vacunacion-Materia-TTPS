using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VacunacionApi.Models;

namespace VacunacionApi.Services
{
    public static class EntidadDosisReglaService
    {
        public static EntidadDosisRegla GetEntidadDosisRegla(VacunasContext _context, int idDosis)
        {
            return _context.EntidadDosisRegla
                .Where(edr => edr.IdDosis == idDosis).FirstOrDefault();
        }
    }
}
