using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VacunacionApi.DTO
{
    public class ResponseListaUsuariosDTO : ResponseCabeceraDTO
    {
        public ResponseListaUsuariosDTO(string estadoTransaccion, bool existenciaErrores, List<string> errores, string emailAdministrador, List<UsuarioDTO> listaUsuariosDTO)
        {
            EstadoTransaccion = estadoTransaccion;
            ExistenciaErrores = existenciaErrores;
            Errores = errores;
            EmailAdministrador = emailAdministrador;
            ListaUsuariosDTO = listaUsuariosDTO;
        }

        public string EmailAdministrador { get; set; }
        public List<UsuarioDTO> ListaUsuariosDTO { get; set; }
    }
}
