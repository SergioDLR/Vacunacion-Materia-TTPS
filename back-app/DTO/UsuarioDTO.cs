using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VacunacionApi.DTO
{
    public class UsuarioDTO
    {
        public UsuarioDTO(int id, string email, string password, int idJurisdiccion, int idRol, string descripcionJurisdiccion, string descripcionRol)
        {
            Id = id;
            Email = email;
            Password = password;
            IdJurisdiccion = idJurisdiccion;
            IdRol = idRol;
            DescripcionJurisdiccion = descripcionJurisdiccion;
            DescripcionRol = descripcionRol;
        }

        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int IdJurisdiccion { get; set; }
        public int IdRol { get; set; }
        public string DescripcionJurisdiccion { get; set; }
        public string DescripcionRol { get; set; }
    }
}
