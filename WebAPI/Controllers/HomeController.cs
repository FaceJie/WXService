using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebHelper;
using WebModel;

namespace WebAPI.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// 配置小程序相关
        /// </summary>
        /// <returns></returns>
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
    }
}