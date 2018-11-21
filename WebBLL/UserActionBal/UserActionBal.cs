using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebDAL;

namespace WebBLL
{
    public class UserActionBal
    {
        UserActionDal dal = new UserActionDal();

        public DataTable GetMenuAction(string userType)
        {
            return dal.GetMenuAction(userType);
        }
        public DataTable GetFileAction(string groupId)
        {
            return dal.GetFileAction(groupId);
        }
        public DataTable GetUserInfo(string userId)
        {
            return dal.GetUserInfo(userId);
        }

        public DataTable GetMyselfPublish(string groupId, string userName)
        {
            return dal.GetMyselfPublish(groupId,userName);
        }

        public DataTable GetfilterList()
        {
            return dal.GetfilterList();
        }

        public bool updateDisPlay(string workId)
        {
            return dal.updateDisPlay(workId);
        }

        public DataTable LifeList()
        {
            return dal.LifeList();
        }
    }
}
