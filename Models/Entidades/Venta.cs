using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gasolinera.Models.Entidades
{
    public class Venta
    {
        [Key]
        public int IdVenta { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        [Required]
        public int IdCliente { get; set; }

        [Required]
        public int IdEmpleado { get; set; }

        [Required]
        [StringLength(50)]
        public string TipoVenta { get; set; }

        [Required]
        public decimal Subtotal { get; set; }

        public decimal Descuento { get; set; }

        public decimal Impuesto { get; set; }

        [Required]
        public decimal Total { get; set; }

        [Required]
        [StringLength(50)]
        public string MetodoPago { get; set; }

        [Required]
        [StringLength(50)]
        public string Estado { get; set; }

        public int? IdOrdenServicio { get; set; }

        [ForeignKey("IdCliente")]
        public virtual Cliente Cliente { get; set; }

        [ForeignKey("IdEmpleado")]
        public virtual Empleado Empleado { get; set; }

        [ForeignKey("IdOrdenServicio")]
        public virtual OrdenServicio OrdenServicio { get; set; }
    }
}