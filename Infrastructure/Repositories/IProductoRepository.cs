using Gasolinera.Models.Entidades;
using gasolinera_2026.Models.Entidades;
using System.Collections.Generic;

namespace Gasolinera.Infrastructure.Repositories
{
    public interface IProductoRepository
    {
        IEnumerable<Producto> ObtenerTodos();
        Producto ObtenerPorId(int id);
        void Agregar(Producto producto);
        void Actualizar(Producto producto);
        void Eliminar(Producto producto);
    }
}