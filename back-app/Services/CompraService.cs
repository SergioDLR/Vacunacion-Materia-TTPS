using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VacunacionApi.Models;

namespace VacunacionApi.Services
{
    public static class CompraService
    {
        public static bool CompraExists(VacunasContext _context, int id)
        {
            return _context.Compra.Any(e => e.Id == id);
        }

        public static Compra GetCompraExistente(VacunasContext _context, int codigoCompra)
        {
            return _context.Compra
                .Where(c => c.Codigo == codigoCompra).FirstOrDefault();
        }
    }
}
