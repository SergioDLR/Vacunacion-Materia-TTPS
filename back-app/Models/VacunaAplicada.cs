using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VacunacionApi.Models
{
    [Table("Vacuna_Aplicada")]
    public partial class VacunaAplicada
    {
        public VacunaAplicada()
        {
            PendienteEnvioDw = new HashSet<PendienteEnvioDw>();
        }

        [Key]
        public int Id { get; set; }
        [Column("Fecha_Vacunacion", TypeName = "date")]
        public DateTime FechaVacunacion { get; set; }
        public int Dni { get; set; }
        [Required]
        [Column("Sexo_Biologico")]
        [StringLength(50)]
        public string SexoBiologico { get; set; }
        [Column("Id_Dosis")]
        public int IdDosis { get; set; }
        [Column("Id_Lote")]
        public int IdLote { get; set; }
        [Column("Id_Jurisdiccion")]
        public int IdJurisdiccion { get; set; }
        [Required]
        [StringLength(250)]
        public string Nombre { get; set; }
        [Required]
        [StringLength(250)]
        public string Apellido { get; set; }
        public bool Embarazada { get; set; }
        [Column("Personal_Salud")]
        public bool PersonalSalud { get; set; }
        [Column("Fecha_Hora_Nacimiento", TypeName = "datetime")]
        public DateTime FechaHoraNacimiento { get; set; }
        [Column("Id_Jurisdiccion_Residencia")]
        public int IdJurisdiccionResidencia { get; set; }
        [Column("Id_Departamento")]
        public int? IdDepartamento { get; set; }
        [Column("Enviado_DW")]
        public bool? EnviadoDw { get; set; }

        [ForeignKey(nameof(IdDepartamento))]
        [InverseProperty(nameof(Departamento.VacunaAplicada))]
        public virtual Departamento IdDepartamentoNavigation { get; set; }
        [ForeignKey(nameof(IdDosis))]
        [InverseProperty(nameof(Dosis.VacunaAplicada))]
        public virtual Dosis IdDosisNavigation { get; set; }
        [ForeignKey(nameof(IdJurisdiccion))]
        [InverseProperty(nameof(Jurisdiccion.VacunaAplicada))]
        public virtual Jurisdiccion IdJurisdiccionNavigation { get; set; }
        [ForeignKey(nameof(IdLote))]
        [InverseProperty(nameof(Lote.VacunaAplicada))]
        public virtual Lote IdLoteNavigation { get; set; }
        [InverseProperty("IdVacunaAplicadaNavigation")]
        public virtual ICollection<PendienteEnvioDw> PendienteEnvioDw { get; set; }
    }
}
