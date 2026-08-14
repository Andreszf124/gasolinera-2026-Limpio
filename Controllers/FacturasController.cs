using System;
using System.Linq;
using System.Web.Mvc;
using Gasolinera.Infrastructure.DbContexts;
using Gasolinera.Models.Entidades;

namespace Gasolinera.Controllers
{
    [Authorize]
    public class FacturasController : Controller
    {
        private readonly GasolineraContext _contexto;

        public FacturasController()
        {
            _contexto = new GasolineraContext();
        }

        [HttpGet]
        public ActionResult Index()
        {
            var facturas = _contexto.Facturas
                .Include("Venta")
                .Include("Venta.Cliente")
                .ToList();

            return View(facturas);
        }

        [HttpGet]
        public ActionResult Detalles(int id)
        {
            var factura = _contexto.Facturas
                .Include("Venta")
                .Include("Venta.Cliente")
                .Include("Venta.Empleado")
                .FirstOrDefault(f => f.IdFactura == id);

            if (factura == null)
            {
                TempData["MensajeError"] = "La factura no existe.";
                return RedirectToAction("Index");
            }

            return View(factura);
        }

        [HttpGet]
        public ActionResult Crear()
        {
            var ventasSinFactura = _contexto.Ventas
                .Where(v => v.Estado == "Activa" && !_contexto.Facturas.Any(f => f.IdVenta == v.IdVenta))
                .ToList();

            ViewBag.Ventas = new SelectList(ventasSinFactura, "IdVenta", "IdVenta");
            return View(new Factura
            {
                FechaEmision = DateTime.Now,
                NumeroFactura = GenerarNumeroFactura()
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear(Factura factura)
        {
            if (!ModelState.IsValid)
            {
                var ventasSinFactura = _contexto.Ventas
                    .Where(v => v.Estado == "Activa" && !_contexto.Facturas.Any(f => f.IdVenta == v.IdVenta))
                    .ToList();

                ViewBag.Ventas = new SelectList(ventasSinFactura, "IdVenta", "IdVenta", factura.IdVenta);
                return View(factura);
            }

            var venta = _contexto.Ventas.Find(factura.IdVenta);

            if (venta == null)
            {
                TempData["MensajeError"] = "La venta seleccionada no existe.";
                return RedirectToAction("Index");
            }

            if (_contexto.Facturas.Any(f => f.IdVenta == factura.IdVenta))
            {
                TempData["MensajeError"] = "Esta venta ya tiene una factura asociada.";
                return RedirectToAction("Index");
            }

            factura.NumeroFactura = GenerarNumeroFactura();
            factura.FechaEmision = DateTime.Now;
            factura.Total = venta.Total;

            _contexto.Facturas.Add(factura);
            _contexto.SaveChanges();

            TempData["MensajeExito"] = $"Factura {factura.NumeroFactura} generada correctamente.";
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public ActionResult Eliminar(int id)
        {
            var factura = _contexto.Facturas
                .Include("Venta")
                .Include("Venta.Cliente")
                .FirstOrDefault(f => f.IdFactura == id);

            if (factura == null)
            {
                TempData["MensajeError"] = "La factura no existe.";
                return RedirectToAction("Index");
            }

            return View(factura);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Eliminar")]
        [Authorize(Roles = "Administrador")]
        public ActionResult EliminarConfirmado(int id)
        {
            var factura = _contexto.Facturas.Find(id);

            if (factura == null)
            {
                TempData["MensajeError"] = "La factura no existe.";
                return RedirectToAction("Index");
            }

            _contexto.Facturas.Remove(factura);
            _contexto.SaveChanges();

            TempData["MensajeExito"] = "Factura eliminada correctamente.";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public JsonResult ObtenerDatosVenta(int idVenta)
        {
            var venta = _contexto.Ventas
                .Include("Cliente")
                .FirstOrDefault(v => v.IdVenta == idVenta);

            if (venta == null)
            {
                return Json(new { success = false, mensaje = "Venta no encontrada." }, JsonRequestBehavior.AllowGet);
            }

            if (_contexto.Facturas.Any(f => f.IdVenta == idVenta))
            {
                return Json(new { success = false, mensaje = "Esta venta ya tiene una factura asociada." }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                success = true,
                total = venta.Total,
                cliente = venta.Cliente?.NombreCompleto ?? "N/A"
            }, JsonRequestBehavior.AllowGet);
        }

        private string GenerarNumeroFactura()
        {
            var anio = DateTime.Now.Year;
            var mes = DateTime.Now.Month.ToString("D2");
            var consecutivo = _contexto.Facturas.Count() + 1;

            return $"F-{anio}{mes}-{consecutivo:D4}";
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