using System;
using System.Linq;
using System.Web.Mvc;
using Gasolinera.Infrastructure.DbContexts;
using Gasolinera.Models.Entidades;
using Microsoft.AspNet.Identity;

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

        // ==========================================
        // LISTA DE FACTURAS
        // ==========================================
        [HttpGet]
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();

            if (User.IsInRole("Administrador") || User.IsInRole("Moderador"))
            {
                var facturas = _contexto.Facturas
                    .Include("Venta")
                    .Include("Venta.Cliente")
                    .OrderByDescending(f => f.FechaEmision)
                    .ToList();

                return View(facturas);
            }
            else
            {
                var cliente = _contexto.Clientes.FirstOrDefault(c => c.Correo == User.Identity.Name);
                var idCliente = cliente != null ? cliente.IdCliente : 0;

                var facturas = _contexto.Facturas
                    .Include("Venta")
                    .Include("Venta.Cliente")
                    .Where(f => f.Venta.IdCliente == idCliente)
                    .OrderByDescending(f => f.FechaEmision)
                    .ToList();

                return View(facturas);
            }
        }

        // ==========================================
        // DETALLES DE FACTURA
        // ==========================================
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

        // ==========================================
        // CREAR FACTURA
        // ==========================================
        [HttpGet]
        [Authorize(Roles = "Administrador, Moderador")]
        public ActionResult Crear()
        {
            ViewBag.Productos = _contexto.Productos.Where(p => p.Stock > 0).ToList();
            ViewBag.Clientes = new SelectList(_contexto.Clientes, "IdCliente", "NombreCompleto");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador, Moderador")]
        public ActionResult Crear(int idCliente, int[] productoIds, int[] cantidades)
        {
            if (productoIds == null || productoIds.Length == 0)
            {
                TempData["MensajeError"] = "Debe seleccionar al menos un producto.";
                ViewBag.Productos = _contexto.Productos.Where(p => p.Stock > 0).ToList();
                ViewBag.Clientes = new SelectList(_contexto.Clientes, "IdCliente", "NombreCompleto");
                return View();
            }

            var cliente = _contexto.Clientes.Find(idCliente);

            if (cliente == null)
            {
                TempData["MensajeError"] = "Cliente no encontrado.";
                return RedirectToAction("Crear");
            }

            decimal total = 0;
            for (int i = 0; i < productoIds.Length; i++)
            {
                var producto = _contexto.Productos.Find(productoIds[i]);
                if (producto != null)
                {
                    total += producto.Precio * cantidades[i];
                }
            }

            var venta = new Venta
            {
                Fecha = DateTime.Now,
                IdCliente = idCliente,
                IdEmpleado = 1,
                TipoVenta = "Productos",
                TipoPago = "Dinero",
                Subtotal = total,
                Descuento = 0,
                Impuesto = 0,
                Total = total,
                MetodoPago = "Efectivo",
                Estado = "Activa",
                PuntosUsados = 0
            };

            _contexto.Ventas.Add(venta);
            _contexto.SaveChanges();

            var factura = new Factura
            {
                IdVenta = venta.IdVenta,
                FechaEmision = DateTime.Now,
                NumeroFactura = GenerarNumeroFactura(),
                Total = total,
                Estado = "Pendiente",
                Observaciones = "Factura generada por compra de productos."
            };

            _contexto.Facturas.Add(factura);
            _contexto.SaveChanges();

            for (int i = 0; i < productoIds.Length; i++)
            {
                var producto = _contexto.Productos.Find(productoIds[i]);
                if (producto != null)
                {
                    producto.Stock -= cantidades[i];
                }
            }

            _contexto.SaveChanges();

            if (venta.TipoPago != "Puntos" && venta.Total > 0)
            {
                var cashbackController = new CashbackController();
                cashbackController.AcumularPuntos(idCliente, venta.IdVenta, venta.Total);
            }

            TempData["MensajeExito"] = $"Factura {factura.NumeroFactura} generada correctamente. Estado: Pendiente de aprobación.";
            return RedirectToAction("Index");
        }

        // ==========================================
        // APROBAR FACTURA
        // ==========================================
        [HttpGet]
        [Authorize(Roles = "Administrador, Moderador")]
        public ActionResult Aprobar(int id)
        {
            var factura = _contexto.Facturas.Find(id);

            if (factura == null)
            {
                TempData["MensajeError"] = "La factura no existe.";
                return RedirectToAction("Index");
            }

            return View(factura);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador, Moderador")]
        public ActionResult Aprobar(int id, string observaciones)
        {
            var factura = _contexto.Facturas.Find(id);

            if (factura == null)
            {
                TempData["MensajeError"] = "La factura no existe.";
                return RedirectToAction("Index");
            }

            factura.Estado = "Aprobada";
            factura.FechaAprobacion = DateTime.Now;
            factura.AprobadoPorId = User.Identity.GetUserId();
            factura.Observaciones = observaciones;

            _contexto.SaveChanges();

            TempData["MensajeExito"] = $"Factura {factura.NumeroFactura} aprobada correctamente.";
            return RedirectToAction("Index");
        }

        // ==========================================
        // RECHAZAR FACTURA
        // ==========================================
        [HttpGet]
        [Authorize(Roles = "Administrador, Moderador")]
        public ActionResult Rechazar(int id)
        {
            var factura = _contexto.Facturas.Find(id);

            if (factura == null)
            {
                TempData["MensajeError"] = "La factura no existe.";
                return RedirectToAction("Index");
            }

            return View(factura);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador, Moderador")]
        public ActionResult Rechazar(int id, string observaciones)
        {
            var factura = _contexto.Facturas.Find(id);

            if (factura == null)
            {
                TempData["MensajeError"] = "La factura no existe.";
                return RedirectToAction("Index");
            }

            factura.Estado = "Rechazada";
            factura.FechaAprobacion = DateTime.Now;
            factura.AprobadoPorId = User.Identity.GetUserId();
            factura.Observaciones = observaciones;

            _contexto.SaveChanges();

            TempData["MensajeExito"] = $"Factura {factura.NumeroFactura} rechazada correctamente.";
            return RedirectToAction("Index");
        }

        // ==========================================
        // ELIMINAR FACTURA
        // ==========================================
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

        // ==========================================
        // MÉTODOS PRIVADOS
        // ==========================================
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