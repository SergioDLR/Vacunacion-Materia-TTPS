using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VacunacionApi.Models
{
    public partial class Dosis
    {
        public Dosis()
        {
            EntidadDosisRegla = new HashSet<EntidadDosisRegla>();
            EntidadVacunaDosis = new HashSet<EntidadVacunaDosis>();
            VacunaAplicada = new HashSet<VacunaAplicada>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(250)]
        public string Descripción { get; set; }

        [InverseProperty("IdDosisNavigation")]
        public virtual ICollection<EntidadDosisRegla> EntidadDosisRegla { get; set; }
        [InverseProperty("IdDosisNavigation")]
        public virtual ICollection<EntidadVacunaDosis> EntidadVacunaDosis { get; set; }
        [InverseProperty("IdDosisNavigation")]
        public virtual ICollection<VacunaAplicada> VacunaAplicada { get; set; }
    }
}
