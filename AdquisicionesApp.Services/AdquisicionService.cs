// AdquisicionesApp.Services/AdquisicionService.cs
using AdquisicionesApp.Data.Models;
using AdquisicionesApp.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdquisicionesApp.Services
{
    public interface IAdquisicionService
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

    public class AdquisicionService : IAdquisicionService
    {
        private readonly IAdquisicionRepository _repository;

        public AdquisicionService(IAdquisicionRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Adquisicion>> GetAllAsync(bool includeInactive = false)
        {
            return await _repository.GetAllAsync(includeInactive);
        }

        public async Task<Adquisicion> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Adquisicion>> FilterAsync(
            string unidad = null,
            string tipoBienServicio = null,
            string proveedor = null,
            DateTime? fechaDesde = null,
            DateTime? fechaHasta = null)
        {
            return await _repository.FilterAsync(unidad, tipoBienServicio, proveedor, fechaDesde, fechaHasta);
        }

        public async Task<int> CreateAsync(Adquisicion adquisicion)
        {
            // Validaciones adicionales si es necesario
            ValidateAdquisicion(adquisicion);

            return await _repository.CreateAsync(adquisicion);
        }

        public async Task UpdateAsync(Adquisicion adquisicion)
        {
            ValidateAdquisicion(adquisicion);
            await _repository.UpdateAsync(adquisicion);
        }

        public async Task DeactivateAsync(int id)
        {
            await _repository.DeactivateAsync(id);
        }

        public async Task<IEnumerable<HistorialAdquisicion>> GetHistorialAsync(int adquisicionId)
        {
            return await _repository.GetHistorialAsync(adquisicionId);
        }

        private void ValidateAdquisicion(Adquisicion adquisicion)
        {
            if (adquisicion == null)
                throw new ArgumentNullException(nameof(adquisicion));

            if (adquisicion.Presupuesto < 0)
                throw new ArgumentException("El presupuesto no puede ser negativo");

            if (adquisicion.Cantidad < 1)
                throw new ArgumentException("La cantidad debe ser mayor a cero");

            if (adquisicion.ValorUnitario < 0)
                throw new ArgumentException("El valor unitario no puede ser negativo");

            if (string.IsNullOrWhiteSpace(adquisicion.Unidad))
                throw new ArgumentException("La unidad administrativa es obligatoria");

            if (string.IsNullOrWhiteSpace(adquisicion.TipoBienServicio))
                throw new ArgumentException("El tipo de bien o servicio es obligatorio");

            if (string.IsNullOrWhiteSpace(adquisicion.Proveedor))
                throw new ArgumentException("El proveedor es obligatorio");
        }
    }
}