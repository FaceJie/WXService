using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using WebAPI.Content.WXViewModel;
using WebBLL;
using WebBLL.VisaLoginBal;
using WebHelper;
using WebModel;

namespace WebAPI.Controllers.LYService
{
    public class LoginController : Controller
    {
        UploadLoginBal bal = new UploadLoginBal();
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        //旅游登录
        public string CheckIsRegister(UserInfoViewModel userInfoViewModel)
        {
            MsgModel ret = new MsgModel();
            UploadUserModel uploadUserModel = bal.UserExsit(userInfoViewModel.NickName);
            if (uploadUserModel != null)
            {
                //存在用户
                ret.scu = true;
                DataCache.SetCache(uploadUserModel.UserId, uploadUserModel);
                ret.msg = uploadUserModel.UserId;
            }
            else
            {
                ret.scu = false;
                ret.msg = GlobalFuc.ModelToJson<UserInfoViewModel>(userInfoViewModel);//供注册使用
            }
            return GlobalFuc.ModelToJson<MsgModel>(ret);
        }
        //旅游系统注册新用户
        public string UserRegister(string nickName, string avatarUrl, string groupNo,string userName, string userTlp, string code)
        {
            MsgModel ret = new MsgModel();
            if (PhoneCode.CheckCode(userTlp, code).Contains("200"))
            {
                string result = bal.UserRegister(nickName, avatarUrl,groupNo, userName, userTlp);
                if (!string.IsNullOrEmpty(result) && result != "erorr")
                {
                    ret.scu = true;
                    ret.msg = result;
                }
                else
                {
                    ret.scu = false;
                    ret.msg = "登录失败！";
                }
            }
            else
            {
                ret.scu = false;
                ret.msg = "验证码有误！";
            }
            return GlobalFuc.ModelToJson<MsgModel>(ret);
        }

        //发送验证码
        public string SendCode(string phone)
        {
            return PhoneCode.SendCode(phone);
        }
    }
}