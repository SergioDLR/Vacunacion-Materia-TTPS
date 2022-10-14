using System.Collections.Generic;
using VacunacionApi.DTO;

namespace VacunacionApi.Controllers
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
