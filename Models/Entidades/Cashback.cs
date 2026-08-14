using Gasolinera.Models.Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace gasolinera_2026.Models.Entidades
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