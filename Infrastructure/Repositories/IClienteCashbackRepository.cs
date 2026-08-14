using gasolinera_2026.Models.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gasolinera_2026.Infrastructure.Repositories
{
    public interface IClienteCashbackRepository
    {
        ClienteCashback ObtenerPorCliente(int idCliente);
        void Agregar(ClienteCashback clienteCashback);
        void Actualizar(ClienteCashback clienteCashback);
        void Guardar();
    }
}