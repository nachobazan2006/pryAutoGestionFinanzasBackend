namespace pryAutoGestionFinanzasBackend.DTOs
{
    public class ComparacionMensualDTO
    {
        public int Mes { get; set; }
        public int Anio { get; set; }

        public decimal IngresosMesActual { get; set; }
        public decimal EgresosMesActual { get; set; }

        public int MesAnterior { get; set; }
        public int AnioAnterior { get; set; }

        public decimal IngresosMesAnterior { get; set; }
        public decimal EgresosMesAnterior { get; set; }

        // Variaciones (%). Si anterior es 0, devolvemos null para evitar división por 0.
        public decimal? VarIngresosPct { get; set; }
        public decimal? VarEgresosPct { get; set; }
    }
}