using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBLL;
using WebHelper;
using WebModel;

namespace WebAPI.Controllers.QZService
{
    public class VisaInfoListController : Controller
    {
        VisaInfoBal bal = new VisaInfoBal();
        // GET: VisaInfoList
        public ActionResult Index()
        {
            return View();
        }

        //判断出签还是入签
        public string CheckStaus(string userId,string sendDataTime, string passportNo, string country,string tihuoWay)
        {
            string serviceName = "";
            string json = "";
            VisaLoginModel visaLoginModel = (VisaLoginModel)DataCache.GetCache(userId);
            if (visaLoginModel==null)
            {
                return json;
            }
            if (!string.IsNullOrEmpty(visaLoginModel.BindPhone))
            {
                string targetName = bal.GetNameByPhone(visaLoginModel.BindPhone);
                if (!string.IsNullOrEmpty(targetName))
                {
                    serviceName = targetName;
                }
                else
                {
                    return json;
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(visaLoginModel.UserName))
                {
                    serviceName = visaLoginModel.UserName;
                }
                else
                {
                    return json;
                }
            }
            DataTable dt = bal.GetVisaInfoParams(serviceName, sendDataTime, passportNo, country, tihuoWay);
            DataRow[] drSignIn, drUnderWay, drSignOut;

            if (dt != null && dt.Rows.Count > 0)
            {
                dt.Columns.Add("Staus", Type.GetType("System.String"));
                /////改变数据的状态
                drSignIn = dt.Select("RealTime IS NULL");//入签
                if (drSignIn.Count() > 0)
                {
                    foreach (DataRow dr in drSignIn)
                    {
                        dr["Staus"] = "-1";
                    }
                }
                drUnderWay = dt.Select("RealTime IS NOT NULL and  FinishTime IS NULL");//办理中
                if (drUnderWay.Count() > 0)
                {
                    foreach (DataRow dr in drUnderWay)
                    {
                        dr["Staus"] = "0";
                    }
                }
                drSignOut = dt.Select(string.Format("RealTime IS NOT NULL and FinishTime IS NOT NULL"));//出签
                if (drSignOut.Count() > 0)
                {
                    foreach (DataRow dr in drSignOut)
                    {
                        dr["Staus"] = "1";
                    }
                }
                json = DatatableToJson.Dtb2Json(dt);
            }
            return json;
        }
    }
}