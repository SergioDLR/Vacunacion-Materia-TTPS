using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VacunacionApi.DTO
{
    public class DetalleMesAnioDTO
    {
        public DetalleMesAnioDTO(int numeroMes, int numeroAnio, int aplicadas)
        {
            NumeroMes = numeroMes;
            NumeroAnio = numeroAnio;
            Aplicadas = aplicadas;
        }

        public int NumeroMes { get; set; }
        public int NumeroAnio { get; set; }
        public int Aplicadas { get; set; }
    }
}
