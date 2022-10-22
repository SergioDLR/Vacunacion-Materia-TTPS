using System.Collections.Generic;

namespace VacunacionApi.DTO
{
    public class ResponseCabeceraDTO
    {
        public string EstadoTransaccion { get; set; }
        public bool ExistenciaErrores { get; set; }
        public List<string> Errores { get; set; }
    }
}