using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VacunacionApi.DTO
{
    public class UsuarioRenaperDTO
    {
        public int DNI { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string genero { get; set; }
        public string jurisdiccion { get; set; }
        public bool embarazada { get; set; }
        public bool personal_salud { get; set; }
    }
}
