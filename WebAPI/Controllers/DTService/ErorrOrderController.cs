using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using WebBLL;
using WebHelper;
using WebModel;

namespace WebAPI.Controllers.DTService
{
    public class ErorrOrderController : Controller
    {
        ErorrOrder bal = new ErorrOrder();
        // GET: ErorrOrder
        public ActionResult Index()
        {
            //分析数据
            TableOrgModel tableOrgModel = new TableOrgModel();
            DataTable dt = bal.GetOrderInfo(tableOrgModel);
            DataRow[] dr_news=dt.Select("erorrData IS NULL");
            foreach (DataRow dr_new in dr_news)
            {
                //ExcuteErorrData(dr_new);
            }

            return View();
        }
        //持续查询待审核的用户
        public string GetAction()
        {
            string json = "";
            DataTable dt = bal.GetAction();
            if (dt != null && dt.Rows.Count > 0)
            {
                json = DatatableToJson.Dtb2Json(dt);
            }
            return json;
        }

        public int ExcuteErorrData(DataRow dr)
        {
            
            if (!string.IsNullOrEmpty(dr["orderCompleteTime"].ToString()))
            {
                string str_erorr = "";
                string recommendedDistance = dr["recommendedDistance"].ToString();
                string reallyDistance = dr["reallyDistance"].ToString();
                string start= dr["orderReallyTime"].ToString();
                int end= Convert.ToInt32(dr["timeLimit"].ToString())*60;
                string distanceRex = @"[+-]?\d+[\.]?\d*";
                double DistanceSpan = Math.Round(double.Parse(Regex.Match(recommendedDistance, distanceRex).Value), 0) - Math.Round(double.Parse(Regex.Match(reallyDistance, distanceRex).Value), 0);
                //计算时间
                double TimeSpan = Math.Round(double.Parse(Regex.Match(start, distanceRex).Value), 0) - Math.Round(Convert.ToDouble(end), 0);
                int erorrIndex = 0;
                if (DistanceSpan <- 5)
                {
                    erorrIndex++;
                    str_erorr += "绕路大于5km，";
                }
                if (TimeSpan > 60&& TimeSpan <= 180)
                {
                    erorrIndex++;
                    str_erorr += "收件超时1小时，";
                }
                if (TimeSpan > 180)
                {
                    erorrIndex++;
                    str_erorr += "故意不结束订单，";
                }
                if (erorrIndex == 0)
                {
                    str_erorr = "正常";
                }
                return bal.UpErorrData(dr["transportId"].ToString(), str_erorr.TrimEnd('，'));
            }
            else
            {
                return 0;
            }
        }


        //用户审核
        public string UpAction(string userId, string type)
        {
            MsgModel ret = new MsgModel();

            if (bal.UpAction(userId, type) > 0)
            {
                ret.scu = true;
                ret.msg = "授权成功！";
            }
            else
            {
                ret.scu = false;
                ret.msg = "授权失败！";
            }
            return GlobalFuc.ModelToJson<MsgModel>(ret);
        }
        //获取订单信息

        public string GetOrderInfo(string page,string limit)
        {
            string sData = Request.Params.Get("data");
            string json = "{\"code\":0,\"msg\":\"成功\"";
            if (!string.IsNullOrEmpty(sData))
            {
                TableOrgModel tableOrgModel = GlobalFuc.JsonToModel<TableOrgModel>(sData);
                DataTable dt = bal.GetOrderInfo(tableOrgModel);
                if (dt != null && dt.Rows.Count > 0)
                {
                    json += ",\"count\":" + dt.Rows.Count;
                    json += ",\"data\":" + DatatableToJson.Dtb2Json(GlobalFuc.GetPagedTable(dt, Convert.ToInt32(page), Convert.ToInt32(limit)))+ "}";
                }
                else
                {
                    json += "\"count\":" + 0;
                    json += "\"data\":" + "}";
                }
            }
            else
            {
                TableOrgModel tableOrgModel = new TableOrgModel() ;
                DataTable dt = bal.GetOrderInfo(tableOrgModel);
                if (dt != null && dt.Rows.Count > 0)
                {
                    json += ",\"count\":" + dt.Rows.Count;
                    json += ",\"data\":" + DatatableToJson.Dtb2Json(GlobalFuc.GetPagedTable(dt, Convert.ToInt32(page), Convert.ToInt32(limit))) + "}";
                }
                else
                {
                    json += "\"count\":" + 0;
                    json += "\"data\":" + "}";
                }
            }
            return json;
        }
        //更新
        public string UpErorrData(string transportId,string erorrData)
        {
            MsgModel ret = new MsgModel();

            if (bal.UpErorrData(transportId, erorrData) > 0)
            {
                ret.scu = true;
                ret.msg = "授权成功！";
            }
            else
            {
                ret.scu = false;
                ret.msg = "授权失败！";
            }
            return GlobalFuc.ModelToJson<MsgModel>(ret);
        }

    }
}