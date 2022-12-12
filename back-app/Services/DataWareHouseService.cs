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
        private readonly DataWareHouseContext _context;
               
        public async Task<bool> CargarDataWareHouse(VacunaAplicada vacunaAplicada, string jurisdiccion, string vacuna, string laboratorio, string tipoVacunaDesarrollada, int idLote)
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
            string sexoBiologico = (vacunaAplicada.SexoBiologico.Equals("Masculino")) ? "M" : "F";
            DVacunado dVacunado = new DVacunado(anio, decadas, veintenas, sexoBiologico, vacunaAplicada.Dni);
            _context.DVacunado.Add(dVacunado);

            //OPERACIÓN DE GUARDADO
            await _context.SaveChangesAsync();

            //H_VACUNADOS
            HVacunados hVacunados = new HVacunados(dTiempo.Id, dLugar.Id, dVacuna.Id, dVacunado.Id);
            _context.HVacunados.Add(hVacunados);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
