using System.Collections.Generic;
using VacunacionApi.Models;

namespace VacunacionApi.DTO
{
    public class ResponseVacunaDesarrolladaDTO : ResponseCabeceraDTO
    {
        public ResponseVacunaDesarrolladaDTO() { }
        public ResponseVacunaDesarrolladaDTO(string emailOperadorNacional, string estadoTransaccion, bool existenciaErrores, List<string> errores, VacunaDesarrolladaDTO vacunaDesarrolladaDTO)
        {
            EmailOperadorNacional = emailOperadorNacional;
            EstadoTransaccion = estadoTransaccion;
            ExistenciaErrores = existenciaErrores;
            Errores = errores;
            VacunaDesarrolladaDTO = vacunaDesarrolladaDTO;
        }
        public string EmailOperadorNacional { get; set; }
        public VacunaDesarrolladaDTO VacunaDesarrolladaDTO { get; set; }
    }
}
