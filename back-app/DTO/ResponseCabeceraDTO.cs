using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VacunacionApi.DTO
{
    public class ResponseCabeceraDTO
    {
        public string EstadoTransaccion { get; set; }
        public bool ExistenciaErrores { get; set; }
        public List<string> Errores { get; set; }
    }
}
