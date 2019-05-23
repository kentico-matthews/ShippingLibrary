using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

using Kentico.Web.Mvc;

namespace MVC
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // Maps routes to Kentico HTTP handlers and features enabled in ApplicationConfig.cs
            // Always map the Kentico routes before adding other routes. Issues may occur if Kentico URLs are matched by a general route, for example images might not be displayed on pages
            routes.Kentico().MapRoutes();
            routes.MapRoute(
                name: "AddToCart",
                url: "AddToCart/{SKUID}",
                defaults: new {controller="Default", action="AddToCart", SKUID = 0 }
            );
            routes.MapRoute(
                name: "ViewCart",
                url: "ViewCart/",
                defaults: new { controller = "Default", action = "ViewCart" }
            );
            routes.MapRoute(
                name: "Shipping",
                url: "Shipping/",
                defaults: new { controller = "Default", action = "Shipping" }
            );
            routes.MapRoute(
                name: "UpdateAddress",
                url: "UpdateAddress/",
                defaults: new { controller = "Default", action = "UpdateAddress" }
            );
            routes.MapRoute(
                name: "Review",
                url: "Review/",
                defaults: new { controller = "Default", action = "Review" }
            );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Default", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
