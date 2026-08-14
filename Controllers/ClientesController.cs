using System.Linq;
using System.Web.Mvc;
using Gasolinera.Infrastructure.DbContexts;
using Gasolinera.Models.Entidades;

namespace Gasolinera.Controllers
{
    [Authorize]
    public class ClientesController : Controller
    {
        private readonly GasolineraContext _contexto;

        public ClientesController()
        {
            _contexto = new GasolineraContext();
        }

        public ActionResult Index()
        {
            var clientes = _contexto.Clientes.ToList();
            return View(clientes);
        }

        public ActionResult Detalles(int id)
        {
            var cliente = _contexto.Clientes.Find(id);

            if (cliente == null)
            {
                TempData["MensajeError"] = "Cliente no encontrado.";
                return RedirectToAction("Index");
            }

            return View(cliente);
        }
    }
}