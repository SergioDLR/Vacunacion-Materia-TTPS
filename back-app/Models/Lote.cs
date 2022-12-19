using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VacunacionApi.Models
{
    public partial class Lote
    {
        public Lote()
        {
            Compra = new HashSet<Compra>();
            Distribucion = new HashSet<Distribucion>();
            VacunaAplicada = new HashSet<VacunaAplicada>();
        }

        public Lote(int idVacunaDesarrollada, DateTime fechaVencimiento)
        {
            IdVacunaDesarrollada = idVacunaDesarrollada;
            FechaVencimiento = fechaVencimiento;
            Disponible = false;
        }

        [Key]
        public int Id { get; set; }
        [Column("Id_Vacuna_Desarrollada")]
        public int IdVacunaDesarrollada { get; set; }
        [Column("Fecha_Vencimiento", TypeName = "date")]
        public DateTime FechaVencimiento { get; set; }
        [Column("Disponible")]
        public bool Disponible { get; set; }
        [Column("Lotes")]
        public int Lotes { get; set; }

        [ForeignKey(nameof(IdVacunaDesarrollada))]
        [InverseProperty(nameof(VacunaDesarrollada.Lote))]
        public virtual VacunaDesarrollada IdVacunaDesarrolladaNavigation { get; set; }
        [InverseProperty("IdLoteNavigation")]
        public virtual ICollection<Compra> Compra { get; set; }
        [InverseProperty("IdLoteNavigation")]
        public virtual ICollection<Distribucion> Distribucion { get; set; }
        [InverseProperty("IdLoteNavigation")]
        public virtual ICollection<VacunaAplicada> VacunaAplicada { get; set; }
    }
}
