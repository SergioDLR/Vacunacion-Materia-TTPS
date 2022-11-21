using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace VacunacionApi.ModelsDataWareHouse
{
    [Table("H_Vencidas")]
    public partial class HVencidas
    {
        [Key]
        public int Id { get; set; }
        [Column("Id_Lugar")]
        public int IdLugar { get; set; }
        [Column("Id_Tiempo")]
        public int IdTiempo { get; set; }
        [Column("Id_Vacuna")]
        public int IdVacuna { get; set; }
        public int Cantidad { get; set; }

        [ForeignKey(nameof(IdLugar))]
        [InverseProperty(nameof(DLugar.HVencidas))]
        public virtual DLugar IdLugarNavigation { get; set; }
        [ForeignKey(nameof(IdTiempo))]
        [InverseProperty(nameof(DTiempo.HVencidas))]
        public virtual DTiempo IdTiempoNavigation { get; set; }
        [ForeignKey(nameof(IdVacuna))]
        [InverseProperty(nameof(DVacuna.HVencidas))]
        public virtual DVacuna IdVacunaNavigation { get; set; }
    }
}
