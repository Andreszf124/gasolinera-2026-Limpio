using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Gasolinera.Models.Entidades
{
    public class Cliente
    {
        [Key]
        public int IdCliente { get; set; }

        [Required]
        [StringLength(100)]
        public string NombreCompleto { get; set; }

        [Required]
        [StringLength(100)]
        public string Correo { get; set; }

        [StringLength(20)]
        public string Telefono { get; set; }

        [StringLength(200)]
        public string Direccion { get; set; }
    }
}