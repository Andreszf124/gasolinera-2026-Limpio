using Gasolinera.Infrastructure.DbContexts;
using Gasolinera.Infrastructure.Repositories;
using gasolinera_2026.Models.Entidades;
using System.Data.Entity;
using System.Linq;


namespace gasolinera_2026.Infrastructure.Repositories
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
            _contexto.Entry(cashback).State = EntityState.Modified;
        }

        public void Guardar()
        {
            _contexto.SaveChanges();
        }
    }
}