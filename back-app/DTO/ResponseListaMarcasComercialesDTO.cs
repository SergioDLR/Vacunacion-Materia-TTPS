using System.Collections.Generic;

namespace VacunacionApi.DTO
{
    public class ResponseListaMarcasComercialesDTO : ResponseCabeceraDTO
    {
        public ResponseListaMarcasComercialesDTO() { }

        public ResponseListaMarcasComercialesDTO(string estadoTransaccion, bool existenciaErrores, List<string> errores, string emailOperadorNacional, List<MarcaComercialDTO> listaMarcasComercialesDTO) 
        {
            EstadoTransaccion = estadoTransaccion; 
            ExistenciaErrores = existenciaErrores; 
            Errores = errores;
            EmailOperadorNacional = emailOperadorNacional;
            ListasMarcasComercialesDTO = listaMarcasComercialesDTO;
        }

        public string EmailOperadorNacional { get; set; }
        public List<MarcaComercialDTO> ListasMarcasComercialesDTO { get; set; }

    }
}
