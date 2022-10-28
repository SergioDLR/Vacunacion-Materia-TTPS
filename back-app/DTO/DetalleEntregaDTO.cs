using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VacunacionApi.DTO
{
    public class DetalleEntregaDTO
    {
        public DetalleEntregaDTO(int idVacuna, int idVacunaDesarrollada, string descripcionVacuna, 
            string descripcionVacunaDesarrollada, int cantidadVacunas, int idLote, DateTime fechaVencimientoLote)
        {
            IdVacuna = idVacuna;
            IdVacunaDesarrollada = idVacunaDesarrollada;
            DescripcionVacuna = descripcionVacuna;
            DescripcionVacunaDesarrollada = descripcionVacunaDesarrollada;
            CantidadVacunas = cantidadVacunas;
            IdLote = idLote;
            FechaVencimientoLote = fechaVencimientoLote;
        }

        public int IdVacuna { get; set; }
        public int IdVacunaDesarrollada { get; set; }
        public string DescripcionVacuna { get; set; }
        public string DescripcionVacunaDesarrollada { get; set; }
        public int CantidadVacunas { get; set; }
        public int IdLote { get; set; }
        public DateTime FechaVencimientoLote { get; set; }
    }
}
