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

        [ForeignKey("IdVenta")]
        public virtual Venta Venta { get; set; }
    }
}