using System.Collections.Generic;

namespace VacunacionApi.DTO
{
    public class VacunaStockNacionDTO
    {
        public VacunaStockNacionDTO() { }

        public VacunaStockNacionDTO(List<VacunaStockOperadorNacionalDTO> vacunasStockOperadorNacionalDTO, int totalVacunasDisponibleNacion)
        {
            VacunasStockOperadorNacionalDTO = vacunasStockOperadorNacionalDTO;
            TotalVacunasDisponibleNacion = totalVacunasDisponibleNacion;
        }   

        public List<VacunaStockOperadorNacionalDTO> VacunasStockOperadorNacionalDTO { get; set; }
        public int TotalVacunasDisponibleNacion { get; set; }
    }
}