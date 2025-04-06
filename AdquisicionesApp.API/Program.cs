// AdquisicionesApp.API/Program.cs
using AdquisicionesApp.Data;
using AdquisicionesApp.Data.Repositories;
using AdquisicionesApp.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.Services.AddControllers();
builder.Services.AddControllers().AddJsonOptions(options =>
{
    //options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
    // O puedes usar esta opción más simple pero menos eficiente:
     options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});

// Configurar DbContext con SQLite (puedes cambiar a SQL Server o PostgreSQL según necesites)
builder.Services.AddDbContext<AdquisicionesDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configurar servicios de la aplicación
builder.Services.AddScoped<IAdquisicionRepository, AdquisicionRepository>();
builder.Services.AddScoped<IAdquisicionService, AdquisicionService>();

// Configurar CORS para permitir solicitudes desde la aplicación Angular
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        builder => builder
            .WithOrigins("http://localhost:4200") // URL de la aplicación Angular
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

// Learn more about configuring Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    // Crear y aplicar migraciones automáticamente en desarrollo
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<AdquisicionesDbContext>();
        dbContext.Database.EnsureCreated();
    }
}

app.UseCors("AllowAngularApp");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();