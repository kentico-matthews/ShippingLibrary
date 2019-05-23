using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

using Kentico.Web.Mvc;

namespace MVC
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            // Enables and configures selected Kentico ASP.NET MVC integration features
            ApplicationConfig.RegisterFeatures(ApplicationBuilder.Current);

            // Registers routes including system routes for enabled features
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
