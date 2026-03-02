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

        public async Task<ResumenMensualResponse> GetResumenMensualAsync(int mes, int anio)
        {
            // Rango de fechas del mes
            var desde = new DateTime(anio, mes, 1);
            var hasta = desde.AddMonths(1).AddTicks(-1);

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
    }
}