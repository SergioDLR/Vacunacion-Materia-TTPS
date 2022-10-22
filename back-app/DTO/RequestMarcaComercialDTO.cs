using System.ComponentModel.DataAnnotations;

namespace VacunacionApi.DTO
{
    public class RequestMarcaComercialDTO
    {
        [Required(ErrorMessage = "El campo email operador nacional es requerido")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "El campo email operador nacional tiene un formato inválido")]
        [StringLength(50, ErrorMessage = "El campo email operador nacional debe tener una longitud máxima de 50 caracteres")]
        public string EmailOperadorNacional { get; set; }

        [Required(ErrorMessage = "El campo marca comercial es requerido")]
        [StringLength(100, ErrorMessage = "El campo marca comercial debe tener una longitud máxima de 100 caracteres")]
        public string DescripcionMarcaComercial { get; set; }
    }
}
