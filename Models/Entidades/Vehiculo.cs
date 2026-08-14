using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gasolinera.Models.Entidades
{
    public class Vehiculo
    {
        [Key]
        public int IdVehiculo { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Placa")]
        public string Placa { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Marca")]
        public string Marca { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Modelo")]
        public string Modelo { get; set; }

        [Required]
        [Display(Name = "Año")]
        public int Anio { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Tipo de Vehículo")]
        public string TipoVehiculo { get; set; } // Carro, Moto, Camión

        [StringLength(20)]
        [Display(Name = "Color")]
        public string Color { get; set; }

        [Required]
        [Display(Name = "Cliente")]
        public int IdCliente { get; set; }

        public DateTime FechaRegistro { get; set; }

        [ForeignKey("IdCliente")]
        public virtual Cliente Cliente { get; set; }
    }
}