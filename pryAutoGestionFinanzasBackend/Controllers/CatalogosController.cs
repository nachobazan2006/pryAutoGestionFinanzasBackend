using Microsoft.AspNetCore.Mvc;
using pryAutoGestionFinanzasBackend.Data;

namespace pryAutoGestionFinanzasBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CatalogosController : ControllerBase
    {
        private readonly AppDbContext _db;

        public CatalogosController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var categorias = _db.Categorias
                .Select(c => new { c.Id, c.Nombre, c.Tipo })
                .ToList();

            var mediosPago = _db.MediosPago
                .Select(m => new { m.Id, m.Nombre })
                .ToList();

            return Ok(new { categorias, mediosPago });
        }
    }
}