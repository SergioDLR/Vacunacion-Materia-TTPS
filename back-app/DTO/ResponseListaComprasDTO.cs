using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VacunacionApi.DTO
{
    public class ResponseListaComprasDTO : ResponseCabeceraDTO
    {
        public ResponseListaComprasDTO()
        {

        }

        public ResponseListaComprasDTO(string estadoTransaccion, bool existenciaErrores, List<string> errores, List<CompraDTO> compras)
        {
            EstadoTransaccion = estadoTransaccion;
            ExistenciaErrores = existenciaErrores;
            Errores = errores;
            Compras = compras;
        }

        public List<CompraDTO> Compras { get; set; }
    }
}
