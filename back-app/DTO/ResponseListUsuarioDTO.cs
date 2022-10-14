using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VacunacionApi.DTO
{
    public class ResponseListUsuarioDTO : ResponseCabeceraDTO
    {
        public List<UsuarioDTO> usuariosDTO { get; set; }
    }
}
