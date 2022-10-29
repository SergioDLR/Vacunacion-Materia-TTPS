using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VacunacionApi.DTO;

namespace VacunacionApi.Services
{
    public class VacunaService
    {
        public List<DosisDTO> ArmarListaDosisDTOHepatitisBHB()
        {
            List<DosisDTO> listaDosis = new List<DosisDTO>();

            try
            {
                //Primera dosis en nacimiento
                List<ReglaDTO> listaReglas1 = new List<ReglaDTO>();
                ReglaDTO regla1 = new ReglaDTO(0, "Hepatitis B (HB) - Aplicar antes de las 12 horas del nacimiento", null, 0, 0.5, null, false, true);
                listaReglas1.Add(regla1);
                DosisDTO dosisDTO1 = new DosisDTO(0, 0, "Hepatitis B (HB) - Primera Dosis Nacimiento", listaReglas1);

                //Primera dosis  
                List<ReglaDTO> listaReglas2 = new List<ReglaDTO>();
                ReglaDTO regla2 = new ReglaDTO(0, "Hepatitis B (HB) - Aplicar desde los 11 años", null, 4015, 0, "0", false, true);
                listaReglas2.Add(regla2);
                DosisDTO dosisDTO2 = new DosisDTO(0, 1, "Hepatitis B (HB) - Primera Dosis", listaReglas2);

                //Segunda dosis
                List<ReglaDTO> listaReglas3 = new List<ReglaDTO>();
                ReglaDTO regla3 = new ReglaDTO(0, "Hepatitis B (HB) - Aplicar al mes de la primera dosis", null, 30, 180, "1", false, true);
                listaReglas3.Add(regla3);
                DosisDTO dosisDTO3 = new DosisDTO(0, 2, "Hepatitis B (HB) - Segunda Dosis", listaReglas3);

                //Tercera dosis
                List<ReglaDTO> listaReglas4 = new List<ReglaDTO>();
                ReglaDTO regla4 = new ReglaDTO(0, "Hepatitis B (HB) - Aplicar a los 6 meses de la primera dosis", null, 180, 0, "1", false, true);
                listaReglas4.Add(regla4);
                DosisDTO dosisDTO4 = new DosisDTO(0, 3, "Hepatitis B (HB) - Tercera Dosis", listaReglas4);

                listaDosis.Add(dosisDTO1);
                listaDosis.Add(dosisDTO2);
                listaDosis.Add(dosisDTO3);
                listaDosis.Add(dosisDTO4);
            }
            catch
            {

            }

            return listaDosis;
        }

        public List<DosisDTO> ArmarListaDosisDTOBCG()
        {
            List<DosisDTO> listaDosis = new List<DosisDTO>();

            try
            {
                //Primera dosis en nacimiento
                List<ReglaDTO> listaReglas1 = new List<ReglaDTO>();
                ReglaDTO regla1 = new ReglaDTO(0, "BCG - Aplicar antes de salir de la maternidad", null, 0, 0, null, false, false);
                listaReglas1.Add(regla1);
                DosisDTO dosisDTO1 = new DosisDTO(0, 0, "BCG - Dosis Única Nacimiento", listaReglas1);

                listaDosis.Add(dosisDTO1);
            }
            catch
            {

            }

            return listaDosis;
        }

        public List<DosisDTO> ArmarListaDosisDTORotavirus()
        {
            List<DosisDTO> listaDosis = new List<DosisDTO>();

            try
            {
                //Primera dosis
                List<ReglaDTO> listaReglas1 = new List<ReglaDTO>();
                ReglaDTO regla1 = new ReglaDTO(0, "Rotavirus - Aplicar desde los 2 meses del nacimiento hasta las 14 semanas y 6 días", null, 60, 104, null, false, false);
                listaReglas1.Add(regla1);
                DosisDTO dosisDTO1 = new DosisDTO(0, 0, "Rotavirus - Primera Dosis", listaReglas1);

                //Segunda dosis
                List<ReglaDTO> listaReglas2 = new List<ReglaDTO>();
                ReglaDTO regla2 = new ReglaDTO(0, "Rotavirus - Aplicar desde los 4 meses del nacimiento hasta los 6 meses", null, 60, 120, "0", false, false);
                listaReglas2.Add(regla2);
                DosisDTO dosisDTO2 = new DosisDTO(0, 1, "Rotavirus - Segunda Dosis", listaReglas2);

                listaDosis.Add(dosisDTO1);
                listaDosis.Add(dosisDTO2);
            }
            catch
            {

            }

            return listaDosis;
        }

        public List<DosisDTO> ArmarListaDosisNeumococoConjugada()
        {
            List<DosisDTO> listaDosis = new List<DosisDTO>();

            try
            {
                //Primera dosis
                List<ReglaDTO> listaReglas1 = new List<ReglaDTO>();
                ReglaDTO regla1 = new ReglaDTO(0, "Neumococo Conjugada - Aplicar desde los 2 meses del nacimiento hasta los 4 meses", null, 60, 120, null, false, false);
                listaReglas1.Add(regla1);
                DosisDTO dosisDTO1 = new DosisDTO(0, 0, "Neumococo Conjugada - Primera Dosis", listaReglas1);

                //Segunda dosis
                List<ReglaDTO> listaReglas2 = new List<ReglaDTO>();
                ReglaDTO regla2 = new ReglaDTO(0, "Neumococo Conjugada - Aplicar desde los 4 meses del nacimiento hasta los 12 meses", null, 120, 365, "0", false, false);
                listaReglas2.Add(regla2);
                DosisDTO dosisDTO2 = new DosisDTO(0, 1, "Neumococo Conjugada - Segunda Dosis", listaReglas2);

                //Refuerzo
                List<ReglaDTO> listaReglas3 = new List<ReglaDTO>();
                ReglaDTO regla3 = new ReglaDTO(0, "Neumococo Conjugada - Aplicar desde los 12 meses del nacimiento", null, 365, 0, "0", false, false);
                listaReglas3.Add(regla3);
                DosisDTO dosisDTO3 = new DosisDTO(0, 2, "Neumococo Conjugada - Refuerzo", listaReglas3);

                listaDosis.Add(dosisDTO1);
                listaDosis.Add(dosisDTO2);
                listaDosis.Add(dosisDTO3);
            }
            catch
            {

            }

            return listaDosis;
        }

        public List<DosisDTO> ArmarListaDosisQuintuplePentavalente()
        {
            List<DosisDTO> listaDosis = new List<DosisDTO>();

            try
            {
                //Primera dosis
                List<ReglaDTO> listaReglas1 = new List<ReglaDTO>();
                ReglaDTO regla1 = new ReglaDTO(0, "Quíntuple Pentavalente - Aplicar desde los 2 meses del nacimiento hasta los 4 meses", null, 60, 120, null, false, false);
                listaReglas1.Add(regla1);
                DosisDTO dosisDTO1 = new DosisDTO(0, 0, "Quíntuple Pentavalente - Primera Dosis", listaReglas1);

                //Segunda dosis
                List<ReglaDTO> listaReglas2 = new List<ReglaDTO>();
                ReglaDTO regla2 = new ReglaDTO(0, "Quíntuple Pentavalente - Aplicar desde los 4 meses del nacimiento hasta los 6 meses", null, 120, 180, "0", false, false);
                listaReglas2.Add(regla2);
                DosisDTO dosisDTO2 = new DosisDTO(0, 1, "Quíntuple Pentavalente - Segunda Dosis", listaReglas2);

                //Refuerzo
                List<ReglaDTO> listaReglas3 = new List<ReglaDTO>();
                ReglaDTO regla3 = new ReglaDTO(0, "Quíntuple Pentavalente - Aplicar desde los 6 meses del nacimiento hasta los 15 meses", null, 180, 450, "0", false, false);
                listaReglas3.Add(regla3);
                DosisDTO dosisDTO3 = new DosisDTO(0, 2, "Quíntuple Pentavalente - Tercera Dosis", listaReglas3);

                //Refuerzo
                List<ReglaDTO> listaReglas4 = new List<ReglaDTO>();
                ReglaDTO regla4 = new ReglaDTO(0, "Quíntuple Pentavalente - Aplicar entre los 15 y 18 meses de nacimiento", null, 450, 540, "0", false, false);
                listaReglas4.Add(regla4);
                DosisDTO dosisDTO4 = new DosisDTO(0, 3, "Quíntuple Pentavalente - Refuerzo", listaReglas4);

                listaDosis.Add(dosisDTO1);
                listaDosis.Add(dosisDTO2);
                listaDosis.Add(dosisDTO3);
                listaDosis.Add(dosisDTO4);
            }
            catch
            {

            }

            return listaDosis;
        }
    }
}
