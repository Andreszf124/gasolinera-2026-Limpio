using System.Linq;
using Gasolinera.Infrastructure.DbContexts;
using Gasolinera.Models.Entidades;

namespace Gasolinera.Infrastructure.Repositories
{
    public class CashbackRepository : ICashbackRepository
    {
        private readonly GasolineraContext _contexto;

        public CashbackRepository(GasolineraContext contexto)
        {
            _contexto = contexto;
        }

        public Cashback ObtenerPorCliente(int idCliente)
        {
            return _contexto.Cashbacks
                .FirstOrDefault(c => c.IdCliente == idCliente);
        }

        public void Agregar(Cashback cashback)
        {
            _contexto.Cashbacks.Add(cashback);
        }

        public void Actualizar(Cashback cashback)
        {
            _contexto.Entry(cashback).State = System.Data.Entity.EntityState.Modified;
        }

        public void Guardar()
        {
            _contexto.SaveChanges();
        }
    }
}