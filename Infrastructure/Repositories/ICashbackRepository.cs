using gasolinera_2026.Models.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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