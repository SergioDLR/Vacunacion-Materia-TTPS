using System.Collections.Generic;

namespace VacunacionApi.DTO
{
    public class ResponseListaVacunasAplicadasDTO : ResponseCabeceraDTO
    {
        public ResponseListaVacunasAplicadasDTO(){}

        public ResponseListaVacunasAplicadasDTO(string estadoTransaccion, bool existenciaErrores, List<string> errores, string emailUsuario, List<VacunaAplicadaConsultaDTO> listaVacunasAplicadasDTO)
        {
            EstadoTransaccion = estadoTransaccion;  
            ExistenciaErrores = existenciaErrores;  
            Errores = errores;
            EmailUsuario = emailUsuario;    
            ListaVacunasAplicadasDTO = listaVacunasAplicadasDTO;
        }

        public string EmailUsuario { get; set; }
        public List<VacunaAplicadaConsultaDTO> ListaVacunasAplicadasDTO { get; set; }
    }
}