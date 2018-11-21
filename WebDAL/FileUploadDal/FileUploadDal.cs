using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebHelper;
using WebModel;

namespace WebDAL
{
    public class FileUploadDal
    {
        public int UploadImgSave(UploadFilesData uploadObj)
        {
            string sql = "insert into UploadFilesData(userName,userHeader,remark,uploadId,groupId,uploadType,uploadTime,display,uploadTitle) values(@userName,@userHeader,@remark,@uploadId,@groupId,@uploadType,@uploadTime,@display,@uploadTitle)";
            SqlParameter[] para = new SqlParameter[] {
                new SqlParameter("@userName",uploadObj.UserName),
                new SqlParameter("@userHeader",uploadObj.UserHeader),
                new SqlParameter("@uploadId",uploadObj.UploadId),
                new SqlParameter("@remark",uploadObj.Remark),
                new SqlParameter("@groupId",uploadObj.GroupId),
                new SqlParameter("@uploadType",uploadObj.UploadType),
                new SqlParameter("@uploadTime",uploadObj.UploadTime),
                new SqlParameter("@display",uploadObj.Display),
                 new SqlParameter("@uploadTitle",uploadObj.UploadTitle),
            };
            return SqlHelper.ExecteNonQueryText(sql, para);

        }
        //判断是否可以上传动态
        public string GroupIdExsit(string userId)
        {
            string sql = string.Format(@"select groupId from dbo.UploadUser where userId='{0}'", userId);
            DataTable dt = SqlHelper.GetTableText(sql, null)[0];
            return dt.Rows[0]["groupId"].ToString();
        }

        public DataTable GetUserInfo(string userId)
        {
            string sql = string.Format(@"select * from dbo.UploadUser where userId='{0}'", userId);
            DataTable dt = SqlHelper.GetTableText(sql, null)[0];
            return dt;
        }

        public DataTable GetUploadId(string workId)
        {
            string sql = string.Format("select * from UploadFilesData where workId='{0}'", workId);
            return SqlHelper.GetTableText(sql, null)[0];
        }
    }
}
