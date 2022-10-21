using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VacunacionApi.Models
{
    [Table("Entidad_Dosis_Regla")]
    public partial class EntidadDosisRegla
    {
        public EntidadDosisRegla(int idDosis, int idRegla)
        {
            IdDosis = idDosis;
            IdRegla = idRegla;
        }

        [Key]
        public int Id { get; set; }
        [Column("Id_Dosis")]
        public int IdDosis { get; set; }
        [Column("Id_Regla")]
        public int IdRegla { get; set; }

        [ForeignKey(nameof(IdDosis))]
        [InverseProperty(nameof(Dosis.EntidadDosisRegla))]
        public virtual Dosis IdDosisNavigation { get; set; }
        [ForeignKey(nameof(IdRegla))]
        [InverseProperty(nameof(Regla.EntidadDosisRegla))]
        public virtual Regla IdReglaNavigation { get; set; }
    }
}
