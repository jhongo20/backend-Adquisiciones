// AdquisicionesApp.Data/Repositories/AdquisicionRepository.cs
using AdquisicionesApp.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdquisicionesApp.Data.Repositories
{
    public interface IAdquisicionRepository
    {
        Task<IEnumerable<Adquisicion>> GetAllAsync(bool includeInactive = false);
        Task<Adquisicion> GetByIdAsync(int id);
        Task<IEnumerable<Adquisicion>> FilterAsync(
            string unidad = null,
            string tipoBienServicio = null,
            string proveedor = null,
            DateTime? fechaDesde = null,
            DateTime? fechaHasta = null);
        Task<int> CreateAsync(Adquisicion adquisicion);
        Task UpdateAsync(Adquisicion adquisicion);
        Task DeactivateAsync(int id);
        Task<IEnumerable<HistorialAdquisicion>> GetHistorialAsync(int adquisicionId);
    }

    public class AdquisicionRepository : IAdquisicionRepository
    {
        private readonly AdquisicionesDbContext _context;

        public AdquisicionRepository(AdquisicionesDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Adquisicion>> GetAllAsync(bool includeInactive = false)
        {
            return await _context.Adquisiciones
                .Where(a => includeInactive || a.Activo)
                .ToListAsync();
        }

        public async Task<Adquisicion> GetByIdAsync(int id)
        {
            return await _context.Adquisiciones
                .Include(a => a.Historial)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<Adquisicion>> FilterAsync(
            string unidad = null,
            string tipoBienServicio = null,
            string proveedor = null,
            DateTime? fechaDesde = null,
            DateTime? fechaHasta = null)
        {
            var query = _context.Adquisiciones.AsQueryable();

            if (!string.IsNullOrEmpty(unidad))
                query = query.Where(a => a.Unidad.Contains(unidad));

            if (!string.IsNullOrEmpty(tipoBienServicio))
                query = query.Where(a => a.TipoBienServicio.Contains(tipoBienServicio));

            if (!string.IsNullOrEmpty(proveedor))
                query = query.Where(a => a.Proveedor.Contains(proveedor));

            if (fechaDesde.HasValue)
                query = query.Where(a => a.FechaAdquisicion >= fechaDesde.Value);

            if (fechaHasta.HasValue)
                query = query.Where(a => a.FechaAdquisicion <= fechaHasta.Value);

            return await query.ToListAsync();
        }

        public async Task<int> CreateAsync(Adquisicion adquisicion)
        {
            _context.Adquisiciones.Add(adquisicion);
            await _context.SaveChangesAsync();
            return adquisicion.Id;
        }

        public async Task UpdateAsync(Adquisicion adquisicion)
        {
            var existingAdquisicion = await _context.Adquisiciones.FindAsync(adquisicion.Id);
            if (existingAdquisicion == null)
                throw new KeyNotFoundException($"Adquisición con ID {adquisicion.Id} no encontrada");

            // Guardar cambios en el historial
            TrackChanges(existingAdquisicion, adquisicion);

            // Actualizar propiedades
            _context.Entry(existingAdquisicion).CurrentValues.SetValues(adquisicion);
            existingAdquisicion.FechaModificacion = DateTime.Now;

            await _context.SaveChangesAsync();
        }

        public async Task DeactivateAsync(int id)
        {
            var adquisicion = await _context.Adquisiciones.FindAsync(id);
            if (adquisicion == null)
                throw new KeyNotFoundException($"Adquisición con ID {id} no encontrada");

            adquisicion.Activo = false;
            adquisicion.FechaModificacion = DateTime.Now;

            // Registrar en historial
            _context.HistorialAdquisiciones.Add(new HistorialAdquisicion
            {
                AdquisicionId = id,
                CampoModificado = "Activo",
                ValorAnterior = "True",
                ValorNuevo = "False",
                FechaModificacion = DateTime.Now
            });

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<HistorialAdquisicion>> GetHistorialAsync(int adquisicionId)
        {
            return await _context.HistorialAdquisiciones
                .Where(h => h.AdquisicionId == adquisicionId)
                .OrderByDescending(h => h.FechaModificacion)
                .ToListAsync();
        }

        private void TrackChanges(Adquisicion existingEntity, Adquisicion newEntity)
        {
            // Comparar propiedades y crear registros de historial para cada cambio
            if (existingEntity.Presupuesto != newEntity.Presupuesto)
            {
                _context.HistorialAdquisiciones.Add(new HistorialAdquisicion
                {
                    AdquisicionId = existingEntity.Id,
                    CampoModificado = "Presupuesto",
                    ValorAnterior = existingEntity.Presupuesto.ToString(),
                    ValorNuevo = newEntity.Presupuesto.ToString(),
                    FechaModificacion = DateTime.Now
                });
            }

            if (existingEntity.Unidad != newEntity.Unidad)
            {
                _context.HistorialAdquisiciones.Add(new HistorialAdquisicion
                {
                    AdquisicionId = existingEntity.Id,
                    CampoModificado = "Unidad",
                    ValorAnterior = existingEntity.Unidad,
                    ValorNuevo = newEntity.Unidad,
                    FechaModificacion = DateTime.Now
                });
            }

            // Continuar con las demás propiedades...
            if (existingEntity.TipoBienServicio != newEntity.TipoBienServicio)
            {
                _context.HistorialAdquisiciones.Add(new HistorialAdquisicion
                {
                    AdquisicionId = existingEntity.Id,
                    CampoModificado = "TipoBienServicio",
                    ValorAnterior = existingEntity.TipoBienServicio,
                    ValorNuevo = newEntity.TipoBienServicio,
                    FechaModificacion = DateTime.Now
                });
            }

            if (existingEntity.Cantidad != newEntity.Cantidad)
            {
                _context.HistorialAdquisiciones.Add(new HistorialAdquisicion
                {
                    AdquisicionId = existingEntity.Id,
                    CampoModificado = "Cantidad",
                    ValorAnterior = existingEntity.Cantidad.ToString(),
                    ValorNuevo = newEntity.Cantidad.ToString(),
                    FechaModificacion = DateTime.Now
                });
            }

            if (existingEntity.ValorUnitario != newEntity.ValorUnitario)
            {
                _context.HistorialAdquisiciones.Add(new HistorialAdquisicion
                {
                    AdquisicionId = existingEntity.Id,
                    CampoModificado = "ValorUnitario",
                    ValorAnterior = existingEntity.ValorUnitario.ToString(),
                    ValorNuevo = newEntity.ValorUnitario.ToString(),
                    FechaModificacion = DateTime.Now
                });
            }

            if (existingEntity.FechaAdquisicion != newEntity.FechaAdquisicion)
            {
                _context.HistorialAdquisiciones.Add(new HistorialAdquisicion
                {
                    AdquisicionId = existingEntity.Id,
                    CampoModificado = "FechaAdquisicion",
                    ValorAnterior = existingEntity.FechaAdquisicion.ToString("yyyy-MM-dd"),
                    ValorNuevo = newEntity.FechaAdquisicion.ToString("yyyy-MM-dd"),
                    FechaModificacion = DateTime.Now
                });
            }

            if (existingEntity.Proveedor != newEntity.Proveedor)
            {
                _context.HistorialAdquisiciones.Add(new HistorialAdquisicion
                {
                    AdquisicionId = existingEntity.Id,
                    CampoModificado = "Proveedor",
                    ValorAnterior = existingEntity.Proveedor,
                    ValorNuevo = newEntity.Proveedor,
                    FechaModificacion = DateTime.Now
                });
            }

            if (existingEntity.Documentacion != newEntity.Documentacion)
            {
                _context.HistorialAdquisiciones.Add(new HistorialAdquisicion
                {
                    AdquisicionId = existingEntity.Id,
                    CampoModificado = "Documentacion",
                    ValorAnterior = existingEntity.Documentacion,
                    ValorNuevo = newEntity.Documentacion,
                    FechaModificacion = DateTime.Now
                });
            }
        }
    }
}