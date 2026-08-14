using System.ComponentModel.DataAnnotations;

namespace Gasolinera.Models
{
    public class UsuarioViewModel
    {
        public string Id { get; set; }

        [Display(Name = "Nombre completo")]
        public string NombreCompleto { get; set; }

        [Display(Name = "Correo electrónico")]
        public string Correo { get; set; }

        [Display(Name = "Rol")]
        public string Rol { get; set; }
    }
}
