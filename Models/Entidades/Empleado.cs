using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace gasolinera_2026.Models.Entidades
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