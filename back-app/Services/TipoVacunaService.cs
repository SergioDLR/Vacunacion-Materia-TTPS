using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VacunacionApi.Models;

namespace VacunacionApi.Services
{
    public static class TipoVacunaService
    {
        public static TipoVacuna GetTipoVacuna(VacunasContext _context, int idTipoVacuna)
        {
            return _context.TipoVacuna
                .Where(tv => tv.Id == idTipoVacuna).FirstOrDefault();
        }

        public static TipoVacuna GetTipoVacunaByDescripcion(VacunasContext _context, string tipo)
        {
            return _context.TipoVacuna
                .Where(tv => tv.Descripcion == tipo).FirstOrDefault();
        }

        public static List<List<string>> VerificarTipoVacuna(VacunasContext _context, List<string> errores, int idTipoVacuna)
        {
            List<List<string>> erroresConcatDescripciones = new List<List<string>>();
            List<string> descripciones = new List<string>();
            TipoVacuna tipoVacunaExistente = GetTipoVacuna(_context, idTipoVacuna);

            if (tipoVacunaExistente == null)
            {
                errores.Add(string.Format("El tipo de vacuna con identificador {0} no está registrado en el sistema", idTipoVacuna));
                descripciones.Add(null);
            }
            else
            {
                descripciones.Add(tipoVacunaExistente.Descripcion);
            }

            erroresConcatDescripciones.Add(errores);
            erroresConcatDescripciones.Add(descripciones);

            return erroresConcatDescripciones;
        }
    }
}
