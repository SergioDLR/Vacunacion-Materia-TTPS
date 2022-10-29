using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VacunacionApi.DTO
{
    public class DistribucionDTO
    {
        public DistribucionDTO(int id, int codigo, int idJurisdiccion, string descripcionJurisdiccion, int idLote, 
            DateTime fechaEntrega, int cantidadVacunas, int aplicadas, int vencidas)
        {
            Id = id;
            Codigo = codigo;
            IdJurisdiccion = idJurisdiccion;
            DescripcionJurisdiccion = descripcionJurisdiccion; 
            IdLote = idLote;
            FechaEntrega = fechaEntrega;
            CantidadVacunas = cantidadVacunas;
            Aplicadas = aplicadas;
            Vencidas = vencidas;
        }

        public int Id { get; set; }
        public int Codigo { get; set; }
        public int IdJurisdiccion { get; set; }
        public string DescripcionJurisdiccion { get; set; }
        public int IdLote { get; set; }
        public DateTime FechaEntrega { get; set; }
        public int CantidadVacunas { get; set; }
        public int Aplicadas { get; set; }
        public int Vencidas { get; set; }
    }
}
