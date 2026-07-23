using Gasolinera.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace gasolinera_2026.Models.Entidades
{
    public class Producto
    {
        [Key]
        public int IdProducto { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(
            100,
            MinimumLength = 3,
            ErrorMessage = "El nombre debe tener entre 3 y 100 caracteres")]
        [Display(Name = "Nombre del producto")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El precio es obligatorio")]
        [Range(
            0.01,
            999999.99,
            ErrorMessage = "El precio debe ser mayor a cero")]
        [Display(Name = "Precio")]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "El stock es obligatorio")]
        [Range(
            0,
            99999,
            ErrorMessage = "El stock no puede ser negativo")]
        [Display(Name = "Stock disponible")]
        public int Stock { get; set; }

        [Required(ErrorMessage = "La categoría es obligatoria")]
        [Display(Name = "Categoría")]
        public CategoriaProducto Categoria { get; set; }
    }
}