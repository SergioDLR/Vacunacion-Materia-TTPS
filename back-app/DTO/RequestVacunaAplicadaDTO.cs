using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VacunacionApi.DTO
{
    public class RequestVacunaAplicadaDTO
    {
        [Required(ErrorMessage = "El campo email vacunador es obligatorio")]
        [StringLength(50, ErrorMessage = "El campo email vacunador debe tener una longitud máxima de 50 caracteres")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "El campo email vacunador tiene un formato inválido")]
        public string EmailVacunador { get; set; }

        [Required(ErrorMessage = "El campo dni es obligatorio")]
        [Range(1000000, 100000000, ErrorMessage = "El campo dni tiene un formato inválido")]
        public int Dni { get; set; }

        [Required(ErrorMessage = "El campo sexo biológico es obligatorio")]
        [StringLength(50, ErrorMessage = "El campo sexo biológico debe tener una longitud máxima de 50 caracteres")]
        public string SexoBiologico { get; set; }

        [Required(ErrorMessage = "El campo nombre es obligatorio")]
        [StringLength(250, ErrorMessage = "El campo nombre debe tener una longitud máxima de 250 caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El campo apellido es obligatorio")]
        [StringLength(250, ErrorMessage = "El campo apellido debe tener una longitud máxima de 250 caracteres")]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "El campo embarazada es obligatorio")]
        public bool Embarazada { get; set; }

        [Required(ErrorMessage = "El campo personal de salud es obligatorio")]
        public bool PersonalSalud { get; set; }

        [Required(ErrorMessage = "El campo id vacuna es obligatorio")]
        [Range(1, 1000000, ErrorMessage = "El campo id vacuna tiene un formato inválido")]
        public int IdVacuna { get; set; }

        [Required(ErrorMessage = "El campo id dosis es obligatorio")]
        [Range(1, 1000000, ErrorMessage = "El campo id dosis tiene un formato inválido")]
        public int IdDosis { get; set; }

        [Required(ErrorMessage = "El campo jurisdicción residencia es obligatorio")]
        [StringLength(250, ErrorMessage = "El campo jurisdicción residencia debe tener una longitud máxima de 250 caracteres")]
        public string JurisdiccionResidencia { get; set; }
    }
}
