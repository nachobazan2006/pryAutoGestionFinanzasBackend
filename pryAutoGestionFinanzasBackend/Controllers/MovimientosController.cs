using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pryAutoGestionFinanzasBackend.Data;
using pryAutoGestionFinanzasBackend.DTOs;
using pryAutoGestionFinanzasBackend.Models;

namespace pryAutoGestionFinanzasBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovimientosController : ControllerBase
    {
        private readonly AppDbContext _db;

        public MovimientosController(AppDbContext db)
        {
            _db = db;
        }

        // ---------------------------
        // Helpers (validación + errores)
        // ---------------------------

        private IActionResult ApiBadRequest(string error, string details)
            => BadRequest(new { error, details });

        private bool IsTipoValido(string? tipo)
            => tipo == "Ingreso" || tipo == "Egreso";

        private async Task<bool> CategoriaExisteAsync(int categoriaId)
            => await _db.Categorias.AnyAsync(c => c.Id == categoriaId);

        private async Task<bool> MedioPagoExisteAsync(int medioPagoId)
            => await _db.MediosPago.AnyAsync(m => m.Id == medioPagoId);

        private async Task<IActionResult?> ValidarMovimientoAsync(string tipo, int categoriaId, int medioPagoId, decimal monto)
        {
            if (monto <= 0)
                return ApiBadRequest("Monto inválido", "El monto debe ser mayor a 0.");

            if (!IsTipoValido(tipo))
                return ApiBadRequest("Tipo inválido", "Tipo debe ser 'Ingreso' o 'Egreso'.");

            if (!await CategoriaExisteAsync(categoriaId))
                return ApiBadRequest("Categoría inválida", "No existe la categoría indicada.");

            if (!await MedioPagoExisteAsync(medioPagoId))
                return ApiBadRequest("Medio de pago inválido", "No existe el medio de pago indicado.");

            return null;
        }

        private static MovimientoResponse ToResponse(Movimiento m)
        {
            return new MovimientoResponse
            {
                Id = m.Id,
                Tipo = m.Tipo,
                CategoriaId = m.CategoriaId,
                MedioPagoId = m.MedioPagoId,
                Monto = m.Monto,
                Fecha = m.Fecha,
                Descripcion = m.Descripcion,
                CreadoPor = m.CreadoPor
            };
        }

        // ---------------------------
        // POST: crear
        // ---------------------------
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateMovimientoRequest request)
        {
            var validationResult = await ValidarMovimientoAsync(
                request.Tipo,
                request.CategoriaId,
                request.MedioPagoId,
                request.Monto
            );

            if (validationResult != null)
                return validationResult;

            var movimiento = new Movimiento
            {
                Tipo = request.Tipo,
                CategoriaId = request.CategoriaId,
                MedioPagoId = request.MedioPagoId,
                Monto = request.Monto,
                Fecha = request.Fecha,
                Descripcion = request.Descripcion,
                CreadoPor = request.CreadoPor
            };

            _db.Movimientos.Add(movimiento);
            await _db.SaveChangesAsync();

            var response = ToResponse(movimiento);

            return CreatedAtAction(nameof(GetById), new { id = movimiento.Id }, response);
        }

        // ---------------------------
        // GET: listar (con filtros)
        // ---------------------------
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? tipo = null,
            [FromQuery] DateTime? desde = null,
            [FromQuery] DateTime? hasta = null,
            [FromQuery] int? categoriaId = null,
            [FromQuery] int? medioPagoId = null
        )
        {
            if (!string.IsNullOrWhiteSpace(tipo) && !IsTipoValido(tipo))
                return ApiBadRequest("Tipo inválido", "Tipo debe ser 'Ingreso' o 'Egreso'.");

            var query = _db.Movimientos
                .Include(m => m.Categoria)
                .Include(m => m.MedioPago)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(tipo))
                query = query.Where(m => m.Tipo == tipo);

            if (desde.HasValue)
                query = query.Where(m => m.Fecha >= desde.Value);

            if (hasta.HasValue)
                query = query.Where(m => m.Fecha <= hasta.Value);

            if (categoriaId.HasValue)
                query = query.Where(m => m.CategoriaId == categoriaId.Value);

            if (medioPagoId.HasValue)
                query = query.Where(m => m.MedioPagoId == medioPagoId.Value);

            var lista = await query
                .OrderByDescending(m => m.Fecha)
                .Select(m => new
                {
                    id = m.Id,
                    tipo = m.Tipo,
                    monto = m.Monto,
                    fecha = m.Fecha,
                    descripcion = m.Descripcion,
                    creadoPor = m.CreadoPor,
                    categoriaId = m.CategoriaId,
                    categoria = m.Categoria != null ? m.Categoria.Nombre : null,
                    medioPagoId = m.MedioPagoId,
                    medioPago = m.MedioPago != null ? m.MedioPago.Nombre : null
                })
                .ToListAsync();

            return Ok(lista);
        }

        // ---------------------------
        // GET by id
        // ---------------------------
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var mov = await _db.Movimientos
                .AsNoTracking()
                .Where(m => m.Id == id)
                .Select(m => new
                {
                    id = m.Id,
                    tipo = m.Tipo,
                    monto = m.Monto,
                    fecha = m.Fecha,
                    descripcion = m.Descripcion,
                    creadoPor = m.CreadoPor,
                    categoriaId = m.CategoriaId,
                    medioPagoId = m.MedioPagoId
                })
                .FirstOrDefaultAsync();

            if (mov == null)
                return NotFound(new { error = "Movimiento no encontrado" });

            return Ok(mov);
        }

        // ---------------------------
        // PUT: editar
        // ---------------------------
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateMovimientoRequest req)
        {
            var mov = await _db.Movimientos.FirstOrDefaultAsync(x => x.Id == id);
            if (mov == null)
                return NotFound(new { error = "Movimiento no encontrado" });

            var validationResult = await ValidarMovimientoAsync(
                req.Tipo,
                req.CategoriaId,
                req.MedioPagoId,
                req.Monto
            );

            if (validationResult != null)
                return validationResult;

            mov.Tipo = req.Tipo;
            mov.CategoriaId = req.CategoriaId;
            mov.MedioPagoId = req.MedioPagoId;
            mov.Monto = req.Monto;
            mov.Fecha = req.Fecha;
            mov.Descripcion = req.Descripcion;
            mov.CreadoPor = req.CreadoPor;

            await _db.SaveChangesAsync();

            return Ok(new { message = "Movimiento actualizado", id = mov.Id });
        }

        // ---------------------------
        // DELETE: borrar
        // ---------------------------
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var mov = await _db.Movimientos.FirstOrDefaultAsync(x => x.Id == id);
            if (mov == null)
                return NotFound(new { error = "Movimiento no encontrado" });

            _db.Movimientos.Remove(mov);
            await _db.SaveChangesAsync();

            return Ok(new { message = "Movimiento eliminado", id });
        }
    }
}