using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Gasolinera.Infrastructure.DbContexts;
using Gasolinera.Models.Entidades;

namespace Gasolinera.Infrastructure.Repositories
{
    public class VentaRepository : IVentaRepository
    {
        private readonly GasolineraContext _contexto;

        public VentaRepository(GasolineraContext contexto)
        {
            _contexto = contexto;
        }

        public IEnumerable<Venta> ObtenerTodos()
        {
            return _contexto.Ventas.ToList();
        }

        public Venta ObtenerPorId(int id)
        {
            return _contexto.Ventas.Find(id);
        }

        public void Agregar(Venta venta)
        {
            _contexto.Ventas.Add(venta);
            _contexto.SaveChanges();
        }

        public void Actualizar(Venta venta)
        {
            _contexto.Entry(venta).State = EntityState.Modified;
            _contexto.SaveChanges();
        }

        public void Eliminar(Venta venta)
        {
            _contexto.Ventas.Remove(venta);
            _contexto.SaveChanges();
        }
    }
}