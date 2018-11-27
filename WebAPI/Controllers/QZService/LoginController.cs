using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAPI.Content.WeCharAuth;
using WebAPI.Content.WXViewModel;
using WebBLL.VisaLoginBal;
using WebHelper;
using WebModel;

namespace WebAPI.Controllers.QZService
{
    public class LoginController : Controller
    {
        VisaLoginBal bal = new VisaLoginBal();
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        //授权
        public string AuthLogin(string code)
        {
            WeCharAuth auth = new WeCharAuth();
            return auth.GetUserInfo(code);
        }


        //签证登录
        public string UserLogin(string openid)
        {

            MsgModel ret = new MsgModel();
            VisaLoginModel visaLoginModel = bal.UserLogin(openid);
            switch (visaLoginModel.Status)
            {
                case "-1":
                    ret.scu = false;
                    ret.msg = "请注册!";
                    break;
                case "0":
                    ret.scu = false;
                    ret.msg = "用户身份审核中!";
                    break;
                case "1":
                    ret.scu = true;
                    ret.msg = GlobalFuc.ModelToJson<VisaLoginModel>(visaLoginModel);
                    break;
            }
            return GlobalFuc.ModelToJson<MsgModel>(ret);
        }
       

        //签证系统注册新用户
        public string UserRegister(string openid,string nickName, string avatarUrl, string userName, string userTlp, string userType, string code,string pwd)
        {
            MsgModel ret = new MsgModel();
            if (string.IsNullOrEmpty(openid))
            {
                ret.scu = false;
                ret.msg = "用户身份获取失败!";
                return GlobalFuc.ModelToJson<MsgModel>(ret);
            }
            if (bal.Checking(userName, pwd))
            {
                ret.scu = false;
                ret.msg = "用户身份审核中!";
                return GlobalFuc.ModelToJson<MsgModel>(ret);
            }
            if (bal.CheckUserName(userName,userType))//true 为存在用户
            {
                ret.scu = false;
                ret.msg = "姓名重复，请重新输入名称!";
                return GlobalFuc.ModelToJson<MsgModel>(ret);
            }
            else
            {
                if (PhoneCode.CheckCode(userTlp, code).Contains("200"))
                {
                    string result = bal.UserRegister(openid,nickName, avatarUrl, userName, userType, userTlp, pwd);
                    if (!string.IsNullOrEmpty(result) && result != "erorr")
                    {
                        ret.scu = true;
                        ret.msg = result;
                    }
                    else
                    {
                        ret.scu = false;
                        ret.msg = "注册失败！";
                    }
                }
                else
                {
                    ret.scu = false;
                    ret.msg = "验证码有误！";
                }
            }
            return GlobalFuc.ModelToJson<MsgModel>(ret);
        }

        //发送验证码
        public string SendCode(string phone)
        {
            return PhoneCode.SendCode(phone);
        }

        public string BindMobilPhone(string userId,string tel,string code)
        {
            MsgModel ret = new MsgModel();
            if (PhoneCode.CheckCode(tel, code).Contains("200"))
            {
                string result = bal.BindMobilPhone(userId,tel);
                if (result=="0")
                {
                    ret.scu = true;
                    ret.msg = "绑定成功！";
                }
                else if (result == "1")
                {
                    ret.scu = false;
                    ret.msg = "绑定失败！";
                }
                else
                {
                    ret.scu = false;
                    ret.msg = "出项异常:"+ result;
                }
            }
            else
            {
                ret.scu = false;
                ret.msg = "验证码有误！";
            }
            return GlobalFuc.ModelToJson<MsgModel>(ret);

        }
    }
}