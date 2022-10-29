using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VacunacionApi.DTO
{
    public class LoteCronDTO
    {
        public LoteCronDTO(int id, DateTime fechaVencimiento, int idVacuna, string descripcionVacuna, int cantidadVacunas)
        {
            Id = id;
            FechaVencimiento = fechaVencimiento;
            IdVacuna = idVacuna;
            DescripcionVacuna = descripcionVacuna;
            CantidadVacunas = cantidadVacunas;
        }

        public int Id { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public int IdVacuna { get; set; }
        public string DescripcionVacuna { get; set; }
        public int CantidadVacunas { get; set; }
    }
}
