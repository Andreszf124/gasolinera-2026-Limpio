using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gasolinera.Common
{
    public enum EstadoOrdenServicio
    {
        Pendiente = 1,
        EnProgreso = 2,
        Completado = 3,
        Entregado = 4,
        Cancelado = 5
    }
}