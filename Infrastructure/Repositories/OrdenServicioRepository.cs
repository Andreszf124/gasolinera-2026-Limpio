using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Gasolinera.Infrastructure.DbContexts;
using Gasolinera.Models.Entidades;

namespace Gasolinera.Infrastructure.Repositories
{
    public class OrdenServicioRepository : IOrdenServicioRepository
    {
        private readonly GasolineraContext _contexto;

        public OrdenServicioRepository(GasolineraContext contexto)
        {
            _contexto = contexto;
        }

        public IEnumerable<OrdenServicio> ObtenerTodos()
        {
            return _contexto.OrdenesServicio.ToList();
        }

        public OrdenServicio ObtenerPorId(int id)
        {
            return _contexto.OrdenesServicio.Find(id);
        }

        public void Agregar(OrdenServicio ordenServicio)
        {
            _contexto.OrdenesServicio.Add(ordenServicio);
            _contexto.SaveChanges();
        }

        public void Actualizar(OrdenServicio ordenServicio)
        {
            _contexto.Entry(ordenServicio).State = EntityState.Modified;
            _contexto.SaveChanges();
        }

        public void Eliminar(OrdenServicio ordenServicio)
        {
            _contexto.OrdenesServicio.Remove(ordenServicio);
            _contexto.SaveChanges();
        }
    }
}