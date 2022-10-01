using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace VacunacionApi.Models
{
    public partial class VacunasContext : DbContext
    {
        public VacunasContext(DbContextOptions<VacunasContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Compra> Compra { get; set; }
        public virtual DbSet<Distribucion> Distribucion { get; set; }
        public virtual DbSet<Dosis> Dosis { get; set; }
        public virtual DbSet<EntidadDosisRegla> EntidadDosisRegla { get; set; }
        public virtual DbSet<EntidadVacunaDosis> EntidadVacunaDosis { get; set; }
        public virtual DbSet<EstadoCompra> EstadoCompra { get; set; }
        public virtual DbSet<Jurisdiccion> Jurisdiccion { get; set; }
        public virtual DbSet<Lote> Lote { get; set; }
        public virtual DbSet<MarcaComercial> MarcaComercial { get; set; }
        public virtual DbSet<Pandemia> Pandemia { get; set; }
        public virtual DbSet<Regla> Regla { get; set; }
        public virtual DbSet<Rol> Rol { get; set; }
        public virtual DbSet<TipoVacuna> TipoVacuna { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }
        public virtual DbSet<Vacuna> Vacuna { get; set; }
        public virtual DbSet<VacunaAplicada> VacunaAplicada { get; set; }
        public virtual DbSet<VacunaDesarrollada> VacunaDesarrollada { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Compra>(entity =>
            {
                entity.HasOne(d => d.IdEstadoCompraNavigation)
                    .WithMany(p => p.Compra)
                    .HasForeignKey(d => d.IdEstadoCompra)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Compra_Estado");

                entity.HasOne(d => d.IdLoteNavigation)
                    .WithMany(p => p.Compra)
                    .HasForeignKey(d => d.IdLote)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Compra_Lote");
            });

            modelBuilder.Entity<Distribucion>(entity =>
            {
                entity.HasOne(d => d.IdJurisdiccionNavigation)
                    .WithMany(p => p.Distribucion)
                    .HasForeignKey(d => d.IdJurisdiccion)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Distribucion_Jurisdiccion");

                entity.HasOne(d => d.IdLoteNavigation)
                    .WithMany(p => p.Distribucion)
                    .HasForeignKey(d => d.IdLote)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Distribucion_Lote");
            });

            modelBuilder.Entity<EntidadDosisRegla>(entity =>
            {
                entity.HasOne(d => d.IdDosisNavigation)
                    .WithMany(p => p.EntidadDosisRegla)
                    .HasForeignKey(d => d.IdDosis)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Entidad_Dosis_Regla_Dosis");

                entity.HasOne(d => d.IdReglaNavigation)
                    .WithMany(p => p.EntidadDosisRegla)
                    .HasForeignKey(d => d.IdRegla)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Entidad_Dosis_Regla_Regla");
            });

            modelBuilder.Entity<EntidadVacunaDosis>(entity =>
            {
                entity.HasOne(d => d.IdDosisNavigation)
                    .WithMany(p => p.EntidadVacunaDosis)
                    .HasForeignKey(d => d.IdDosis)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Entidad_Vacuna_Dosis_Dosis");

                entity.HasOne(d => d.IdVacunaNavigation)
                    .WithMany(p => p.EntidadVacunaDosis)
                    .HasForeignKey(d => d.IdVacuna)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Entidad_Vacuna_Dosis_Vacuna");
            });

            modelBuilder.Entity<Lote>(entity =>
            {
                entity.HasOne(d => d.IdVacunaDesarrolladaNavigation)
                    .WithMany(p => p.Lote)
                    .HasForeignKey(d => d.IdVacunaDesarrollada)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Lote_Vacuna_Desarrollada");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasOne(d => d.IdJurisdiccionNavigation)
                    .WithMany(p => p.Usuario)
                    .HasForeignKey(d => d.IdJurisdiccion)
                    .HasConstraintName("FK_Usuario_Jurisdiccion");

                entity.HasOne(d => d.IdRolNavigation)
                    .WithMany(p => p.Usuario)
                    .HasForeignKey(d => d.IdRol)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Usuario_Rol");
            });

            modelBuilder.Entity<Vacuna>(entity =>
            {
                entity.HasOne(d => d.IdPandemiaNavigation)
                    .WithMany(p => p.Vacuna)
                    .HasForeignKey(d => d.IdPandemia)
                    .HasConstraintName("FK_Vacuna_Pandemia");

                entity.HasOne(d => d.IdTipoVacunaNavigation)
                    .WithMany(p => p.Vacuna)
                    .HasForeignKey(d => d.IdTipoVacuna)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Vacuna_Tipo_Vacuna");
            });

            modelBuilder.Entity<VacunaAplicada>(entity =>
            {
                entity.HasOne(d => d.IdDosisNavigation)
                    .WithMany(p => p.VacunaAplicada)
                    .HasForeignKey(d => d.IdDosis)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Vacuna_Aplicada_Dosis");

                entity.HasOne(d => d.IdJurisdiccionNavigation)
                    .WithMany(p => p.VacunaAplicada)
                    .HasForeignKey(d => d.IdJurisdiccion)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Vacuna_Aplicada_Jurisdiccion");

                entity.HasOne(d => d.IdLoteNavigation)
                    .WithMany(p => p.VacunaAplicada)
                    .HasForeignKey(d => d.IdLote)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Vacuna_Aplicada_Lote");
            });

            modelBuilder.Entity<VacunaDesarrollada>(entity =>
            {
                entity.HasOne(d => d.IdMarcaComercialNavigation)
                    .WithMany(p => p.VacunaDesarrollada)
                    .HasForeignKey(d => d.IdMarcaComercial)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Vacuna_Desarrollada_Marca_Comercial");

                entity.HasOne(d => d.IdVacunaNavigation)
                    .WithMany(p => p.VacunaDesarrollada)
                    .HasForeignKey(d => d.IdVacuna)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Vacuna_Desarrollada_Vacuna");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
