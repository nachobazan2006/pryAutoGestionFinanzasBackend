namespace pryAutoGestionFinanzasBackend.DTOs
{
    public class TopCategoriaDTO
    {
        public int CategoriaId { get; set; }
        public string Categoria { get; set; } = "";
        public decimal Total { get; set; }
    }
}