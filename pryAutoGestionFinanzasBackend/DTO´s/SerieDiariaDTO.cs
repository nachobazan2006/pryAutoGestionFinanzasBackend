namespace pryAutoGestionFinanzasBackend.DTOs
{
    public class SerieDiariaDTO
    {
        public int Dia { get; set; }        // 1..31
        public decimal Total { get; set; }  // total del día
    }
}