using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace gasolinera_2026.Models.Entidades
{
    public class Factura
    {
        [Key]
        public int IdFactura { get; set; }

        [Required]
        [Display(Name = "Venta")]
        public int IdVenta { get; set; }

        [Required]
        [Display(Name = "Fecha de emisión")]
        public DateTime FechaEmision { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Número de factura")]
        public string NumeroFactura { get; set; }

        [Required]
        [Display(Name = "Total")]
        public decimal Total { get; set; }

        // Propiedades de navegación
        [ForeignKey("IdVenta")]
        public virtual Venta Venta { get; set; }
    }
}