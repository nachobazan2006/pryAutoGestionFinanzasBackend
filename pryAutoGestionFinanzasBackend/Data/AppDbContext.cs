using Microsoft.EntityFrameworkCore;
using pryAutoGestionFinanzasBackend.Models;

namespace pryAutogestionFinanzas.Backend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Estas 3 líneas son las "tablas"
        public DbSet<Movimiento> Movimientos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<MedioPago> MediosPago { get; set; }
    }
}