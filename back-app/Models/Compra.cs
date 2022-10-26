using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VacunacionApi.Models
{
    public partial class Compra
    {
        public Compra(int idLote, int idEstadoCompra, int cantidadVacunas, int codigo, DateTime fechaEntrega)
        {
            IdLote = idLote;
            IdEstadoCompra = idEstadoCompra;
            CantidadVacunas = cantidadVacunas;
            Codigo = codigo;
            FechaCompra = DateTime.Now;
            FechaEntrega = fechaEntrega;
            Distribuidas = 0;
            Vencidas = 0;
        }

        [Key]
        public int Id { get; set; }
        [Column("Id_Lote")]
        public int IdLote { get; set; }
        [Column("Id_Estado_Compra")]
        public int IdEstadoCompra { get; set; }
        [Column("Cantidad_Vacunas")]
        public int CantidadVacunas { get; set; }
        public int Codigo { get; set; }
        [Column("Fecha_Compra", TypeName = "date")]
        public DateTime FechaCompra { get; set; }
        [Column("Fecha_Entrega", TypeName = "date")]
        public DateTime FechaEntrega { get; set; }
        [Column("Distribuidas")]
        public int Distribuidas { get; set; }
        [Column("Vencidas")]
        public int Vencidas { get; set; }

        [ForeignKey(nameof(IdEstadoCompra))]
        [InverseProperty(nameof(EstadoCompra.Compra))]
        public virtual EstadoCompra IdEstadoCompraNavigation { get; set; }
        [ForeignKey(nameof(IdLote))]
        [InverseProperty(nameof(Lote.Compra))]
        public virtual Lote IdLoteNavigation { get; set; }
    }
}
