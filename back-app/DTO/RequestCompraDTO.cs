using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VacunacionApi.DTO
{
    public class RequestCompraDTO
    {
        [Required(ErrorMessage = "El campo email operador nacional es obligatorio")]
        [StringLength(50, ErrorMessage = "El campo email operador nacional debe tener una longitud máxima de 50 caracteres")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "El campo email operador nacional tiene un formato inválido")]
        public string EmailOperadorNacional { get; set; }

        [Required(ErrorMessage = "El campo id vacuna desarrollada es obligatorio")]
        [Range(1, 1000000, ErrorMessage = "El campo id vacuna desarrollada tiene un formato inválido")]
        public int IdVacunaDesarrollada { get; set; }

        [Required(ErrorMessage = "El campo cantidad vacunas es obligatorio")]
        [Range(1000, 100000000, ErrorMessage = "El campo cantidad vacunas tiene un formato inválido")]
        public int CantidadVacunas { get; set; }
    }
}
