using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Gasolinera.Models;

namespace Gasolinera.Controllers
{
    [Authorize]
    public class PerfilController : Controller
    {
        private ApplicationUserManager _userManager;

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
    }
}