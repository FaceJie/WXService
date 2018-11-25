using System;
using System.Data;
using System.Data.SqlClient;
using WebHelper;
using WebModel;

namespace WebDAL
{
    public class FilesUpDal
    {
        public bool Save(FilesUpViewModel model)
        {
            string sql = "insert into NeedFiles (needId,country,discription) values(@needId,@country,@discription)";
            SqlParameter[] para = new SqlParameter[] {
                new SqlParameter("@needId",model.needId),
                new SqlParameter("@country",model.country),
                new SqlParameter("@discription",model.discription)
            };

            return SqlHelper.ExecteNonQueryText(sql, para) > 0 ? true : false;
        }

        public DataTable GetVisaTableList(string fileName)
        {
            string sql = "";
            if (string.IsNullOrEmpty(fileName))
            {
                sql = string.Format("select * from NeedFiles");
            }
            else
            {
                sql = string.Format("select * from NeedFiles where country like '%{0}%'", fileName);
            }

            DataTable dt = SqlHelper.GetTableText(sql, null)[0];
            return dt;
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

        public bool SaveName(FilesUpViewModel filesUpViewModel)
        {
            string sql = "";
            SqlParameter[] para = null;
            if (filesUpViewModel.fileImg == null)
            {
                sql = "update NeedFiles set fileName=@fileName,fileUrl=@fileUrl,fileType=@fileType where needId=@needId";
                para = new SqlParameter[] {
                new SqlParameter("@fileName",filesUpViewModel.fileName),
                new SqlParameter("@fileUrl",filesUpViewModel.fileUrl),
                new SqlParameter("@fileType",filesUpViewModel.fileType),
                new SqlParameter("@needId",filesUpViewModel.needId)
                };
            }
            else
            {
                sql = "update NeedFiles set fileImg=@fileImg where needId=@needId";
                para = new SqlParameter[] {
                new SqlParameter("@fileImg",filesUpViewModel.fileImg),
                new SqlParameter("@needId",filesUpViewModel.needId)
                };
            }
            return SqlHelper.ExecteNonQueryText(sql, para) > 0 ? true : false;
        }

        public DataTable GetVisaNeedInfo()
        {
            string sql = string.Format("select * from NeedFiles order by country");
            DataTable dt = SqlHelper.GetTableText(sql, null)[0];
            return dt;
        }
    }
}