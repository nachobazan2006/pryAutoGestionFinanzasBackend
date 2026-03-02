namespace pryAutoGestionFinanzasBackend.DTOs
{
    public class UpdateMovimientoRequest
    {
        public string Tipo { get; set; } = "";     // "Ingreso" / "Egreso"
        public int CategoriaId { get; set; }
        public int MedioPagoId { get; set; }
        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; }
        public string? Descripcion { get; set; }
    }
}