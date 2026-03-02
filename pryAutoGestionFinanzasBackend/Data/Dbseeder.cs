using pryAutoGestionFinanzasBackend.Models;

namespace pryAutoGestionFinanzasBackend.Data
{
    public static class DbSeeder
    {
        public static void Seed(AppDbContext db)
        {
            // 1) Medios de pago
            if (!db.MediosPago.Any())
            {
                db.MediosPago.AddRange(
                    new MedioPago { Nombre = "Efectivo" },
                    new MedioPago { Nombre = "Débito" },
                    new MedioPago { Nombre = "Crédito" },
                    new MedioPago { Nombre = "Transferencia" }
                );
            }

            // 2) Categorías (mínimo viable para arrancar)
            if (!db.Categorias.Any())
            {
                db.Categorias.AddRange(
                    // EGRESO
                    new Categoria { Nombre = "Comida", Tipo = "Egreso" },
                    new Categoria { Nombre = "Transporte", Tipo = "Egreso" },
                    new Categoria { Nombre = "Servicios", Tipo = "Egreso" },
                    new Categoria { Nombre = "Salud", Tipo = "Egreso" },
                    new Categoria { Nombre = "Ocio", Tipo = "Egreso" },

                    // INGRESO
                    new Categoria { Nombre = "Sueldo", Tipo = "Ingreso" },
                    new Categoria { Nombre = "Freelance", Tipo = "Ingreso" },
                    new Categoria { Nombre = "Ventas", Tipo = "Ingreso" }
                );
            }

            db.SaveChanges();
        }
    }
}