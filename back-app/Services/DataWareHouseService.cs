using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VacunacionApi.DTO;
using VacunacionApi.Models;
using VacunacionApi.ModelsDataWareHouse;

namespace VacunacionApi.Services
{
    public class DataWareHouseService
    {
        //private readonly DataWareHouseContext _context;
               
        //public async Task<bool> CargarDataWareHouse(VacunaAplicada vacunaAplicada, string jurisdiccion, string vacuna, string laboratorio, string tipoVacunaDesarrollada, int idLote)
        //{
        //    //D_TIEMPO
        //    DTiempo dTiempo = new DTiempo(vacunaAplicada.FechaVacunacion.Day, vacunaAplicada.FechaVacunacion.Month, vacunaAplicada.FechaVacunacion.Year);
        //    _context.DTiempo.Add(dTiempo);

        //    //D_LUGAR
        //    DLugar dLugar = new DLugar(jurisdiccion, vacunaAplicada.Departamento);
        //    _context.DLugar.Add(dLugar);

        //    //D_VACUNA
        //    DVacuna dVacuna = new DVacuna(vacuna, laboratorio, tipoVacunaDesarrollada, idLote);
        //    _context.DVacuna.Add(dVacuna);

        //    //D_VACUNADO
        //    int anioActual = DateTime.Now.Year;
        //    int anio = vacunaAplicada.FechaHoraNacimiento.Year;
        //    int decadas = (anioActual - anio) / 10;
        //    int veintenas = (anioActual - anio) / 20;
        //    string sexoBiologico = (vacunaAplicada.SexoBiologico.Equals("Masculino")) ? "M" : "F";
        //    DVacunado dVacunado = new DVacunado(anio, decadas, veintenas, sexoBiologico, vacunaAplicada.Dni);
        //    _context.DVacunado.Add(dVacunado);

        //    //OPERACIÓN DE GUARDADO
        //    await _context.SaveChangesAsync();

        //    //H_VACUNADOS
        //    HVacunados hVacunados = new HVacunados(dTiempo.Id, dLugar.Id, dVacuna.Id, dVacunado.Id);
        //    _context.HVacunados.Add(hVacunados);
        //    await _context.SaveChangesAsync();

        //    return true;
        //}

        public async Task<bool> CargarDataWareHouse2(DataWareHouseContext _context, UsuarioRenaperDTO usuario, string[] vacunas, string tipoVacuna)
        {
            //D_TIEMPO
            DTiempo dTiempo = new DTiempo(Convert.ToDateTime(usuario.fecha_vacunacion).Day, Convert.ToDateTime(usuario.fecha_vacunacion).Month, Convert.ToDateTime(usuario.fecha_vacunacion).Year);
            _context.DTiempo.Add(dTiempo);

            //D_LUGAR
            DLugar dLugar = new DLugar(usuario.jurisdiccion, usuario.ciudad);
            _context.DLugar.Add(dLugar);

            //D_VACUNA
            DVacuna dVacuna = new DVacuna(vacunas[0], vacunas[1], tipoVacuna, usuario.lotes);
            _context.DVacuna.Add(dVacuna);

            //D_VACUNADO
            int anioActual = DateTime.Now.Year;
            int anio = Convert.ToDateTime(usuario.fecha_hora_nacimiento).Year;
            int decadas = (anioActual - anio) / 10;
            int veintenas = (anioActual - anio) / 20;
            DVacunado dVacunado = new DVacunado(anio, decadas, veintenas, usuario.genero, usuario.DNI);
            _context.DVacunado.Add(dVacunado);

            //OPERACIÓN DE GUARDADO
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
