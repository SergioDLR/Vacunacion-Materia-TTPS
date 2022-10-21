using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VacunacionApi.Models
{
    [Table("Entidad_Vacuna_Dosis")]
    public partial class EntidadVacunaDosis
    {
        public EntidadVacunaDosis()
        { 
        
        }
        public EntidadVacunaDosis(int idVacuna, int idDosis, int? orden)
        {
            IdVacuna = idVacuna;
            IdDosis = idDosis;
            Orden = orden;
        }

        [Key]
        public int Id { get; set; }
        [Column("Id_Vacuna")]
        public int IdVacuna { get; set; }
        public int? Orden { get; set; }
        [Column("Id_Dosis")]
        public int IdDosis { get; set; }

        [ForeignKey(nameof(IdDosis))]
        [InverseProperty(nameof(Dosis.EntidadVacunaDosis))]
        public virtual Dosis IdDosisNavigation { get; set; }
        [ForeignKey(nameof(IdVacuna))]
        [InverseProperty(nameof(Vacuna.EntidadVacunaDosis))]
        public virtual Vacuna IdVacunaNavigation { get; set; }
    }
}
