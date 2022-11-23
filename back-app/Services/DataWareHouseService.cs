using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VacunacionApi.ModelsDataWareHouse;

namespace VacunacionApi.Services
{
    public class DataWareHouseService
    {
        public async Task<bool> CargarDataWareHouse(DataWareHouseContext _context)
        {
            string path = @"D:\Libro1.csv";
            string[] lineas = File.ReadAllLines(path);

            for(int i=1; i<=lineas.Count(); i++)
            {
                string[] lista = lineas[i].Split(",");

                //D_TIEMPO
                string[] fechaVacunacion = lista[3].Split("-");
                DTiempo dTiempo = new DTiempo(Convert.ToInt32(fechaVacunacion[2]), Convert.ToInt32(fechaVacunacion[1]), Convert.ToInt32(fechaVacunacion[0]));
                _context.DTiempo.Add(dTiempo);

                //D_LUGAR
                string provincia = lista[2];
                string departamento = lista[7];
                DLugar dLugar = new DLugar(provincia, departamento);
                _context.DLugar.Add(dLugar);

                //D_VACUNA
                string vacunaDesarrollada = lista[0].Split("_")[0];
                string laboratorio = lista[0].Split("_")[1];
                string tipoVacunaDesarrollada = "No registra";
                int idLote = Convert.ToInt32(lista[1]);
                DVacuna dVacuna = new DVacuna(vacunaDesarrollada, laboratorio, tipoVacunaDesarrollada, idLote);
                _context.DVacuna.Add(dVacuna);

                //D_VACUNADO
                string[] fechaNacimiento = lista[5].Split("-");
                int anioActual = DateTime.Now.Year;
                int anio = Convert.ToInt32(fechaNacimiento[0]);
                int decadas = (anioActual - anio) / 10;
                int veintenas = (anioActual - anio) / 20;
                string sexoBiologico = lista[6];
                int dni = Convert.ToInt32(lista[4]);
                DVacunado dVacunado = new DVacunado(anio, decadas, veintenas, sexoBiologico, dni);
                _context.DVacunado.Add(dVacunado);

                //OPERACIÓN DE GUARDADO
                await _context.SaveChangesAsync();

                //H_VACUNADOS
                HVacunados hVacunados = new HVacunados(dTiempo.Id, dLugar.Id, dVacuna.Id, dVacunado.Id);
                _context.HVacunados.Add(hVacunados);
                await _context.SaveChangesAsync();
            }

            return true;
        }
    }
}
