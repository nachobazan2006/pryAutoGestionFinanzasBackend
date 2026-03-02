using Microsoft.EntityFrameworkCore;
using pryAutoGestionFinanzasBackend.Models;

namespace pryAutoGestionFinanzasBackend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Movimiento> Movimientos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<MedioPago> MediosPago { get; set; }
    }
}