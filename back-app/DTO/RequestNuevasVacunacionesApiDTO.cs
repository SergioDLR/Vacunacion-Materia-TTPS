using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VacunacionApi.DTO
{
    public class RequestNuevasVacunacionesApiDTO
    {
        public string Email { get; set; }
        public List<UsuarioRenaperDTO> Usuarios { get; set; }
    }
}
