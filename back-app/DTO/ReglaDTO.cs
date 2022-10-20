using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VacunacionApi.DTO
{
    public class ReglaDTO
    {
        public ReglaDTO(int id, string descripcion, string mesesVacunacion, double? lapsoMinimoDias, double? lapsoMaximoDias, string otros)
        {
            Id = id;
            Descripcion = descripcion;
            MesesVacunacion = mesesVacunacion;
            LapsoMinimoDias = lapsoMinimoDias;
            LapsoMaximoDias = lapsoMaximoDias;
            Otros = otros;
        }

        public int Id { get; set; }
        public string Descripcion { get; set; }
        public string MesesVacunacion { get; set; }
        public double? LapsoMinimoDias { get; set; }
        public double? LapsoMaximoDias { get; set; }
        public string Otros { get; set; }
    }
}
