using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBLL;
using WebHelper;

namespace WebAPI.Controllers.QZService
{
    public class PFDLoadController : Controller
    {
        FilesUpBal bal = new FilesUpBal();
        // GET: PFDLoad
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult PdfIndex()
        {
            return View();
        }
        public ActionResult DownIndex()
        {
            return View();
        }
        //获取签证的文件列表
        public string GetVisaTableList(string fileName)
        {
            string json = "";
            DataTable dt = bal.GetVisaTableList(fileName);
            if (dt != null && dt.Rows.Count > 0)
            {
                json = DatatableToJson.Dtb2Json(dt);
            }
            return json;
        }
    }
}