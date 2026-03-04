using System;

namespace TuProyectoBackend.Models
{
    public class AporteAhorro
    {
        public int Id { get; set; }

        public int MetaAhorroId { get; set; }

        public decimal Monto { get; set; }

        public DateTime Fecha { get; set; }

        public string? Nota { get; set; }

        // Relación
        public MetaAhorro MetaAhorro { get; set; } = null!;
    }
}