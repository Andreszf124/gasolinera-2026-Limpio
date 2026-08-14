using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Gasolinera.Models;

namespace Gasolinera
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            CrearRoles();
        }

        // Crea los roles del sistema si todavía no existen.
        // Así el proyecto funciona en cualquier computadora sin
        // tener que insertar los roles a mano con SQL.
        private void CrearRoles()
        {
            var contexto = new ApplicationDbContext();
            var almacenRoles = new RoleStore<IdentityRole>(contexto);
            var administradorRoles = new RoleManager<IdentityRole>(almacenRoles);

            string[] roles = { "Administrador", "Moderador", "Mecánico", "Bodeguero", "Usuario" };

            foreach (var rol in roles)
            {
                if (!administradorRoles.RoleExists(rol))
                {
                    administradorRoles.Create(new IdentityRole(rol));
                }
            }

            contexto.Dispose();
        }
    }
}
