using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VacunacionApi.DTO
{
    public class EnvioVacunaDTO
    {
        [Required(ErrorMessage = "El campo id vacuna es obligatorio")]
        [Range(1, 1000000, ErrorMessage = "El campo id vacuna tiene un formato inválido")]
        public int IdVacuna { get; set; }

        [Range(0, 1000000, ErrorMessage = "El campo id vacuna desarrollada tiene un formato inválido")]
        public int? IdVacunaDesarrollada { get; set; }

        [Required(ErrorMessage = "El campo cantidad vacunas es obligatorio")]
        [Range(1, 100000000, ErrorMessage = "El campo cantidad vacunas tiene un formato inválido")]
        public int CantidadVacunas { get; set; }
    }
}
