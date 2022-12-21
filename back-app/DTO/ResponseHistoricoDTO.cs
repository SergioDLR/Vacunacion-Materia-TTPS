using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VacunacionApi.DTO
{
    public class ResponseHistoricoDTO: ResponseCabeceraDTO
    {
        public ResponseHistoricoDTO(string estadoTransaccion, bool existenciaErrores, List<string> errores, string jurisdiccion, int idLote, 
            string vacunaDesarrollada, int totalAplicadas, int saldoTotal, List<DetalleMesAnioDTO> detallesMesesAnios, string email)
        {
            EstadoTransaccion = estadoTransaccion;
            ExistenciaErrores = existenciaErrores;
            Errores = errores;
            Jurisdiccion = jurisdiccion;
            IdLote = idLote;
            VacunaDesarrollada = vacunaDesarrollada;
            TotalAplicadas = totalAplicadas;
            SaldoTotal = saldoTotal;
            DetallesMesesAnios = detallesMesesAnios;
            Email = email;
        }

        public string Jurisdiccion { get; set; }
        public int IdLote { get; set; }
        public string VacunaDesarrollada { get; set; }
        public int TotalAplicadas { get; set; }
        public int SaldoTotal { get; set; }
        public List<DetalleMesAnioDTO> DetallesMesesAnios { get; set; }
        public string Email { get; set; }
    }
}
