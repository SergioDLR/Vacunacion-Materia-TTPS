using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VacunacionApi.DTO
{
    public class DetalleEntregaDTO
    {
        public DetalleEntregaDTO(int idVacuna, int idVacunaDesarrollada, string descripcionVacuna, 
            string descripcionVacunaDesarrollada, int cantidadVacunas, int idLote, DateTime fechaVencimientoLote)
        {
            IdVacuna = idVacuna;
            IdVacunaDesarrollada = idVacunaDesarrollada;
            DescripcionVacuna = descripcionVacuna;
            DescripcionVacunaDesarrollada = descripcionVacunaDesarrollada;
            CantidadVacunas = cantidadVacunas;
            IdLote = idLote;
            FechaVencimientoLote = fechaVencimientoLote;
        }

        [Required(ErrorMessage = "El campo id vacuna es obligatorio")]
        [Range(1, 1000000, ErrorMessage = "El campo id vacuna tiene un formato inválido")]
        public int IdVacuna { get; set; }

        [Range(1, 1000000, ErrorMessage = "El campo id vacuna desarrollada tiene un formato inválido")]
        public int? IdVacunaDesarrollada { get; set; }

        [StringLength(250, ErrorMessage = "El campo descripción vacuna debe tener una longitud máxima de 250 caracteres")]
        public string DescripcionVacuna { get; set; }

        [StringLength(501, ErrorMessage = "El campo descripción vacuna desarrollada debe tener una longitud máxima de 501 caracteres")]
        public string DescripcionVacunaDesarrollada { get; set; }

        [Required(ErrorMessage = "El campo cantidad vacunas es obligatorio")]
        [Range(1, 100000000, ErrorMessage = "El campo cantidad vacunas tiene un formato inválido")]
        public int CantidadVacunas { get; set; }

        [Required(ErrorMessage = "El campo id lote es obligatorio")]
        [Range(1, 1000000, ErrorMessage = "El campo id lote tiene un formato inválido")]
        public int IdLote { get; set; }
        
        public DateTime FechaVencimientoLote { get; set; }
    }
}
