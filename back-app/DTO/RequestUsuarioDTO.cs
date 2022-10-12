using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VacunacionApi.DTO 
{
    public class RequestUsuarioDTO
    {
        [Required(ErrorMessage = "El campo email administrador es obligatorio")]
        [StringLength(50, ErrorMessage = "El campo email administrador debe tener una longitud máxima de 50 caracteres")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "El campo email administrador tiene un formato inválido")]
        public string EmailAdministrador { get; set; }

        [Required(ErrorMessage = "El campo email es obligatorio")]
        [StringLength(50, ErrorMessage = "El campo email debe tener una longitud máxima de 50 caracteres")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "El campo email tiene un formato inválido")]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "El campo password es obligatorio")]
        [StringLength(50, ErrorMessage = "El campo password debe tener una longitud máxima de 50 caracteres")]
        public string Password { get; set; }

        [Required(ErrorMessage = "El campo id jurisdicción es obligatorio")]
        [Range(1, 10000, ErrorMessage = "El campo id jurisdicción tiene un formato inválido")]
        public int IdJurisdiccion { get; set; }

        [Required(ErrorMessage = "El campo id rol es obligatorio")]
        [Range(1, 10000, ErrorMessage = "El campo id rol tiene un formato inválido")]
        public int IdRol { get; set; }
    }
}
