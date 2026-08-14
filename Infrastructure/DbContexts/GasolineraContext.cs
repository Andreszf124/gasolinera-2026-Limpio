using Gasolinera.Models.Entidades;
using gasolinera_2026.Models.Entidades;
using System.Data.Entity;

namespace Gasolinera.Infrastructure.DbContexts
{
    public class GasolineraContext : DbContext
    {
        public GasolineraContext()
            : base("name=GasolineraConexion")
        {
        }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Empleado> Empleados { get; set; }
        public DbSet<OrdenServicio> OrdenesServicio { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Venta> Ventas { get; set; }
        public DbSet<Factura> Facturas { get; set; }
        public DbSet<Cashback> Cashbacks { get; set; }
        public DbSet<MovimientoCashback> MovimientosCashback { get; set; }
    }
}