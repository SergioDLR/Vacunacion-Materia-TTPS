using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VacunacionApi.Models;

namespace VacunacionApi.Services
{
    public static class PandemiaService
    {
        public static Pandemia GetPandemia(VacunasContext _context, int idPandemia)
        {
            return _context.Pandemia
                .Where(p => p.Id == idPandemia).FirstOrDefault();
        }

        public static Pandemia GetPandemiaByDescripcion(VacunasContext _context, string descripcionPandemia)
        {
            return _context.Pandemia
                .Where(p => p.Descripcion == descripcionPandemia).FirstOrDefault();
        }

        public static int CalcularCantidadDosisPandemia(Pandemia pandemia)
        {
            int cantidadDosis = 0;
            DateTime fechaInicio = pandemia.FechaInicio;
            fechaInicio = fechaInicio.AddDays(pandemia.IntervaloMinimoDias);

            while (fechaInicio <= pandemia.FechaFin)
            {
                cantidadDosis++;
                fechaInicio = fechaInicio.AddDays(pandemia.IntervaloMinimoDias);
            }
      
            return cantidadDosis;
        }

        public static List<List<string>> VerificarPandemia(VacunasContext _context, List<string> errores, int idPandemia)
        {
            List<List<string>> erroresConcatDescripciones = new List<List<string>>();
            List<string> descripciones = new List<string>();
            Pandemia pandemiaExistente = GetPandemia(_context, idPandemia);

            if (pandemiaExistente == null)
            {
                errores.Add(string.Format("La pandemia con identificador {0} no está registrada en el sistema", idPandemia));
                descripciones.Add(null);
            }
            else
            {
                descripciones.Add(pandemiaExistente.Descripcion);
            }

            erroresConcatDescripciones.Add(errores);
            erroresConcatDescripciones.Add(descripciones);

            return erroresConcatDescripciones;
        }
    }
}
