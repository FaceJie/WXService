using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebDAL;
using WebModel;

namespace WebBLL
{
    public class FileUploadBal
    {
        FileUploadDal dal = new FileUploadDal();
        public int UploadImgSave(UploadFilesData uploadObj)
        {
            return dal.UploadImgSave(uploadObj);
        }

        public DataTable GetUploadId(string workId)
        {
            return dal.GetUploadId(workId);
        }

        public DataTable GetUserInfo(string userId)
        {
            return dal.GetUserInfo(userId);
        }

        public string GroupIdExsit(string userId)
        {
            return dal.GroupIdExsit(userId);
        }
    }
}
