using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebAPI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            string[] NS;
            NS = "WebAPI.Controllers.DTService".Split('|');
            routes.MapRoute(
              "DTService", // 路由名称  
              "DTService/{controller}/{action}/{id}", // 带有参数的 URL  
              new { controller = "DTService", action = "Index", id = UrlParameter.Optional }, NS
            );
            NS = "WebAPI.Controllers.QZService".Split('|');
            routes.MapRoute(
              "QZService", // 路由名称  
              "QZService/{controller}/{action}/{id}", // 带有参数的 URL  
              new { controller = "QZService", action = "Index", id = UrlParameter.Optional }, NS  
            );
            NS = "WebAPI.Controllers.LYService".Split('|');
            routes.MapRoute(
              "LYService", // 路由名称  
              "LYService/{controller}/{action}/{id}", // 带有参数的 URL  
              new { controller = "LYService", action = "Index", id = UrlParameter.Optional }, NS
            );
          
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller="Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}

