using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VacunacionApi.Models;

namespace VacunacionApi.Services
{
    public static class VacunaAplicadaVerificacionService
    {
        public static List<List<string>> ObtenerProximaDosisRotavirusVaricela(VacunasContext _context, DateTime fechaNacimiento, List<Dosis> dosisAplicadas, string descripcionVacuna, bool embarazada, bool personalSalud)
        {
            string proximaDosis = null;
            List<string> listaProximasDosis = new List<string>();
            List<string> alertasVacunacion = new List<string>();
            List<List<string>> listaResultado = new List<List<string>>();
            Vacuna vacuna = VacunaService.GetVacunaByDescripcion(_context, descripcionVacuna);
            List<EntidadVacunaDosis> evdLista = _context.EntidadVacunaDosis.Where(evd => evd.IdVacuna == vacuna.Id).ToList();
            List<Regla> reglas = new List<Regla>();
            List<Dosis> dosis = new List<Dosis>();

            foreach (EntidadVacunaDosis evd in evdLista)
            {
                Dosis d = DosisService.GetDosis(_context, evd.IdDosis);
                dosis.Add(d);
            }

            //Estableciendo orden
            reglas.Sort((a, b) => a.Id.CompareTo(b.Id));

            foreach (Dosis d in dosis)
            {
                EntidadDosisRegla edr = EntidadDosisReglaService.GetEntidadDosisRegla(_context, d.Id);
                Regla r = ReglaService.GetRegla(_context, edr.IdRegla);
                reglas.Add(r);
            }

            var diasNacido = (DateTime.Now - fechaNacimiento).TotalDays;

            if (dosisAplicadas.Count == 0)
            {
                if (diasNacido < reglas[0].LapsoMinimoDias)
                {
                    proximaDosis = dosis[0].Descripcion;
                    alertasVacunacion.Add(reglas[0].Descripcion);
                }
                else if (diasNacido >= reglas[0].LapsoMinimoDias && diasNacido < reglas[0].LapsoMaximoDias)
                    proximaDosis = dosis[0].Descripcion;
                
                else if (diasNacido >= reglas[0].LapsoMaximoDias && diasNacido < reglas[1].LapsoMinimoDias && (descripcionVacuna == "Rotavirus"))
                {
                    alertasVacunacion.Add(reglas[0].Descripcion);
                    proximaDosis = dosis[0].Descripcion;
                }
                else if (diasNacido >= reglas[1].LapsoMinimoDias && diasNacido < reglas[1].LapsoMaximoDias)
                    proximaDosis = dosis[1].Descripcion;
                
                else if (diasNacido >= reglas[1].LapsoMaximoDias)
                {
                    alertasVacunacion.Add(reglas[1].Descripcion);
                    proximaDosis = dosis[1].Descripcion;
                }
            }
            else
            {
                Dosis ultimaDosisAplicada = dosisAplicadas.Last();

                if (ultimaDosisAplicada.Descripcion == dosis.First().Descripcion)
                {
                    if (diasNacido < reglas[1].LapsoMinimoDias)
                    {
                        alertasVacunacion.Add(reglas[1].Descripcion);
                        proximaDosis = dosis[1].Descripcion;
                    }
                    if (diasNacido >= reglas[1].LapsoMinimoDias && diasNacido < reglas[1].LapsoMaximoDias)
                    {
                        proximaDosis = dosis[1].Descripcion;
                    }
                    if (diasNacido >= reglas[1].LapsoMaximoDias)
                    {
                        proximaDosis = dosis[1].Descripcion;
                        alertasVacunacion.Add(reglas[1].Descripcion);
                    }
                }
                else if (ultimaDosisAplicada.Descripcion == dosis.Last().Descripcion)
                {
                    alertasVacunacion.Add("Todas las dosis fueron aplicadas");
                    proximaDosis = dosis.Last().Descripcion;
                }
            }

            if (embarazada)
                alertasVacunacion.Add("La persona está embarazada");

            if (personalSalud)
                alertasVacunacion.Add("La persona es personal de salud");

            listaProximasDosis.Add(proximaDosis);
            listaResultado.Add(listaProximasDosis);
            listaResultado.Add(alertasVacunacion);

            return listaResultado;
        }
    }
}
