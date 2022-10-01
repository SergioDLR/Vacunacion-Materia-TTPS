using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VacunacionApi.Models
{
    public partial class Pandemia
    {
        public Pandemia()
        {
            Vacuna = new HashSet<Vacuna>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(250)]
        public string Descripcion { get; set; }
        [Column("Fecha_Inicio", TypeName = "date")]
        public DateTime FechaInicio { get; set; }
        [Column("Fecha_Fin", TypeName = "date")]
        public DateTime? FechaFin { get; set; }

        [InverseProperty("IdPandemiaNavigation")]
        public virtual ICollection<Vacuna> Vacuna { get; set; }
    }
}
