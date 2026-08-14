using Gasolinera.Infrastructure.DbContexts;
using Gasolinera.Models.Entidades;
using gasolinera_2026.Infrastructure.Repositories;
using gasolinera_2026.Models.Entidades;
using System.Collections.Generic;
using System.Linq;

namespace Gasolinera.Infrastructure.Repositories
{
    public class MovimientoCashbackRepository : IMovimientoCashbackRepository
    {
        private readonly GasolineraContext _contexto;

        public MovimientoCashbackRepository(GasolineraContext contexto)
        {
            _contexto = contexto;
        }

        public IEnumerable<MovimientoCashback> ObtenerPorCliente(int idCliente)
        {
            return _contexto.MovimientosCashback
                .Where(m => m.IdCliente == idCliente)
                .OrderByDescending(m => m.FechaMovimiento)
                .ToList();
        }

        public void Agregar(MovimientoCashback movimiento)
        {
            _contexto.MovimientosCashback.Add(movimiento);
        }

        public void Guardar()
        {
            _contexto.SaveChanges();
        }
    }
}