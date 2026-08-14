using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Gasolinera.Infrastructure.DbContexts;
using Gasolinera.Models.Entidades;

namespace Gasolinera.Controllers
{
    [Authorize(Roles = "Administrador, Bodeguero")]
    public class CategoriasController : Controller
    {
        private readonly GasolineraContext _contexto;

        public CategoriasController()
        {
            _contexto = new GasolineraContext();
        }

        [HttpGet]
        public ActionResult Index()
        {
            var categorias = _contexto.Categorias.ToList();
            return View(categorias);
        }

        [HttpGet]
        public ActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear(Categoria categoria)
        {
            if (!ModelState.IsValid)
            {
                return View(categoria);
            }

            _contexto.Categorias.Add(categoria);
            _contexto.SaveChanges();

            TempData["MensajeExito"] = "Categoría creada correctamente.";
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public ActionResult Editar(int id)
        {
            var categoria = _contexto.Categorias.Find(id);

            if (categoria == null)
            {
                TempData["MensajeError"] = "Categoría no encontrada.";
                return RedirectToAction("Index");
            }

            return View(categoria);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public ActionResult Editar(Categoria categoria)
        {
            if (!ModelState.IsValid)
            {
                return View(categoria);
            }

            _contexto.Entry(categoria).State = EntityState.Modified;
            _contexto.SaveChanges();

            TempData["MensajeExito"] = "Categoría actualizada correctamente.";
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public ActionResult Eliminar(int id)
        {
            var categoria = _contexto.Categorias.Find(id);

            if (categoria == null)
            {
                TempData["MensajeError"] = "Categoría no encontrada.";
                return RedirectToAction("Index");
            }

            return View(categoria);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Eliminar")]
        [Authorize(Roles = "Administrador")]
        public ActionResult EliminarConfirmado(int id)
        {
            var categoria = _contexto.Categorias.Find(id);

            if (categoria == null)
            {
                TempData["MensajeError"] = "Categoría no encontrada.";
                return RedirectToAction("Index");
            }

            if (categoria.Productos.Any())
            {
                TempData["MensajeError"] = "No se puede eliminar una categoría con productos asociados.";
                return RedirectToAction("Index");
            }

            _contexto.Categorias.Remove(categoria);
            _contexto.SaveChanges();

            TempData["MensajeExito"] = "Categoría eliminada correctamente.";
            return RedirectToAction("Index");
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