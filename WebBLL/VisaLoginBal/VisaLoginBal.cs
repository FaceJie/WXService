using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebDAL;
using WebModel;

namespace WebBLL.VisaLoginBal
{
    public class VisaLoginBal
    {
        VisaLoginDal dal = new VisaLoginDal();

        //1
        public VisaLoginModel UserExsit(string userNikeName)
        {
            return dal.UserExsit(userNikeName);
        }

        public bool Checking(string userName, string pwd)
        {
            return dal.Checking(userName,pwd);
        }
        //2
        public string UserRegister(string openid,string nickName, string avatarUrl, string userName, string userType, string userTlp, string pwd)
        {
            return dal.UserRegister(openid,nickName, avatarUrl,userName,userType, userTlp,pwd);
        }

        public VisaLoginModel UserLogin(string openid)
        {
            return dal.UserLogin(openid);
        }

        public string GetIPRecord(string mobile, string ShowIp)
        {
            return dal.GetIPRecord(mobile, ShowIp);
        }

        public string BindMobilPhone(string userId, string tel)
        {
            return dal.BindMobilPhone(userId, tel);
        }

        public bool CheckUserName(string userName, string userType)
        {
            return dal.CheckUserName(userName, userType);
        }
    }
}
