using System.Collections.Generic;

namespace VacunacionApi.DTO
{
    public class VacunaDesarrolladaStockDTO
    {
        public int Id { get; set; }
        public int IdVacuna { get; set; }
        public int IdMarcaComercial { get; set; }
        public string Descripcion { get; set; }
        public List<LoteStockDTO> ListaLotesStock { get; set; }
        public int TotalLotes { get; set; }
        public int TotalLotesVencido { get; set; }
        public int TotalLotesDisponible { get; set; }


        public VacunaDesarrolladaStockDTO(int id, int idVacuna, int idMarcaComercial, string descripcion, List<LoteStockDTO> listaLotesStock, int totalLotes, int totalLotesVencido, int totalLotesDisponible)
        {
            Id = id;
            IdVacuna = idVacuna;
            IdMarcaComercial = idMarcaComercial;
            Descripcion = descripcion;
            ListaLotesStock = listaLotesStock;
            TotalLotes = totalLotes;
            TotalLotesVencido = totalLotesVencido;
            TotalLotesDisponible = totalLotesDisponible;
        }
    }
}