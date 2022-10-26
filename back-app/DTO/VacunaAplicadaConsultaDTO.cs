using System;

namespace VacunacionApi.DTO
{
    public class VacunaAplicadaConsultaDTO
    {
        public VacunaAplicadaConsultaDTO() { }

        public VacunaAplicadaConsultaDTO(int dni, string apellido, string nombre, DateTime fechaDeVacunacion, string descrpcionJurisdiccion, int idLoteVacuna, int idVacunaDesarrollada, string descripcionVacuna, string descripcionMarcaComercial, string descripcionDosis) {
            Dni = dni;  
            Apellido = apellido;    
            Nombre = nombre;    
            FechaVacunacion = fechaDeVacunacion;
            DescripcionJurisdiccion = descrpcionJurisdiccion;   
            IdLoteVacuna = idLoteVacuna;    
            IdVacunaDesarrollada = idVacunaDesarrollada;
            DescripcionVacuna = descripcionVacuna;
            DescripcionMarcaComercial = descripcionMarcaComercial;
            DescripcionDosis = descripcionDosis;    
        }
        public int Dni { get; set; }
        public string Apellido { get; set; }
        public string Nombre { get; set; }
        public DateTime FechaVacunacion { get; set; }
        public string DescripcionJurisdiccion { get; set; }
        public int IdLoteVacuna { get; set; }
        public int IdVacunaDesarrollada { get; set; }
        public string DescripcionVacuna { get; set; }
        public string DescripcionMarcaComercial { get; set; }
        public string DescripcionDosis { get; set; }
    }
}