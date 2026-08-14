using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Gasolinera.Models;
using Gasolinera.Infrastructure.DbContexts;

namespace Gasolinera.Controllers
{
    public class AccountController : Controller
    {
        private ApplicationUserManager _userManager;
        private ApplicationSignInManager _signInManager;

        public ApplicationUserManager UserManager
        {
            get => _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            private set => _userManager = value;
        }

        public ApplicationSignInManager SignInManager
        {
            get => _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            private set => _signInManager = value;
        }

        // ========================================== //
        // LOGIN                                      //
        // ========================================== //
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await SignInManager.PasswordSignInAsync(
                model.Correo,
                model.Contrasena,
                model.RecordarMe,
                shouldLockout: false);

            if (result == SignInStatus.Success)
                return RedirectToLocal(returnUrl);

            ModelState.AddModelError("", "Correo o contraseña incorrectos.");
            return View(model);
        }

        // ========================================== //
        // REGISTRO                                   //
        // ========================================== //
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Registro(RegistroViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = new ApplicationUser
            {
                UserName = model.Correo,
                Email = model.Correo,
                NombreCompleto = model.NombreCompleto
            };

            var result = await UserManager.CreateAsync(user, model.Contrasena);

            if (result.Succeeded)
            {
                await UserManager.AddToRoleAsync(user.Id, "Usuario");
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                try
                {
                    var contexto = new GasolineraContext();
                    var nuevoCliente = new Gasolinera.Models.Entidades.Cliente
                    {
                        NombreCompleto = model.NombreCompleto,
                        Correo = model.Correo,
                        Telefono = "",
                        Direccion = ""
                    };
                    contexto.Clientes.Add(nuevoCliente);
                    contexto.SaveChanges();
                }
                catch
                {
                    // Si falla, continuar
                }

                TempData["MensajeExito"] = "Registro exitoso. Bienvenido al sistema.";
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error);

            return View(model);
        }

        // ========================================== //
        // CERRAR SESIÓN                              //
        // ========================================== //
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CerrarSesion()
        {
            HttpContext.GetOwinContext().Authentication.SignOut(
                DefaultAuthenticationTypes.ApplicationCookie);

            TempData["MensajeExito"] = "Sesión cerrada correctamente.";
            return RedirectToAction("Login", "Account");
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction("Index", "Home");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }
            base.Dispose(disposing);
        }
    }
}