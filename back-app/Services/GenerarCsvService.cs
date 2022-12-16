using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VacunacionApi.Services
{
    public static class GenerarCsvService
    {
        public static void GenerarCsv(string ruta, StringBuilder salida)
        {
            File.AppendAllText(ruta, salida.ToString(), Encoding.UTF8);
        }

        public static List<string> GetTipoVacuna(List<string> tiposVacunas, List<string> vacunasArnn, List<string> vacunasVectorViral, List<string> vacunasSubunidadesProteicas, string vacuna)
        {
            List<string> tipoVacuna = new List<string>();

            if (vacunasArnn.Contains(vacuna))
            { 
                tipoVacuna.Add(tiposVacunas[0]);
                tipoVacuna.Add("Existe");
            }
            else if (vacunasVectorViral.Contains(vacuna))
            {
                tipoVacuna.Add(tiposVacunas[1]);
                tipoVacuna.Add("Existe");
            }
            else if (vacunasSubunidadesProteicas.Contains(vacuna))
            {
                tipoVacuna.Add(tiposVacunas[2]);
                tipoVacuna.Add("Existe");
            }
            else
            {
                Random random = new Random();
                tipoVacuna.Add(tiposVacunas[random.Next(0, 2)]);
                tipoVacuna.Add("No existe");
            }

            return tipoVacuna;
        } 
    }
}
