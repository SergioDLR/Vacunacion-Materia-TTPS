using System;
using System.ComponentModel.DataAnnotations;

namespace VacunacionApi.DTO
{
    public class RequestVacunaDesarrolladaDTO
    {
        [Required(ErrorMessage = "El campo email operador nacional es obligatorio")]
        [StringLength(50, ErrorMessage = "El campo email operador nacional debe tener una longitud máxima de 50 caracteres")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "El campo email operador nacional tiene un formato inválido")]
        public string EmailOperadorNacional { get; set; }


        [Required(ErrorMessage = "El campo id vacuna es obligatorio")]
        [Range(1, 10000, ErrorMessage = "El campo id vacuna tiene un formato inválido")]
        public int IdVacuna { get; set; }


        [Required(ErrorMessage = "El campo marca comercial es obligatorio")]
        public string MarcaComercial { get; set; }

        
        [Required(ErrorMessage = "El campo dias de demora es obligatorio")]
        [Range(0, 10000, ErrorMessage = "El campo dias de demora tiene un formato inválido")]
        public int DiasDemoraEntrega { get; set; }


        [Required(ErrorMessage = "El campo precio de vacuna desarrollada es obligatorio")]
        [RegularExpression(@"^\d{1,12}(?:[.,]\d{1,2})?$", ErrorMessage = "El campo precio de vacuna tiene un formato inválido")]
        public float PrecioVacunaDesarrollada { get; set; }
    }
}