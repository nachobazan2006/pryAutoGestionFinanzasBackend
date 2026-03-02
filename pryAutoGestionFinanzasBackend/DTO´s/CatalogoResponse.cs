using pryAutoGestionFinanzasBackend.DTO_s;
using System.Collections.Generic;

namespace pryAutoGestionFinanzasBackend.DTOs
{
    public class CatalogosResponse
    {
        public List<CategoriaDTO> Categorias { get; set; } = new();
        public List<MedioPagoDTO> MediosPago { get; set; } = new();
    }
}