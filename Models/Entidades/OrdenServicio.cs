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
        [StringLength(20)]
        [Display(Name = "Placa del Vehículo")]
        public string PlacaVehiculo { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Marca")]
        public string MarcaModelo { get; set; }

        [Required]
        [Display(Name = "Año del Vehículo")]
        public int Anio { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Tipo de Vehículo")]
        public string TipoVehiculo { get; set; }

        [Required]
        [StringLength(500)]
        [Display(Name = "Diagnóstico del Vehículo")]
        public string Diagnostico { get; set; }

        [StringLength(1000)]
        [Display(Name = "Trabajos Realizados")]
        public string TrabajosRealizados { get; set; }

        [Display(Name = "Costo de Mano de Obra")]
        public decimal CostoManoObra { get; set; }

        [StringLength(500)]
        [Display(Name = "Lista de Repuestos Utilizados")]
        public string ListaRepuestosUtilizados { get; set; }

        [Required]
        [Display(Name = "Fecha de Entrada")]
        public DateTime FechaEntrada { get; set; }

        [Display(Name = "Fecha de Finalización")]
        public DateTime? FechaFinalizacion { get; set; }

        [Required]
        [Display(Name = "Estado")]
        public EstadoOrdenServicio Estado { get; set; }

        [Required]
        [Display(Name = "Cliente")]
        public string NombreCliente { get; set; }

        [Display(Name = "Mecánico Asignado")]
        public int? IdEmpleado { get; set; }

        [ForeignKey("IdEmpleado")]
        public virtual Empleado Empleado { get; set; }
    }
}