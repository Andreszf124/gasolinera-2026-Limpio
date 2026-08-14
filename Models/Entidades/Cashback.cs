using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace  Gasolinera.Models.Entidades
{
    public class Cashback
    {
        [Key]
        public int IdCashback { get; set; }

        [Required]
        public int IdCliente { get; set; }

        [Required]
        public decimal PuntosAcumulados { get; set; }

        [Required]
        public decimal PuntosCanjeados { get; set; }

        [Required]
        public decimal PuntosDisponibles { get; set; }

        [Required]
        public DateTime FechaActualizacion { get; set; }

        [ForeignKey("IdCliente")]
        public virtual Cliente Cliente { get; set; }
    }
}