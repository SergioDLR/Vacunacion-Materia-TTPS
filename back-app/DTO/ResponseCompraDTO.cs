using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VacunacionApi.DTO
{
    public class ResponseCompraDTO : ResponseCabeceraDTO
    {
        public ResponseCompraDTO(string estadoTransaccion, bool existenciaErrores, List<string> errores, CompraDTO compraDTO)
        {
            EstadoTransaccion = estadoTransaccion;
            ExistenciaErrores = existenciaErrores;
            Errores = errores;
            CompraDTO = compraDTO;
        }

        public CompraDTO CompraDTO { get; set; }
    }
}
