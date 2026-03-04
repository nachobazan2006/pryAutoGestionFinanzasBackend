using Microsoft.EntityFrameworkCore;
using pryAutoGestionFinanzasBackend.Models;
using TuProyectoBackend.Models;

namespace pryAutoGestionFinanzasBackend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Movimiento> Movimientos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<MedioPago> MediosPago { get; set; }

        public DbSet<MetaAhorro> MetasAhorro { get; set; }
        public DbSet<AporteAhorro> AportesAhorro { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ============================
            // RELACIÓN META → APORTES
            // ============================

            modelBuilder.Entity<AporteAhorro>()
                .HasOne(a => a.MetaAhorro)
                .WithMany(m => m.Aportes)
                .HasForeignKey(a => a.MetaAhorroId)
                .OnDelete(DeleteBehavior.Cascade);

            // ============================
            // PRECISIÓN DECIMAL (FINANZAS)
            // ============================

            modelBuilder.Entity<MetaAhorro>()
                .Property(m => m.Objetivo)
                .HasPrecision(18, 2);

            modelBuilder.Entity<AporteAhorro>()
                .Property(a => a.Monto)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Movimiento>()
                .Property(m => m.Monto)
                .HasPrecision(18, 2);
        }
    }
}