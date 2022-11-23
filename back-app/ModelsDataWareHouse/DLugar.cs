using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace VacunacionApi.ModelsDataWareHouse
{
    [Table("D_Lugar")]
    public partial class DLugar
    {
        public DLugar(string provincia, string departamento)
        {
            Provincia = provincia;
            Departamento = departamento;
        }

        public DLugar()
        {
            HVacunados = new HashSet<HVacunados>();
            HVencidas = new HashSet<HVencidas>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(250)]
        public string Provincia { get; set; }
        [Required]
        [StringLength(250)]
        public string Departamento { get; set; }

        [InverseProperty("IdLugarNavigation")]
        public virtual ICollection<HVacunados> HVacunados { get; set; }
        [InverseProperty("IdLugarNavigation")]
        public virtual ICollection<HVencidas> HVencidas { get; set; }
    }
}
