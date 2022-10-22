using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VacunacionApi.DTO
{
    public class ResponseListaVacunasDTO : ResponseCabeceraDTO
    {
        public ResponseListaVacunasDTO(string estadoTransaccion, bool existenciaErrores, List<string> errores, string emailOperadorNacional, List<VacunaDTO> listaVacunasDTO)
        {
            EstadoTransaccion = estadoTransaccion;
            ExistenciaErrores = existenciaErrores;
            Errores = errores;
            EmailOperadorNacional = emailOperadorNacional;
            ListaVacunasDTO = listaVacunasDTO;
        }

        public string EmailOperadorNacional { get; set; }
        public List<VacunaDTO> ListaVacunasDTO { get; set; }
    }
}
