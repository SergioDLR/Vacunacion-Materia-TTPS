using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VacunacionApi.DTO;

namespace VacunacionApi.Models
{
    public partial class Usuario
    {
        public Usuario(RequestUsuarioDTO model)
        {
            Email = model.Email;
            Password = model.Password;
            IdJurisdiccion = model.IdJurisdiccion;
            IdRol = model.IdRol;
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Email { get; set; }
        [Required]
        [StringLength(50)]
        public string Password { get; set; }
        [Column("Id_Jurisdiccion")]
        public int? IdJurisdiccion { get; set; }
        [Column("Id_Rol")]
        public int IdRol { get; set; }

        [ForeignKey(nameof(IdJurisdiccion))]
        [InverseProperty(nameof(Jurisdiccion.Usuario))]
        public virtual Jurisdiccion IdJurisdiccionNavigation { get; set; }
        [ForeignKey(nameof(IdRol))]
        [InverseProperty(nameof(Rol.Usuario))]
        public virtual Rol IdRolNavigation { get; set; }
    }
}
