using System;

namespace VacunacionApi.DTO
{
    public class VacunaStockAnalistaProvincialDTO
    {
        public VacunaStockAnalistaProvincialDTO() { }

        public VacunaStockAnalistaProvincialDTO(int idLote, DateTime fechaVencimientoLote, int idVacuna, int idVacunaDesarrollada, string descripcion)
        {
            IdLote = idLote;
            VencimientoLote = fechaVencimientoLote;
            IdVacuna = idVacuna;
            IdVacunaDesarrollada = idVacunaDesarrollada;
            Descripcion = descripcion;
        }

        public int IdLote { get; set; }
        public DateTime VencimientoLote { get; set; }
        public int IdVacuna { get; set; }
        public int IdVacunaDesarrollada { get; set; }
        public string Descripcion { get; set; }
    }
}