using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Gasolinera.Infrastructure.DbContexts;
using Gasolinera.Infrastructure.Repositories;
using Gasolinera.Models.Entidades;

namespace Gasolinera.Controllers
{
    public class VentasController : Controller
    {
        private readonly IVentaRepository _repositorio;
        private readonly GasolineraContext _contexto;
        private readonly CashbackController _cashbackController;

        public VentasController()
        {
            _contexto = new GasolineraContext();
            _repositorio = new VentaRepository(_contexto);
            _cashbackController = new CashbackController();
        }

        public ActionResult Index()
        {
            var ventas = _repositorio.ObtenerTodos();
            return View(ventas);
        }

        [HttpGet]
        public ActionResult Crear()
        {
            ViewBag.Clientes = ObtenerClientes();
            ViewBag.Empleados = ObtenerEmpleadosVentas();
            ViewBag.OrdenesServicio = ObtenerOrdenesServicio();
            return View(new Venta());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear(Venta venta)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Clientes = ObtenerClientes();
                ViewBag.Empleados = ObtenerEmpleadosVentas();
                ViewBag.OrdenesServicio = ObtenerOrdenesServicio();
                TempData["MensajeAdvertencia"] = "Revise los datos del formulario.";
                return View(venta);
            }

            if (venta.TipoPago != "Puntos" && venta.Total > 0)
            {
                _cashbackController.AcumularPuntos(venta.IdCliente, venta.IdVenta, venta.Total);
            }

            venta.Fecha = DateTime.Now;
            venta.Estado = "Activa";

            _repositorio.Agregar(venta);

            TempData["MensajeExito"] = "Venta registrada correctamente.";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Detalles(int id)
        {
            var venta = _repositorio.ObtenerPorId(id);

            if (venta == null)
            {
                TempData["MensajeError"] = "La venta no existe.";
                return RedirectToAction("Index");
            }

            return View(venta);
        }

        [HttpGet]
        public ActionResult Editar(int id)
        {
            var venta = _repositorio.ObtenerPorId(id);

            if (venta == null)
            {
                TempData["MensajeError"] = "La venta no existe.";
                return RedirectToAction("Index");
            }

            ViewBag.Clientes = ObtenerClientes();
            ViewBag.Empleados = ObtenerEmpleadosVentas();
            ViewBag.OrdenesServicio = ObtenerOrdenesServicio();
            return View(venta);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(Venta venta)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Clientes = ObtenerClientes();
                ViewBag.Empleados = ObtenerEmpleadosVentas();
                ViewBag.OrdenesServicio = ObtenerOrdenesServicio();
                TempData["MensajeAdvertencia"] = "Revise los datos del formulario.";
                return View(venta);
            }

            _repositorio.Actualizar(venta);

            TempData["MensajeExito"] = "Venta actualizada correctamente.";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Eliminar(int id)
        {
            var venta = _repositorio.ObtenerPorId(id);

            if (venta == null)
            {
                TempData["MensajeError"] = "La venta no existe.";
                return RedirectToAction("Index");
            }

            return View(venta);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Eliminar")]
        public ActionResult EliminarConfirmado(int id)
        {
            var venta = _repositorio.ObtenerPorId(id);

            if (venta == null)
            {
                TempData["MensajeError"] = "La venta no existe.";
                return RedirectToAction("Index");
            }

            _repositorio.Eliminar(venta);

            TempData["MensajeExito"] = "Venta eliminada correctamente.";
            return RedirectToAction("Index");
        }

        private List<SelectListItem> ObtenerClientes()
        {
            var clientes = _contexto.Clientes
                .Select(c => new SelectListItem
                {
                    Value = c.IdCliente.ToString(),
                    Text = c.NombreCompleto + " (ID: " + c.IdCliente + ")"
                })
                .ToList();

            clientes.Insert(0, new SelectListItem { Value = "", Text = "-- Seleccione un cliente --" });
            return clientes;
        }

        private List<SelectListItem> ObtenerEmpleadosVentas()
        {
            List<SelectListItem> empleados;

            if (_contexto.Empleados.Any())
            {
                empleados = _contexto.Empleados
                    .Select(e => new SelectListItem
                    {
                        Value = e.IdEmpleado.ToString(),
                        Text = e.NombreCompleto + " (ID: " + e.IdEmpleado + ")"
                    })
                    .ToList();
            }
            else
            {
                empleados = new List<SelectListItem>
                {
                    new SelectListItem { Value = "1", Text = "Juan Pérez - Administrador" },
                    new SelectListItem { Value = "2", Text = "María López - Cajera" },
                    new SelectListItem { Value = "3", Text = "Carlos Rojas - Supervisor" },
                    new SelectListItem { Value = "4", Text = "Ana Morales - Vendedor" },
                    new SelectListItem { Value = "5", Text = "Pedro Gómez - Mecánico" }
                };
            }

            empleados.Insert(0, new SelectListItem
            {
                Value = "",
                Text = "-- Seleccione un empleado --"
            });

            return empleados;
        }

        private List<SelectListItem> ObtenerOrdenesServicio()
        {
            var ordenes = _contexto.OrdenesServicio
                .Select(o => new SelectListItem
                {
                    Value = o.IdOrdenServicio.ToString(),
                    Text = "Orden #" + o.IdOrdenServicio + " - " + o.PlacaVehiculo
                })
                .ToList();

            ordenes.Insert(0, new SelectListItem { Value = "", Text = "-- Ninguna --" });
            return ordenes;
        }
    }
}