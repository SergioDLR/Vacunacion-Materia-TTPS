using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VacunacionApi.Models;
using VacunacionApi.ModelsDataWareHouse;

namespace VacunacionApi.Services
{
    public class DataWareHouseService
    {               
        public async Task<bool> CargarDataWareHouse(DataWareHouseContext _context, VacunaAplicada vacunaAplicada, string jurisdiccion, string vacuna, string laboratorio, string tipoVacunaDesarrollada, int idLote)
        {
            //D_TIEMPO
            DTiempo dTiempo = new DTiempo(vacunaAplicada.FechaVacunacion.Day, vacunaAplicada.FechaVacunacion.Month, vacunaAplicada.FechaVacunacion.Year);
            _context.DTiempo.Add(dTiempo);
                     
            //D_LUGAR
            DLugar dLugar = new DLugar(jurisdiccion, vacunaAplicada.Departamento);
            _context.DLugar.Add(dLugar);
            
            //D_VACUNA
            DVacuna dVacuna = new DVacuna(vacuna, laboratorio, tipoVacunaDesarrollada, idLote);
            _context.DVacuna.Add(dVacuna);
            
            //D_VACUNADO
            int anioActual = DateTime.Now.Year;
            int anio = vacunaAplicada.FechaHoraNacimiento.Year;
            int decadas = (anioActual - anio) / 10;
            int veintenas = (anioActual - anio) / 20;
            string sexoBiologico = vacunaAplicada.SexoBiologico;
            DVacunado dVacunado = new DVacunado(anio, decadas, veintenas, sexoBiologico, vacunaAplicada.Dni);
            _context.DVacunado.Add(dVacunado);
            await _context.SaveChangesAsync();

            //H_VACUNADOS
            HVacunados hVacunados = new HVacunados(dTiempo.Id, dLugar.Id, dVacuna.Id, dVacunado.Id);
            _context.HVacunados.Add(hVacunados);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> CargarVencidasDataWareHouse(DataWareHouseContext _context, int loteVencido)
        {
            //D_TIEMPO
            DTiempo dTiempo = new DTiempo(DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year);
            _context.DTiempo.Add(dTiempo);
            await _context.SaveChangesAsync();
            
            //D_VACUNA
            List<DVacuna> dVacunas = _context.DVacuna.Where(v => v.IdLote == loteVencido).ToList();
            foreach (DVacuna v in dVacunas)
            {
                HVacunados vacunado = await _context.HVacunados.Where(hv => hv.IdVacuna == v.Id).FirstOrDefaultAsync();
                if (vacunado != null)
                {
                    DLugar dLugar = await _context.DLugar.Where(l => l.Id == vacunado.IdLugar).FirstOrDefaultAsync();
                    HVencidas hVencida = new HVencidas();
                    hVencida.IdTiempo = dTiempo.Id;
                    hVencida.IdLugar = dLugar.Id;
                    hVencida.IdVacuna = v.Id;
                    _context.HVencidas.Add(hVencida);
                    await _context.SaveChangesAsync();
                }
            }
           
            return true;
        }
    }
}
