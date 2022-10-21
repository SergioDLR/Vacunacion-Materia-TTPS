using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VacunacionApi.DTO
{
    public class ReglaDTO
    {
        public ReglaDTO(int id, string descripcion, string mesesVacunacion, double? lapsoMinimoDias, double? lapsoMaximoDias, string otros, bool embarazada, bool personalSalud)
        {
            Id = id;
            Descripcion = descripcion;
            MesesVacunacion = mesesVacunacion;
            LapsoMinimoDias = lapsoMinimoDias;
            LapsoMaximoDias = lapsoMaximoDias;
            Otros = otros;
            Embarazada = embarazada;
            PersonalSalud = personalSalud;
        }

        public int Id { get; set; }
        public string Descripcion { get; set; }
        public string MesesVacunacion { get; set; }
        public double? LapsoMinimoDias { get; set; }
        public double? LapsoMaximoDias { get; set; }
        public string Otros { get; set; }
        public bool Embarazada { get; set; }
        public bool PersonalSalud { get; set; }
    }
}
