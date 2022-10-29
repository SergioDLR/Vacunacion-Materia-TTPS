namespace VacunacionApi.DTO
{
    public class VacunaStockDTO
    {
        public VacunaStockDTO(int idVacunaDesarrollada, string descripcion)
        {
            IdVacunaDesarrollada = idVacunaDesarrollada;
            Descripcion = descripcion;
        }

        public int IdVacunaDesarrollada { get; set; }
        public string Descripcion { get; set; }
    }
}