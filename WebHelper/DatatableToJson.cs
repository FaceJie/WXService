using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace WebHelper
{
    public static class DatatableToJson
    {
        /// <summary>
        /// DataTable转Json
        /// </summary>
        /// <param name="dtb"></param>
        /// <returns></returns>
        public static string Dtb2Json(DataTable dtb)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            jss.MaxJsonLength = Int32.MaxValue;
            ArrayList dic = new ArrayList();
            foreach (DataRow row in dtb.Rows)
            {
                Dictionary<string, object> drow = new Dictionary<string, object>();
                foreach (DataColumn col in dtb.Columns)
                {
                    drow.Add(col.ColumnName, row[col.ColumnName]);
                }
                dic.Add(drow);
            }
            return jss.Serialize(dic);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dtb"></param>
        /// <returns></returns>
        public static DataTable RowsToDataTable(DataRow[] rows)
        {
            if (rows == null || rows.Length == 0) return null;
            DataTable tmp = rows[0].Table.Clone();
            foreach (DataRow row in rows)
                tmp.Rows.Add(row.ItemArray);
            return tmp;
        }
        /// <summary>
        /// DataTable转Json并分页
        /// </summary>
        /// <param name="dtb"></param>
        /// <returns></returns>
        public static string Dtb2Json(DataTable dtb, int pageIndex, int pageSize, int start)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            ArrayList dic = new ArrayList();
            for (int i = start; i < pageIndex * pageSize; i++)
            {
                if (i >= dtb.Rows.Count) break;
                DataRow row = dtb.Rows[i];
                Dictionary<string, object> drow = new Dictionary<string, object>();
                foreach (DataColumn col in dtb.Columns)
                {
                    drow.Add(col.ColumnName, row[col.ColumnName]);
                }
                dic.Add(drow);

            }
            return jss.Serialize(dic);
        }
        /// <summary>
        /// Json转DataTable
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static DataTable Json2Dtb(string json)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            ArrayList dic = jss.Deserialize<ArrayList>(json);
            DataTable dtb = new DataTable();
            if (dic.Count > 0)
            {
                foreach (Dictionary<string, object> drow in dic)
                {
                    if (dtb.Columns.Count == 0)
                    {
                        foreach (string key in drow.Keys)
                        {
                            dtb.Columns.Add(key, drow[key].GetType());
                        }
                    }
                    DataRow row = dtb.NewRow();
                    foreach (string key in drow.Keys)
                    {
                        row[key] = drow[key];
                    }
                    dtb.Rows.Add(row);
                }
            }
            return dtb;
        }
    }
}
