using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VacunacionApi.DTO
{
    public class ResponseCronDTO
    {
        public ResponseCronDTO(List<CompraCronDTO> comprasRecibidasHoy, List<LoteCronDTO> lotesVencidosHoy)
        {
            ComprasRecibidasHoy = comprasRecibidasHoy;
            LotesVencidosHoy = lotesVencidosHoy;
        }

        public List<CompraCronDTO> ComprasRecibidasHoy { get; set; }
        public List<LoteCronDTO> LotesVencidosHoy { get; set; }
    }
}
