using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VacunacionApi.Models
{
    public partial class Vacuna
    {
        public Vacuna()
        {
            EntidadVacunaDosis = new HashSet<EntidadVacunaDosis>();
            VacunaDesarrollada = new HashSet<VacunaDesarrollada>();
        }

        [Key]
        public int Id { get; set; }
        [Column("Id_Tipo_Vacuna")]
        public int IdTipoVacuna { get; set; }
        [Required]
        [StringLength(250)]
        public string Descripcion { get; set; }
        [Column("Id_Pandemia")]
        public int? IdPandemia { get; set; }
        [Column("Cantidad_Dosis")]
        public int CantidadDosis { get; set; }

        [ForeignKey(nameof(IdPandemia))]
        [InverseProperty(nameof(Pandemia.Vacuna))]
        public virtual Pandemia IdPandemiaNavigation { get; set; }
        [ForeignKey(nameof(IdTipoVacuna))]
        [InverseProperty(nameof(TipoVacuna.Vacuna))]
        public virtual TipoVacuna IdTipoVacunaNavigation { get; set; }
        [InverseProperty("IdVacunaNavigation")]
        public virtual ICollection<EntidadVacunaDosis> EntidadVacunaDosis { get; set; }
        [InverseProperty("IdVacunaNavigation")]
        public virtual ICollection<VacunaDesarrollada> VacunaDesarrollada { get; set; }
    }
}
