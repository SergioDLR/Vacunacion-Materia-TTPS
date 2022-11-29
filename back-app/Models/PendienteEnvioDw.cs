using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace VacunacionApi.Models
{
    [Table("Pendiente_Envio_DW")]
    public partial class PendienteEnvioDw
    {
        [Key]
        public int Id { get; set; }
        [Column("Id_Vacuna_Aplicada")]
        public int IdVacunaAplicada { get; set; }

        [ForeignKey(nameof(IdVacunaAplicada))]
        [InverseProperty(nameof(VacunaAplicada.PendienteEnvioDw))]
        public virtual VacunaAplicada IdVacunaAplicadaNavigation { get; set; }
    }
}
