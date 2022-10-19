using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VacunacionApi.DTO
{
    public class VacunaCalendarioDTO
    {
        public VacunaCalendarioDTO(int id, string descripcion, List<DosisDTO> dosis)
        {
            Id = id;
            Descripcion = descripcion;
            Dosis = dosis;
        }

        public int Id { get; set; }
        public string Descripcion { get; set; }
        public List<DosisDTO> Dosis { get; set; }
    }
}
