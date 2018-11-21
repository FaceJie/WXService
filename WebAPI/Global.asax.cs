using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace WebAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            //日志
            System.IO.FileInfo fileinfo = new System.IO.FileInfo(Server.MapPath("~/App_Data/log4net.Config"));
            log4net.Config.XmlConfigurator.Configure(fileinfo);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

        }
    }
}
