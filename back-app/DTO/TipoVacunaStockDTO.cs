using System.Collections.Generic;

namespace VacunacionApi.DTO
{
    public class TipoVacunaStockDTO
    {
        public int Id { get; set; }
        public string DescripcionTipoVacuna { get; set; }
        public List<VacunaDesarrolladaStockDTO> ListaVacunasDesarrolladas { get; set; }
        public int TotalVacunaDesarrollada { get; set; }
        public int TotalVacunaDesarrolladaVencida { get; set; }
        public int TotalVacunaDesarrolladaDisponible { get; set; }

        public TipoVacunaStockDTO(int id, string descripcionTipoVacuna, List<VacunaDesarrolladaStockDTO> listaVacunasDesarrolladas, int totalVacunaDesarrollada, int totalVacunaDesarrolladaVencida, int totalVacunaDesarrolladaDisponible)
        {
            Id = id;
            DescripcionTipoVacuna = descripcionTipoVacuna;
            ListaVacunasDesarrolladas = listaVacunasDesarrolladas;
            TotalVacunaDesarrollada = totalVacunaDesarrollada;
            TotalVacunaDesarrolladaVencida = totalVacunaDesarrolladaVencida;
            TotalVacunaDesarrolladaDisponible = totalVacunaDesarrolladaDisponible;
        }   
    }
}
