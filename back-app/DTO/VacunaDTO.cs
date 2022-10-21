using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VacunacionApi.DTO
{
    public class VacunaDTO
    {
        public VacunaDTO(int id, string descripcion, int idTipoVacuna, string descripcionTipoVacuna, int? idPandemia, string descripcionPandemia, int cantidadDosis, List<DosisDTO> dosis)
        {
            Id = id;
            Descripcion = descripcion;
            IdTipoVacuna = idTipoVacuna;
            DescripcionTipoVacuna = descripcionTipoVacuna;
            IdPandemia = idPandemia;
            DescripcionPandemia = descripcionPandemia;
            CantidadDosis = cantidadDosis;
            Dosis = dosis;
        }

        public int Id { get; set; }
        public string Descripcion { get; set; }
        public int IdTipoVacuna { get; set; }
        public string DescripcionTipoVacuna { get; set; }
        public int? IdPandemia { get; set; }
        public string DescripcionPandemia { get; set; }
        public int CantidadDosis { get; set; }
        public List<DosisDTO> Dosis { get; set; }
    }
}
