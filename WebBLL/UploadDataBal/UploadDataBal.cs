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
    public class UploadDataBal
    {
        UploadDataDal dal = new UploadDataDal();
        public int SaveSendDate(SendDataViewModel sendDataViewModel)
        {
            return dal.SaveSendDate(sendDataViewModel);
        }
        public DataTable GetUserInfo(string userId)
        {
            return dal.GetUserInfo(userId);
        }
       
        public int CompleteDate(CompleteDataViewModel completeDataViewModel)
        {
            throw new NotImplementedException();
        }

        public int SavaLocationAll(string userId, string transportId, string postion)
        {
            return dal.SavaLocationAll(userId, transportId, postion);
        }

        public DataTable GetOrderList(string userId)
        {
            return dal.GetOrderList(userId);
        }
        public DataTable GetLogiscOrderList(string userId)
        {
            return dal.GetLogiscOrderList(userId);
        }
        
        public DataTable GetCompleteOrderList(string transportId)
        {
            return dal.GetCompleteOrderList(transportId);
        }
        public DataTable GetCompleteOrderListByUserId(string userId)
        {
            return dal.GetCompleteOrderListByUserId(userId);
        }
        public DataTable GetOrderByUserId(string userId)
        {
            return dal.GetOrderByUserId(userId);
        }

        public DataTable GetOrderByUserIdDetails(string userId, string transportId)
        {
            return dal.GetOrderByUserIdDetails(userId,transportId);
        }

        public DataTable userAddressManager(string userId,string positionAddress, string logiscPosition)
        {
            return dal.userAddressManager(userId, positionAddress, logiscPosition);
        }

        public DataTable GetUserAddress(string userId)
        {
            return dal.GetUserAddress(userId);
        }

        public int UpdateStatus( string transportId, string startPosition,string clickOlder, UnitsModel unitsModel)
        {
            return dal.UpdateStatus(transportId,startPosition, clickOlder, unitsModel);
            
        }

        public int ShurtOrder(UnitsModel unitsModel)
        {
            return dal.ShurtOrder(unitsModel);
        }

        public DataTable GetDetailInfo(string targetTransportId)
        {
            return dal.GetDetailInfo(targetTransportId);
        }

        public DataTable GetReallDetailInfo(string targetTransportId)
        {
            return dal.GetReallDetailInfo(targetTransportId);
        }

        public int ClientReciveOrder(string userId, string transportId)
        {
            return dal.ClientReciveOrder(userId, transportId);
        }

        public int SaveNextPosition(string optionTransportNextId, string postion, UnitsModel unitsModel)
        {
            return dal.SaveNextPosition(optionTransportNextId, postion, unitsModel);
        }
    }
}
