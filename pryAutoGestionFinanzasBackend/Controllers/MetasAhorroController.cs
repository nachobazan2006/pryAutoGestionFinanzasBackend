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
        public async Task<ActionResult<IEnumerable<MetaAhorro>>> GetMetas()
        {
            return await _context.MetasAhorro
                .Include(m => m.Aportes)
                .ToListAsync();
        }

        // GET: api/MetasAhorro/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MetaAhorro>> GetMeta(int id)
        {
            var meta = await _context.MetasAhorro
                .Include(m => m.Aportes)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (meta == null)
                return NotFound();

            return meta;
        }

        // POST: api/MetasAhorro
        [HttpPost]
        public async Task<ActionResult<MetaAhorro>> CreateMeta(MetaAhorro meta)
        {
            _context.MetasAhorro.Add(meta);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMeta), new { id = meta.Id }, meta);
        }

        // PUT: api/MetasAhorro/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMeta(int id, MetaAhorro meta)
        {
            if (id != meta.Id)
                return BadRequest();

            _context.Entry(meta).State = EntityState.Modified;
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