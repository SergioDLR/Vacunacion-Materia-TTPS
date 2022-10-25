using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VacunacionApi.DTO
{
    public class VacunaDesarrolladaDTO
    {
        public VacunaDesarrolladaDTO(int id, string descripcion, int idLote)
        {
            Id = id;
            Descripcion = descripcion;
            IdLote = idLote;
        }

        public int Id { get; set; }
        public string Descripcion { get; set; }
        public int IdLote { get; set; }
    }
}
