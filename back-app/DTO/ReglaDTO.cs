using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VacunacionApi.DTO
{
    public class ReglaDTO
    {
        public ReglaDTO(int id, string descripcion, string mesesVacunacion, int? edadMinimaMeses, int? intervaloMinimoMeses, string otraRegla)
        {
            Id = id;
            Descripcion = descripcion;
            MesesVacunacion = mesesVacunacion;
            EdadMinimaMeses = edadMinimaMeses;
            IntervaloMinimoMeses = intervaloMinimoMeses;
            OtraRegla = otraRegla;
        }

        public int Id { get; set; }
        public string Descripcion { get; set; }
        public string MesesVacunacion { get; set; }
        public int? EdadMinimaMeses { get; set; }
        public int? IntervaloMinimoMeses { get; set; }
        public string OtraRegla { get; set; }
    }
}
