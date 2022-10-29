using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VacunacionApi.DTO
{
    public class ResponseDistribucionDTO : ResponseCabeceraDTO
    {
        public ResponseDistribucionDTO(string estadoTransaccion, bool existenciaErrores, List<string> errores,
            string emailOperadorNacional, int idJurisdiccion, List<SolicitudEntregaDTO> listaSolicitudesEntregas)
        {
            EstadoTransaccion = estadoTransaccion;
            ExistenciaErrores = existenciaErrores;
            Errores = errores;
            EmailOperadorNacional = emailOperadorNacional;
            IdJurisdiccion = idJurisdiccion;
            ListaSolicitudesEntregas = listaSolicitudesEntregas;
        }

        public string EmailOperadorNacional { get; set; }
        public int IdJurisdiccion { get; set; }
        public List<SolicitudEntregaDTO> ListaSolicitudesEntregas { get; set; }
    }
}
