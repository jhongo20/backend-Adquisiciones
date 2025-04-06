// AdquisicionesApp.Data/AdquisicionesDbContext.cs
using AdquisicionesApp.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AdquisicionesApp.Data
{
    public class AdquisicionesDbContext : DbContext
    {
        public AdquisicionesDbContext(DbContextOptions<AdquisicionesDbContext> options) : base(options)
        {
        }

        public DbSet<Adquisicion> Adquisiciones { get; set; }
        public DbSet<HistorialAdquisicion> HistorialAdquisiciones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuraciones adicionales
            modelBuilder.Entity<Adquisicion>()
                .HasMany(a => a.Historial)
                .WithOne(h => h.Adquisicion)
                .HasForeignKey(h => h.AdquisicionId)
                .OnDelete(DeleteBehavior.Cascade);

            // Opcional: Datos de ejemplo para testing
            modelBuilder.Entity<Adquisicion>().HasData(
                new Adquisicion
                {
                    Id = 1,
                    Presupuesto = 10000000000,
                    Unidad = "Dirección de Medicamentos y Tecnologías en Salud",
                    TipoBienServicio = "Medicamentos",
                    Cantidad = 10000,
                    ValorUnitario = 1000,
                    FechaAdquisicion = new DateTime(2023, 7, 20),
                    Proveedor = "Laboratorios Bayer S.A.",
                    Documentacion = "Orden de compra No. 2023-07-20-001, factura No. 2023-07-20-001",
                    Activo = true,
                    FechaCreacion = DateTime.Now
                }
            );
        }
    }
}