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

        // ---------------------------
        // Helpers
        // ---------------------------
        private IActionResult ApiBadRequest(string error, string details)
            => BadRequest(new { error, details });

        private bool MesValido(int mes) => mes >= 1 && mes <= 12;
        private bool AnioValido(int anio) => anio >= 2000 && anio <= 2100;
        private bool TipoValido(string tipo) => tipo == "Ingreso" || tipo == "Egreso";

        // ---------------------------
        // 1) Resumen mensual
        // GET /api/estadisticas/resumen?mes=3&anio=2026
        // ---------------------------
        [HttpGet("resumen")]
        public async Task<IActionResult> GetResumen([FromQuery] int mes, [FromQuery] int anio)
        {
            if (!MesValido(mes))
                return ApiBadRequest("Mes inválido", "El mes debe estar entre 1 y 12.");

            if (!AnioValido(anio))
                return ApiBadRequest("Año inválido", "El año debe estar en un rango razonable.");

            var resumen = await _estadisticasService.GetResumenMensualAsync(mes, anio);
            return Ok(resumen);
        }

        // ---------------------------
        // 2) Balance histórico
        // GET /api/estadisticas/balance-historico
        // ---------------------------
        [HttpGet("balance-historico")]
        public async Task<IActionResult> GetBalanceHistorico()
        {
            var data = await _estadisticasService.GetBalanceHistoricoAsync();
            return Ok(data);
        }

        // ---------------------------
        // 3) Serie diaria
        // GET /api/estadisticas/serie-diaria?mes=3&anio=2026&tipo=Egreso
        // ---------------------------
        [HttpGet("serie-diaria")]
        public async Task<IActionResult> GetSerieDiaria(
            [FromQuery] int mes,
            [FromQuery] int anio,
            [FromQuery] string tipo = "Egreso"
        )
        {
            if (!MesValido(mes))
                return ApiBadRequest("Mes inválido", "El mes debe estar entre 1 y 12.");

            if (!AnioValido(anio))
                return ApiBadRequest("Año inválido", "El año debe estar en un rango razonable.");

            if (!TipoValido(tipo))
                return ApiBadRequest("Tipo inválido", "Tipo debe ser 'Ingreso' o 'Egreso'.");

            var data = await _estadisticasService.GetSerieDiariaAsync(mes, anio, tipo);
            return Ok(data);
        }

        // ---------------------------
        // 4) Top categorías
        // GET /api/estadisticas/top-categorias?mes=3&anio=2026&tipo=Egreso&top=5
        // ---------------------------
        [HttpGet("top-categorias")]
        public async Task<IActionResult> GetTopCategorias(
            [FromQuery] int mes,
            [FromQuery] int anio,
            [FromQuery] string tipo = "Egreso",
            [FromQuery] int top = 5
        )
        {
            if (!MesValido(mes))
                return ApiBadRequest("Mes inválido", "El mes debe estar entre 1 y 12.");

            if (!AnioValido(anio))
                return ApiBadRequest("Año inválido", "El año debe estar en un rango razonable.");

            if (!TipoValido(tipo))
                return ApiBadRequest("Tipo inválido", "Tipo debe ser 'Ingreso' o 'Egreso'.");

            var data = await _estadisticasService.GetTopCategoriasAsync(mes, anio, tipo, top);
            return Ok(data);
        }

        // ---------------------------
        // 5) Comparación mensual
        // GET /api/estadisticas/comparacion-mensual?mes=3&anio=2026
        // ---------------------------
        [HttpGet("comparacion-mensual")]
        public async Task<IActionResult> GetComparacionMensual(
            [FromQuery] int mes,
            [FromQuery] int anio
        )
        {
            if (!MesValido(mes))
                return ApiBadRequest("Mes inválido", "El mes debe estar entre 1 y 12.");

            if (!AnioValido(anio))
                return ApiBadRequest("Año inválido", "El año debe estar en un rango razonable.");

            var data = await _estadisticasService.GetComparacionMensualAsync(mes, anio);
            return Ok(data);
        }
    }
}