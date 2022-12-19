using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VacunacionApi.DTO
{
    public class LoteJurisdiccionDTO
    {
        public LoteJurisdiccionDTO(int lote, string vacunaDesarrollada, string laboratorio, string tipoVacunaDesarrollada)
        {
            Lote = lote;
            VacunaDesarrollada = vacunaDesarrollada;
            Laboratorio = laboratorio;
            TipoVacunaDesarrollada = tipoVacunaDesarrollada;
            JurisdiccionesADistribuir = new List<JurisdiccionVacunaAplicadaDTO>();
        }

        public int Lote { get; set; }
        public string VacunaDesarrollada { get; set; }
        public string Laboratorio { get; set; }
        public string TipoVacunaDesarrollada { get; set; }
        public List<JurisdiccionVacunaAplicadaDTO> JurisdiccionesADistribuir { get; set; }
    }
}
