using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Products.BackEnd
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //  Esta linea ejecuta la actualizacion de la base de datos (Modelos), busca la clase de conecion (DataContextLocal) y  \\
            //  La carpeta donde reside la configuración (Migrations > Configuratin)                                                \\
            Database.SetInitializer(
                new MigrateDatabaseToLatestVersion<
                    Models.DataContextLocal, 
                    Migrations.Configuration>());

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
