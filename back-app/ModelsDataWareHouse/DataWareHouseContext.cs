using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace VacunacionApi.ModelsDataWareHouse
{
    public partial class DataWareHouseContext : DbContext
    {
        public DataWareHouseContext(DbContextOptions<DataWareHouseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<DLugar> DLugar { get; set; }
        public virtual DbSet<DTiempo> DTiempo { get; set; }
        public virtual DbSet<DVacuna> DVacuna { get; set; }
        public virtual DbSet<DVacunado> DVacunado { get; set; }
        public virtual DbSet<HVacunados> HVacunados { get; set; }
        public virtual DbSet<HVencidas> HVencidas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<HVacunados>(entity =>
            {
                entity.HasOne(d => d.IdLugarNavigation)
                    .WithMany(p => p.HVacunados)
                    .HasForeignKey(d => d.IdLugar)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_H_Vacunados_D_Lugar");

                entity.HasOne(d => d.IdTiempoNavigation)
                    .WithMany(p => p.HVacunados)
                    .HasForeignKey(d => d.IdTiempo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_H_Vacunados_D_Tiempo");

                entity.HasOne(d => d.IdVacunaNavigation)
                    .WithMany(p => p.HVacunados)
                    .HasForeignKey(d => d.IdVacuna)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_H_Vacunados_D_Vacuna");

                entity.HasOne(d => d.IdVacunadoNavigation)
                    .WithMany(p => p.HVacunados)
                    .HasForeignKey(d => d.IdVacunado)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_H_Vacunados_D_Vacunado");
            });

            modelBuilder.Entity<HVencidas>(entity =>
            {
                entity.HasOne(d => d.IdLugarNavigation)
                    .WithMany(p => p.HVencidas)
                    .HasForeignKey(d => d.IdLugar)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_H_Vencidas_D_Lugar");

                entity.HasOne(d => d.IdTiempoNavigation)
                    .WithMany(p => p.HVencidas)
                    .HasForeignKey(d => d.IdTiempo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_H_Vencidas_D_Tiempo");

                entity.HasOne(d => d.IdVacunaNavigation)
                    .WithMany(p => p.HVencidas)
                    .HasForeignKey(d => d.IdVacuna)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_H_Vencidas_D_Vacuna");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
