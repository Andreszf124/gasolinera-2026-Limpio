using System.ComponentModel.DataAnnotations;
using Gasolinera.Common;

namespace Gasolinera.Models.Entidades
{
    public class Producto
    {
        [Key]
        public int IdProducto { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Nombre del producto")]
        public string Nombre { get; set; }

        [Required]
        [Display(Name = "Precio")]
        public decimal Precio { get; set; }

        [Required]
        [Display(Name = "Stock disponible")]
        public int Stock { get; set; }

        [Required]
        [Display(Name = "Categoría")]
        public CategoriaProducto Categoria { get; set; }
    }
}