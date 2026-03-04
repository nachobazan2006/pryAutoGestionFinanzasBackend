using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pryAutoGestionFinanzasBackend.Data;
using TuProyectoBackend.Models;

namespace pryAutoGestionFinanzasBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AportesAhorroController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AportesAhorroController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/AportesAhorro/ByMeta/5
        [HttpGet("ByMeta/{metaId}")]
        public async Task<ActionResult<IEnumerable<AporteAhorro>>> GetByMeta(int metaId)
        {
            return await _context.AportesAhorro
                .Where(a => a.MetaAhorroId == metaId)
                .OrderByDescending(a => a.Fecha)
                .ToListAsync();
        }

        // POST: api/AportesAhorro
        [HttpPost]
        public async Task<ActionResult<AporteAhorro>> CreateAporte(AporteAhorro aporte)
        {
            _context.AportesAhorro.Add(aporte);
            await _context.SaveChangesAsync();

            return Ok(aporte);
        }

        // DELETE: api/AportesAhorro/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAporte(int id)
        {
            var aporte = await _context.AportesAhorro.FindAsync(id);

            if (aporte == null)
                return NotFound();

            _context.AportesAhorro.Remove(aporte);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}