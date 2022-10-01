using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VacunacionApi.Models
{
    public partial class Jurisdiccion
    {
        public Jurisdiccion()
        {
            Distribucion = new HashSet<Distribucion>();
            Usuario = new HashSet<Usuario>();
            VacunaAplicada = new HashSet<VacunaAplicada>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(250)]
        public string Descripcion { get; set; }
        [Column("Cantidad_Habitantes")]
        public int CantidadHabitantes { get; set; }

        [InverseProperty("IdJurisdiccionNavigation")]
        public virtual ICollection<Distribucion> Distribucion { get; set; }
        [InverseProperty("IdJurisdiccionNavigation")]
        public virtual ICollection<Usuario> Usuario { get; set; }
        [InverseProperty("IdJurisdiccionNavigation")]
        public virtual ICollection<VacunaAplicada> VacunaAplicada { get; set; }
    }
}
