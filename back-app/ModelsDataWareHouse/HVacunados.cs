using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace VacunacionApi.ModelsDataWareHouse
{
    [Table("H_Vacunados")]
    public partial class HVacunados
    {
        public HVacunados(int idTiempo, int idLugar, int idVacuna, int idVacunado)
        {
            IdTiempo = idTiempo;
            IdLugar = idLugar;
            IdVacuna = idVacuna;
            IdVacunado = idVacunado;
        }

        [Key]
        public int Id { get; set; }
        [Column("Id_Tiempo")]
        public int IdTiempo { get; set; }
        [Column("Id_Lugar")]
        public int IdLugar { get; set; }
        [Column("Id_Vacuna")]
        public int IdVacuna { get; set; }
        [Column("Id_Vacunado")]
        public int IdVacunado { get; set; }
        public int Cantidad { get; set; }

        [ForeignKey(nameof(IdLugar))]
        [InverseProperty(nameof(DLugar.HVacunados))]
        public virtual DLugar IdLugarNavigation { get; set; }
        [ForeignKey(nameof(IdTiempo))]
        [InverseProperty(nameof(DTiempo.HVacunados))]
        public virtual DTiempo IdTiempoNavigation { get; set; }
        [ForeignKey(nameof(IdVacuna))]
        [InverseProperty(nameof(DVacuna.HVacunados))]
        public virtual DVacuna IdVacunaNavigation { get; set; }
        [ForeignKey(nameof(IdVacunado))]
        [InverseProperty(nameof(DVacunado.HVacunados))]
        public virtual DVacunado IdVacunadoNavigation { get; set; }
    }
}
