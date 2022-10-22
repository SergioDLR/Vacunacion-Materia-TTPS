using System.Collections.Generic;

namespace VacunacionApi.DTO
{
    public class ResponseMarcaComercialDTO : ResponseCabeceraDTO
    {
        public ResponseMarcaComercialDTO() 
        {
        }
        public ResponseMarcaComercialDTO(string emailOperadorNacional, string estadoTransaccion, bool existenciaErrores, List<string> errores, MarcaComercialDTO marcaComercialDTO)
        {
            EmailOperadorNacional = emailOperadorNacional;
            EstadoTransaccion = estadoTransaccion;
            ExistenciaErrores = existenciaErrores;
            Errores = errores;
            MarcaComercialDTO = marcaComercialDTO;
        }

        public string EmailOperadorNacional { get; set; }
        public MarcaComercialDTO MarcaComercialDTO { get; set; }
    }
}
