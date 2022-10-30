using System;
using System.Collections.Generic;

namespace VacunacionApi.DTO
{
    public class VacunaStockOperadorNacionalDTO
    {
        public VacunaStockOperadorNacionalDTO() { }

        public VacunaStockOperadorNacionalDTO(int idJurisdiccion, string descripcionJurisdiccion, List<VacunaStockAnalistaProvincialDTO> listaVacunasStockDTOJurisdiccion, int totalJurisdiccion)
        {
            IdJurisdiccion = idJurisdiccion;
            DescripcionJurisdiccion = descripcionJurisdiccion;
            ListaVacunasStockDTOJurisdiccion = listaVacunasStockDTOJurisdiccion;
            TotalJurisdiccionDisponible = totalJurisdiccion;  
        }
        public int IdJurisdiccion { get; set; }
        public string DescripcionJurisdiccion { get; set; }
        public List<VacunaStockAnalistaProvincialDTO> ListaVacunasStockDTOJurisdiccion { get; set; }
        public int TotalJurisdiccionDisponible { get; set; }
    }
}