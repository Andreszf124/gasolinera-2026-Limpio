using System.Linq;
using System.Web.Mvc;
using Gasolinera.Infrastructure.DbContexts;
using Gasolinera.Models;
using Microsoft.AspNet.Identity;

namespace Gasolinera.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class AuditoriaController : Controller
    {
        private readonly GasolineraContext _contexto;
        private readonly ApplicationDbContext _identityContext;

        public AuditoriaController()
        {
            _contexto = new GasolineraContext();
            _identityContext = new ApplicationDbContext();
        }

        public ActionResult Index()
        {
         
            var movimientos = _contexto.MovimientosCashback
                .Include("Cliente")
                .Include("Venta")
                .OrderByDescending(m => m.FechaMovimiento)
                .Take(50)
                .ToList();

            ViewBag.Movimientos = movimientos;

     
            ViewBag.TotalOrdenes = _contexto.OrdenesServicio.Count();
            ViewBag.TotalVentas = _contexto.Ventas.Count();
            ViewBag.TotalFacturas = _contexto.Facturas.Count();
            ViewBag.TotalClientes = _contexto.Clientes.Count();
            ViewBag.TotalProductos = _contexto.Productos.Count();
            ViewBag.TotalUsuarios = _identityContext.Users.Count();

            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _contexto.Dispose();
                _identityContext.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}