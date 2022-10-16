using System;
using System.Collections.Generic;

namespace VacunacionApi.DTO
{
    public class ResponseUsuarioDTO : ResponseCabeceraDTO
    {
        public ResponseUsuarioDTO(string estadoTransaccion, bool existenciaErrores, List<string> errores, string emailAdministrador, UsuarioDTO usuarioDTO)
        {
            EstadoTransaccion = estadoTransaccion;
            ExistenciaErrores = existenciaErrores;
            Errores = errores;
            EmailAdministrador = emailAdministrador;
            UsuarioDTO = usuarioDTO;
        }

        public string EmailAdministrador { get; set; }
        public UsuarioDTO UsuarioDTO { get; set; }
    }
}
