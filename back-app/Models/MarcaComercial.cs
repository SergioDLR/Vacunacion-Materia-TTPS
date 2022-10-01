using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VacunacionApi.Models
{
    [Table("Marca_Comercial")]
    public partial class MarcaComercial
    {
        public MarcaComercial()
        {
            VacunaDesarrollada = new HashSet<VacunaDesarrollada>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(250)]
        public string Descripcion { get; set; }

        [InverseProperty("IdMarcaComercialNavigation")]
        public virtual ICollection<VacunaDesarrollada> VacunaDesarrollada { get; set; }
    }
}
