using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VacunacionApi.Models;

namespace VacunacionApi.Services
{
    public static class LoteService
    {
        public static Lote GetLote(VacunasContext _context, int idLote)
        {
            return _context.Lote
                .Where(l => l.Id == idLote).FirstOrDefault();
        }
    }
}
