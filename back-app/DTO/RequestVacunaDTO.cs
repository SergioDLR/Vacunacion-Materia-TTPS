using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VacunacionApi.DTO
{
    public class RequestVacunaDTO
    {
        [Required(ErrorMessage = "El campo email operador nacional es obligatorio")]
        [StringLength(50, ErrorMessage = "El campo email operador nacional debe tener una longitud máxima de 50 caracteres")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "El campo email operador nacional tiene un formato inválido")]
        public string EmailOperadorNacional { get; set; }

        [Required(ErrorMessage = "El campo id tipo vacuna es obligatorio")]
        [Range(1, 10000, ErrorMessage = "El campo id tipo vacuna tiene un formato inválido")]
        public int IdTipoVacuna { get; set; }

        [Required(ErrorMessage = "El campo descripción es obligatorio")]
        [StringLength(250, ErrorMessage = "El campo descripción debe tener una longitud máxima de 250 caracteres")]
        public string Descripcion { get; set; }

        [Range(0, 10000, ErrorMessage = "El campo id pandemia tiene un formato inválido")]
        public int? IdPandemia { get; set; }
    }
}
