using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;

namespace WebAPI.Content.WeCharAuth
{
    public class WeCharAuth
    {
        JavaScriptSerializer Jss = new JavaScriptSerializer();
        /// <summary>
        /// 微信appID
        /// </summary>
        public static string WXAppId
        {
            get
            {
                return "wxba14a4538901b10d";
            }

        }


        /// <summary>
        /// 微信app密码
        /// </summary>
        public static string WXAppSecret
        {
            get
            {
                return "c3da57fbbca6264fb7c14df85a5b2116";
            }

        }

        /// <summary>
        ///用code换取获取用户信息（包括非关注用户的）
        /// </summary>
        /// <param name="Appid"></param>
        /// <param name="Appsecret"></param>
        /// <param name="Code">回调页面带的code参数</param>
        /// <returns>获取用户信息（json格式）</returns>
        public string GetUserInfo(string code)
        {
            string url = string.Format("https://api.weixin.qq.com/sns/jscode2session?appid={0}&secret={1}&js_code={2}&grant_type=authorization_code", WXAppId, WXAppSecret,code);
            string ReText = UrlHttpClient.WebRequestPostOrGet(url, "");//post/get方法获取信息
            Dictionary<string, object> DicText = (Dictionary<string, object>)Jss.DeserializeObject(ReText);
            if (!DicText.ContainsKey("openid"))
                return "";
            return DicText["openid"].ToString();
        }
    }
    public static class UrlHttpClient
    {
        //发起Http请求
        public static string HttpPost(string url, Stream data, IDictionary<object, string> headers = null)
        {
            System.Net.WebRequest request = HttpWebRequest.Create(url);
            request.Method = "POST";
            if (data != null)
                request.ContentLength = data.Length;
            //request.ContentType = "application/x-www-form-urlencoded;charset=utf-8";

            if (headers != null)
            {
                foreach (var v in headers)
                {
                    if (v.Key is HttpRequestHeader)
                        request.Headers[(HttpRequestHeader)v.Key] = v.Value;
                    else
                        request.Headers[v.Key.ToString()] = v.Value;
                }
            }
            HttpWebResponse response = null;
            try
            {
                // Get the response.
                response = (HttpWebResponse)request.GetResponse();
                // Display the status.
                Console.WriteLine(response.StatusDescription);
                // Get the stream containing content returned by the server.
                Stream dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                string responseFromServer = reader.ReadToEnd();
                // Display the content.
                Console.WriteLine(responseFromServer);
                // Cleanup the streams and the response.
                reader.Close();
                dataStream.Close();
                response.Close();
                return responseFromServer;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(response.StatusDescription);
                return e.Message;
            }
        }
        #region Post/Get提交调用抓取
        /// <summary>
        /// Post/get 提交调用抓取
        /// </summary>
        /// <param name="url">提交地址</param>
        /// <param name="param">参数</param>
        /// <returns>string</returns>
        public static string WebRequestPostOrGet(string sUrl, string sParam)
        {
            byte[] bt = System.Text.Encoding.UTF8.GetBytes(sParam);

            Uri uriurl = new Uri(sUrl);
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(uriurl);//HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url + (url.IndexOf("?") > -1 ? "" : "?") + param);
            req.Method = "Post";
            req.Timeout = 120 * 1000;
            req.ContentType = "application/x-www-form-urlencoded;";
            req.ContentLength = bt.Length;

            using (Stream reqStream = req.GetRequestStream())//using 使用可以释放using段内的内存
            {
                reqStream.Write(bt, 0, bt.Length);
                reqStream.Flush();
            }
            try
            {
                using (WebResponse res = req.GetResponse())
                {
                    //在这里对接收到的页面内容进行处理 

                    Stream resStream = res.GetResponseStream();

                    StreamReader resStreamReader = new StreamReader(resStream, System.Text.Encoding.UTF8);

                    string resLine;

                    System.Text.StringBuilder resStringBuilder = new System.Text.StringBuilder();

                    while ((resLine = resStreamReader.ReadLine()) != null)
                    {
                        resStringBuilder.Append(resLine + System.Environment.NewLine);
                    }

                    resStream.Close();
                    resStreamReader.Close();

                    return resStringBuilder.ToString();
                }
            }
            catch (Exception ex)
            {
                return ex.Message;//url错误时候回报错
            }
        }
        #endregion Post/Get提交调用抓取
    }
}