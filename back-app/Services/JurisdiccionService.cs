using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VacunacionApi.Models;

namespace VacunacionApi.Services
{
    public static class JurisdiccionService
    {
        public static Jurisdiccion GetJurisdiccion(VacunasContext _context, int idJurisdiccion)
        {
            return _context.Jurisdiccion
                .Where(j => j.Id == idJurisdiccion).FirstOrDefault();
        }

        public static Jurisdiccion GetJurisdiccionByDescripcion(VacunasContext _context, string descripcionJurisdiccion)
        {
            return _context.Jurisdiccion
                .Where(j => j.Descripcion == descripcionJurisdiccion).FirstOrDefault();
        }

        public static List<List<string>> VerificarJurisdiccion(VacunasContext _context, List<string> errores, int idJurisdiccion)
        {
            List<List<string>> erroresConcatDescripciones = new List<List<string>>();
            List<string> descripciones = new List<string>();
            Jurisdiccion jurisdiccionExistente = GetJurisdiccion(_context, idJurisdiccion);

            if (jurisdiccionExistente == null)
            {
                errores.Add(string.Format("La jurisdicción con identificador {0} no está registrada en el sistema", idJurisdiccion));
                descripciones.Add(null);
            }
            else
                descripciones.Add(jurisdiccionExistente.Descripcion);

            erroresConcatDescripciones.Add(errores);
            erroresConcatDescripciones.Add(descripciones);

            return erroresConcatDescripciones;
        }
    }
}
