using Microsoft.EntityFrameworkCore;
using pryAutoGestionFinanzasBackend.Data;
using pryAutoGestionFinanzasBackend.DTOs;

namespace pryAutoGestionFinanzasBackend.Services
{
    public class EstadisticasService
    {
        private readonly AppDbContext _db;

        public EstadisticasService(AppDbContext db)
        {
            _db = db;
        }

        // ---------------------------
        // Helpers
        // ---------------------------
        private static (DateTime desde, DateTime hasta) GetRangoMes(int mes, int anio)
        {
            var desde = new DateTime(anio, mes, 1);
            var hasta = desde.AddMonths(1).AddTicks(-1);
            return (desde, hasta);
        }

        private static decimal? VarPct(decimal actual, decimal anterior)
        {
            if (anterior == 0m) return null;
            return ((actual - anterior) / anterior) * 100m;
        }

        // ---------------------------
        // 1) Resumen mensual
        // ---------------------------
        public async Task<ResumenMensualResponse> GetResumenMensualAsync(int mes, int anio)
        {
            var (desde, hasta) = GetRangoMes(mes, anio);

            var movimientosMes = _db.Movimientos
                .AsNoTracking()
                .Where(m => m.Fecha >= desde && m.Fecha <= hasta);

            var totalIngresos = await movimientosMes
                .Where(m => m.Tipo == "Ingreso")
                .SumAsync(m => (decimal?)m.Monto) ?? 0m;

            var totalEgresos = await movimientosMes
                .Where(m => m.Tipo == "Egreso")
                .SumAsync(m => (decimal?)m.Monto) ?? 0m;

            return new ResumenMensualResponse
            {
                Mes = mes,
                Anio = anio,
                TotalIngresos = totalIngresos,
                TotalEgresos = totalEgresos
            };
        }

        // ---------------------------
        // 2) Top categorías del mes (por tipo)
        // ---------------------------
        public async Task<List<TopCategoriaDTO>> GetTopCategoriasAsync(int mes, int anio, string tipo, int top)
        {
            var (desde, hasta) = GetRangoMes(mes, anio);

            // Seguridad por si te mandan top raro
            if (top <= 0) top = 5;
            if (top > 20) top = 20;

            var query = _db.Movimientos
                .AsNoTracking()
                .Include(m => m.Categoria)
                .Where(m => m.Fecha >= desde && m.Fecha <= hasta)
                .Where(m => m.Tipo == tipo);

            var result = await query
                .GroupBy(m => new
                {
                    m.CategoriaId,
                    CategoriaNombre = m.Categoria != null ? m.Categoria.Nombre : "Sin categoría"
                })
                .Select(g => new TopCategoriaDTO
                {
                    CategoriaId = g.Key.CategoriaId,
                    Categoria = g.Key.CategoriaNombre,
                    Total = g.Sum(x => x.Monto)
                })
                .OrderByDescending(x => x.Total)
                .Take(top)
                .ToListAsync();

            return result;
        }

        // ---------------------------
        // 3) Balance histórico (acumulado)
        // ---------------------------
        public async Task<BalanceHistoricoDTO> GetBalanceHistoricoAsync()
        {
            var totalIngresos = await _db.Movimientos
                .AsNoTracking()
                .Where(m => m.Tipo == "Ingreso")
                .SumAsync(m => (decimal?)m.Monto) ?? 0m;

            var totalEgresos = await _db.Movimientos
                .AsNoTracking()
                .Where(m => m.Tipo == "Egreso")
                .SumAsync(m => (decimal?)m.Monto) ?? 0m;

            return new BalanceHistoricoDTO
            {
                TotalIngresos = totalIngresos,
                TotalEgresos = totalEgresos
            };
        }

        // ---------------------------
        // 4) Serie diaria del mes (incluye días con 0)
        // ---------------------------
        public async Task<List<SerieDiariaDTO>> GetSerieDiariaAsync(int mes, int anio, string tipo)
        {
            var (desde, hasta) = GetRangoMes(mes, anio);
            var diasEnMes = DateTime.DaysInMonth(anio, mes);

            var data = await _db.Movimientos
                .AsNoTracking()
                .Where(m => m.Tipo == tipo)
                .Where(m => m.Fecha >= desde && m.Fecha <= hasta)
                .GroupBy(m => m.Fecha.Day)
                .Select(g => new { Dia = g.Key, Total = g.Sum(x => x.Monto) })
                .ToListAsync();

            var dict = data.ToDictionary(x => x.Dia, x => x.Total);

            var result = new List<SerieDiariaDTO>(diasEnMes);
            for (int d = 1; d <= diasEnMes; d++)
            {
                result.Add(new SerieDiariaDTO
                {
                    Dia = d,
                    Total = dict.TryGetValue(d, out var total) ? total : 0m
                });
            }

            return result;
        }

        // ---------------------------
        // 5) Comparación mes actual vs mes anterior
        // ---------------------------
        public async Task<ComparacionMensualDTO> GetComparacionMensualAsync(int mes, int anio)
        {
            var (desdeActual, hastaActual) = GetRangoMes(mes, anio);

            var dtAnterior = new DateTime(anio, mes, 1).AddMonths(-1);
            var mesAnterior = dtAnterior.Month;
            var anioAnterior = dtAnterior.Year;

            var (desdeAnterior, hastaAnterior) = GetRangoMes(mesAnterior, anioAnterior);

            var qActual = _db.Movimientos.AsNoTracking().Where(m => m.Fecha >= desdeActual && m.Fecha <= hastaActual);
            var ingresosActual = await qActual.Where(m => m.Tipo == "Ingreso").SumAsync(m => (decimal?)m.Monto) ?? 0m;
            var egresosActual = await qActual.Where(m => m.Tipo == "Egreso").SumAsync(m => (decimal?)m.Monto) ?? 0m;

            var qAnterior = _db.Movimientos.AsNoTracking().Where(m => m.Fecha >= desdeAnterior && m.Fecha <= hastaAnterior);
            var ingresosAnterior = await qAnterior.Where(m => m.Tipo == "Ingreso").SumAsync(m => (decimal?)m.Monto) ?? 0m;
            var egresosAnterior = await qAnterior.Where(m => m.Tipo == "Egreso").SumAsync(m => (decimal?)m.Monto) ?? 0m;

            return new ComparacionMensualDTO
            {
                Mes = mes,
                Anio = anio,

                IngresosMesActual = ingresosActual,
                EgresosMesActual = egresosActual,

                MesAnterior = mesAnterior,
                AnioAnterior = anioAnterior,

                IngresosMesAnterior = ingresosAnterior,
                EgresosMesAnterior = egresosAnterior,

                VarIngresosPct = VarPct(ingresosActual, ingresosAnterior),
                VarEgresosPct = VarPct(egresosActual, egresosAnterior)
            };
        }
    }
}