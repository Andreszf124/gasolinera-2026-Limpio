using gasolinera_2026.Models.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using Gasolinera.Models.Entidades;

namespace gasolinera_2026.Infrastructure.Repositories
{
    public interface IMovimientoCashbackRepository
    {
        IEnumerable<MovimientoCashback> ObtenerPorCliente(int idCliente);
        void Agregar(MovimientoCashback movimiento);
        void Guardar();
    }
}