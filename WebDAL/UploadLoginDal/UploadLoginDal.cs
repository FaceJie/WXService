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
    public class UploadLoginDal
    {
        public UploadUserModel UserExsit(object userNikeName)
        {
            string sql = "select * from UploadUser where userNikeName=@userNikeName";
            SqlParameter[] pars ={
                     new SqlParameter("@userNikeName",userNikeName)
                                };
            DataTable dt = SqlHelper.GetTableText(sql, pars)[0];
            UploadUserModel uploadUserModel = null;
            if (dt != null && dt.Rows.Count > 0)
            {
                uploadUserModel = new UploadUserModel();
                loadEntity(dt.Rows[0], uploadUserModel);
            }
            return uploadUserModel;
        }

        public string UserRegister(string nickName, string avatarUrl, string groupNo, string userName, string userTlp)
        {
            try
            {
                string sql = "insert into UploadUser(groupId,userType,UserHeader,userName,userTlp,userNikeName) output inserted.userId  values(@groupId,@userType,@UserHeader,@userName,@userTlp,@userNikeName)";
                SqlParameter[] pars ={
                     new SqlParameter("@groupId",groupNo),
                     new SqlParameter("@userType",3),
                     new SqlParameter("@UserHeader",avatarUrl),
                     new SqlParameter("@userName",userName),
                     new SqlParameter("@userTlp",userTlp),
                     new SqlParameter("@userNikeName",nickName)
             };
                string result = SqlHelper.ExecuteScalarText(sql, pars).ToString();
                if (!string.IsNullOrEmpty(result))
                {
                    string sqllog = "insert into UploadUserLog(userName, loginTime, remark, actionName) values ('" + userName + "','" + DateTime.Now + "','登陆成功！','注册人员登录！')";
                    SqlHelper.ExecteNonQueryText(sqllog, null);
                    return result;
                }
                else
                {
                    string sqllog = "insert into UploadUserLog(userName, loginTime, remark, actionName) values ('" + userName + "','" + DateTime.Now + "','登陆失败！','注册人员登录！')";
                    SqlHelper.ExecteNonQueryText(sqllog, null);
                    return "";
                }
            }
            catch (Exception)
            {
                
                return "erorr";
            }
        }

        public void loadEntity(DataRow row, UploadUserModel model)
        {
            model.UserId = row["userId"] != DBNull.Value ? row["userId"].ToString() : string.Empty;
            model.GroupId = row["groupId"] != DBNull.Value ? row["groupId"].ToString() : string.Empty;
            model.UserHeader = row["userHeader"] != DBNull.Value ? row["userHeader"].ToString() : string.Empty;
            model.UserType = row["userType"] != DBNull.Value ? row["userType"].ToString() : string.Empty;
            model.UserName = row["userName"] != DBNull.Value ? row["userName"].ToString() : string.Empty;
            model.UserTlp = row["userTlp"] != DBNull.Value ? row["userTlp"].ToString() : string.Empty;
            model.UserNikeName = row["userNikeName"] != DBNull.Value ? row["userNikeName"].ToString() : string.Empty;
            if (Convert.ToDateTime(row["entryTime"])!=null)
            {
                model.EntryTime = Convert.ToDateTime(row["entryTime"]);
            }
           
        }
    }
}
