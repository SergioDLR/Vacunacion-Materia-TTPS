using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VacunacionApi.DTO
{
    public class RequestUsuarioUpdateDTO
    {
        [Required(ErrorMessage = "El campo email administrador es obligatorio")]
        [StringLength(50, ErrorMessage = "El campo email administrador debe tener una longitud máxima de 50 caracteres")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "El campo email administrador tiene un formato inválido")]
        public string EmailAdministrador { get; set; }

        [Required(ErrorMessage = "El campo email es obligatorio")]
        [StringLength(50, ErrorMessage = "El campo email debe tener una longitud máxima de 50 caracteres")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "El campo email tiene un formato inválido")]
        public string Email { get; set; }

        [StringLength(50, ErrorMessage = "El campo password nuevo debe tener una longitud máxima de 50 caracteres")]
        public string PasswordNuevo { get; set; }

        [Range(0, 10000, ErrorMessage = "El campo id jurisdicción nuevo tiene un formato inválido")]
        public int IdJurisdiccionNuevo { get; set; }

        [Range(0, 10000, ErrorMessage = "El campo id rol nuevo tiene un formato inválido")]
        public int IdRolNuevo { get; set; }
    }
}
