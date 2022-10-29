using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VacunacionApi.DTO
{
    public class RequestRegistrarDistribucionDTO
    {
        [Required(ErrorMessage = "El campo email operador nacional es obligatorio")]
        [StringLength(50, ErrorMessage = "El campo email operador nacional debe tener una longitud máxima de 50 caracteres")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "El campo email operador nacional tiene un formato inválido")]
        public string EmailOperadorNacional { get; set; }

        [Required(ErrorMessage = "El campo id jurisdicción es obligatorio")]
        [Range(1, 1000000, ErrorMessage = "El campo id jurisdicción tiene un formato inválido")]
        public int IdJurisdiccion { get; set; }

        [Required(ErrorMessage = "El campo listado de vacunas a distribuir es obligatorio")]
        public List<DetalleEntregaDTO> ListaDetallesEntregas { get; set; }
    }
}
