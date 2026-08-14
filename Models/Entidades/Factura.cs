using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gasolinera.Models.Entidades
{
    public class Factura
    {
        [Key]
        public int IdFactura { get; set; }

        [Required]
        public int IdVenta { get; set; }

        [Required]
        public DateTime FechaEmision { get; set; }

        [Required]
        [StringLength(50)]
        public string NumeroFactura { get; set; }

        [Required]
        public decimal Total { get; set; }

        [Required]
        [StringLength(20)]
        public string Estado { get; set; } = "Pendiente";

        [StringLength(500)]
        public string Observaciones { get; set; }

        public DateTime? FechaAprobacion { get; set; }
        public string AprobadoPorId { get; set; }

        [ForeignKey("IdVenta")]
        public virtual Venta Venta { get; set; }
    }
}