namespace pryAutoGestionFinanzasBackend.DTOs
{
    public class MovimientoResponse
    {
        public int Id { get; set; }
        public string Tipo { get; set; } = "";
        public int CategoriaId { get; set; }
        public int MedioPagoId { get; set; }
        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; }
        public string? Descripcion { get; set; }
    }
}