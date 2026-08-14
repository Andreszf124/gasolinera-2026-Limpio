using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Gasolinera.Common;
using Gasolinera.Infrastructure.DbContexts;
using Gasolinera.Infrastructure.Repositories;
using Gasolinera.Models.Entidades;
using Microsoft.AspNet.Identity;

namespace Gasolinera.Controllers
{
    [Authorize]
    public class OrdenesServicioController : Controller
    {
        private readonly IOrdenServicioRepository _repositorio;
        private readonly GasolineraContext _contexto;

        public OrdenesServicioController()
        {
            _contexto = new GasolineraContext();
            _repositorio = new OrdenServicioRepository(_contexto);
        }

        // ========================================== //
        // LISTA DE ÓRDENES                           //
        // ========================================== //
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var cliente = _contexto.Clientes.FirstOrDefault(c => c.Correo == User.Identity.Name);
            var idCliente = cliente?.IdCliente ?? 0;

            var ordenes = _repositorio.ObtenerTodos();

            // Si no es Admin, Moderador o Mecánico, solo ve sus propias órdenes
            if (!User.IsInRole("Administrador") && !User.IsInRole("Moderador") && !User.IsInRole("Mecánico"))
            {
                var vehiculosIds = _contexto.Vehiculos
                    .Where(v => v.IdCliente == idCliente)
                    .Select(v => v.IdVehiculo)
                    .ToList();

                ordenes = ordenes.Where(o => vehiculosIds.Contains(o.IdVehiculo)).ToList();
            }

            return View(ordenes);
        }

        // ========================================== //
        // CREAR ORDEN DE SERVICIO                    //
        // ========================================== //
        [HttpGet]
        public ActionResult Crear()
        {
            var cliente = ObtenerCliente();
            if (cliente == null)
            {
                TempData["MensajeError"] = "No se encontró un cliente asociado a su cuenta.";
                return RedirectToAction("Index");
            }

            var vehiculos = _contexto.Vehiculos
                .Where(v => v.IdCliente == cliente.IdCliente)
                .ToList();

            if (!vehiculos.Any())
            {
                TempData["MensajeAdvertencia"] = "Primero debe registrar un vehículo.";
                return RedirectToAction("Index", "Vehiculos");
            }

            ViewBag.Vehiculos = new SelectList(vehiculos, "IdVehiculo", "Placa");
            ViewBag.TiposServicio = ObtenerTiposServicio();
            ViewBag.Empleados = ObtenerEmpleados();

            return View(new OrdenServicio
            {
                FechaEntrada = DateTime.Now,
                Estado = EstadoOrdenServicio.Pendiente,
                NombreCliente = cliente.NombreCompleto
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear(OrdenServicio ordenServicio)
        {
            if (!ModelState.IsValid)
            {
                var cliente = ObtenerCliente();
                ViewBag.Vehiculos = new SelectList(_contexto.Vehiculos.Where(v => v.IdCliente == cliente.IdCliente), "IdVehiculo", "Placa");
                ViewBag.TiposServicio = ObtenerTiposServicio();
                ViewBag.Empleados = ObtenerEmpleados();
                TempData["MensajeAdvertencia"] = "Revise los datos del formulario.";
                return View(ordenServicio);
            }

            // Asignar nombre de cliente
            var cliente = ObtenerCliente();
            ordenServicio.NombreCliente = cliente?.NombreCompleto ?? User.Identity.Name;

            _repositorio.Agregar(ordenServicio);

            TempData["MensajeExito"] = "Orden de servicio registrada correctamente. Un moderador la revisará pronto.";
            return RedirectToAction("Index");
        }

        // ========================================== //
        // DETALLES DE ORDEN                          //
        // ========================================== //
        [HttpGet]
        public ActionResult Detalles(int id)
        {
            var orden = _repositorio.ObtenerPorId(id);

            if (orden == null)
            {
                TempData["MensajeError"] = "La orden de servicio no existe.";
                return RedirectToAction("Index");
            }

            // Verificar permisos
            if (!User.IsInRole("Administrador") && !User.IsInRole("Moderador") && !User.IsInRole("Mecánico"))
            {
                var cliente = ObtenerCliente();
                var vehiculo = _contexto.Vehiculos.FirstOrDefault(v => v.IdVehiculo == orden.IdVehiculo);
                if (vehiculo == null || vehiculo.IdCliente != cliente?.IdCliente)
                {
                    TempData["MensajeError"] = "No tiene permisos para ver esta orden de servicio.";
                    return RedirectToAction("Index");
                }
            }

            return View(orden);
        }

        // ========================================== //
        // MÉTODOS PRIVADOS                           //
        // ========================================== //
        private Cliente ObtenerCliente()
        {
            return _contexto.Clientes.FirstOrDefault(c => c.Correo == User.Identity.Name);
        }

        private List<SelectListItem> ObtenerEmpleados()
        {
            var empleados = _contexto.Empleados
                .Select(e => new SelectListItem
                {
                    Value = e.IdEmpleado.ToString(),
                    Text = e.NombreCompleto + " - " + e.Cargo
                })
                .ToList();

            empleados.Insert(0, new SelectListItem { Value = "", Text = "-- Seleccione un mecánico --" });
            return empleados;
        }

        private List<SelectListItem> ObtenerTiposServicio()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Text = "Cambio de Aceite", Value = "Cambio de Aceite" },
                new SelectListItem { Text = "Revisión de Frenos", Value = "Revisión de Frenos" },
                new SelectListItem { Text = "Cambio de Filtros", Value = "Cambio de Filtros" },
                new SelectListItem { Text = "Diagnóstico General", Value = "Diagnóstico General" },
                new SelectListItem { Text = "Alineación y Balanceo", Value = "Alineación y Balanceo" },
                new SelectListItem { Text = "Revisión de Motor", Value = "Revisión de Motor" },
                new SelectListItem { Text = "Servicio de Suspensión", Value = "Servicio de Suspensión" },
                new SelectListItem { Text = "Sistema Eléctrico", Value = "Sistema Eléctrico" },
                new SelectListItem { Text = "Servicio de Transmisión", Value = "Servicio de Transmisión" },
                new SelectListItem { Text = "Revisión de Aire Acondicionado", Value = "Revisión de Aire Acondicionado" },
                new SelectListItem { Text = "Escaneo Computarizado", Value = "Escaneo Computarizado" },
                new SelectListItem { Text = "Otro", Value = "Otro" }
            };
        }

        private List<SelectListItem> ObtenerEstados()
        {
            List<SelectListItem> estados = new List<SelectListItem>();

            estados.Add(new SelectListItem { Text = "Pendiente", Value = "1" });
            estados.Add(new SelectListItem { Text = "En Progreso", Value = "2" });
            estados.Add(new SelectListItem { Text = "Completado", Value = "3" });
            estados.Add(new SelectListItem { Text = "Entregado", Value = "4" });
            estados.Add(new SelectListItem { Text = "Cancelado", Value = "5" });

            estados.Insert(0, new SelectListItem { Value = "", Text = "-- Seleccione un estado --" });
            return estados;
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