using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Gasolinera.Common;
using Gasolinera.Infrastructure.DbContexts;
using Gasolinera.Infrastructure.Repositories;
using Gasolinera.Models.Entidades;

namespace Gasolinera.Controllers
{
    public class OrdenesServicioController : Controller
    {
        private readonly IOrdenServicioRepository _repositorio;
        private readonly GasolineraContext _contexto;

        public OrdenesServicioController()
        {
            _contexto = new GasolineraContext();
            _repositorio = new OrdenServicioRepository(_contexto);
        }

        public ActionResult Index()
        {
            var ordenes = _repositorio.ObtenerTodos();
            return View(ordenes);
        }

        [HttpGet]
        public ActionResult Crear()
        {
            ViewBag.Empleados = ObtenerEmpleados();
            ViewBag.Estados = ObtenerEstados();
            ViewBag.TiposVehiculo = ObtenerTiposVehiculo();

            return View(new OrdenServicio
            {
                FechaEntrada = DateTime.Now,
                Estado = EstadoOrdenServicio.Pendiente
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear(OrdenServicio ordenServicio)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Empleados = ObtenerEmpleados();
                ViewBag.Estados = ObtenerEstados();
                ViewBag.TiposVehiculo = ObtenerTiposVehiculo();
                TempData["MensajeAdvertencia"] = "Revise los datos del formulario.";
                return View(ordenServicio);
            }

            _repositorio.Agregar(ordenServicio);

            TempData["MensajeExito"] = "Orden de servicio registrada correctamente.";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Detalles(int id)
        {
            var orden = _repositorio.ObtenerPorId(id);

            if (orden == null)
            {
                TempData["MensajeError"] = "La orden de servicio no existe.";
                return RedirectToAction("Index");
            }

            return View(orden);
        }

        [HttpGet]
        public ActionResult Editar(int id)
        {
            var orden = _repositorio.ObtenerPorId(id);

            if (orden == null)
            {
                TempData["MensajeError"] = "La orden de servicio no existe.";
                return RedirectToAction("Index");
            }

            ViewBag.Empleados = ObtenerEmpleados();
            ViewBag.Estados = ObtenerEstados();
            ViewBag.TiposVehiculo = ObtenerTiposVehiculo();

            return View(orden);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(OrdenServicio ordenServicio)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Empleados = ObtenerEmpleados();
                ViewBag.Estados = ObtenerEstados();
                ViewBag.TiposVehiculo = ObtenerTiposVehiculo();
                TempData["MensajeAdvertencia"] = "Revise los datos del formulario.";
                return View(ordenServicio);
            }

            _repositorio.Actualizar(ordenServicio);

            TempData["MensajeExito"] = "Orden de servicio actualizada correctamente.";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Eliminar(int id)
        {
            var orden = _repositorio.ObtenerPorId(id);

            if (orden == null)
            {
                TempData["MensajeError"] = "La orden de servicio no existe.";
                return RedirectToAction("Index");
            }

            return View(orden);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Eliminar")]
        public ActionResult EliminarConfirmado(int id)
        {
            var orden = _repositorio.ObtenerPorId(id);

            if (orden == null)
            {
                TempData["MensajeError"] = "La orden de servicio no existe.";
                return RedirectToAction("Index");
            }

            _repositorio.Eliminar(orden);

            TempData["MensajeExito"] = "Orden de servicio eliminada correctamente.";
            return RedirectToAction("Index");
        }

        private List<SelectListItem> ObtenerEmpleados()
        {
            List<SelectListItem> empleados;

            if (_contexto.Empleados.Any())
            {
                empleados = _contexto.Empleados
                    .Select(e => new SelectListItem
                    {
                        Value = e.IdEmpleado.ToString(),
                        Text = e.NombreCompleto + " - " + e.Cargo + " (ID: " + e.IdEmpleado + ")"
                    })
                    .ToList();
            }
            else
            {
                empleados = new List<SelectListItem>
        {
            new SelectListItem { Value = "1", Text = "Juan Pérez - Mecánico" },
            new SelectListItem { Value = "2", Text = "María López - Mecánico" },
            new SelectListItem { Value = "3", Text = "Pedro Gómez - Mecánico" }
        };
            }

            empleados.Insert(0, new SelectListItem { Value = "", Text = "-- Seleccione un mecánico --" });
            return empleados; // <--- ESTE RETURN ES EL QUE FALTABA
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

        private List<SelectListItem> ObtenerTiposVehiculo()
        {
            List<SelectListItem> tipos = new List<SelectListItem>();

            tipos.Add(new SelectListItem { Text = "Carro", Value = "Carro" });
            tipos.Add(new SelectListItem { Text = "Moto", Value = "Moto" });
            tipos.Add(new SelectListItem { Text = "Camión", Value = "Camión" });

            tipos.Insert(0, new SelectListItem { Value = "", Text = "-- Seleccione un tipo de vehículo --" });
            return tipos;
        }
    }
}