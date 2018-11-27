using System;
using System.Data;
using WebDAL;
using WebModel;

namespace WebBLL
{
    public class ErorrOrder
    {
        ErorrOrderDal dal = new ErorrOrderDal();
        public int UpAction(string userId, string type)
        {
            return dal.UpAction(userId,type);
        }

        public DataTable GetAction()
        {
            return dal.GetAction();
        }
        //表格数据
        public DataTable GetOrderInfo(TableOrgModel tableOrgModel)
        {
            return dal.GetOrderInfo(tableOrgModel);
        }

        public int UpErorrData(string transportId, string erorrData)
        {
            return dal.UpErorrData(transportId, erorrData);
        }
    }
}