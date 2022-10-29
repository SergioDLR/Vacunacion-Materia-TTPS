namespace VacunacionApi.DTO
{
    public class StockJurisdiccionDTO
    {
        public int IdJurisdiccion { get; set; }
        public string DescripcionJurisdiccion { get; set; }
        public StockDTO StockJurisdiccion { get; set; }

        public StockJurisdiccionDTO() { }

        public StockJurisdiccionDTO(int idJurisdiccion, string descripcionJurisdiccion, StockDTO stockJurisdiccion)
        {
            IdJurisdiccion = idJurisdiccion;
            DescripcionJurisdiccion = descripcionJurisdiccion;
            StockJurisdiccion = stockJurisdiccion;
        }
    }
}
