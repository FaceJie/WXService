using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebHelper;

namespace WebDAL
{
    public class VisaInfoDal
    {
        //查询与名字相关的业务员以及送签与出签时间
        public DataTable GetVisaInfoParams(string serviceName, string sendDataTime, string passportNo, string country)
        {
            string sql = "SELECT GroupNo,PassportNo, RealTime, FinishTime, client, InTime, OutTime, Name, EnglishName, Sex, Birthday, PassportNo, EntryTime, Types FROM VisaInfoWeChatView WHERE Types = '个签'";
            List<SqlParameter> para = new List<SqlParameter>();
            if (!string.IsNullOrEmpty(serviceName))
            {
                sql += " and client=@client ";
                para.Add(new SqlParameter("@client", serviceName));
            }
            if (!string.IsNullOrEmpty(sendDataTime))
            {
                para.Add(new SqlParameter("@sendDataTime", sendDataTime));
                sql += " and RealTime=@sendDataTime ";
            }
            if (!string.IsNullOrEmpty(passportNo))
            {
                para.Add(new SqlParameter("@PassportNo", passportNo));
                sql += " and PassportNo=@PassportNo ";
            }
            if (!string.IsNullOrEmpty(country))
            {
                para.Add(new SqlParameter("@Country", country));
                sql += " and Country=@Country ";
            }
            sql += " ORDER BY EntryTime DESC";
            return SqlHelper.GetTableTextByParams(sql, para)[0];
        }

        public string GetNameByPhone(string bindPhone)
        {
            string targetName = "";
            string sql = "select * from VisaLogin where userTlp=@userTlp";
            SqlParameter[] pars ={
                                    new SqlParameter("@userTlp",bindPhone)
                                };
            DataTable dt = SqlHelper.GetTableText(sql, pars)[0];
            if (dt.Rows.Count > 0 && dt != null)
            {
                targetName = dt.Rows[0]["userName"].ToString();
            }
            else
            {
                targetName = "";
            }
            return targetName;
        }
    }
}
