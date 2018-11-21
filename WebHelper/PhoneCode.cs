using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebHelper
{
    public static class PhoneCode
    {
        public static string SendCode(string mobile)
        {
            string msg = "";
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
        public static string CheckCode(string mobile,string code)
        {
            string msg = "";
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
