using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using StructureMap;
using StuSherwin.Model;

namespace StuSherwin.Mvc
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "OldPostUrl",
                "{year}/{month}/{title}.html",
                new { controller = "Post", action = "Redirect" },
                new { year = @"\d{4}", month = @"\d{2}" });

            routes.MapRoute(
                "Admin",
                "Admin/{action}/{id}",
                new { controller = "Admin", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                "Category", 
                "{category}", 
                new { controller = "Post", action = "Index" } 
            );

            routes.MapRoute(
                "Post",
                "{category}/Post/{id}",
                new { controller = "Post", action = "Display" }
            );

            routes.MapRoute(
                "Default",
                "{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
            RegisterDependencies();
        }

        private void RegisterDependencies()
        {
            ObjectFactory.Initialize(x =>
            {
                x.For<IRecaptchaService>()
                    .Use<RecaptchaService>()
                    .Ctor<string>("verificationUrl")
                    .EqualToAppSetting("RecaptchaVerificationUrl")
                    .Ctor<string>("privateKey")
                    .EqualToAppSetting("RecaptchaPrivateKey");
            });

            ControllerBuilder.Current.SetControllerFactory(
                new StructureMapControllerFactory()
            );
        }
    }
}