using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VacunacionApi.DTO
{
    public class JurisdiccionVacunaAplicadaDTO
    {
        public JurisdiccionVacunaAplicadaDTO(int idJurisdiccion, string descripcionJurisdiccion, int cantidadAplicadas)
        {
            IdJurisdiccion = idJurisdiccion;
            DescripcionJurisdiccion = descripcionJurisdiccion;
            CantidadAplicadas = cantidadAplicadas;
        }

        public int IdJurisdiccion { get; set; }
        public string DescripcionJurisdiccion { get; set; }
        public int CantidadAplicadas { get; set; }
    }
}
