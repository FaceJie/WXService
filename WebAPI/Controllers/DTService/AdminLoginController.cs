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
    public class AdminLoginController : Controller
    {
        DTBal bal = new DTBal();
        // GET: AdminLogin
        public ActionResult Index()
        {
            return View();
        }
        public string Login(string username, string password)
        {
            MsgModel ret = new MsgModel();
            try
            {
                UserInfo user = new UserInfo();
                DataTable result = bal.Login(username, password);
                user.userId = result.Rows[0]["userId"].ToString();
                user.userName= result.Rows[0]["username"].ToString();
                user.userTlp = result.Rows[0]["userTlp"].ToString();
                if (result!=null&&result.Rows.Count> 0)
                {
                    ret.scu = true;
                    Session["userInfo"]= user;
                    ret.msg = "登陆成功！";
                }
                else
                {
                    ret.scu = false;
                    ret.msg = "用户名或密码错误！";
                }
            }
            catch (Exception e)
            {
                ret.scu = false;
                ret.msg = e.Message.ToString();
            }
            return GlobalFuc.ModelToJson<MsgModel>(ret);
        }
    }
    
}