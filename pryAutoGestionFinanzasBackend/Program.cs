using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using pryAutoGestionFinanzasBackend.Data;
using pryAutoGestionFinanzasBackend.Services;

var builder = WebApplication.CreateBuilder(args);

// --------------------
// Services (DI)
// --------------------

// Controllers + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DB: EF Core + SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// App Services
builder.Services.AddScoped<EstadisticasService>();

var app = builder.Build();

// --------------------
// Middleware
// --------------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// --------------------
// DB init + seed
// --------------------
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
    DbSeeder.Seed(db);
}

app.Run();