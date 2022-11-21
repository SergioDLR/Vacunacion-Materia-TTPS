using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace VacunacionApi.ModelsDataWareHouse
{
    [Table("D_Vacuna")]
    public partial class DVacuna
    {
        public DVacuna()
        {
            HVacunados = new HashSet<HVacunados>();
            HVencidas = new HashSet<HVencidas>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(250)]
        public string Laboratorio { get; set; }
        [Required]
        [Column("Tipo_Vacuna_Desarrollada")]
        [StringLength(250)]
        public string TipoVacunaDesarrollada { get; set; }
        [Column("Id_Lote")]
        public int IdLote { get; set; }
        [Required]
        [Column("Vacuna__Desarrollada")]
        [StringLength(250)]
        public string VacunaDesarrollada { get; set; }

        [InverseProperty("IdVacunaNavigation")]
        public virtual ICollection<HVacunados> HVacunados { get; set; }
        [InverseProperty("IdVacunaNavigation")]
        public virtual ICollection<HVencidas> HVencidas { get; set; }
    }
}
