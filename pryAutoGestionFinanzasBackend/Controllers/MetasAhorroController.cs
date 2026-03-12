using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pryAutoGestionFinanzasBackend.Data;
using TuProyectoBackend.Models;

namespace pryAutoGestionFinanzasBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MetasAhorroController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MetasAhorroController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/MetasAhorro
        [HttpGet]
        public async Task<IActionResult> GetMetas()
        {
            var lista = await _context.MetasAhorro
                .OrderByDescending(m => m.Id)
                .Select(m => new
                {
                    id = m.Id,
                    nombre = m.Nombre,
                    objetivo = m.Objetivo,
                    moneda = m.Moneda,
                    lugarGuardado = m.LugarGuardado,
                    fechaObjetivo = m.FechaObjetivo,
                    creadoPor = m.CreadoPor
                })
                .ToListAsync();

            return Ok(lista);
        }

        // GET: api/MetasAhorro/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMeta(int id)
        {
            var meta = await _context.MetasAhorro
                .Where(m => m.Id == id)
                .Select(m => new
                {
                    id = m.Id,
                    nombre = m.Nombre,
                    objetivo = m.Objetivo,
                    moneda = m.Moneda,
                    lugarGuardado = m.LugarGuardado,
                    fechaObjetivo = m.FechaObjetivo,
                    creadoPor = m.CreadoPor
                })
                .FirstOrDefaultAsync();

            if (meta == null)
                return NotFound();

            return Ok(meta);
        }

        // POST: api/MetasAhorro
        [HttpPost]
        public async Task<IActionResult> CreateMeta(MetaAhorro meta)
        {
            _context.MetasAhorro.Add(meta);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMeta), new { id = meta.Id }, new
            {
                id = meta.Id,
                nombre = meta.Nombre,
                objetivo = meta.Objetivo,
                moneda = meta.Moneda,
                lugarGuardado = meta.LugarGuardado,
                fechaObjetivo = meta.FechaObjetivo,
                creadoPor = meta.CreadoPor
            });
        }

        // PUT: api/MetasAhorro/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMeta(int id, MetaAhorro meta)
        {
            if (id != meta.Id)
                return BadRequest();

            var existente = await _context.MetasAhorro.FindAsync(id);
            if (existente == null)
                return NotFound();

            existente.Nombre = meta.Nombre;
            existente.Objetivo = meta.Objetivo;
            existente.Moneda = meta.Moneda;
            existente.LugarGuardado = meta.LugarGuardado;
            existente.FechaObjetivo = meta.FechaObjetivo;
            existente.CreadoPor = meta.CreadoPor;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/MetasAhorro/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMeta(int id)
        {
            var meta = await _context.MetasAhorro.FindAsync(id);

            if (meta == null)
                return NotFound();

            _context.MetasAhorro.Remove(meta);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}