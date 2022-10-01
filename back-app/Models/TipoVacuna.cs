using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VacunacionApi.Models
{
    [Table("Tipo_Vacuna")]
    public partial class TipoVacuna
    {
        public TipoVacuna()
        {
            Vacuna = new HashSet<Vacuna>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(250)]
        public string Descripcion { get; set; }

        [InverseProperty("IdTipoVacunaNavigation")]
        public virtual ICollection<Vacuna> Vacuna { get; set; }
    }
}
