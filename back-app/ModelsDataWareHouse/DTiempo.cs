using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace VacunacionApi.ModelsDataWareHouse
{
    [Table("D_Tiempo")]
    public partial class DTiempo
    {
        public DTiempo()
        {
            HVacunados = new HashSet<HVacunados>();
            HVencidas = new HashSet<HVencidas>();
        }

        [Key]
        public int Id { get; set; }
        public int Dia { get; set; }
        public int Mes { get; set; }
        public int Anio { get; set; }

        [InverseProperty("IdTiempoNavigation")]
        public virtual ICollection<HVacunados> HVacunados { get; set; }
        [InverseProperty("IdTiempoNavigation")]
        public virtual ICollection<HVencidas> HVencidas { get; set; }
    }
}
