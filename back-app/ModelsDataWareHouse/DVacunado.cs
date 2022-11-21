using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace VacunacionApi.ModelsDataWareHouse
{
    [Table("D_Vacunado")]
    public partial class DVacunado
    {
        public DVacunado()
        {
            HVacunados = new HashSet<HVacunados>();
        }

        [Key]
        public int Id { get; set; }
        public int Anio { get; set; }
        public int Decadas { get; set; }
        public int Veintenas { get; set; }
        [Required]
        [Column("Sexo_Biologico")]
        [StringLength(50)]
        public string SexoBiologico { get; set; }

        [InverseProperty("IdVacunadoNavigation")]
        public virtual ICollection<HVacunados> HVacunados { get; set; }
    }
}
