using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VacunacionApi.DTO
{
    public class ResponseVacunaAplicadaDTO : ResponseCabeceraDTO
    {
        public ResponseVacunaAplicadaDTO()
        {

        }

        public ResponseVacunaAplicadaDTO(string estadoTransaccion, bool existenciaErrores, List<string> errores, RequestVacunaAplicadaDTO request, 
            DosisDTO dosisCorrespondienteAplicacion, List<string> alertasVacunacion, VacunaDesarrolladaVacunacionDTO vacunaDesarrolladaAplicacion)
        {
            EstadoTransaccion = estadoTransaccion;
            ExistenciaErrores = existenciaErrores;
            Errores = errores;
            RequestVacunaAplicada = request;
            DosisCorrespondienteAplicacion = dosisCorrespondienteAplicacion;
            AlertasVacunacion = alertasVacunacion;
            VacunaDesarrolladaAplicacion = vacunaDesarrolladaAplicacion;
        }

        public RequestVacunaAplicadaDTO RequestVacunaAplicada { get; set; }
        public DosisDTO DosisCorrespondienteAplicacion { get; set; }
        public List<string> AlertasVacunacion { get; set; }
        public VacunaDesarrolladaVacunacionDTO VacunaDesarrolladaAplicacion { get; set; }
    }
}
