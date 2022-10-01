using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VacunacionApi.Models
{
    public partial class Regla
    {
        public Regla()
        {
            EntidadDosisRegla = new HashSet<EntidadDosisRegla>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(250)]
        public string Descripcion { get; set; }
        [Column("Meses_Vacunacion")]
        [StringLength(250)]
        public string MesesVacunacion { get; set; }
        [Column("Edad_Minima_Meses")]
        public int? EdadMinimaMeses { get; set; }
        [Column("Intervalo_Minimo_Meses")]
        public int? IntervaloMinimoMeses { get; set; }
        [Column("Otra_Regla")]
        [StringLength(250)]
        public string OtraRegla { get; set; }

        [InverseProperty("IdReglaNavigation")]
        public virtual ICollection<EntidadDosisRegla> EntidadDosisRegla { get; set; }
    }
}
