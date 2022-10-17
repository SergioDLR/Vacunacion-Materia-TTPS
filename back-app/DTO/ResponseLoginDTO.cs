using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VacunacionApi.DTO
{
    public class ResponseLoginDTO : ResponseCabeceraDTO
    {
        public ResponseLoginDTO(string estadoTransaccion, bool existenciaErrores, List<string> errores, UsuarioDTO usuarioDTO)
        {
            EstadoTransaccion = estadoTransaccion;
            ExistenciaErrores = existenciaErrores;
            Errores = errores;
            UsuarioDTO = usuarioDTO;
        }

        public UsuarioDTO UsuarioDTO { get; set; }
    }
}
