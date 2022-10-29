using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VacunacionApi.DTO
{
    public class CompraCronDTO
    {
        public CompraCronDTO(int id, int idLote, DateTime fechaCompra, DateTime fechaRecepcion, int cantidadVacunas, int idEstadoCompra, string descripcionEstadoCompra)
        {
            Id = id;
            IdLote = idLote;
            FechaCompra = fechaCompra;
            FechaRecepcion = fechaRecepcion;
            CantidadVacunas = cantidadVacunas;
            IdEstadoCompra = idEstadoCompra;
            DescripcionEstadoCompra = descripcionEstadoCompra;
        }

        public int Id { get; set; }
        public int IdLote { get; set; }
        public DateTime FechaCompra { get; set; }
        public DateTime FechaRecepcion { get; set; }
        public int CantidadVacunas { get; set; }
        public int IdEstadoCompra { get; set; }
        public string DescripcionEstadoCompra { get; set; }
    }
}
