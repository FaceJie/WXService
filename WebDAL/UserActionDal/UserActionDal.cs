using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebHelper;

namespace WebDAL
{
    public class UserActionDal
    {
        //获取权限列表
        public DataTable GetMenuAction(string userType)
        {
            string sql = string.Format(@"select a.userType,a.actionId,b.actionName,b.actionUrl,b.actionImage 
                                         from dbo.UserAction as a
	                                          inner join dbo.ControllerAction as b 
	                                     on a.actionId=b.actionId
                                         where userType='{0}'
                                         order by b.older", userType);
            DataTable dt = SqlHelper.GetTableText(sql, null)[0];
            return dt;
        }
        //获取最新发布------完成
        public DataTable GetFileAction(string groupId)
        {
            string sql = string.Format(@"select * from dbo.UploadFilesData
                                         where groupId='{0}' and display=1 and uploadType=1 order by uploadTime desc", groupId);
            DataTable dt = SqlHelper.GetTableText(sql, null)[0];
            return dt;
        }

        public DataTable GetUserInfo(string userId)
        {
            string sql = string.Format(@"select userHeader,userType,userName,groupId from dbo.UploadUser where userId='{0}'", userId);
            DataTable dt = SqlHelper.GetTableText(sql, null)[0];
            return dt;
        }

        public DataTable GetMyselfPublish(string groupId, string userName)
        {
            string sql = string.Format(@"select * from dbo.UploadFilesData where groupId='{0}' and userName='{1}' order by uploadTime desc", groupId, userName);
            DataTable dt = SqlHelper.GetTableText(sql, null)[0];
            return dt;
        }
        //获取动态--------完成
        public DataTable LifeList()
        {
            string sql = string.Format(@"select * from dbo.UploadFilesData where uploadType=2 and display=1 order by uploadTime desc");
            DataTable dt = SqlHelper.GetTableText(sql, null)[0];
            return dt;
        }

        //查询审核列表----完成
        public DataTable GetfilterList()
        {
            string sql = string.Format(@"select * from dbo.UploadFilesData where display=0 order by uploadTime desc"); ;
            DataTable dt = SqlHelper.GetTableText(sql, null)[0];
            return dt;
        }
        public bool updateDisPlay(string workId)
        {
            string sql = string.Format(@"update dbo.UploadFilesData set display=1 where workId='{0}'", workId);
            return SqlHelper.ExecteNonQueryText(sql, null) == 1 ? true : false;
        }
    }
}
