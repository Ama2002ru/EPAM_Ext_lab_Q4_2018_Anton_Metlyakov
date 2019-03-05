namespace Quiz
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;

    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            /* route - еще поиск по строке ... */
            routes.MapRoute(
                name: "user-search",
                url: "users/{name}",
                defaults: new { controller = "user", action = "index", name = UrlParameter.Optional });

            routes.MapRoute(
                name: "logon",
                url: "logon",
                defaults: new { controller = "logon", action = "LogOn" });

            routes.MapRoute(
                name: "logoff",
                url: "logoff",
                defaults: new { controller = "logon", action = "LogOff" });

            routes.MapRoute(
                name: "user-create",
                url: "create-user",
                defaults: new { controller = "User", action = "Create" });

            routes.MapRoute(
                name: "user-edit",
                url: "user/{id}/edit",
                defaults: new { controller = "User", action = "Edit", id = "id" },
                constraints: new { id = @"\d+" });

            routes.MapRoute(
                name: "user-delete",
                url: "user/{id}/delete",
                defaults: new { controller = "User", action = "Delete", id = "id" },
                constraints: new { id = @"\d+" });

            routes.MapRoute(
                name: "user",
                url: "user/{id}",
                defaults: new { controller = "user", action = "Details", id = "id" },
                constraints: new { id = @"\d+" });

            routes.MapRoute(
                name: "users",
                url: "user/index",
                defaults: new { controller = "user", action = "Index" });

            routes.MapRoute(
                name: "courses",
                url: "courses",
                defaults: new { controller = "course", action = "Index" });

            routes.MapRoute(
                name: "Quizes",
                url: "Quises",
                defaults: new { controller = "Quiz", action = "Index" });

            routes.MapRoute(
                name: "Questions",
                url: "Questions",
                defaults: new { controller = "Question", action = "Index" });

            routes.MapRoute(
                name: "Workbook",
                url: "Workbook",
                defaults: new { controller = "Workbook", action = "Index" });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "User", action = "Index", id = UrlParameter.Optional });
        }
    }
}
