using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VacunacionApi.Models
{
    public partial class Distribucion
    {
        [Key]
        public int Id { get; set; }
        public int Codigo { get; set; }
        [Column("Id_Jurisdiccion")]
        public int IdJurisdiccion { get; set; }
        [Column("Id_Lote")]
        public int IdLote { get; set; }
        [Column("Fecha_Entrega", TypeName = "date")]
        public DateTime FechaEntrega { get; set; }
        [Column("Cantidad_Vacunas")]
        public int CantidadVacunas { get; set; }
        [Column("Aplicadas")]
        public int Aplicadas { get; set; }
        [Column("Vencidas")]
        public int Vencidas { get; set; }

        [ForeignKey(nameof(IdJurisdiccion))]
        [InverseProperty(nameof(Jurisdiccion.Distribucion))]
        public virtual Jurisdiccion IdJurisdiccionNavigation { get; set; }
        [ForeignKey(nameof(IdLote))]
        [InverseProperty(nameof(Lote.Distribucion))]
        public virtual Lote IdLoteNavigation { get; set; }
    }
}
