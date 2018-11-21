using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WebBLL.VisaLoginBal;
using WebHelper;
using WebModel;

namespace WebAPI.Controllers
{
    /// <summary>
    /// 此登陆为控制器的微信验证登录，此次授权在小程序前端完成，暂不使用
    /// </summary>
    public class LoginController : Controller
    {

        //微信小程序相关配置
        JavaScriptSerializer Jss = new JavaScriptSerializer();
        Oauth2 auth = new Oauth2();
        VisaLoginBal bal = new VisaLoginBal();
        String APPID = "wxba14a4538901b10d";
        String APPSERECT = "c3da57fbbca6264fb7c14df85a5b2116";
        String backUrl = "http://www.iot-esta.com/Login/Index";

        // GET: Login
        public string Index()
        {
            //这里完成授权
            string code = Request["code"];
            MsgModel ret = new MsgModel();
            string userInfo = auth.CodeGetOpenid(APPID, APPSERECT, code);//得到微信的信息
            Dictionary<string, object> DicText = (Dictionary<string, object>)Jss.DeserializeObject(userInfo);
            DataCache.SetCache(DicText["openid"].ToString(), userInfo);//把当前的小程序信息存储起来
            //if (bal.GetUserInfoById(DicText["openid"].ToString()))
            //{
            //    //存在用户
            //    ret.scu = true;
            //    ret.msg = DicText["openid"].ToString();
            //}
            //else
            //{
            //    ret.scu = false;
            //    ret.msg = userInfo;//供注册使用
            //}
            return GlobalFuc.ModelToJson<MsgModel>(ret);
        }
        public ActionResult AuthLogin()
        {
            String url = auth.GetCodeUrl(APPID, backUrl, "snsapi_userinfo");
            return Redirect(url);
        }

        public string SaveUserInfo(string openid,string nickName, string avatarUrl, string province,string city)
        {
            MsgModel ret = new MsgModel();
            String userInfo = Request.Form["wechatInfo"];
            Dictionary<string, object> DicText = (Dictionary<string, object>)Jss.DeserializeObject(userInfo);
            String userName = Request.Form["username"];
            String userPhone = Request.Form["userPhone"];
            String userCompany = Request.Form["userCompany"];
            String code = Request.Form["code"];
            try
            {
                if (CheckCode()=="200")
                {
                    //if (bal.insertUserInfo(userName, userPhone, userCompany, DicText["nickname"].ToString(), DicText["headimgurl"].ToString(), DicText["openid"].ToString()) == "ok")
                    //{
                    //    ret.scu = true;
                    //    ret.msg = "注册成功！";
                    //}
                    //else
                    //{
                    //    ret.scu = false;
                    //    ret.msg = "注册失败！";
                    //}
                }
                else
                {
                    ret.scu = false;
                    ret.msg = "请输出正确的验证码！";
                } 
            }
            catch (Exception ex)
            {
                ret.scu = false;
                ret.msg = ex.Message.ToString();
            }
            return GlobalFuc.ModelToJson<MsgModel>(ret);
        }

        public string SendCode()
        {
            string msg = "";
            string mobile = Request["mobile"];
            String url = "https://api.netease.im/sms/sendcode.action";
            url += "?templateid=3932135&mobile=" + mobile;//请输入正确的手机号

            //网易云信分配的账号，请替换你在管理后台应用下申请的Appkey
            String appKey = "dc8b5d76ca7015f53f3bec361ade561b";
            //网易云信分配的密钥，请替换你在管理后台应用下申请的appSecret
            String appSecret = "ea5125c41272";
            //随机数（最大长度128个字符）
            String nonce = "12345";

            TimeSpan ts = DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1);
            Int32 ticks = System.Convert.ToInt32(ts.TotalSeconds);
            //当前UTC时间戳，从1970年1月1日0点0 分0 秒开始到现在的秒数(String)
            String curTime = ticks.ToString();
            //SHA1(AppSecret + Nonce + CurTime),三个参数拼接的字符串，进行SHA1哈希计算，转化成16进制字符(String，小写)
            String checkSum = CheckSumBuilder.getCheckSum(appSecret, nonce, curTime);

            IDictionary<object, String> headers = new Dictionary<object, String>();
            headers["AppKey"] = appKey;
            headers["Nonce"] = nonce;
            headers["CurTime"] = curTime;
            headers["CheckSum"] = checkSum;
            headers["ContentType"] = "application/x-www-form-urlencoded;charset=utf-8";
            //执行Http请求
            msg = HttpClient.HttpPost(url, null, headers);
            return msg;
        }
        public string CheckCode()
        {
            string msg = "";
            string mobile = Request["mobile"];
            string code = Request["code"];
            String url = "https://api.netease.im/sms/verifycode.action";
            url += "?mobile=" + mobile + "&code=" + code;//请输入正确的手机号
            //网易云信分配的账号，请替换你在管理后台应用下申请的Appkey
            String appKey = "dc8b5d76ca7015f53f3bec361ade561b";
            //网易云信分配的密钥，请替换你在管理后台应用下申请的appSecret
            String appSecret = "ea5125c41272";
            //随机数（最大长度128个字符）
            String nonce = "12345";

            TimeSpan ts = DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1);
            Int32 ticks = System.Convert.ToInt32(ts.TotalSeconds);
            //当前UTC时间戳，从1970年1月1日0点0 分0 秒开始到现在的秒数(String)
            String curTime = ticks.ToString();
            //SHA1(AppSecret + Nonce + CurTime),三个参数拼接的字符串，进行SHA1哈希计算，转化成16进制字符(String，小写)
            String checkSum = CheckSumBuilder.getCheckSum(appSecret, nonce, curTime);

            IDictionary<object, String> headers = new Dictionary<object, String>();
            headers["AppKey"] = appKey;
            headers["Nonce"] = nonce;
            headers["CurTime"] = curTime;
            headers["CheckSum"] = checkSum;
            headers["ContentType"] = "application/x-www-form-urlencoded;charset=utf-8";
            msg = HttpClient.HttpPost(url, null, headers);
            return msg;
        }
    }
}