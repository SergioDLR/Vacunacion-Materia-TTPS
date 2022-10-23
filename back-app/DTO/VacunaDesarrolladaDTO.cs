using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace VacunacionApi.DTO
{
    public class VacunaDesarrolladaDTO
    {
        public VacunaDesarrolladaDTO() { }

        public VacunaDesarrolladaDTO(int id, int idVacuna, int idMarcaComercial, int diasDeDemora, float precioVacunaDesarrollada, DateTime fechaHasta)
        {
            Id = id;
            IdVacuna = idVacuna;
            IdMarcaComercial = idMarcaComercial;
            DiasDemoraEntrega = diasDeDemora;
            PrecioVacuna = precioVacunaDesarrollada;
            FechaDesde = DateTime.Now;
            FechaHasta = fechaHasta;
        }

        public int Id { get; set; }
        public int IdVacuna { get; set; }
        public int IdMarcaComercial { get; set; }
        public int DiasDemoraEntrega { get; set; }
        public float PrecioVacuna { get; set; }
        public DateTime FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }
    }
}