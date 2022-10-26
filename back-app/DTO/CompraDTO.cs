using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VacunacionApi.DTO
{
    public class CompraDTO
    {
        public CompraDTO(int id, int idLote, DateTime? fechaVencimientoLote, int idVacunaDesarrollada, string descripcionVacunaDesarrollada, int idEstadoCompra,
            string descripcionEstadoCompra, int cantidadVacunas, int codigo, DateTime? fechaCompra, DateTime? fechaEntrega)
        {
            Id = id;
            IdLote = idLote;
            FechaVencimientoLote = fechaVencimientoLote;
            IdVacunaDesarrollada = idVacunaDesarrollada;
            DescripcionVacunaDesarrollada = descripcionVacunaDesarrollada;
            IdEstadoCompra = idEstadoCompra;
            DescripcionEstadoCompra = descripcionEstadoCompra;
            CantidadVacunas = cantidadVacunas;
            Codigo = codigo;
            FechaCompra = fechaCompra;
            FechaEntrega = fechaEntrega;
        }

        public int Id { get; set; }
        public int IdLote { get; set; }
        public DateTime? FechaVencimientoLote { get; set; }
        public int IdVacunaDesarrollada { get; set; }
        public string DescripcionVacunaDesarrollada { get; set; }
        public int IdEstadoCompra { get; set; }
        public string DescripcionEstadoCompra { get; set; }
        public int CantidadVacunas { get; set; }
        public int Codigo { get; set; }
        public DateTime? FechaCompra { get; set; }
        public DateTime? FechaEntrega { get; set; }
    }
}
