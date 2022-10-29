using System;
using System.Reflection.Metadata.Ecma335;

namespace VacunacionApi.DTO
{
    public class LoteStockDTO
    {
        public int Id { get; set; }
        public int CantidadInicialVacunas { get; set; }
        public bool Disponible { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public int CantidadRestante { get; set; }

        public LoteStockDTO(int id, int cantidadInicialVacunas, bool disponible, DateTime fechaVencimiento, int cantidadRestante)
        {
            Id = id;
            CantidadInicialVacunas = cantidadInicialVacunas;
            Disponible = disponible;
            FechaVencimiento = fechaVencimiento;
            CantidadRestante = cantidadRestante;
        }
    }
}