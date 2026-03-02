namespace pryAutoGestionFinanzasBackend.DTOs
{
    public class ResumenMensualResponse
    {
        public int Mes { get; set; }
        public int Anio { get; set; }
        public decimal TotalIngresos { get; set; }
        public decimal TotalEgresos { get; set; }
        public decimal Balance => TotalIngresos - TotalEgresos;
    }
}