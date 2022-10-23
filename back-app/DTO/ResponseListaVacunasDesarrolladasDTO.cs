using System.Collections.Generic;
using VacunacionApi.Models;

namespace VacunacionApi.DTO
{
    public class ResponseListaVacunasDesarrolladasDTO : ResponseCabeceraDTO
    {
        public ResponseListaVacunasDesarrolladasDTO (string emailOperadorNacional, string estadoTransaccion, bool existenciaErrores, List<string> errores, List<VacunaDesarrolladaDTO> vacunasDesarrolladasDTO)
        {
            EmailOperadorNacional = emailOperadorNacional;
            EstadoTransaccion = estadoTransaccion;
            ExistenciaErrores = existenciaErrores;
            Errores = errores;
            ListaVacunasDesarrolladasDTO = vacunasDesarrolladasDTO;
        }
        public string EmailOperadorNacional { get; set; }
        public List<VacunaDesarrolladaDTO> ListaVacunasDesarrolladasDTO { get; set; }
    }
}