using Gasolinera.Common;
using Gasolinera.Common;
using Gasolinera.Common;
using Gasolinera.Models.Entidades;
using gasolinera_2026.Models.Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Gasolinera.Models.Entidades
{
    public class OrdenServicio
    {
        [Key]
        public int IdOrdenServicio { get; set; }

        [Required]
        [StringLength(20)]
        public string PlacaVehiculo { get; set; }

        [Required]
        [StringLength(50)]
        public string MarcaModelo { get; set; }

        [Required]
        public int Anio { get; set; }

        [Required]
        [StringLength(500)]
        public string Diagnostico { get; set; }

        [StringLength(1000)]
        public string TrabajosRealizados { get; set; }

        public decimal CostoManoObra { get; set; }

        [StringLength(500)]
        public string RepuestosUtilizadosResumen { get; set; }

        [Required]
        public DateTime FechaEntrada { get; set; }

        public DateTime? FechaFinalizacion { get; set; }

        [Required]
        public EstadoOrdenServicio Estado { get; set; }

        [Required]
        public int IdCliente { get; set; }

        public int? IdEmpleado { get; set; }

        public virtual Cliente Cliente { get; set; }
        public virtual Empleado Empleado { get; set; }
    }
}