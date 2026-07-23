using Gasolinera.Common;
using Gasolinera.Infrastructure.DbContexts;
using Gasolinera.Infrastructure.Repositories;
using Gasolinera.Models.Entidades;
using gasolinera_2026.Models.Entidades;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Gasolinera.Controllers
{
    public class ProductosController : Controller
    {
        private readonly IProductoRepository _repositorio;

        public ProductosController()
        {
            var contexto = new GasolineraContext();
            _repositorio = new ProductoRepository(contexto);
        }

        [HttpGet]
        public ActionResult Index()
        {
            var productos = _repositorio.ObtenerTodos();
            return View(productos);
        }

        [HttpGet]
        public ActionResult Detalles(int id)
        {
            var producto = _repositorio.ObtenerPorId(id);

            if (producto == null)
            {
                TempData["MensajeError"] = "El producto no existe.";
                return RedirectToAction("Index");
            }

            return View(producto);
        }

        [HttpGet]
        public ActionResult Crear()
        {
            ViewBag.Categorias = ObtenerCategorias();
            return View(new Producto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear(Producto producto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categorias = ObtenerCategorias();
                TempData["MensajeAdvertencia"] = "Revise los datos del formulario.";
                return View(producto);
            }

            _repositorio.Agregar(producto);

            TempData["MensajeExito"] = "Producto registrado correctamente.";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Editar(int id)
        {
            var producto = _repositorio.ObtenerPorId(id);

            if (producto == null)
            {
                TempData["MensajeError"] = "El producto no existe.";
                return RedirectToAction("Index");
            }

            ViewBag.Categorias = ObtenerCategorias();
            return View(producto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(Producto producto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categorias = ObtenerCategorias();
                TempData["MensajeAdvertencia"] = "Revise los datos del formulario.";
                return View(producto);
            }

            _repositorio.Actualizar(producto);

            TempData["MensajeExito"] = "Producto actualizado correctamente.";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Eliminar(int id)
        {
            var producto = _repositorio.ObtenerPorId(id);

            if (producto == null)
            {
                TempData["MensajeError"] = "El producto no existe.";
                return RedirectToAction("Index");
            }

            return View(producto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Eliminar")]
        public ActionResult EliminarConfirmado(int id)
        {
            var producto = _repositorio.ObtenerPorId(id);

            if (producto == null)
            {
                TempData["MensajeError"] = "El producto no existe.";
                return RedirectToAction("Index");
            }

            _repositorio.Eliminar(producto);

            TempData["MensajeExito"] = "Producto eliminado correctamente.";
            return RedirectToAction("Index");
        }

        private List<SelectListItem> ObtenerCategorias()
        {
            List<SelectListItem> categorias = new List<SelectListItem>();

            categorias.Add(new SelectListItem { Text = "Repuesto", Value = "1" });
            categorias.Add(new SelectListItem { Text = "Aceite", Value = "2" });
            categorias.Add(new SelectListItem { Text = "Filtro", Value = "3" });
            categorias.Add(new SelectListItem { Text = "Bujía", Value = "4" });
            categorias.Add(new SelectListItem { Text = "Accesorio", Value = "5" });

            return categorias;
        }
    }
}