using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Gasolinera.Infrastructure.DbContexts;
using Gasolinera.Models.Entidades;
using Microsoft.AspNet.Identity;

namespace Gasolinera.Controllers
{
    [Authorize]
    public class VehiculosController : Controller
    {
        private readonly GasolineraContext _contexto;

        public VehiculosController()
        {
            _contexto = new GasolineraContext();
        }

        public ActionResult Index()
        {
            var usuarioCliente = ObtenerClientePorUsuario();

            if (usuarioCliente == null)
            {
                TempData["MensajeError"] = "No se encontró un cliente asociado a su cuenta.";
                return View(new List<Vehiculo>());
            }

            var vehiculos = _contexto.Vehiculos
                .Where(v => v.IdCliente == usuarioCliente.IdCliente)
                .OrderBy(v => v.Placa)
                .ToList();

            return View(vehiculos);
        }

        public ActionResult Crear()
        {
            ViewBag.TiposVehiculo = ObtenerListaTiposVehiculo();
            return View(new Vehiculo());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear(Vehiculo vehiculo)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.TiposVehiculo = ObtenerListaTiposVehiculo();
                return View(vehiculo);
            }

            var usuarioCliente = ObtenerClientePorUsuario();

            if (usuarioCliente == null)
            {
                TempData["MensajeError"] = "No se encontró un cliente asociado a su cuenta.";
                return RedirectToAction("Index");
            }

            // Validar placa duplicada
            if (_contexto.Vehiculos.Any(v => v.Placa == vehiculo.Placa && v.IdCliente == usuarioCliente.IdCliente))
            {
                ModelState.AddModelError("Placa", "Ya tiene un vehículo registrado con esta placa.");
                ViewBag.TiposVehiculo = ObtenerListaTiposVehiculo();
                return View(vehiculo);
            }

            vehiculo.IdCliente = usuarioCliente.IdCliente;
            vehiculo.FechaRegistro = DateTime.Now;

            _contexto.Vehiculos.Add(vehiculo);
            _contexto.SaveChanges();

            TempData["MensajeExito"] = $"Vehículo {vehiculo.Placa} registrado correctamente.";
            return RedirectToAction("Index");
        }

        public ActionResult Eliminar(int id)
        {
            var usuarioCliente = ObtenerClientePorUsuario();
            var vehiculo = _contexto.Vehiculos
                .FirstOrDefault(v => v.IdVehiculo == id && v.IdCliente == usuarioCliente.IdCliente);

            if (vehiculo == null)
            {
                TempData["MensajeError"] = "Vehículo no encontrado.";
                return RedirectToAction("Index");
            }

            return View(vehiculo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Eliminar")]
        public ActionResult EliminarConfirmado(int id)
        {
            var usuarioCliente = ObtenerClientePorUsuario();
            var vehiculo = _contexto.Vehiculos
                .FirstOrDefault(v => v.IdVehiculo == id && v.IdCliente == usuarioCliente.IdCliente);

            if (vehiculo == null)
            {
                TempData["MensajeError"] = "Vehículo no encontrado.";
                return RedirectToAction("Index");
            }

            _contexto.Vehiculos.Remove(vehiculo);
            _contexto.SaveChanges();

            TempData["MensajeExito"] = "Vehículo eliminado correctamente.";
            return RedirectToAction("Index");
        }

        private Cliente ObtenerClientePorUsuario()
        {
            var email = User.Identity.Name;
            return _contexto.Clientes.FirstOrDefault(c => c.Correo == email);
        }

        private List<SelectListItem> ObtenerListaTiposVehiculo()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Text = "Carro", Value = "Carro" },
                new SelectListItem { Text = "Moto", Value = "Moto" },
                new SelectListItem { Text = "Camión", Value = "Camión" },
                new SelectListItem { Text = "SUV", Value = "SUV" },
                new SelectListItem { Text = "Pickup", Value = "Pickup" }
            };
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