using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VacunacionApi.Models
{
    [Table("Estado_Compra")]
    public partial class EstadoCompra
    {
        public EstadoCompra()
        {
            Compra = new HashSet<Compra>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Descripcion { get; set; }

        [InverseProperty("IdEstadoCompraNavigation")]
        public virtual ICollection<Compra> Compra { get; set; }
    }
}
