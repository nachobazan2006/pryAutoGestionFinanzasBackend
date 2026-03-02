using Microsoft.AspNetCore.Mvc;
using pryAutoGestionFinanzasBackend.Services;

namespace pryAutoGestionFinanzasBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EstadisticasController : ControllerBase
    {
        private readonly EstadisticasService _estadisticasService;

        public EstadisticasController(EstadisticasService estadisticasService)
        {
            _estadisticasService = estadisticasService;
        }

        [HttpGet("resumen")]
        public async Task<IActionResult> GetResumen([FromQuery] int mes, [FromQuery] int anio)
        {
            // Validación simple de mes/año
            if (mes < 1 || mes > 12)
                return BadRequest(new { error = "Mes inválido", details = "El mes debe estar entre 1 y 12." });

            if (anio < 2000 || anio > 2100)
                return BadRequest(new { error = "Año inválido", details = "El año debe estar en un rango razonable." });

            var resumen = await _estadisticasService.GetResumenMensualAsync(mes, anio);
            return Ok(resumen);
        }
    }
}