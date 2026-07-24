using System.Collections.Generic;
using Gasolinera.Models.Entidades;

namespace Gasolinera.Infrastructure.Repositories
{
    public interface IVentaRepository
    {
        IEnumerable<Venta> ObtenerTodos();
        Venta ObtenerPorId(int id);
        void Agregar(Venta venta);
        void Actualizar(Venta venta);
        void Eliminar(Venta venta);
    }
}