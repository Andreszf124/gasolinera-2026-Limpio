using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Gasolinera.Models;

namespace Gasolinera.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class UsuariosController : Controller
    {
        private ApplicationUserManager _userManager;
        private readonly ApplicationDbContext _contexto;

        public UsuariosController()
        {
            _contexto = new ApplicationDbContext();
        }

        public ApplicationUserManager UserManager
        {
            get => _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            private set => _userManager = value;
        }

        // ========================================== //
        // LISTA DE USUARIOS                          //
        // ========================================== //
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var usuarios = _contexto.Users
                .OrderBy(u => u.Email)
                .ToList();

            var lista = new List<UsuarioViewModel>();

            foreach (var usuario in usuarios)
            {
                var roles = await UserManager.GetRolesAsync(usuario.Id);

                lista.Add(new UsuarioViewModel
                {
                    Id = usuario.Id,
                    NombreCompleto = usuario.NombreCompleto,
                    Correo = usuario.Email,
                    Rol = roles.Any() ? string.Join(", ", roles) : "Sin rol"
                });
            }

            return View(lista);
        }

        // ========================================== //
        // CAMBIAR EL ROL DE UN USUARIO               //
        // ========================================== //
        [HttpGet]
        public async Task<ActionResult> Editar(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                TempData["MensajeError"] = "Usuario no encontrado.";
                return RedirectToAction("Index");
            }

            var usuario = await UserManager.FindByIdAsync(id);

            if (usuario == null)
            {
                TempData["MensajeError"] = "Usuario no encontrado.";
                return RedirectToAction("Index");
            }

            var roles = await UserManager.GetRolesAsync(usuario.Id);

            ViewBag.Roles = ObtenerRoles();

            return View(new UsuarioViewModel
            {
                Id = usuario.Id,
                NombreCompleto = usuario.NombreCompleto,
                Correo = usuario.Email,
                Rol = roles.FirstOrDefault()
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Editar(UsuarioViewModel modelo)
        {
            var usuario = await UserManager.FindByIdAsync(modelo.Id);

            if (usuario == null)
            {
                TempData["MensajeError"] = "Usuario no encontrado.";
                return RedirectToAction("Index");
            }

            // Evitar que el administrador se quite el rol a sí mismo
            if (usuario.Id == User.Identity.GetUserId())
            {
                TempData["MensajeError"] = "No puede cambiar su propio rol.";
                return RedirectToAction("Index");
            }

            if (string.IsNullOrEmpty(modelo.Rol))
            {
                ModelState.AddModelError("Rol", "Debe seleccionar un rol.");
                ViewBag.Roles = ObtenerRoles();
                return View(modelo);
            }

            // Quitar los roles actuales y asignar el nuevo
            var rolesActuales = await UserManager.GetRolesAsync(usuario.Id);

            foreach (var rol in rolesActuales)
            {
                await UserManager.RemoveFromRoleAsync(usuario.Id, rol);
            }

            await UserManager.AddToRoleAsync(usuario.Id, modelo.Rol);

            TempData["MensajeExito"] = "Rol actualizado correctamente.";
            return RedirectToAction("Index");
        }

        // ========================================== //
        // MÉTODOS PRIVADOS                           //
        // ========================================== //
        private List<SelectListItem> ObtenerRoles()
        {
            var roles = _contexto.Roles
                .OrderBy(r => r.Name)
                .ToList();

            return roles
                .Select(r => new SelectListItem
                {
                    Value = r.Name,
                    Text = r.Name
                })
                .ToList();
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
