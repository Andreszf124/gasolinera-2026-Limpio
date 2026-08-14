using System.Collections.Generic;
using Gasolinera.Models.Entidades;

namespace Gasolinera.Infrastructure.Repositories
{
    public interface IMovimientoCashbackRepository
    {
        IEnumerable<MovimientoCashback> ObtenerPorCliente(int idCliente);
        void Agregar(MovimientoCashback movimiento);
        void Guardar();
    }
}