using System;
using System.Collections.Generic;

namespace TuProyectoBackend.Models
{
    public class MetaAhorro
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public decimal Objetivo { get; set; }

        public string Moneda { get; set; } = "ARS";

        public string LugarGuardado { get; set; } = string.Empty;

        public DateTime? FechaObjetivo { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        // Relación
        public ICollection<AporteAhorro> Aportes { get; set; } = new List<AporteAhorro>();
    }
}