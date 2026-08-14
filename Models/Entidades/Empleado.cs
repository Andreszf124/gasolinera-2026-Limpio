using System.ComponentModel.DataAnnotations;

namespace Gasolinera.Models.Entidades
{
    public class Empleado
    {
        [Key]
        public int IdEmpleado { get; set; }

        [Required]
        [StringLength(100)]
        public string NombreCompleto { get; set; }

        [Required]
        [StringLength(100)]
        public string Correo { get; set; }

        [StringLength(20)]
        public string Telefono { get; set; }

        [StringLength(50)]
        public string Cargo { get; set; }
    }
}