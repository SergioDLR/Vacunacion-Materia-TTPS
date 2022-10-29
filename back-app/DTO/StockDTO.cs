using System.Collections.Generic;

namespace VacunacionApi.DTO
{
    public class StockDTO
    {
        public List<TipoVacunaStockDTO> ListaTiposVacuna { get; set; }
        public int Total { get; set; }
        public int TotalVencido { get; set; }
        public int TotalDisponible { get; set; }

        public StockDTO(List<TipoVacunaStockDTO> listaTiposVacuna, int total, int totalVencido, int totalDisponible)
        {
            ListaTiposVacuna = listaTiposVacuna;
            Total = total;
            TotalVencido = totalVencido;
            TotalDisponible = totalDisponible;
        }
    }
}