using System.Collections.Generic;
using VacunacionApi.DTO;

namespace VacunacionApi.Controllers
{
    public class ResponseUsuarioDTO : ResponseCabeceraDTO
    {
        public ResponseUsuarioDTO()
        { 
        }

        public ResponseUsuarioDTO(string estadoTransaccion, bool existenciaErrores, List<string> errores, string emailAdministrador, 
            string email, string password, int idJurisdiccion, int idRol, string descripcionJurisdiccion, string descripcionRol)
        {
            EstadoTransaccion = estadoTransaccion;
            ExistenciaErrores = existenciaErrores;
            Errores = errores;
            EmailAdministrador = emailAdministrador;
            Email = email;
            Password = password;
            IdJurisdiccion = idJurisdiccion;
            IdRol = idRol;
            DescripcionJurisdiccion = descripcionJurisdiccion;
            DescripcionRol = descripcionRol;
        }

        public string EmailAdministrador { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int IdJurisdiccion { get; set; }
        public int IdRol { get; set; }
        public string DescripcionJurisdiccion { get; set; }
        public string DescripcionRol { get; set; }
    }
}
