using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Gasolinera.Models.Entidades
{
    public class Categoria
    {
        [Key]
        public int CategoriaId { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [StringLength(500)]
        public string Descripcion { get; set; }

        public bool Activa { get; set; } = true;

        public virtual ICollection<Producto> Productos { get; set; }

        public Categoria()
        {
            Productos = new HashSet<Producto>();
        }
    }
}