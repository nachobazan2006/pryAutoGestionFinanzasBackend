using System;

namespace pryAutoGestionFinanzasBackend.Models
{
    public class Movimiento
    {
        public int Id { get; set; }                      // PK

        public string Tipo { get; set; } = "";           // "Ingreso" o "Egreso"

        public int CategoriaId { get; set; }             // FK
        public Categoria? Categoria { get; set; }        // navegación

        public decimal Monto { get; set; }               // 4500.00
        public DateTime Fecha { get; set; }              // fecha del movimiento

        public int MedioPagoId { get; set; }             // FK
        public MedioPago? MedioPago { get; set; }        // navegación

        public string? Descripcion { get; set; }         // opcional
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}