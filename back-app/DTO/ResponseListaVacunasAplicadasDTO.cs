using System.Collections.Generic;

namespace VacunacionApi.DTO
{
    public class ResponseListaVacunasAplicadasDTO : ResponseCabeceraDTO
    {
        public ResponseListaVacunasAplicadasDTO(){}

        public ResponseListaVacunasAplicadasDTO(string estadoTransaccion, bool existenciaErrores, List<string> errores, string emailUsuario, List<VacunaAplicadaDTO> listaVacunasAplicadasDTO)
        {
            EstadoTransaccion = estadoTransaccion;  
            ExistenciaErrores = existenciaErrores;  
            Errores = errores;
            EmailUsuario = emailUsuario;    
            ListaVacunasAplicadasDTO = listaVacunasAplicadasDTO;
        }

        public string EmailUsuario { get; set; }
        public List<VacunaAplicadaDTO> ListaVacunasAplicadasDTO { get; set; }
    }
}