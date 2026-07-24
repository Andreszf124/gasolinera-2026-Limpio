using System.Collections.Generic;
using Gasolinera.Models.Entidades;

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