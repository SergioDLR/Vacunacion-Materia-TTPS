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
            List<DosisDTO> listaDosis = null;

            try
            {
                //Primera dosis en nacimiento
                List<ReglaDTO> listaReglas1 = new List<ReglaDTO>();
                ReglaDTO regla1 = new ReglaDTO(0, "Aplicar antes de las 12 horas del nacimiento", null, 0, 0, "0");
                listaReglas1.Add(regla1);
                DosisDTO dosisDTO1 = new DosisDTO(0, 0, "Primera Dosis Nacimiento", listaReglas1);

                //Primera dosis  
                List<ReglaDTO> listaReglas2 = new List<ReglaDTO>();
                ReglaDTO regla2 = new ReglaDTO(0, "Aplicar desde los 11 años", null, 132, 0, null);
                listaReglas2.Add(regla2);
                DosisDTO dosisDTO2 = new DosisDTO(0, 1, "Primera Dosis", listaReglas2);

                //Segunda dosis
                List<ReglaDTO> listaReglas3 = new List<ReglaDTO>();
                ReglaDTO regla3 = new ReglaDTO(0, "Aplicar al mes de la primera dosis", null, 132, 1, null);
                listaReglas3.Add(regla3);
                DosisDTO dosisDTO3 = new DosisDTO(0, 2, "Segunda Dosis", listaReglas3);

                //Tercera dosis
                List<ReglaDTO> listaReglas4 = new List<ReglaDTO>();
                ReglaDTO regla4 = new ReglaDTO(0, "Aplicar a los 6 meses de la primera dosis", null, 132, 5, null);
                listaReglas4.Add(regla4);
                DosisDTO dosisDTO4 = new DosisDTO(0, 3, "Tercera Dosis", listaReglas4);

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
            List<DosisDTO> listaDosis = null;

            try
            {
                //Primera dosis en nacimiento
                List<ReglaDTO> listaReglas1 = new List<ReglaDTO>();
                ReglaDTO regla1 = new ReglaDTO(0, "Aplicar antes de salir de la maternidad", null, 0, 0, "0");
                listaReglas1.Add(regla1);
                DosisDTO dosisDTO1 = new DosisDTO(0, 0, "Dosis Única Nacimiento", listaReglas1);

                listaDosis.Add(dosisDTO1);
            }
            catch
            {

            }

            return listaDosis;
        }

        public List<DosisDTO> ArmarListaDosisDTORotavirus()
        {
            List<DosisDTO> listaDosis = null;

            try
            {
                //Primera dosis
                List<ReglaDTO> listaReglas1 = new List<ReglaDTO>();
                ReglaDTO regla1 = new ReglaDTO(0, "Aplicar desde los 2 meses del nacimiento hasta las 14 semanas y 6 días", null, 2, 0, "3.5");
                listaReglas1.Add(regla1);
                DosisDTO dosisDTO1 = new DosisDTO(0, 0, "Primera Dosis", listaReglas1);

                //Segunda dosis
                List<ReglaDTO> listaReglas2 = new List<ReglaDTO>();
                ReglaDTO regla2 = new ReglaDTO(0, "Aplicar desde los 4 meses del nacimiento hasta los 6 meses", null, 4, 2, "6");
                listaReglas2.Add(regla2);
                DosisDTO dosisDTO2 = new DosisDTO(0, 1, "Segunda Dosis", listaReglas2);

                listaDosis.Add(dosisDTO1);
                listaDosis.Add(dosisDTO2);
            }
            catch
            {

            }

            return listaDosis;
        }
    }
}
