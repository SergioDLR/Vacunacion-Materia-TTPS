using System.Collections.Generic;

namespace VacunacionApi.DTO
{
    public class ResponseStockNacionalDTO : ResponseCabeceraDTO
    {
        public int TotalNacion { get; set; }
        public int TotalNacionVencido { get; set; }
        public int TotalNacionDisponible { get; set; }
        public List<StockJurisdiccionDTO> StockJurisdicciones { get; set; }
        public string EmailOperadorNacional { get; set; }

        public ResponseStockNacionalDTO(string estadoTransaccion, bool existenciaErrores, List<string> errores, string emailOperadorNacional, int totalNacion, int totalNacionVencido, int totalNacionDisponible, List<StockJurisdiccionDTO> stockJurisdicciones)
        {
            EstadoTransaccion = estadoTransaccion;
            ExistenciaErrores = existenciaErrores;
            Errores = errores;
            EmailOperadorNacional = emailOperadorNacional;
            TotalNacion = totalNacion;
            TotalNacionVencido = totalNacionVencido;
            TotalNacionDisponible = totalNacionDisponible;
            StockJurisdicciones = stockJurisdicciones;
        }   
    }
}