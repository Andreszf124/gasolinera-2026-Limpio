using Gasolinera.Infrastructure.DbContexts;
using gasolinera_2026.Models.Entidades;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace gasolinera_2026.Infrastructure.Repositories
{
    public class ClienteCashbackRepository : IClienteCashbackRepository
    {
        private readonly GasolineraContext _contexto;

        public ClienteCashbackRepository(GasolineraContext contexto)
        {
            _contexto = contexto;
        }

        public ClienteCashback ObtenerPorCliente(int idCliente)
        {
            return _contexto.ClientesCashback
                .FirstOrDefault(c => c.IdCliente == idCliente);
        }

        public void Agregar(ClienteCashback clienteCashback)
        {
            _contexto.ClientesCashback.Add(clienteCashback);
        }

        public void Actualizar(ClienteCashback clienteCashback)
        {
            _contexto.Entry(clienteCashback).State = EntityState.Modified;
        }

        public void Guardar()
        {
            _contexto.SaveChanges();
        }
    }
}