using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using VacunacionApi.DTO;
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

        private async Task<List<UsuarioRenaperDTO>> GetUsuarios(int desde, int hasta)
        {
            List<UsuarioRenaperDTO> usuarios = new List<UsuarioRenaperDTO>();
            UsuarioRenaperDTO usuario = null;

            using (var httpClient = new HttpClient())
            {
                for (int dni = desde; dni < hasta; dni++)
                {
                    var respuesta = await httpClient.GetAsync("https://api.claudioraverta.com/personas/" + dni);

                    if (respuesta.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var respuestaString = await respuesta.Content.ReadAsStringAsync();
                        usuario = JsonSerializer.Deserialize<UsuarioRenaperDTO>(respuestaString,
                            new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

                        if (usuario != null)
                            usuarios.Add(usuario);
                    }
                }
            }

            return usuarios;
        }

        //GET: api/VacunasAplicadas/GenerarCsv? emailOperadorNacional = juan@gmail.com
        //[HttpGet]
        //[Route("GenerarCsv")]
        public async Task<ResponseCargarVacunaDTO> GenerarCsv(VacunasContext _context, string emailOperadorNacional = null)
        {
                ResponseCargarVacunaDTO response;
                List<string> errores = new List<string>();

                if (emailOperadorNacional == null)
                {
                    errores.Add(string.Format("El email operador nacional es obligatorio"));
                }
                else
                {
                    errores = RolService.VerificarCredencialesUsuario(_context, emailOperadorNacional, errores, "Operador Nacional");
                }

                if (errores.Count > 0)
                    response = new ResponseCargarVacunaDTO("Rechazada", true, errores, emailOperadorNacional);
                else
                {
                    List<UsuarioRenaperDTO> usuarios = new List<UsuarioRenaperDTO>();
                    int id = 1;
                    string separador = ",";
                    List<string> tiposVacunas = new List<string>() { "arnn", "vector_viral", "subunidades_proteicas" };
                    List<string> vacunasArnn = new List<string>();
                    List<string> vacunasVectorViral = new List<string>();
                    List<string> vacunasSubunidadesProteicas = new List<string>();
                    DataWareHouseService servicio = new DataWareHouseService();

                    StringBuilder salidaDLugar = new StringBuilder();
                    StringBuilder salidaDTiempo = new StringBuilder();
                    StringBuilder salidaDVacuna = new StringBuilder();
                    StringBuilder salidaDVacunado = new StringBuilder();

                    string rutaDLugar = @"F:\D_lugar.csv";
                    string rutaDTiempo = @"F:\D_tiempo.csv";
                    string rutaDVacuna = @"F:\D_vacuna.csv";
                    string rutaDVacunado = @"F:\D_vacunado.csv";

                    string lineaDLugar = "id,provincia,departamentos";
                    string lineaDTiempo = "id,dia,mes,anio";
                    string lineaDVacuna = "id,Laboratorio,Tipo_vacuna_desarrollada,id_lote,Vacuna_desarrollada";
                    string lineaDVacunado = "id,anio,decadas,veintenas,sexo_biologico,dni";

                    salidaDLugar.AppendLine(string.Join(separador, lineaDLugar));
                    salidaDTiempo.AppendLine(string.Join(separador, lineaDTiempo));
                    salidaDVacuna.AppendLine(string.Join(separador, lineaDVacuna));
                    salidaDVacunado.AppendLine(string.Join(separador, lineaDVacunado));

                    usuarios = await GetUsuarios(30000000, 31000000);

                    foreach (UsuarioRenaperDTO usuarioRenaper in usuarios)
                    {
                        List<string> tipoVacuna = GenerarCsvService.GetTipoVacuna(tiposVacunas, vacunasArnn, vacunasVectorViral, vacunasSubunidadesProteicas, usuarioRenaper.vacunas);

                        if (tipoVacuna[1] == "No existe")
                        {
                            switch (tipoVacuna[0])
                            {
                                case "arnn":
                                    vacunasArnn.Add(usuarioRenaper.vacunas);
                                    break;
                                case "vector_viral":
                                    vacunasVectorViral.Add(usuarioRenaper.vacunas);
                                    break;
                                case "subunidades_proteicas":
                                    vacunasSubunidadesProteicas.Add(usuarioRenaper.vacunas);
                                    break;
                                default:
                                    break;
                            }
                        }

                        string[] vacunas = usuarioRenaper.vacunas.Split("_");
                        int anioNac = Convert.ToDateTime(usuarioRenaper.fecha_hora_nacimiento).Year;
                        int anioHoy = DateTime.Now.Year;
                        int decadas = (anioHoy - anioNac) / 10;
                        int veintenas = (anioHoy - anioNac) / 20;

                        lineaDLugar = id + "," + usuarioRenaper.jurisdiccion + "," + usuarioRenaper.ciudad;
                        salidaDLugar.AppendLine(string.Join(separador, lineaDLugar));

                        lineaDTiempo = id + "," + Convert.ToDateTime(usuarioRenaper.fecha_vacunacion).Day + "," + Convert.ToDateTime(usuarioRenaper.fecha_vacunacion).Month + "," + Convert.ToDateTime(usuarioRenaper.fecha_vacunacion).Year;
                        salidaDTiempo.AppendLine(string.Join(separador, lineaDTiempo));

                        lineaDVacuna = id + "," + vacunas[1] + "," + tipoVacuna[0] + "," + usuarioRenaper.lotes + "," + vacunas[0];
                        salidaDVacuna.AppendLine(string.Join(separador, lineaDVacuna));

                        lineaDVacunado = id + "," + anioNac + "," + decadas + "," + veintenas + "," + usuarioRenaper.genero + "," + usuarioRenaper.DNI;
                        salidaDVacunado.AppendLine(string.Join(separador, lineaDVacunado));

                        id++;
                    }

                    GenerarCsvService.GenerarCsv(rutaDLugar, salidaDLugar);
                    GenerarCsvService.GenerarCsv(rutaDTiempo, salidaDTiempo);
                    GenerarCsvService.GenerarCsv(rutaDVacuna, salidaDVacuna);
                    GenerarCsvService.GenerarCsv(rutaDVacunado, salidaDVacunado);

                    response = new ResponseCargarVacunaDTO("Aceptada", true, errores, emailOperadorNacional);
                }

                return response;
        }
    }
}
