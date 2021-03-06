﻿namespace Quiz
{
    using System;
    using System.Globalization;
    using System.Threading;
    using System.Web.Configuration;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    using DAL;
    using MvcEnumFlags;
    using Ninject;

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Error(object sender, EventArgs e)
        {
            Logger.Error("Возникла ошибка: ");
            Logger.Error(Server.GetLastError().Message.ToString() + Server.GetLastError().ToString());
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Logger.Log = new Log4NetLogger();
            Logger.Info("Logging start");

            // биндер для bitwise RoleEnum. 
            ModelBinders.Binders.Add(typeof(RoleEnum), new EnumFlagsModelBinder());

            var kernel = new StandardKernel();
            //// по совету Гугля уберу валидацию от Ninject
            kernel.Unbind<ModelValidatorProvider>();
            DependencyResolver.SetResolver(new NinjectDependencyResolver(kernel));
        }

        protected void Application_Stop()
        {
            Logger.Info("Logging end");
        }
    }
}
