using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBLL;
using WebHelper;

namespace WebAPI.Controllers.LYService
{
    public class UserActionController : Controller
    {
        UserActionBal bal = new UserActionBal();
        // GET: UserAction
        public ActionResult Index()
        {
            return View();
        }
        //获取发布内容和公告------完成
        public string GetMenuActionAndPublish(string userId)
        {
            string json = "";
            DataTable dtheader = bal.GetUserInfo(userId);
            DataTable dt_action = bal.GetMenuAction(dtheader.Rows[0]["userType"].ToString());
            //是否为当前团可见，否则就是所有人
            DataTable dt_file = bal.GetFileAction(dtheader.Rows[0]["groupId"].ToString());
            json = "{";
            if (dt_action != null && dt_action.Rows.Count > 0)
            {
                json += "\"actionTable\":" + DatatableToJson.Dtb2Json(dt_action) + ",";
            }
            if (dt_file != null && dt_file.Rows.Count > 0)
            {
                //这是客服发布的公告
                json += "\"fileTable\":" + DatatableToJson.Dtb2Json(dt_file);
            }
            json = (json.Substring(json.Length - 1) == ",") ? json.Substring(0, json.Length - 1) : json;

            json += "}";
            return json;
        }
        //个人中心---完成
        public string GetMyselfPublish(string userId)
        {
            string json = "";
            DataTable dtheader = bal.GetUserInfo(userId);
            //用1代表个人可看
            DataTable dt = bal.GetMyselfPublish(dtheader.Rows[0]["groupId"].ToString(), dtheader.Rows[0]["userName"].ToString());
            json = "{";
            json += "\"userName\":" + '"' + dtheader.Rows[0]["userName"].ToString() + '"' + ",";
            json += "\"userHeader\":" + '"' + dtheader.Rows[0]["userHeader"].ToString() + '"' + ",";
            json += "\"userType\":" + '"' + dtheader.Rows[0]["userType"].ToString() + '"' + ",";
            if (dt != null && dt.Rows.Count > 0)
            {
                json += "\"publishTable\":" + DatatableToJson.Dtb2Json(dt);
            }
            json = (json.Substring(json.Length - 1) == ",") ? json.Substring(0, json.Length - 1) : json;

            json += "}";
            return json;
        }

        //获取旅游动态-----完成
        public string GetLifeList()
        {
            string json = "";
            //是否为当前团可见，否则就是所有人
            DataTable dt_life = bal.LifeList();
            json = "{";
            if (dt_life != null && dt_life.Rows.Count > 0)
            {
                json += "\"lifeTable\":" + DatatableToJson.Dtb2Json(dt_life);
            }
            json += "}";
            return json;
        }
        public string updateDisPlay(string workId)
        {
            string json = "erorr";
            if (bal.updateDisPlay(workId))
            {
                json = "ok";
            }
            return json;
        }
        //审核列表----完成
        public string GetfilterList(string userId)
        {
            string json = "";
            DataTable dtheader = bal.GetUserInfo(userId);
            DataTable dt = bal.GetfilterList();
            json = "{";
            json += "\"userName\":" + '"' + dtheader.Rows[0]["userName"].ToString() + '"' + ",";
            json += "\"userHeader\":" + '"' + dtheader.Rows[0]["userHeader"].ToString() + '"' + ",";
            json += "\"userType\":" + '"' + dtheader.Rows[0]["userType"].ToString() + '"' + ",";
            if (dt != null && dt.Rows.Count > 0)
            {
                json += "\"acrossTable\":" + DatatableToJson.Dtb2Json(dt);
            }
            json = (json.Substring(json.Length - 1) == ",") ? json.Substring(0, json.Length - 1) : json;

            json += "}";
            return json;
        }
    }
}