using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pryAutoGestionFinanzasBackend.Data;
using pryAutoGestionFinanzasBackend.DTO_s;
using pryAutoGestionFinanzasBackend.DTOs;

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

        private static bool IsTipoValido(string? tipo)
            => tipo == "Ingreso" || tipo == "Egreso";

        // GET /api/catalogos  -> todo junto
        [HttpGet]
        public async Task<ActionResult<CatalogosResponse>> GetAll()
        {
            var categorias = await _db.Categorias
                .AsNoTracking()
                .OrderBy(c => c.Tipo).ThenBy(c => c.Nombre)
                .Select(c => new CategoriaDTO
                {
                    Id = c.Id,
                    Nombre = c.Nombre,
                    Tipo = c.Tipo
                })
                .ToListAsync();

            var medios = await _db.MediosPago
                .AsNoTracking()
                .OrderBy(m => m.Nombre)
                .Select(m => new MedioPagoDTO
                {
                    Id = m.Id,
                    Nombre = m.Nombre
                })
                .ToListAsync();

            return Ok(new CatalogosResponse
            {
                Categorias = categorias,
                MediosPago = medios
            });
        }

        // GET /api/catalogos/categorias?tipo=Ingreso|Egreso
        [HttpGet("categorias")]
        public async Task<ActionResult<List<CategoriaDTO>>> GetCategorias([FromQuery] string? tipo = null)
        {
            if (!string.IsNullOrWhiteSpace(tipo) && !IsTipoValido(tipo))
                return BadRequest(new { error = "Tipo inválido", details = "Tipo debe ser 'Ingreso' o 'Egreso'." });

            var query = _db.Categorias.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(tipo))
                query = query.Where(c => c.Tipo == tipo);

            var categorias = await query
                .OrderBy(c => c.Nombre)
                .Select(c => new CategoriaDTO
                {
                    Id = c.Id,
                    Nombre = c.Nombre,
                    Tipo = c.Tipo
                })
                .ToListAsync();

            return Ok(categorias);
        }

        // GET /api/catalogos/medios-pago
        [HttpGet("medios-pago")]
        public async Task<ActionResult<List<MedioPagoDTO>>> GetMediosPago()
        {
            var medios = await _db.MediosPago
                .AsNoTracking()
                .OrderBy(m => m.Nombre)
                .Select(m => new MedioPagoDTO
                {
                    Id = m.Id,
                    Nombre = m.Nombre
                })
                .ToListAsync();

            return Ok(medios);
        }
    }
}