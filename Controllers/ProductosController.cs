using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Gasolinera.Common;
using Gasolinera.Infrastructure.DbContexts;
using Gasolinera.Infrastructure.Repositories;
using Gasolinera.Models.Entidades;

namespace Gasolinera.Controllers
{
    [Authorize]
    public class ProductosController : Controller
    {
        private readonly IProductoRepository _repositorio;
        private readonly GasolineraContext _contexto;

        public ProductosController()
        {
            _contexto = new GasolineraContext();
            _repositorio = new ProductoRepository(_contexto);
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
        [Authorize(Roles = "Administrador, Moderador, Bodeguero")]
        public ActionResult Crear()
        {
            ViewBag.Categorias = ObtenerCategorias();
            ViewBag.CategoriaList = new SelectList(_contexto.Categorias, "CategoriaId", "Nombre");
            return View(new Producto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador, Moderador, Bodeguero")]
        public ActionResult Crear(Producto producto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categorias = ObtenerCategorias();
                ViewBag.CategoriaList = new SelectList(_contexto.Categorias, "CategoriaId", "Nombre", producto.CategoriaId);
                TempData["MensajeAdvertencia"] = "Revise los datos del formulario.";
                return View(producto);
            }

            _repositorio.Agregar(producto);

            TempData["MensajeExito"] = "Producto registrado correctamente.";
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "Administrador, Moderador")]
        public ActionResult Editar(int id)
        {
            var producto = _repositorio.ObtenerPorId(id);

            if (producto == null)
            {
                TempData["MensajeError"] = "El producto no existe.";
                return RedirectToAction("Index");
            }

            ViewBag.Categorias = ObtenerCategorias();
            ViewBag.CategoriaList = new SelectList(_contexto.Categorias, "CategoriaId", "Nombre", producto.CategoriaId);
            return View(producto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador, Moderador")]
        public ActionResult Editar(Producto producto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categorias = ObtenerCategorias();
                ViewBag.CategoriaList = new SelectList(_contexto.Categorias, "CategoriaId", "Nombre", producto.CategoriaId);
                TempData["MensajeAdvertencia"] = "Revise los datos del formulario.";
                return View(producto);
            }

            _repositorio.Actualizar(producto);

            TempData["MensajeExito"] = "Producto actualizado correctamente.";
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "Administrador")]
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
        [Authorize(Roles = "Administrador")]
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