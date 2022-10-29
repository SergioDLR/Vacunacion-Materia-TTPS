using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VacunacionApi.DTO
{
    public class ResponseListaDistribucionesDTO : ResponseCabeceraDTO
    {
        public ResponseListaDistribucionesDTO()
        {

        }

        public ResponseListaDistribucionesDTO(string estadoTransaccion, bool existenciaErrores, List<string> errores, List<DistribucionDTO> distribuciones)
        {
            EstadoTransaccion = estadoTransaccion;
            ExistenciaErrores = existenciaErrores;
            Errores = errores;
            Distribuciones = distribuciones;
        }

        public List<DistribucionDTO> Distribuciones { get; set; }
    }
}
