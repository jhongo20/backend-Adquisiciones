// AdquisicionesApp.API/Controllers/AdquisicionesController.cs
using AdquisicionesApp.Data.Models;
using AdquisicionesApp.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdquisicionesApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowAngularApp")]
    public class AdquisicionesController : ControllerBase
    {
        private readonly IAdquisicionService _adquisicionService;

        public AdquisicionesController(IAdquisicionService adquisicionService)
        {
            _adquisicionService = adquisicionService;
        }

        // GET: api/Adquisiciones
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Adquisicion>>> GetAdquisiciones([FromQuery] bool includeInactive = false)
        {
            try
            {
                var adquisiciones = await _adquisicionService.GetAllAsync(includeInactive);
                return Ok(adquisiciones);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        // GET: api/Adquisiciones/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Adquisicion>> GetAdquisicion(int id)
        {
            try
            {
                var adquisicion = await _adquisicionService.GetByIdAsync(id);

                if (adquisicion == null)
                {
                    return NotFound($"Adquisición con ID {id} no encontrada");
                }

                return Ok(adquisicion);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        // GET: api/Adquisiciones/filtrar
        [HttpGet("filtrar")]
        public async Task<ActionResult<IEnumerable<Adquisicion>>> FilterAdquisiciones(
            [FromQuery] string unidad = null,
            [FromQuery] string tipoBienServicio = null,
            [FromQuery] string proveedor = null,
            [FromQuery] DateTime? fechaDesde = null,
            [FromQuery] DateTime? fechaHasta = null)
        {
            try
            {
                var adquisiciones = await _adquisicionService.FilterAsync(
                    unidad, tipoBienServicio, proveedor, fechaDesde, fechaHasta);

                return Ok(adquisiciones);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        // POST: api/Adquisiciones
        [HttpPost]
        public async Task<ActionResult<Adquisicion>> CreateAdquisicion([FromBody] Adquisicion adquisicion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var id = await _adquisicionService.CreateAsync(adquisicion);
                return CreatedAtAction(nameof(GetAdquisicion), new { id }, adquisicion);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        // PUT: api/Adquisiciones/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAdquisicion(int id, [FromBody] Adquisicion adquisicion)
        {
            if (id != adquisicion.Id)
            {
                return BadRequest("El ID en la URL no coincide con el ID en el cuerpo de la solicitud");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _adquisicionService.UpdateAsync(adquisicion);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        // DELETE: api/Adquisiciones/5/desactivar
        [HttpPut("{id}/desactivar")]
        public async Task<IActionResult> DeactivateAdquisicion(int id)
        {
            try
            {
                await _adquisicionService.DeactivateAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        // GET: api/Adquisiciones/5/historial
        [HttpGet("{id}/historial")]
        public async Task<ActionResult<IEnumerable<HistorialAdquisicion>>> GetHistorial(int id)
        {
            try
            {
                var historial = await _adquisicionService.GetHistorialAsync(id);
                return Ok(historial);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }
    }
}