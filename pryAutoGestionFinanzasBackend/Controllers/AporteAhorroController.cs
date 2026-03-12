using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pryAutoGestionFinanzasBackend.Data;
using pryAutoGestionFinanzasBackend.Dtos;
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

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var lista = await _context.AportesAhorro
                .OrderByDescending(a => a.Fecha)
                .Select(a => new
                {
                    id = a.Id,
                    metaAhorroId = a.MetaAhorroId,
                    fecha = a.Fecha,
                    monto = a.Monto,
                    notas = a.Nota,
                    creadoPor = a.CreadoPor
                })
                .ToListAsync();

            return Ok(lista);
        }

        // GET: api/AportesAhorro/ByMeta/5
        [HttpGet("ByMeta/{metaId}")]
        public async Task<IActionResult> GetByMeta(int metaId)
        {
            var lista = await _context.AportesAhorro
                .Where(a => a.MetaAhorroId == metaId)
                .OrderByDescending(a => a.Fecha)
                .Select(a => new
                {
                    id = a.Id,
                    metaAhorroId = a.MetaAhorroId,
                    fecha = a.Fecha,
                    monto = a.Monto,
                    notas = a.Nota,
                    creadoPor = a.CreadoPor
                })
                .ToListAsync();

            return Ok(lista);
        }

        // POST: api/AportesAhorro
        [HttpPost]
        public async Task<ActionResult<AporteAhorro>> CreateAporte(AporteAhorroDto dto)
        {
            var aporte = new AporteAhorro
            {
                MetaAhorroId = dto.MetaAhorroId,
                Fecha = dto.Fecha,
                Monto = dto.Monto,
                Nota = dto.Nota,
                CreadoPor = dto.CreadoPor
            };

            _context.AportesAhorro.Add(aporte);
            await _context.SaveChangesAsync();

            return Ok(aporte);
        }

        // PUT: api/AportesAhorro/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAporte(int id, AporteAhorroDto dto)
        {
            var existente = await _context.AportesAhorro.FindAsync(id);

            if (existente == null)
                return NotFound();

            existente.MetaAhorroId = dto.MetaAhorroId;
            existente.Fecha = dto.Fecha;
            existente.Monto = dto.Monto;
            existente.Nota = dto.Nota;

            await _context.SaveChangesAsync();

            return NoContent();
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