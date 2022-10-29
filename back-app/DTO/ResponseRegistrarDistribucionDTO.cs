using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VacunacionApi.DTO
{
    public class ResponseRegistrarDistribucionDTO : ResponseCabeceraDTO
    {
        public ResponseRegistrarDistribucionDTO(string estadoTransaccion, bool existenciaErrores, List<string> errores,
            string emailOperadorNacional, int idJurisdiccion, List<DistribucionDTO> listaDistribuciones)
        {
            EstadoTransaccion = estadoTransaccion;
            ExistenciaErrores = existenciaErrores;
            Errores = errores;
            EmailOperadorNacional = emailOperadorNacional;
            IdJurisdiccion = idJurisdiccion;
            ListaDistribuciones = listaDistribuciones;
        }

        public string EmailOperadorNacional { get; set; }
        public int IdJurisdiccion { get; set; }
        public List<DistribucionDTO> ListaDistribuciones { get; set; }
    }
}
