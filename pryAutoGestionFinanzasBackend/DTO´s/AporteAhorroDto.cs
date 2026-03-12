namespace pryAutoGestionFinanzasBackend.Dtos
{
    public class AporteAhorroDto
    {
        public int Id { get; set; }
        public int MetaAhorroId { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Monto { get; set; }
        public string? Nota { get; set; }
        public string CreadoPor { get; set; } = "";
    }
}