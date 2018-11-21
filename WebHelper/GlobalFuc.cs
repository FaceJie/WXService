using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;

namespace WebHelper
{
    public static class GlobalFuc
    {
        public static decimal GetNumberOrNull(decimal? num)
        {
            return num == null ? 0 : decimal.Parse(num.ToString());
        }

        public static int GetIntOrNull(object num)
        {
            if (num == null) return 0;
            return num.ToString() == "" ? 0 : int.Parse(num.ToString());
        }

        public static void ResponseJson(HttpResponse Response)
        {
            Response.ContentType = "application/json";
            Response.Write(new JavaScriptSerializer().Serialize(null));////这个很关键，否则error  
            Response.End();
        }

        public static void ResponseJson(HttpResponse Response, string josn)
        {
            Response.ContentType = "application/json";
            Response.Write(josn);////这个很关键，否则error  
            Response.End();
        }

        public static void ResponseJson(HttpResponse Response, bool back)
        {
            Response.ContentType = "application/json";
            Response.Write(new JavaScriptSerializer().Serialize(back));////这个很关键，否则error  
            Response.End();
        }
        public static string ListToJson<T>(IList<T> modles)
        {
            return new JavaScriptSerializer().Serialize(modles);
        }
        public static void ResponseJson(HttpResponse Response, DataTable back)
        {
            Response.ContentType = "application/json";
            Response.Write(DatatableToJson.Dtb2Json(back));////这个很关键，否则error  
            Response.End();
        }

        public static void ResponseJson(HttpResponse Response, DataTable back, int pageIndex, int pageSize, int start)
        {
            Response.ContentType = "application/json";
            Response.Write(DatatableToJson.Dtb2Json(back, pageIndex, pageSize, start));////这个很关键，否则error  
            Response.End();
        }

        public static List<T> JsonToList<T>(string json)
        {
            List<T> list = new List<T>();
            if (json.Length > 2 && json[0] != '[' && json[json.Length - 1] != ']')
            {//不是list模式的组装成list，一般就是单条记录。
                json = "[" + json + "]";
            }
            JavaScriptSerializer js = new JavaScriptSerializer();
            list = js.Deserialize(json, typeof(List<T>)) as List<T>;
            return list;
        }

        public static T JsonToModel<T>(string json)
        {
            //T t = (T)Activator.CreateInstance(typeof(T));
            JavaScriptSerializer js = new JavaScriptSerializer();
            return (T)js.Deserialize(json, typeof(T));
        }

        public static string ModelToJson<T>(object model)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.MissingMemberHandling = MissingMemberHandling.Ignore;
            return JsonConvert.SerializeObject(model, Formatting.Indented, settings);

        }

        public static void ResponseJson<T>(HttpResponse Response, IList<T> list)
        {
            Response.ContentType = "application/json";
            Response.Write(new JavaScriptSerializer().Serialize(list));////这个很关键，否则error  
            Response.End();
        }

        public static void ResponseJsonWithTotal<T>(HttpResponse Response, IList<T> list, long rows)
        {
            Response.ContentType = "application/json";
            string ret = "{\"total\":";
            ret += rows.ToString();
            ret += ",\"mode\":";
            ret += new JavaScriptSerializer().Serialize(list);
            ret += "}";
            Response.Write(ret);////这个很关键，否则error  
            Response.End();
        }

        public static void ResponseJsonWithTotal(HttpResponse Response, DataTable dt, long rows)
        {
            Response.ContentType = "application/json";
            string ret = "{\"total\":";
            ret += rows.ToString();
            ret += ",\"mode\":";
            ret += DatatableToJson.Dtb2Json(dt);
            ret += "}";
            Response.Write(ret);////这个很关键，否则error  
            Response.End();
        }


        public static System.DateTime ConvertIntDateTime(double d)
        {
            System.DateTime time = System.DateTime.MinValue;
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            time = startTime.AddSeconds(d);
            return time;
        }
        /// <summary>
        /// 将c# DateTime时间格式转换为Unix时间戳格式
        /// </summary>
        /// <param name="time">时间</param>
        /// <returns>double</returns>
        public static double ConvertDateTimeInt(System.DateTime time)
        {
            double intResult = 0;
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            intResult = (time - startTime).TotalSeconds;
            return intResult;
        }


        /// <summary>
        /// HttpPost
        /// </summary>
        /// <param name="Url"></param>
        /// <param name="postDataStr"></param>
        /// <param name="cookie"></param>
        /// <returns></returns>
        private static string HttpPost(string Url, string postDataStr, CookieContainer cookie)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = Encoding.UTF8.GetByteCount(postDataStr);
            request.CookieContainer = cookie;
            Stream myRequestStream = request.GetRequestStream();
            StreamWriter myStreamWriter = new StreamWriter(myRequestStream, Encoding.GetEncoding("gb2312"));
            myStreamWriter.Write(postDataStr);
            myStreamWriter.Close();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            response.Cookies = cookie.GetCookies(response.ResponseUri);
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }

        public static string HttpGet(string Url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }


        public static string HashtableToJson(Hashtable hr)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            if (hr.Count > 0)
            {
                foreach (DictionaryEntry item in hr)
                {
                    if (item.Value != null)
                    {
                        sb.Append("\"" + item.Key + "\":");
                        sb.Append("\"" + item.Value + "\"");
                        sb.Append(",");
                    }
                }
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append("}");

            return sb.ToString();

        }

        #region DataTable分页
        /// <summary>
        /// DataTable分页
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="PageIndex">页索引,注意：从1开始</param>
        /// <param name="PageSize">每页大小</param>
        /// <returns>分好页的DataTable数据</returns> 
        public static DataTable GetPagedTable(DataTable dt, int PageIndex, int PageSize)
        {
            if (PageIndex == 0) { return dt; }
            DataTable newdt = dt.Copy();
            newdt.Clear();
            int rowbegin = (PageIndex - 1) * PageSize;
            int rowend = PageIndex * PageSize;

            if (rowbegin >= dt.Rows.Count)
            { return newdt; }

            if (rowend > dt.Rows.Count)
            { rowend = dt.Rows.Count; }
            for (int i = rowbegin; i <= rowend - 1; i++)
            {
                DataRow newdr = newdt.NewRow();
                DataRow dr = dt.Rows[i];
                foreach (DataColumn column in dt.Columns)
                {
                    newdr[column.ColumnName] = dr[column.ColumnName];
                }
                newdt.Rows.Add(newdr);
            }
            return newdt;
        }

        /// <summary>
        /// 返回分页的页数
        /// </summary>
        /// <param name="count">总条数</param>
        /// <param name="pageye">每页显示多少条</param>
        /// <returns>如果 结尾为0：则返回1</returns>
        public static int PageCount(int count, int pageye)
        {
            int page = 0;
            int sesepage = pageye;
            if (count % sesepage == 0) { page = count / sesepage; }
            else { page = (count / sesepage) + 1; }
            if (page == 0) { page += 1; }
            return page;
        }
        #endregion

#if 返回页面Tree


        public static string GetCataHtml(string muCode)
        {
            StringBuilder sb = new StringBuilder();

            IList<EasyGoModel.Sys.SYS_MODULE> mods = GlobalVar.GetCurCata();
            if (mods == null || mods.Count == 0)
            {
                return String.Empty;
            }
            var firstL = mods.Where(p => p.PARENTID == 0);
            if (firstL.Count() == 0)
            {
                return String.Empty;
            }
            SYS_MODULE curMod = mods.Where(p => p.MODCODE == muCode).First();
            if (firstL != null && firstL.Count() > 0)
            {
                foreach (var item in firstL)
                {
                    if (item.OID == curMod.PARENTID)
                    {
                        sb.Append("<li class='active'>");
                    }
                    else
                    {
                        sb.Append("<li>");
                    }
                    sb.Append("<a href='" + item.MODURL + "'><i class='" + item.HEADERCLASS + "'></i> <span class='nav-label'>" + item.MODNAME + "</span></a>");
                    var secondL = mods.Where(p => p.PARENTID == item.OID);
                    if (secondL != null && secondL.Count() > 0)
                    {
                        sb.Append("<ul class='nav nav-second-level collapse'>");
                        foreach (var item2 in secondL)
                        {
                            if (item2.OID == curMod.OID)
                            {
                                sb.Append("<li class='active'>");
                            }
                            else
                            {
                                sb.Append("<li>");
                            }
                            sb.Append("<a href=" + item2.MODURL + ">" + item2.MODNAME + "</a>");
                            sb.Append("</li>");
                        }
                        sb.Append("</ul>");
                    }
                    sb.Append("</li>");
                }
            }
            return sb.ToString();
        }
#endif
    }
}
