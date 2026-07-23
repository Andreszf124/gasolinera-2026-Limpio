using Gasolinera.Models.Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace gasolinera_2026.Models.Entidades
{
    public class Venta
    {
        [Key]
        public int IdVenta { get; set; }

        [Required]
        [Display(Name = "Fecha")]
        public DateTime Fecha { get; set; }

        [Required]
        [Display(Name = "Cliente")]
        public int IdCliente { get; set; }

        [Required]
        [Display(Name = "Empleado")]
        public int IdEmpleado { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Tipo de venta")]
        public string TipoVenta { get; set; }

        [Required]
        [Display(Name = "Subtotal")]
        public decimal Subtotal { get; set; }

        [Display(Name = "Descuento")]
        public decimal Descuento { get; set; }

        [Display(Name = "Impuesto")]
        public decimal Impuesto { get; set; }

        [Required]
        [Display(Name = "Total")]
        public decimal Total { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Método de pago")]
        public string MetodoPago { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Estado")]
        public string Estado { get; set; }

        [Display(Name = "Orden de servicio")]
        public int? IdOrdenServicio { get; set; }

        // Propiedades de navegación
        [ForeignKey("IdCliente")]
        public virtual Cliente Cliente { get; set; }

        [ForeignKey("IdEmpleado")]
        public virtual Empleado Empleado { get; set; }

        [ForeignKey("IdOrdenServicio")]
        public virtual OrdenServicio OrdenServicio { get; set; }
    }
}