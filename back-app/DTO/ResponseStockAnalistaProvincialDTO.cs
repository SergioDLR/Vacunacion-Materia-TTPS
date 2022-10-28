using System.Collections.Generic;

namespace VacunacionApi.DTO
{
    public class ResponseStockAnalistaProvincialDTO : ResponseCabeceraDTO
    {
        public StockJurisdiccionDTO StockJurisdiccion { get; set; }
        public string EmailAnalistaProvincial { get; set; }

        public ResponseStockAnalistaProvincialDTO() { }

        public ResponseStockAnalistaProvincialDTO(string estadoTransaccion, bool existenciaErrores, List<string> errores, string emailAnalistaProvincial, StockJurisdiccionDTO stockJurisdiccion)
        {
            EstadoTransaccion = estadoTransaccion;
            ExistenciaErrores = existenciaErrores;
            Errores = errores;
            EmailAnalistaProvincial = emailAnalistaProvincial;  
            StockJurisdiccion = stockJurisdiccion;
        }   
    }
}