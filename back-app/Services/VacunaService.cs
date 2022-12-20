using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VacunacionApi.DTO;
using VacunacionApi.Models;

namespace VacunacionApi.Services
{
    public static class VacunaService
    {
        public static bool VacunaExists(VacunasContext _context, int id)
        {
            return _context.Vacuna.Any(e => e.Id == id);
        }

        public static Vacuna GetVacuna(VacunasContext _context, int idVacuna)
        {
            return _context.Vacuna
                .Where(vac => vac.Id == idVacuna).FirstOrDefault();
        }

        public static Vacuna GetVacunaByDescripcion(VacunasContext _context, string vacuna)
        {
            return _context.Vacuna
                .Where(vac => vac.Descripcion == vacuna).FirstOrDefault();
        }

        public static List<DosisDTO> ArmarListaDosisDTO(List<ReglaDTO> listaReglas, List<string> listaDescripcionesDosis)
        {
            List<DosisDTO> listaDosis = new List<DosisDTO>();

            for (int i=0; i<listaReglas.Count(); i++)
            {
                listaDosis.Add(new DosisDTO(0, i, listaDescripcionesDosis[i], new List<ReglaDTO>() { listaReglas[i] }));
            }

            return listaDosis;
        }

        public static VacunaCalendarioAnualPandemiaDTO RespaldarDosisReglasByVacuna(VacunasContext _context, VacunaCalendarioAnualPandemiaDTO vacunaCalendario = null)
        {
            foreach (DosisDTO dosisDTO in vacunaCalendario.Dosis)
            {
                Dosis dosis = new Dosis(0, dosisDTO.Descripcion);
                _context.Dosis.Add(dosis);
                _context.SaveChangesAsync();
                EntidadVacunaDosis entidadVacunaDosis = new EntidadVacunaDosis(vacunaCalendario.Id, dosis.Id, dosisDTO.Orden);
                _context.EntidadVacunaDosis.Add(entidadVacunaDosis);
                _context.SaveChangesAsync();
                dosisDTO.Id = dosis.Id;

                foreach (ReglaDTO reglaDTO in dosisDTO.Reglas)
                {
                    Regla reglaExistente = _context.Regla.Where(r => r.Descripcion == reglaDTO.Descripcion).FirstOrDefault();
                    int idRegla = 0;

                    if (reglaExistente == null)
                    {
                        Regla regla = new Regla(reglaDTO.Descripcion, reglaDTO.MesesVacunacion, reglaDTO.LapsoMinimoDias, reglaDTO.LapsoMaximoDias, reglaDTO.Otros, reglaDTO.Embarazada, reglaDTO.PersonalSalud);
                        _context.Regla.Add(regla);
                        _context.SaveChangesAsync();
                        idRegla = regla.Id;
                    }
                    else
                        idRegla = reglaExistente.Id;

                    reglaDTO.Id = idRegla;

                    EntidadDosisRegla entidadDosisRegla = new EntidadDosisRegla(dosis.Id, idRegla);
                    _context.EntidadDosisRegla.Add(entidadDosisRegla);
                    _context.SaveChangesAsync();
                }
            }

            return vacunaCalendario;
        }

        public static List<DescripcionVacunaCalendarioAnualDTO> GetDescripcionesVacunasCalendario()
        {
            List<DescripcionVacunaCalendarioAnualDTO> descripcionesVacunasCalendario = new List<DescripcionVacunaCalendarioAnualDTO>();
            descripcionesVacunasCalendario.Add(new DescripcionVacunaCalendarioAnualDTO(1, "Hepatitis B (HB)"));
            descripcionesVacunasCalendario.Add(new DescripcionVacunaCalendarioAnualDTO(2, "BCG"));
            descripcionesVacunasCalendario.Add(new DescripcionVacunaCalendarioAnualDTO(3, "Rotavirus"));
            descripcionesVacunasCalendario.Add(new DescripcionVacunaCalendarioAnualDTO(4, "Neumococo Conjugada"));
            descripcionesVacunasCalendario.Add(new DescripcionVacunaCalendarioAnualDTO(5, "Quíntuple Pentavalente"));
            descripcionesVacunasCalendario.Add(new DescripcionVacunaCalendarioAnualDTO(6, "Salk IPV"));
            descripcionesVacunasCalendario.Add(new DescripcionVacunaCalendarioAnualDTO(7, "Meningocócica Conjugada"));
            descripcionesVacunasCalendario.Add(new DescripcionVacunaCalendarioAnualDTO(8, "Triple Viral (SRP)"));
            descripcionesVacunasCalendario.Add(new DescripcionVacunaCalendarioAnualDTO(9, "Hepatitis A (HA)"));
            descripcionesVacunasCalendario.Add(new DescripcionVacunaCalendarioAnualDTO(10, "Varicela"));
            descripcionesVacunasCalendario.Add(new DescripcionVacunaCalendarioAnualDTO(11, "Triple Bacteriana (DTP)"));
            descripcionesVacunasCalendario.Add(new DescripcionVacunaCalendarioAnualDTO(12, "Triple Bacteriana Acelular"));
            descripcionesVacunasCalendario.Add(new DescripcionVacunaCalendarioAnualDTO(13, "VPH"));
            descripcionesVacunasCalendario.Add(new DescripcionVacunaCalendarioAnualDTO(14, "Doble Bacteriana (DT)"));
            descripcionesVacunasCalendario.Add(new DescripcionVacunaCalendarioAnualDTO(15, "Doble Viral (SR)"));
            descripcionesVacunasCalendario.Add(new DescripcionVacunaCalendarioAnualDTO(16, "Fiebre Amarilla (FA)"));
            descripcionesVacunasCalendario.Add(new DescripcionVacunaCalendarioAnualDTO(17, "Fiebre Hemorrágica Argentina (FHA)"));

            return descripcionesVacunasCalendario;
        }

        public static List<DescripcionVacunaCalendarioAnualDTO> GetVacunasAnuales()
        {
            List<DescripcionVacunaCalendarioAnualDTO> descripcionesVacunasAnuales = new List<DescripcionVacunaCalendarioAnualDTO>();
            descripcionesVacunasAnuales.Add(new DescripcionVacunaCalendarioAnualDTO(1, "Antigripal"));
            descripcionesVacunasAnuales.Add(new DescripcionVacunaCalendarioAnualDTO(2, "Neumonía"));
            descripcionesVacunasAnuales.Add(new DescripcionVacunaCalendarioAnualDTO(3, "Sarampión"));
            descripcionesVacunasAnuales.Add(new DescripcionVacunaCalendarioAnualDTO(4, "Tos Ferina"));

            return descripcionesVacunasAnuales;
        }

        public static List<VacunaCalendarioAnualPandemiaDTO> GetVacunasCalendario()
        {
            List<VacunaCalendarioAnualPandemiaDTO> vacunasCalendario = new List<VacunaCalendarioAnualPandemiaDTO>()
                {
                    new VacunaCalendarioAnualPandemiaDTO(1, "Hepatitis B (HB)", ArmarListaDosisDTO("Hepatitis B (HB)")),
                    new VacunaCalendarioAnualPandemiaDTO(2, "BCG", ArmarListaDosisDTO("BCG")),
                    new VacunaCalendarioAnualPandemiaDTO(3, "Rotavirus", ArmarListaDosisDTO("Rotavirus")),
                    new VacunaCalendarioAnualPandemiaDTO(4, "Neumococo Conjugada", ArmarListaDosisDTO("Neumococo Conjugada")),
                    new VacunaCalendarioAnualPandemiaDTO(5, "Quíntuple Pentavalente", ArmarListaDosisDTO("Quíntuple Pentavalente")),
                    new VacunaCalendarioAnualPandemiaDTO(6, "Salk IPV", ArmarListaDosisDTO("Salk IPV")),
                    new VacunaCalendarioAnualPandemiaDTO(7, "Meningocócica Conjugada", ArmarListaDosisDTO("Meningocócica Conjugada")),
                    new VacunaCalendarioAnualPandemiaDTO(8, "Triple Viral (SRP)", ArmarListaDosisDTO("Triple Viral (SRP)")),
                    new VacunaCalendarioAnualPandemiaDTO(9, "Hepatitis A (HA)", ArmarListaDosisDTO("Hepatitis A (HA)")),
                    new VacunaCalendarioAnualPandemiaDTO(10, "Varicela", ArmarListaDosisDTO("Varicela")),
                    new VacunaCalendarioAnualPandemiaDTO(11, "Triple Bacteriana (DTP)", ArmarListaDosisDTO("Triple Bacteriana (DTP)")),
                    new VacunaCalendarioAnualPandemiaDTO(12, "Triple Bacteriana Acelular", ArmarListaDosisDTO("Triple Bacteriana Acelular")),
                    new VacunaCalendarioAnualPandemiaDTO(13, "VPH", ArmarListaDosisDTO("VPH")),
                    new VacunaCalendarioAnualPandemiaDTO(14, "Doble Bacteriana (DT)", ArmarListaDosisDTO("Doble Bacteriana (DT)")),
                    new VacunaCalendarioAnualPandemiaDTO(15, "Doble Viral (SR)", ArmarListaDosisDTO("Doble Viral (SR)")),
                    new VacunaCalendarioAnualPandemiaDTO(16, "Fiebre Amarilla (FA)", ArmarListaDosisDTO("Fiebre Amarilla (FA)")),
                    new VacunaCalendarioAnualPandemiaDTO(17, "Fiebre Hemorrágica Argentina (FHA)", ArmarListaDosisDTO("Fiebre Hemorrágica Argentina (FHA)"))
                };
         
            return vacunasCalendario;
        }

        public static List<DosisDTO> ArmarListaDosisDTO(string descripcionVacuna)
        {
            List<DosisDTO> listaDosis = new List<DosisDTO>();
            var listaReglas = new List<ReglaDTO>();
            var listaDescripcionesDosis = new List<string>();

            switch (descripcionVacuna)
            {
                case "Hepatitis B (HB)":
                    listaReglas = new List<ReglaDTO>()
                        {
                            new ReglaDTO(0, descripcionVacuna + " - " + "Aplicar antes de las 12 horas del nacimiento", null, 0, 0.5, null, false, true),
                            new ReglaDTO(0, descripcionVacuna + " - " + "Aplicar desde los 11 años", null, 4015, 0, null, false, true),
                            new ReglaDTO(0, descripcionVacuna + " - " + "Aplicar al mes de la primera dosis", null, 30, 180, "1", false, true),
                            new ReglaDTO(0, descripcionVacuna + " - " + "Aplicar a los 6 meses de la primera dosis", null, 180, 0, "1", false, true),
                        };

                    listaDescripcionesDosis = new List<string>(){
                            descripcionVacuna + " - " + "Primera Dosis Nacimiento",
                            descripcionVacuna + " - " + "Primera Dosis",
                            descripcionVacuna + " - " + "Segunda Dosis",
                            descripcionVacuna + " - " + "Tercera Dosis",
                        };

                    listaDosis = ArmarListaDosisDTO(listaReglas, listaDescripcionesDosis);
                    break;

                case "BCG":
                    listaReglas = new List<ReglaDTO>()
                        {
                            new ReglaDTO(0, descripcionVacuna + " - " + "Aplicar antes de salir de la maternidad", null, 0, 0, null, false, false),
                        };

                    listaDescripcionesDosis = new List<string>(){
                            descripcionVacuna + " - " + "Dosis Única Nacimiento",
                        };

                    listaDosis = ArmarListaDosisDTO(listaReglas, listaDescripcionesDosis);
                    break;

                case "Rotavirus":
                    listaReglas = new List<ReglaDTO>()
                        {
                            new ReglaDTO(0, descripcionVacuna + " - " + "Aplicar desde los 2 meses del nacimiento hasta las 14 semanas y 6 días", null, 60, 104, null, false, false),
                            new ReglaDTO(0, descripcionVacuna + " - " + "Aplicar desde los 4 meses del nacimiento hasta los 6 meses", null, 120, 180, null, false, false),
                        };

                    listaDescripcionesDosis = new List<string>(){
                            descripcionVacuna + " - " + "Primera Dosis",
                            descripcionVacuna + " - " + "Segunda Dosis",
                        };

                    listaDosis = ArmarListaDosisDTO(listaReglas, listaDescripcionesDosis);
                    break;

                case "Neumococo Conjugada":
                    listaReglas = new List<ReglaDTO>()
                        {
                            new ReglaDTO(0, descripcionVacuna + " - " + "Aplicar desde los 2 meses del nacimiento hasta los 4 meses", null, 60, 120, null, false, false),
                            new ReglaDTO(0, descripcionVacuna + " - " + "Aplicar desde los 4 meses del nacimiento hasta los 12 meses", null, 120, 365, null, false, false),
                            new ReglaDTO(0, descripcionVacuna + " - " + "Aplicar desde los 12 meses del nacimiento", null, 365, 0, null, false, false),
                        };

                    listaDescripcionesDosis = new List<string>(){
                            descripcionVacuna + " - " + "Primera Dosis",
                            descripcionVacuna + " - " + "Segunda Dosis",
                            descripcionVacuna + " - " + "Refuerzo",
                        };

                    listaDosis = ArmarListaDosisDTO(listaReglas, listaDescripcionesDosis);
                    break;

                case "Quíntuple Pentavalente":
                    listaReglas = new List<ReglaDTO>()
                        {
                            new ReglaDTO(0, descripcionVacuna + " - " + "Aplicar desde los 2 meses del nacimiento hasta los 4 meses", null, 60, 120, null, false, false),
                            new ReglaDTO(0, descripcionVacuna + " - " + "Aplicar desde los 4 meses del nacimiento hasta los 6 meses", null, 120, 180, null, false, false),
                            new ReglaDTO(0, descripcionVacuna + " - " + "Aplicar desde los 6 meses del nacimiento hasta los 15 meses", null, 180, 450, null, false, false),
                            new ReglaDTO(0, descripcionVacuna + " - " + "Aplicar entre los 15 y 18 meses de nacimiento", null, 450, 540, null, false, false),
                        };

                    listaDescripcionesDosis = new List<string>(){
                            descripcionVacuna + " - " + "Primera Dosis",
                            descripcionVacuna + " - " + "Segunda Dosis",
                            descripcionVacuna + " - " + "Tercera Dosis",
                            descripcionVacuna + " - " + "Refuerzo",
                        };

                    listaDosis = ArmarListaDosisDTO(listaReglas, listaDescripcionesDosis);
                    break;

                case "Salk IPV":
                    listaReglas = new List<ReglaDTO>()
                        {
                            new ReglaDTO(0, descripcionVacuna + " - " + "Aplicar desde los 2 meses del nacimiento hasta los 4 meses (intramuscular)", null, 60, 120, null, false, false),
                            new ReglaDTO(0, descripcionVacuna + " - " + "Aplicar desde los 4 meses del nacimiento hasta los 6 meses (intramuscular)", null, 120, 180, null, false, false),
                            new ReglaDTO(0, descripcionVacuna + " - " + "Aplicar desde los 6 meses del nacimiento hasta los 5 años (intramuscular)", null, 180, 1825, null, false, false),
                            new ReglaDTO(0, descripcionVacuna + " - " + "Aplicar entre los 5 y 6 años (intramuscular)", null, 1825, 2190, null, false, false),
                        };

                    listaDescripcionesDosis = new List<string>(){
                            descripcionVacuna + " - " + "Primera Dosis",
                            descripcionVacuna + " - " + "Segunda Dosis",
                            descripcionVacuna + " - " + "Tercera Dosis",
                            descripcionVacuna + " - " + "Refuerzo",
                        };

                    listaDosis = ArmarListaDosisDTO(listaReglas, listaDescripcionesDosis);
                    break;

                case "Meningocócica Conjugada":
                    listaReglas = new List<ReglaDTO>()
                        {
                            new ReglaDTO(0, descripcionVacuna + " - " + "Aplicar desde los 3 meses del nacimiento hasta los 5 meses", null, 90, 150, null, false, false),
                            new ReglaDTO(0, descripcionVacuna + " - " + "Aplicar desde los 5 meses del nacimiento hasta los 15 meses", null, 150, 450, null, false, false),
                            new ReglaDTO(0, descripcionVacuna + " - " + "Aplicar desde los 15 meses del nacimiento hasta los 11 años", null, 450, 4015, null, false, false),
                            new ReglaDTO(0, descripcionVacuna + " - " + "Aplicar a partir de los 11 años", null, 4015, 0, null, false, false),
                        };

                    listaDescripcionesDosis = new List<string>(){
                            descripcionVacuna + " - " + "Primera Dosis",
                            descripcionVacuna + " - " + "Segunda Dosis",
                            descripcionVacuna + " - " + "Refuerzo",
                            descripcionVacuna + " - " + "Dosis Única",
                        };

                    listaDosis = ArmarListaDosisDTO(listaReglas, listaDescripcionesDosis);
                    break;

                case "Triple Viral (SRP)":
                    listaReglas = new List<ReglaDTO>()
                        {
                            new ReglaDTO(0, descripcionVacuna + " - " + "Aplicar desde los 12 meses del nacimiento hasta los 5 años", null, 365, 1825, null, false, false),
                            new ReglaDTO(0, descripcionVacuna + " - " + "Aplicar entre los 5 y 6 años", null, 1825, 2190, null, false, false),
                            new ReglaDTO(0, descripcionVacuna + " - " + "Aplicar a partir de los 11 años, si no recibió anteriores (1 dosis de triple viral + 1 dosis de doble viral)", null, 4015, 0, null, false, false),
                        };

                    listaDescripcionesDosis = new List<string>(){
                            descripcionVacuna + " - " + "Primera Dosis",
                            descripcionVacuna + " - " + "Segunda Dosis",
                            descripcionVacuna + " - " + "Refuerzo Triple Viral + Doble Viral",
                        };

                    listaDosis = ArmarListaDosisDTO(listaReglas, listaDescripcionesDosis);
                    break;

                case "Hepatitis A (HA)":
                    listaReglas = new List<ReglaDTO>()
                        {
                            new ReglaDTO(0, descripcionVacuna + " - " + "Aplicar desde los 12 meses del nacimiento", null, 365, 0, null, false, false),
                        };

                    listaDescripcionesDosis = new List<string>(){
                            descripcionVacuna + " - " + "Dosis Única",
                        };

                    listaDosis = ArmarListaDosisDTO(listaReglas, listaDescripcionesDosis);
                    break;

                case "Varicela":
                    listaReglas = new List<ReglaDTO>()
                        {
                            new ReglaDTO(0, descripcionVacuna + " - " + "Aplicar desde los 15 meses del nacimiento hasta los 5 años", null, 450, 1825, null, false, false),
                            new ReglaDTO(0, descripcionVacuna + " - " + "Aplicar entre los 5 y 6 años", null, 1825, 2190, null, false, false),
                        };

                    listaDescripcionesDosis = new List<string>(){
                            descripcionVacuna + " - " + "Primera Dosis",
                            descripcionVacuna + " - " + "Segunda Dosis",
                        };

                    listaDosis = ArmarListaDosisDTO(listaReglas, listaDescripcionesDosis);
                    break;

                case "Triple Bacteriana (DTP)":
                    listaReglas = new List<ReglaDTO>()
                        {
                            new ReglaDTO(0, descripcionVacuna + " - " + "Aplicar entre los 5 y 6 años", null, 1825, 2160, null, false, false),
                        };

                    listaDescripcionesDosis = new List<string>(){
                            descripcionVacuna + " - " + "Refuerzo",
                        };

                    listaDosis = ArmarListaDosisDTO(listaReglas, listaDescripcionesDosis);
                    break;

                case "Triple Bacteriana Acelular":
                    listaReglas = new List<ReglaDTO>()
                        {
                            new ReglaDTO(0, descripcionVacuna + " - " + "Aplicar a partir de los 11 años", null, 4015, 0, null, true, true),
                            new ReglaDTO(0, descripcionVacuna + " - " + "Aplicar a partir de la semana 20 de gestación", null, 0, 0, null, true, true),
                            new ReglaDTO(0, descripcionVacuna + " - " + "Aplicar a personal que atiende niños menores de 1 año. Revacunar a los 5 años", null, 0, 0, null, true, true),
                        };

                    listaDescripcionesDosis = new List<string>(){
                            descripcionVacuna + " - " + "Dosis Única",
                            descripcionVacuna + " - " + "Dosis Embarazo",
                            descripcionVacuna + " - " + "Dosis Personal Salud",
                        };

                    listaDosis = ArmarListaDosisDTO(listaReglas, listaDescripcionesDosis);
                    break;

                case "VPH":
                    listaReglas = new List<ReglaDTO>()
                        {
                            new ReglaDTO(0, descripcionVacuna + " - " + "Aplicar a partir de los 11 años", null, 4015, 0, null, false, false),
                            new ReglaDTO(0, descripcionVacuna + " - " + "Aplicar a los 6 meses de la primera dosis", null, 180, 0, "0", false, false),
                        };

                    listaDescripcionesDosis = new List<string>(){
                            descripcionVacuna + " - " + "Primera Dosis",
                            descripcionVacuna + " - " + "Segunda Dosis",
                        };

                    listaDosis = ArmarListaDosisDTO(listaReglas, listaDescripcionesDosis);
                    break;

                case "Doble Bacteriana (DT)":
                    listaReglas = new List<ReglaDTO>()
                        {
                            new ReglaDTO(0, descripcionVacuna + " - " + "Aplicar a partir de los 18 años. Componente tétanos-difteria (refuerzo cada 10 años)", null, 6570, 0, null, true, false),
                            new ReglaDTO(0, descripcionVacuna + " - " + "Aplicar después de la primera dosis. Componente tétanos-difteria (refuerzo cada 10 años)", null, 0, 0, null, true, false),
                            new ReglaDTO(0, descripcionVacuna + " - " + "Aplicar después de la segunda dosis. Componente tétanos-difteria (refuerzo cada 10 años)", null, 0, 0, null, true, false),
                        };

                    listaDescripcionesDosis = new List<string>(){
                            descripcionVacuna + " - " + "Primera Dosis",
                            descripcionVacuna + " - " + "Segunda Dosis",
                            descripcionVacuna + " - " + "Tercera Dosis",
                        };

                    listaDosis = ArmarListaDosisDTO(listaReglas, listaDescripcionesDosis);
                    break;

                case "Doble Viral (SR)":
                    listaReglas = new List<ReglaDTO>()
                        {
                            new ReglaDTO(0, descripcionVacuna + " - " + "Aplicar desde los 11 años. Si no hay aplicaciones previas, aplicar 1 dosis o bien 1 dosis de Triple Viral + 1 dosis de Doble Viral", null, 4015, 0, null, false, true),
                        };

                    listaDescripcionesDosis = new List<string>(){
                            descripcionVacuna + " - " + "Primera Dosis",
                        };

                    listaDosis = ArmarListaDosisDTO(listaReglas, listaDescripcionesDosis);
                    break;

                case "Fiebre Amarilla (FA)":
                    listaReglas = new List<ReglaDTO>()
                        {
                            new ReglaDTO(0, descripcionVacuna + " - " + "Aplicar a partir de los 18 meses hasta los 11 años (para residentes en zona de riesgo)", null, 540, 4015, null, false, false),
                            new ReglaDTO(0, descripcionVacuna + " - " + "Aplicar a los 10 años de la primera dosis (para residentes en zona de riesgo)", null, 3650, 0, "0", false, false),
                        };

                    listaDescripcionesDosis = new List<string>(){
                            descripcionVacuna + " - " + "Primera Dosis",
                            descripcionVacuna + " - " + "Refuerzo",
                        };

                    listaDosis = ArmarListaDosisDTO(listaReglas, listaDescripcionesDosis);
                    break;

                case "Fiebre Hemorrágica Argentina (FHA)":
                    listaReglas = new List<ReglaDTO>()
                        {
                            new ReglaDTO(0, descripcionVacuna + " - " + "Aplicar desde los 15 años. Residentes o trabajadores con riesgo ocupacional en zonas de riesgo", null, 5475, 0, null, false, false),
                        };

                    listaDescripcionesDosis = new List<string>(){
                            descripcionVacuna + " - " + "Primera Dosis",
                        };

                    listaDosis = ArmarListaDosisDTO(listaReglas, listaDescripcionesDosis);
                    break;

                default:
                    break;
            }

            return listaDosis;
        }
    }
}
