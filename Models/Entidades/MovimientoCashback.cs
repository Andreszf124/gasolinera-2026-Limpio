using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Gasolinera.Common;

namespace Gasolinera.Models.Entidades
{
    public class MovimientoCashback
    {
        [Key]
        public int IdMovimientoCashback { get; set; }

        [Required]
        public int IdCliente { get; set; }

        public int? IdVenta { get; set; }

        [Required]
        public decimal Monto { get; set; }

        [Required]
        public decimal PuntosGenerados { get; set; }

        [Required]
        public TipoMovimientoCashback TipoMovimiento { get; set; }

        [Required]
        public DateTime FechaMovimiento { get; set; }

        public string UsuarioResponsableId { get; set; }

        [StringLength(500)]
        public string Observaciones { get; set; }

        [ForeignKey("IdCliente")]
        public virtual Cliente Cliente { get; set; }

        [ForeignKey("IdVenta")]
        public virtual Venta Venta { get; set; }
    }
}