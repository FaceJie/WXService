using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBLL;
using WebHelper;

namespace WebAPI.Controllers.QZService
{
    public class VisaSerachController : Controller
    {
        VisaSerachBal bal = new VisaSerachBal();
        // GET: VisaSerach
        public ActionResult Index()
        {
            return View();
        }
        //1
        public string GetCountList()
        {
            string json = "";
            DataTable dt = bal.GetCountList();
            if (dt != null && dt.Rows.Count > 0)
            {
                json = DatatableToJson.Dtb2Json(dt);
            }
            return json;
        }
    }
}