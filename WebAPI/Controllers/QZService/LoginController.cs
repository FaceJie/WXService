using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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

        //签证登录
        public string CheckIsRegister(UserInfoViewModel userInfoViewModel)
        {
            MsgModel ret = new MsgModel();
            if (bal.Checking(userInfoViewModel.NickName))
            {
                ret.scu = true;
                ret.msg = "用户身份审核中!";
                return GlobalFuc.ModelToJson<MsgModel>(ret);
            }
            try
            {
                VisaLoginModel visaLoginModel = bal.UserExsit(userInfoViewModel.NickName);
                if (visaLoginModel != null)
                {
                    //存在用户
                    ret.scu = true;
                    DataCache.SetCache(visaLoginModel.UserId, visaLoginModel);//为了减少缓存，只把userId存储起来
                    ret.msg = visaLoginModel.UserId + "*" + visaLoginModel.UserType;
                }
                else
                {
                    ret.scu = false;
                    ret.msg = GlobalFuc.ModelToJson<UserInfoViewModel>(userInfoViewModel);//供注册使用
                }
            }
            catch (Exception ex)
            {
                ret.scu = false;
                ret.msg = ex.Message.ToString();//供注册使用
            }
           
            return GlobalFuc.ModelToJson<MsgModel>(ret);
        }
       

        //签证系统注册新用户
        public string UserRegister(string nickName, string avatarUrl, string userName, string userTlp, string userType, string code)
        {
            MsgModel ret = new MsgModel();
            if (bal.Checking(nickName))
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
                    string result = bal.UserRegister(nickName, avatarUrl, userName, userType, userTlp);
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