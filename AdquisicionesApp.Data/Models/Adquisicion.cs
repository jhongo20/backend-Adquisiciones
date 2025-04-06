using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AdquisicionesApp.Data.Models
{
    public class Adquisicion
    {
        public int Id { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "El presupuesto debe ser un valor positivo")]
        public decimal Presupuesto { get; set; }

        [Required]
        [StringLength(100)]
        public string Unidad { get; set; }

        [Required]
        [StringLength(100)]
        public string TipoBienServicio { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a cero")]
        public int Cantidad { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "El valor unitario debe ser un valor positivo")]
        public decimal ValorUnitario { get; set; }

        public decimal ValorTotal => Cantidad * ValorUnitario;

        [Required]
        public DateTime FechaAdquisicion { get; set; }

        [Required]
        [StringLength(100)]
        public string Proveedor { get; set; }

        [StringLength(500)]
        public string Documentacion { get; set; }

        public bool Activo { get; set; } = true;

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        public DateTime? FechaModificacion { get; set; }

        // Relación con el historial
        public virtual ICollection<HistorialAdquisicion> Historial { get; set; } = new List<HistorialAdquisicion>();
    }

    public class HistorialAdquisicion
    {
        public int Id { get; set; }

        public int AdquisicionId { get; set; }
        public virtual Adquisicion Adquisicion { get; set; }

        [Required]
        public string CampoModificado { get; set; }

        public string ValorAnterior { get; set; }

        public string ValorNuevo { get; set; }

        [Required]
        public DateTime FechaModificacion { get; set; } = DateTime.Now;

        [Required]
        public string UsuarioModificacion { get; set; } = "Sistema"; // Por defecto, puede cambiarse si implementas autenticación
    }
}