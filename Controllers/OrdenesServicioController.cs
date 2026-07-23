using Gasolinera.Common;
using Gasolinera.Infrastructure.DbContexts;
using Gasolinera.Infrastructure.Repositories;
using Gasolinera.Models.Entidades;
using System;
using System.Web.Mvc;



namespace Gasolinera.Controllers
{
    public class OrdenesServicioController : Controller
    {
        private readonly IOrdenServicioRepository _repositorio;

        public OrdenesServicioController()
        {
            var contexto = new GasolineraContext();
            _repositorio = new OrdenServicioRepository(contexto);
        }

        public ActionResult Index()
        {
            var ordenes = _repositorio.ObtenerTodos();
            return View(ordenes);
        }

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

        public ActionResult Crear()
        {
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
                TempData["MensajeAdvertencia"] = "Revise los datos del formulario.";
                return View(ordenServicio);
            }

            _repositorio.Agregar(ordenServicio);

            TempData["MensajeExito"] = "Orden de servicio registrada correctamente.";
            return RedirectToAction("Index");
        }

        public ActionResult Editar(int id)
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
        public ActionResult Editar(OrdenServicio ordenServicio)
        {
            if (!ModelState.IsValid)
            {
                TempData["MensajeAdvertencia"] = "Revise los datos del formulario.";
                return View(ordenServicio);
            }

            _repositorio.Actualizar(ordenServicio);

            TempData["MensajeExito"] = "Orden de servicio actualizada correctamente.";
            return RedirectToAction("Index");
        }

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
    }
}