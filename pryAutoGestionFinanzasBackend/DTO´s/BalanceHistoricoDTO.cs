namespace pryAutoGestionFinanzasBackend.DTOs
{
    public class BalanceHistoricoDTO
    {
        public decimal TotalIngresos { get; set; }
        public decimal TotalEgresos { get; set; }
        public decimal Balance => TotalIngresos - TotalEgresos;
    }
}