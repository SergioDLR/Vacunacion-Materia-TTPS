using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VacunacionApi.DTO
{
    public class DosisDTO
    {
        public DosisDTO(int id, int orden, string descripcion, List<ReglaDTO> reglas)
        {
            Id = id;
            Orden = orden;
            Descripcion = descripcion;
            Reglas = reglas;
        }

        public int Id { get; set; }
        public int Orden { get; set; }
        public string Descripcion { get; set; }
        public List<ReglaDTO> Reglas { get; set; }
    }
}
