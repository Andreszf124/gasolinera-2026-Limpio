using System.Linq;
using System.Web.Mvc;
using Gasolinera.Infrastructure.DbContexts;

namespace Gasolinera.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class DashboardController : Controller
    {
        private readonly GasolineraContext _contexto;

        public DashboardController()
        {
            _contexto = new GasolineraContext();
        }

        public ActionResult Index()
        {
            ViewBag.TotalProductos = _contexto.Productos.Count();
            ViewBag.TotalClientes = _contexto.Clientes.Count();
            ViewBag.TotalVentas = _contexto.Ventas.Count();
            ViewBag.TotalFacturas = _contexto.Facturas.Count();

            var hoy = System.DateTime.Now.Date;
            ViewBag.VentasHoy = _contexto.Ventas.Where(v => v.Fecha >= hoy).Count();
            ViewBag.TotalVentasHoy = _contexto.Ventas.Where(v => v.Fecha >= hoy).Sum(v => (decimal?)v.Total) ?? 0;
            ViewBag.OrdenesPendientes = _contexto.OrdenesServicio
                .Where(o => o.Estado == Common.EstadoOrdenServicio.Pendiente).Count();

            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _contexto.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}