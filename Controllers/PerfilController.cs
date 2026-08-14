using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Gasolinera.Models;
using Gasolinera.Infrastructure.DbContexts;

namespace Gasolinera.Controllers
{
    [Authorize]
    public class PerfilController : Controller
    {
        private ApplicationUserManager _userManager;
        private readonly GasolineraContext _contexto;

        public PerfilController()
        {
            _contexto = new GasolineraContext();
        }

        public ApplicationUserManager UserManager
        {
            get => _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            private set => _userManager = value;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var userId = User.Identity.GetUserId();
            var usuario = await UserManager.FindByIdAsync(userId);

            if (usuario == null)
            {
                return HttpNotFound();
            }

            // Obtener puntos del cliente
            var cliente = _contexto.Clientes.FirstOrDefault(c => c.Correo == usuario.Email);
            if (cliente != null)
            {
                var cashback = _contexto.Cashbacks.FirstOrDefault(c => c.IdCliente == cliente.IdCliente);
                ViewBag.PuntosDisponibles = cashback?.PuntosDisponibles ?? 0;
                ViewBag.PuntosAcumulados = cashback?.PuntosAcumulados ?? 0;
                ViewBag.PuntosCanjeados = cashback?.PuntosCanjeados ?? 0;
                ViewBag.ClienteId = cliente.IdCliente;
            }
            else
            {
                ViewBag.PuntosDisponibles = 0;
                ViewBag.PuntosAcumulados = 0;
                ViewBag.PuntosCanjeados = 0;
                ViewBag.ClienteId = 0;
            }

            var roles = await UserManager.GetRolesAsync(userId);
            ViewBag.Roles = roles;

            return View(usuario);
        }

        [HttpGet]
        public async Task<ActionResult> Editar()
        {
            var userId = User.Identity.GetUserId();
            var usuario = await UserManager.FindByIdAsync(userId);

            if (usuario == null)
            {
                return HttpNotFound();
            }

            return View(usuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Editar(ApplicationUser model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = User.Identity.GetUserId();
            var usuario = await UserManager.FindByIdAsync(userId);

            if (usuario == null)
            {
                return HttpNotFound();
            }

            usuario.NombreCompleto = model.NombreCompleto;
            usuario.Email = model.Email;
            usuario.UserName = model.Email;

            var result = await UserManager.UpdateAsync(usuario);

            if (result.Succeeded)
            {
                TempData["MensajeExito"] = "Perfil actualizado correctamente.";
                return RedirectToAction("Index");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }

            return View(usuario);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _contexto.Dispose();
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }
            }
            base.Dispose(disposing);
        }
    }
}