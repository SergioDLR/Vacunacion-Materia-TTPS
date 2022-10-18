namespace VacunacionApi.DTO
{
    public class MarcaComercialDTO
    {
        public MarcaComercialDTO() { }
        public MarcaComercialDTO(int id, string descripcion)
        {
            Id = id;
            Descripcion = descripcion;
        }

        public int Id { get; set; }
        public string Descripcion { get; set; }
    }
}
