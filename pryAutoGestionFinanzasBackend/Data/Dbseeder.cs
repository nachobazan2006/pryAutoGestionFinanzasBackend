using pryAutogestionFinanzas.Backend.Data;
using pryAutoGestionFinanzasBackend.Models;

namespace pryAutoGestionFinanzasBackend.Data
{
    public static class DbSeeder
    {
        public static void Seed(AppDbContext db)
        {
            // Medios de pago
            if (!db.MediosPago.Any())
            {
                db.MediosPago.AddRange(
                    new MedioPago { Nombre = "Efectivo" },
                    new MedioPago { Nombre = "Débito" },
                    new MedioPago { Nombre = "Crédito" },
                    new MedioPago { Nombre = "Transferencia" }
                );
            }

            // Categorías (mezcla de egresos e ingresos, mínimo viable)
            if (!db.Categorias.Any())
            {
                db.Categorias.AddRange(
                    new Categoria { Nombre = "Comida", Tipo = "Egreso" },
                    new Categoria { Nombre = "Transporte", Tipo = "Egreso" },
                    new Categoria { Nombre = "Servicios", Tipo = "Egreso" },
                    new Categoria { Nombre = "Salud", Tipo = "Egreso" },
                    new Categoria { Nombre = "Sueldo", Tipo = "Ingreso" },
                    new Categoria { Nombre = "Freelance", Tipo = "Ingreso" }
                );
            }

            db.SaveChanges();
        }
    }
}