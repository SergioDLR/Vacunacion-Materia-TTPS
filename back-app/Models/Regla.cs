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
        public Regla(string descripcion, string mesesVacunacion, double? lapsoMinimoDias, double? lapsoMaximoDias, string otros)
        {
            Descripcion = descripcion;
            MesesVacunacion = mesesVacunacion;
            LapsoMinimoDias = lapsoMinimoDias;
            LapsoMaximoDias = lapsoMaximoDias;
            Otros = otros;
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(250)]
        public string Descripcion { get; set; }
        [Column("Meses_Vacunacion")]
        [StringLength(250)]
        public string MesesVacunacion { get; set; }
        [Column("Lapso_Minimo_Dias")]
        public double? LapsoMinimoDias { get; set; }
        [Column("Lapso_Maximo_Dias")]
        public double? LapsoMaximoDias { get; set; }
        [Column("Otros")]
        [StringLength(250)]
        public string Otros { get; set; }

        [InverseProperty("IdReglaNavigation")]
        public virtual ICollection<EntidadDosisRegla> EntidadDosisRegla { get; set; }
    }
}
