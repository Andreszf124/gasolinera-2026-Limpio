using Gasolinera.Models.Entidades;

namespace Gasolinera.Infrastructure.Repositories
{
    public interface ICashbackRepository
    {
        Cashback ObtenerPorCliente(int idCliente);
        void Agregar(Cashback cashback);
        void Actualizar(Cashback cashback);
        void Guardar();
    }
}