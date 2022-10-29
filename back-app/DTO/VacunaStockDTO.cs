namespace VacunacionApi.DTO
{
    public class VacunaStockDTO
    {
        public VacunaStockDTO(int idVacuna, int idVacunaDesarrollada, string descripcion)
        {
            IdVacuna = idVacuna; 
            IdVacunaDesarrollada = idVacunaDesarrollada;
            Descripcion = descripcion;
        }

        public int IdVacuna { get; set; }
        public int IdVacunaDesarrollada { get; set; }
        public string Descripcion { get; set; }
    }
}