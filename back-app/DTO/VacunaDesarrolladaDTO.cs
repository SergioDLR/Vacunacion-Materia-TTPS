using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace VacunacionApi.DTO
{
    public class VacunaDesarrolladaDTO
    {
        public VacunaDesarrolladaDTO() { }

        public VacunaDesarrolladaDTO(int id, int idVacuna, string descripcionVacuna, int idMarcaComercial, string descripcionMarcaComercial, int diasDeDemora, double precioVacunaDesarrollada, DateTime? fechaHasta)
        {
            Id = id;
            IdVacuna = idVacuna;
            DescripcionVacuna = descripcionVacuna;
            IdMarcaComercial = idMarcaComercial;
            DescripcionMarcaComercial = descripcionMarcaComercial;  
            DiasDemoraEntrega = diasDeDemora;
            PrecioVacuna = precioVacunaDesarrollada;
            FechaDesde = DateTime.Now;

            if (fechaHasta != null)
                FechaHasta = fechaHasta;
            else
                FechaHasta = null;
        }

        public int Id { get; set; }
        public int IdVacuna { get; set; }
        public string DescripcionVacuna { get; set; }
        public int IdMarcaComercial { get; set; }
        public string DescripcionMarcaComercial { get; set; }
        public int DiasDemoraEntrega { get; set; }
        public double PrecioVacuna { get; set; }
        public DateTime FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }
    }
}