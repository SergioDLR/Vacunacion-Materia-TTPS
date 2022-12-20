using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VacunacionApi.DTO;
using VacunacionApi.Models;

namespace VacunacionApi.Services
{
    public static class DistribucionService
    {
        public static bool DistribucionExists(VacunasContext _context, int id)
        {
            return _context.Distribucion.Any(e => e.Id == id);
        }

        public static Distribucion GetDistribucionExistente(VacunasContext _context, int codigoDistribucion)
        {
            return _context.Distribucion
                .Where(d => d.Codigo == codigoDistribucion).FirstOrDefault();
        }

        public static List<DetalleEntregaDTO> GetListaByListaCompras(VacunasContext _context, List<Compra> listaCompras, int cantidadVacunasDemanda, EnvioVacunaDTO envio)
        {
            List<DetalleEntregaDTO> listaDetallesEntregasDTO = new List<DetalleEntregaDTO>();
            int cantidadTotalDistribuidasLotes = 0;

            foreach (Compra com in listaCompras)
            {
                Lote lote = _context.Lote.Where(l => l.Id == com.IdLote).FirstOrDefault();

                List<Distribucion> distribucionesLote = _context.Distribucion
                    .Where(d => d.IdLote == lote.Id).ToList();

                int cantidadTotalDistribuidas = 0;
                int disponibles = 0;
                int otorgadas = 0;

                foreach (Distribucion distribucion in distribucionesLote)
                {
                    cantidadTotalDistribuidas += distribucion.CantidadVacunas;
                }

                disponibles = com.CantidadVacunas - cantidadTotalDistribuidas;

                if (disponibles >= cantidadVacunasDemanda)
                {
                    otorgadas = cantidadVacunasDemanda - cantidadTotalDistribuidasLotes;
                    cantidadTotalDistribuidasLotes += otorgadas;
                }
                else
                {
                    if (cantidadTotalDistribuidasLotes + disponibles < cantidadVacunasDemanda)
                    {
                        cantidadTotalDistribuidasLotes += disponibles;
                        otorgadas = disponibles;
                    }
                    else
                    {
                        otorgadas = cantidadVacunasDemanda - cantidadTotalDistribuidasLotes;
                        if (otorgadas < 0)
                            otorgadas *= -1;
                        cantidadTotalDistribuidasLotes += otorgadas;
                    }
                }

                if (otorgadas != 0)
                {
                    Vacuna vacunaLote = VacunaService.GetVacuna(_context, envio.IdVacuna);
                    VacunaDesarrollada vacunaDesarrolladaLote = VacunaDesarrolladaService.GetVacunaDesarrollada(_context, lote.IdVacunaDesarrollada);
                    MarcaComercial marcaComercial = MarcaComercialService.GetMarcaComercial(_context, vacunaDesarrolladaLote.IdMarcaComercial);
                    string descripcionVacunaDesarrollada = vacunaLote.Descripcion + " " + marcaComercial.Descripcion;

                    DetalleEntregaDTO detalleEntregaDTO = new DetalleEntregaDTO(vacunaLote.Id, vacunaDesarrolladaLote.Id,
                        vacunaLote.Descripcion, descripcionVacunaDesarrollada, otorgadas, lote.Id, lote.FechaVencimiento);

                    listaDetallesEntregasDTO.Add(detalleEntregaDTO);
                }

                if (cantidadTotalDistribuidasLotes == cantidadVacunasDemanda)
                    break;
            }

            return listaDetallesEntregasDTO;
        }

        public static List<DetalleEntregaDTO> GetListaDetallesEntregas(VacunasContext _context, EnvioVacunaDTO envio)
        {
            List<DetalleEntregaDTO> listaDetallesEntregasDTO = new List<DetalleEntregaDTO>();
                       
            List<Compra> compras = new List<Compra>();

            if (envio.IdVacunaDesarrollada == null || envio.IdVacunaDesarrollada == 0)
            {
                compras = _context.Compra
                    .Where(l => l.IdLoteNavigation.Disponible == true
                        && l.IdLoteNavigation.IdVacunaDesarrolladaNavigation.IdVacuna == envio.IdVacuna
                        && l.IdLoteNavigation.FechaVencimiento > DateTime.Now)
                    .OrderBy(l => l.IdLoteNavigation.FechaVencimiento)
                    .ToList();
            }
            else
            {
                compras = _context.Compra
                    .Where(l => l.IdLoteNavigation.Disponible == true
                        && l.IdLoteNavigation.IdVacunaDesarrollada == envio.IdVacunaDesarrollada
                        && l.IdLoteNavigation.IdVacunaDesarrolladaNavigation.IdVacuna == envio.IdVacuna
                        && l.IdLoteNavigation.FechaVencimiento > DateTime.Now)
                    .OrderBy(l => l.IdLoteNavigation.FechaVencimiento)
                    .ToList();
            }

            listaDetallesEntregasDTO = GetListaByListaCompras(_context, compras, envio.CantidadVacunas, envio);

            if (envio.IdVacunaDesarrollada != null && envio.IdVacunaDesarrollada != 0)
            {
                int cantidadEntrega = 0;
                foreach (DetalleEntregaDTO detalle in listaDetallesEntregasDTO)
                {
                    cantidadEntrega += detalle.CantidadVacunas;
                }
                if (cantidadEntrega < envio.CantidadVacunas)
                {
                    List<Compra> comprasAdicional = _context.Compra
                        .Where(l => l.IdLoteNavigation.Disponible == true
                            && l.IdLoteNavigation.IdVacunaDesarrolladaNavigation.IdVacuna == envio.IdVacuna
                            && l.IdLoteNavigation.FechaVencimiento > DateTime.Now)
                        .OrderBy(l => l.IdLoteNavigation.FechaVencimiento)
                        .ToList();

                    List<DetalleEntregaDTO> listaDetallesAdicional = GetListaByListaCompras(_context, comprasAdicional, (envio.CantidadVacunas - cantidadEntrega), envio);

                    foreach (DetalleEntregaDTO detDTO in listaDetallesAdicional)
                    {
                        if (!listaDetallesEntregasDTO.Any(x => x.IdLote == detDTO.IdLote))
                            listaDetallesEntregasDTO.Add(detDTO);
                    }
                }
            }
            
            return listaDetallesEntregasDTO;
        }
    }
}
