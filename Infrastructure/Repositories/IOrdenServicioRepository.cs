using System.Collections.Generic;
using Gasolinera.Models.Entidades;

namespace Gasolinera.Infrastructure.Repositories
{
    public interface IOrdenServicioRepository
    {
        IEnumerable<OrdenServicio> ObtenerTodos();
        OrdenServicio ObtenerPorId(int id);
        void Agregar(OrdenServicio ordenServicio);
        void Actualizar(OrdenServicio ordenServicio);
        void Eliminar(OrdenServicio ordenServicio);
    }
}