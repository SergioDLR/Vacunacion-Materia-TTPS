using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VacunacionApi.Models;

namespace VacunacionApi.Services
{
    public static class MarcaComercialService
    {
        public static MarcaComercial GetMarcaComercial(VacunasContext _context, int idMarcaComercial)
        {
            return _context.MarcaComercial
                .Where(mc => mc.Id == idMarcaComercial).FirstOrDefault();
        }

        public static MarcaComercial GetMarcaComercialByDescripcion(VacunasContext _context, string descripcionMarcaComercial)
        {
            return _context.MarcaComercial
                .Where(mc => mc.Descripcion == descripcionMarcaComercial).FirstOrDefault();
        }

        public static bool MarcaComercialExists(VacunasContext _context, int id)
        {
            return _context.MarcaComercial.Any(e => e.Id == id);
        }
    }
}
