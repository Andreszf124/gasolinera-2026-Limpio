using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Gasolinera.Common;

namespace Gasolinera.Models.Entidades
{
    public class OrdenServicio
    {
        [Key]
        public int IdOrdenServicio { get; set; }

        [Required]
        [Display(Name = "Vehículo")]
        public int IdVehiculo { get; set; }

        [Display(Name = "Cliente")]
        public string NombreCliente { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Tipo de Servicio")]
        public string TipoServicio { get; set; } 

        [Required]
        [StringLength(1000)]
        [Display(Name = "Descripción del Servicio")]
        public string DescripcionServicio { get; set; }

        [Display(Name = "Fecha de Entrada")]
        public DateTime FechaEntrada { get; set; }

        [Display(Name = "Fecha de Finalización")]
        public DateTime? FechaFinalizacion { get; set; }

        [Required]
        [Display(Name = "Estado")]
        public EstadoOrdenServicio Estado { get; set; }

        [Display(Name = "Mecánico Asignado")]
        public int? IdEmpleado { get; set; }

        [Display(Name = "Costo Estimado")]
        public decimal? CostoEstimado { get; set; }

        [Display(Name = "Observaciones del Mecánico")]
        [StringLength(500)]
        public string ObservacionesMecanico { get; set; }

        [ForeignKey("IdVehiculo")]
        public virtual Vehiculo Vehiculo { get; set; }

        [ForeignKey("IdEmpleado")]
        public virtual Empleado Empleado { get; set; }
    }
}