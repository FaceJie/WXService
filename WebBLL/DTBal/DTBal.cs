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
    public class DTBal
    {
        DTDal dal = new DTDal();
        public DataTable GetAllOrderLocationData()
        {
            return dal.GetAllOrderLocationData();
        }

        public MsgModel CompleteOrderData(string logicsId, string orderDatas ,UserInfo user)
        {
            return dal.CompleteOrderData(logicsId, orderDatas, user);
        }

        public DataTable GetAllLogiscLocationData()
        {
            return dal.GetAllLogiscLocationData();
        }

        public DataTable Login(string username, string password)
        {
            return dal.Login(username,password);
        }

     
    }
}
