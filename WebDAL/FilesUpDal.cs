using System;
using System.Data;
using System.Data.SqlClient;
using WebHelper;
using WebModel;

namespace WebDAL
{
    public class FilesUpDal
    {
        public bool Save(CountryVisaInfoNeed model)
        {
            string sql = "insert into CountryVisaInfoNeed(needId,needTableName,needTableUrl,needDemandName,needDemandUrl,needTableImgUrl,needDemandImgUrl,needCountry,needEnglishCountry) values(@needId,@needTableName,@needTableUrl,@needDemandName,@needDemandUrl,@needTableImgUrl,@needDemandImgUrl,@needCountry,@needEnglishCountry)";
            SqlParameter[] para = new SqlParameter[] {
                new SqlParameter("@needId",model.needId),
                new SqlParameter("@needTableName",model.needTableName),
                new SqlParameter("@needTableUrl",model.needTableUrl),
                new SqlParameter("@needDemandName",model.needDemandName),
                new SqlParameter("@needDemandUrl",model.needDemandUrl),
                 new SqlParameter("@needTableImgUrl",model.needTableImgUrl),
                new SqlParameter("@needDemandImgUrl",model.needDemandImgUrl),
                new SqlParameter("@needCountry",model.needCountry),
                new SqlParameter("@needEnglishCountry",model.needEnglishCountry)
            };

            return SqlHelper.ExecteNonQueryText(sql, para) > 0 ? true : false;
        }

        public DataTable GetCountry()
        {
            string sql = string.Format("select needCountry from CountryVisaInfoNeed");
            DataTable dt = SqlHelper.GetTableText(sql, null)[0];
            return dt;
        }
        //没用的方法
        public DataTable GetVisaNeedInfoByCountry(string countryName)
        {
            string sql = string.Format("select * from CountryVisaInfoNeed where countryName=countryName");
            DataTable dt = SqlHelper.GetTableText(sql, null)[0];
            return dt;
        }

        public bool SaveName(string fileReallyName,string tempNeedId, string where)
        {
            string sql = "";
            if (where=="needTableName")
            {
                sql = string.Format("update CountryVisaInfoNeed set needTableName='{0}' where needId='{1}'", fileReallyName, tempNeedId);
            }
            else
            {
                sql =string.Format("update CountryVisaInfoNeed set needDemandName='{0}' where needId='{1}'", fileReallyName, tempNeedId);
            }
            return SqlHelper.ExecteNonQueryText(sql, null) > 0 ? true : false;
        }

        public DataTable GetVisaNeedInfo()
        {
            string sql = string.Format("select * from CountryVisaInfoNeed order by needCountry");
            DataTable dt = SqlHelper.GetTableText(sql, null)[0];
            return dt;
        }
    }
}