using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VacunacionApi.DTO
{
    public class DosisDTO
    {
        public DosisDTO(int id, string descripcion, List<ReglaDTO> reglas)
        {
            Id = id;
            Descripcion = descripcion;
            Reglas = reglas;
        }

        public int Id { get; set; }
        public string Descripcion { get; set; }
        public List<ReglaDTO> Reglas { get; set; }
    }
}
