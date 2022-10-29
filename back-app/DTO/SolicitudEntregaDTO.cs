using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VacunacionApi.DTO
{
    public class SolicitudEntregaDTO
    {
        public SolicitudEntregaDTO(EnvioVacunaDTO solicitud, List<string> alertas, int cantidadEntrega, List<DetalleEntregaDTO> listaDetallesEntregas)
        {
            Solicitud = solicitud;
            Alertas = alertas;
            CantidadEntrega = cantidadEntrega;
            ListaDetallesEntregas = listaDetallesEntregas;
        }

        public EnvioVacunaDTO Solicitud { get; set; }
        public List<string> Alertas { get; set; }
        public int CantidadEntrega { get; set; }
        public List<DetalleEntregaDTO> ListaDetallesEntregas { get; set; }
    }
}
