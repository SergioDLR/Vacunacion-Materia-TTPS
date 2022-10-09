using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VacunacionApi.Models
{
    [Table("Vacuna_Desarrollada")]
    public partial class VacunaDesarrollada
    {
        public VacunaDesarrollada()
        {
            Lote = new HashSet<Lote>();
        }

        [Key]
        public int Id { get; set; }
        [Column("Id_Vacuna")]
        public int IdVacuna { get; set; }
        [Column("Id_Marca_Comercial")]
        public int IdMarcaComercial { get; set; }
        [Column("Dias_Demora_Entrega")]
        public int DiasDemoraEntrega { get; set; }
        [Column("Intervalo_Minimo_Meses")]
        public int IntervaloMinimoMeses { get; set; }
        [Column("Precio_Vacuna")]
        public double PrecioVacuna { get; set; }
        [Column("Fecha_Desde", TypeName = "date")]
        public DateTime FechaDesde { get; set; }
        [Column("Fecha_Hasta", TypeName = "date")]
        public DateTime? FechaHasta { get; set; }

        [ForeignKey(nameof(IdMarcaComercial))]
        [InverseProperty(nameof(MarcaComercial.VacunaDesarrollada))]
        public virtual MarcaComercial IdMarcaComercialNavigation { get; set; }
        [ForeignKey(nameof(IdVacuna))]
        [InverseProperty(nameof(Vacuna.VacunaDesarrollada))]
        public virtual Vacuna IdVacunaNavigation { get; set; }
        [InverseProperty("IdVacunaDesarrolladaNavigation")]
        public virtual ICollection<Lote> Lote { get; set; }
    }
}
