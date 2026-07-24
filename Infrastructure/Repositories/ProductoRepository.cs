using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Gasolinera.Infrastructure.DbContexts;
using Gasolinera.Models.Entidades;

namespace Gasolinera.Infrastructure.Repositories
{
    public class ProductoRepository : IProductoRepository
    {
        private readonly GasolineraContext _contexto;

        public ProductoRepository(GasolineraContext contexto)
        {
            _contexto = contexto;
        }

        public IEnumerable<Producto> ObtenerTodos()
        {
            return _contexto.Productos.ToList();
        }

        public Producto ObtenerPorId(int id)
        {
            return _contexto.Productos.Find(id);
        }

        public void Agregar(Producto producto)
        {
            _contexto.Productos.Add(producto);
            _contexto.SaveChanges();
        }

        public void Actualizar(Producto producto)
        {
            _contexto.Entry(producto).State = EntityState.Modified;
            _contexto.SaveChanges();
        }

        public void Eliminar(Producto producto)
        {
            _contexto.Productos.Remove(producto);
            _contexto.SaveChanges();
        }
    }
}