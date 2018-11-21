using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WebModel;

namespace WebHelper
{
    public class AmapUtil
    {
        const string key = "05ae64320aa1fe168349d4fa41f6269b";
        /// <summary>
        /// 根据经纬度获取地址
        /// </summary>
        /// <param name="LngLatStr">经度纬度组成的字符串 例如:"113.692100,34.752853"</param>
        /// <param name="timeout">超时时间默认10秒</param>
        /// <returns>失败返回"" </returns>
        public static string GetLocationByLngLat(string LngLatStr, int timeout = 10000)
        {
            string formatted_address = "";
            string url = $"http://restapi.amap.com/v3/geocode/regeo?key={key}&location={LngLatStr}";
            string queryResult = GetLocationByURL(url, timeout);
            JObject jd = JsonHelper.strJsonModel(queryResult);//字符串转换成json格式
            if (jd["status"].ToString() == "1") //成功
            {
                JObject jsonObject = (JObject)jd["regeocode"]["addressComponent"]["streetNumber"];
                if (jsonObject!=null)
                {
                    formatted_address = jsonObject["street"].ToString()+ jsonObject["number"].ToString();
                }
            }
            return formatted_address;
        }

        /// <summary>
        /// 根据经纬度获取地址
        /// </summary>
        /// <param name="lng">经度 例如:113.692100</param>
        /// <param name="lat">维度 例如:34.752853</param>
        /// <param name="timeout">超时时间默认10秒</param>
        /// <returns>失败返回"" </returns>
        public static string GetLocationByLngLat(double lng, double lat, int timeout = 10000)
        {
            string url = $"http://restapi.amap.com/v3/geocode/regeo?key={key}&location={lng},{lat}";
            return GetLocationByURL(url, timeout);
        }
        /// <summary>
        /// 根据地理位置获取经纬度存入数据库
        /// </summary>
        /// <param name="addressViewModel"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public static string GetLngLatByAddress(AddressViewModel addressViewModel, int timeout = 10000)
        {
            string url = $"http://http://restapi.amap.com/v3/geocode/geo?key=" + key + "&city=" + addressViewModel.city + "&address=" + addressViewModel.address;
            return GetLocationByURL(url, timeout);
        }
        public static RouteCommandModel GetRouteCommand(string origin, string destination, int timeout = 10000)
        {
            RouteCommandModel routeCommandModel = new RouteCommandModel();
            string url = $"https://restapi.amap.com/v4/direction/bicycling?key=" + key + "&origin=" + origin + "&destination=" + destination;//高德骑行路径规划
            string queryResult = GetLocationByURL(url, timeout);
            JObject jd = JsonHelper.strJsonModel(queryResult);//字符串转换成json格式
            if (jd["errmsg"].ToString() == "OK") //成功
            {
                //获取到推荐数据
                JObject data = (JObject)jd["data"];
                JArray jsonArr = (JArray)data["paths"];
                if (jsonArr.Count > 0)
                {
                    JObject jsonobj = (JObject)jsonArr[0];
                    routeCommandModel.duration = Convert.ToInt32(Convert.ToInt64(jsonArr[0]["duration"].ToString()) * 0.017).ToString() + "min";
                    routeCommandModel.distance = Convert.ToDouble(Convert.ToInt64(jsonArr[0]["distance"].ToString()) * 0.001).ToString("0.00") + "km";
                    routeCommandModel.steps = jsonArr[0]["steps"].ToObject <List< RouteModel>>();
                }
            }
            return routeCommandModel;
        }
        
        /// <summary>
        /// 获取2个地址的距离和预计行驶时间
        /// </summary>
        /// <param name="startLonLat">起始地经度纬度</param>
        /// <param name="endLonLat">目的地经度纬度</param>
        /// <returns></returns>
        public static string GetDistance(string startLonLat, string endLonLat, int timeout = 10000)
        {
            MsgModel msg = new MsgModel();
            string duration = "";  //起始地与目的地之间的距离
            string distance = "";
            string queryUrl = "http://restapi.amap.com/v3/distance?key=" + key + "&origins=" + startLonLat + "&destination=" + endLonLat; //高德接口
            string queryResult = GetLocationByURL(queryUrl, timeout); ; //请求接口数据
            JObject jd = JsonHelper.strJsonModel(queryResult);//字符串转换成json格式
            if (jd["status"].ToString() == "1") //成功
            {
                JArray jsonArr = (JArray)jd["results"];
                if (jsonArr.Count > 0)
                {
                    JObject jsonobj = (JObject)jsonArr[0];
                    duration = Convert.ToInt32(Convert.ToInt64(jsonArr[0]["duration"].ToString()) * 0.017).ToString() + "min";
                    distance = Convert.ToDouble(Convert.ToInt64(jsonArr[0]["distance"].ToString()) * 0.001).ToString("0.00") + "km";
                }
            }
            return duration + "-" + distance;
        }

        public static double OnlyGetDistance(string startLonLat, string endLonLat, int timeout = 10000)
        {
            MsgModel msg = new MsgModel();
            double distance=0;
            string queryUrl = "http://restapi.amap.com/v3/distance?key=" + key + "&origins=" + startLonLat + "&destination=" + endLonLat + "&type=0"; //高德接口
            string queryResult = GetLocationByURL(queryUrl, timeout); ; //请求接口数据
            JObject jd = JsonHelper.strJsonModel(queryResult);//字符串转换成json格式
            if (jd["status"].ToString() == "1") //成功
            {
                JArray jsonArr = (JArray)jd["results"];
                if (jsonArr.Count > 0)
                {
                    JObject jsonobj = (JObject)jsonArr[0];
                    distance = Convert.ToDouble(Convert.ToInt64(jsonArr[0]["distance"].ToString()) * 0.001);
                }
            }
            return distance;
        }
        /// <summary>
        /// 根据URL获取地址
        /// </summary>
        /// <param name="url">Get方法的URL</param>
        /// <param name="timeout">超时时间默认10秒</param>
        /// <returns></returns>
        private static string GetLocationByURL(string url, int timeout = 10000)
        {
            string strResult = "";
            try
            {
                HttpWebRequest req = WebRequest.Create(url) as HttpWebRequest;
                req.ContentType = "multipart/form-data";
                req.Accept = "*/*";
                req.UserAgent = "";
                req.Timeout = timeout;
                req.Method = "GET";
                req.KeepAlive = true;
                HttpWebResponse response = req.GetResponse() as HttpWebResponse;
                using (StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    strResult = sr.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                strResult = ex.ToString();
            }
            return strResult;
        }
    }
}
