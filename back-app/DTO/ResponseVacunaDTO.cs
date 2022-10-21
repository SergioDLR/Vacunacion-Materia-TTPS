using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VacunacionApi.DTO
{
    public class ResponseVacunaDTO : ResponseCabeceraDTO
    {
        public ResponseVacunaDTO()
        {
        }

        public ResponseVacunaDTO(string estadoTransaccion, bool existenciaErrores, List<string> errores, string emailOperadorNacional, VacunaDTO vacunaDTO)
        {
            EstadoTransaccion = estadoTransaccion;
            ExistenciaErrores = existenciaErrores;
            Errores = errores;
            EmailOperadorNacional = emailOperadorNacional;
            VacunaDTO = vacunaDTO;
        }

        public string EmailOperadorNacional { get; set; }
        public VacunaDTO VacunaDTO { get; set; }
    }
}
