using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace VacunacionApi.Models
{
    public partial class Departamento
    {
        public Departamento()
        {
            VacunaAplicada = new HashSet<VacunaAplicada>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(250)]
        public string Descripcion { get; set; }
        [Column("Id_Jurisdiccion")]
        public int IdJurisdiccion { get; set; }

        [ForeignKey(nameof(IdJurisdiccion))]
        [InverseProperty(nameof(Jurisdiccion.Departamento))]
        public virtual Jurisdiccion IdJurisdiccionNavigation { get; set; }
        [InverseProperty("IdDepartamentoNavigation")]
        public virtual ICollection<VacunaAplicada> VacunaAplicada { get; set; }
    }
}
