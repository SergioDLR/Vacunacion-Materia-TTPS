using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VacunacionApi.DTO
{
    public class ResponseCrearVacunaAplicadaDTO : ResponseCabeceraDTO 
    {
        public ResponseCrearVacunaAplicadaDTO(string estadoTransaccion, bool existenciaErrores, List<string> errores, int idVacunaAplicada,
            RequestCrearVacunaAplicadaDTO request, string descripcionVacuna, string descripcionDosis, string descripcionRegla, string descripcionVacunaDesarrollada)
        {
            EstadoTransaccion = estadoTransaccion;
            ExistenciaErrores = existenciaErrores;
            Errores = errores;
            IdVacunaAplicada = idVacunaAplicada;
            Request = request;
            DescripcionVacuna = descripcionVacuna;
            DescripcionDosis = descripcionDosis;
            DescripcionRegla = descripcionRegla;
            DescripcionVacunaDesarrollada = descripcionVacunaDesarrollada;
        }

        public int IdVacunaAplicada { get; set; }
        public RequestCrearVacunaAplicadaDTO Request { get; set; }
        public string DescripcionVacuna { get; set; }
        public string DescripcionDosis { get; set; }
        public string DescripcionRegla { get; set; }
        public string DescripcionVacunaDesarrollada { get; set; }
    }
}
