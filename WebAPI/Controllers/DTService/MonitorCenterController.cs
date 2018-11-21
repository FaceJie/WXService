using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBLL;
using WebHelper;
using WebModel;

namespace WebAPI.Controllers.DTService
{
    //监控中心，监控各个平台的快递物流情况
    public class MonitorCenterController : Controller
    {
        DTBal bal = new DTBal();
        // GET: MonitorCenter
        public ActionResult Index()
        {
            return View();
        }
        //获取所有订单
        public string GetAllOrderLocationData()
        {
            string json = "";
            DataTable dt = bal.GetAllOrderLocationData();
            if (dt != null && dt.Rows.Count > 0)
            {
                json = DatatableToJson.Dtb2Json(dt);
            }
            return json;
        }
        //获取所有后勤位置信息
        public string GetAllLogiscLocationData()
        {
            string json = "";
            DataTable dt = bal.GetAllLogiscLocationData();
            if (dt != null && dt.Rows.Count > 0)
            {
                json = DatatableToJson.Dtb2Json(dt);
            }
            return json;
        }
        public string CompleteOrderData(string logicsId,string orderDatas)
        {
            MsgModel msg = new MsgModel();
            UserInfo user = Session["userInfo"] as UserInfo;
            msg = bal.CompleteOrderData(logicsId,orderDatas, user);
            return GlobalFuc.ModelToJson<MsgModel>(msg);
        }

    }
}