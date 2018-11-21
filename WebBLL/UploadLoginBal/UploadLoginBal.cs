using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebDAL;
using WebModel;

namespace WebBLL
{
    public class UploadLoginBal
    {
        UploadLoginDal dal = new UploadLoginDal();
        public UploadUserModel UserExsit(string userNikeName)
        {
            return dal.UserExsit(userNikeName);
        }


        public string UserRegister(string nickName, string avatarUrl, string groupNo, string userName, string userTlp)
        {
            return dal.UserRegister(nickName,avatarUrl, groupNo, userName,userTlp);
        }
    }
}
