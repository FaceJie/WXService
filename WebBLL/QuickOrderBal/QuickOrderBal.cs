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
    public class QuickOrderBal
    {
        QuickOrderDal dal = new QuickOrderDal();
        public DataTable QuickSendData(string sUserId)
        {

            return dal.QuickSendData(sUserId);
        }

        public int SaveQuickSendData(QuickSendDataViewModel quickSendDataViewModel)
        {
            return dal.SaveQuickSendData(quickSendDataViewModel);
        }

        public DataTable GetWaitOrderList(string userId)
        {
            return dal.GetWaitOrderList(userId);
        }

        public DataTable GetCompleteOrderList(string userId)
        {
            return dal.GetCompleteOrderList(userId);
        }

        public int UpdateStatus(string transportId, string startPosition, string clickOlder, UnitsModel unitsModel)
        {
            return dal.UpdateStatus(transportId, startPosition, clickOlder, unitsModel);
        }

        public int SavaLocationAll(string userId, string transportId, string postion)
        {
            return dal.SavaLocationAll(userId, transportId, postion);
        }

        public DataTable GetDetailInfo(string transportId)
        {
            return dal.GetDetailInfo(transportId);
        }

        public DataTable userAddressManager(string userId, string positionAddress, string position)
        {
            return dal.userAddressManager(userId, positionAddress, position);
        }

        public int SaveNextPosition(string optionTransportNextId, string position, UnitsModel unitsModel)
        {
            return dal.SaveNextPosition(optionTransportNextId,position,unitsModel);
        }

        public DataTable GetReallDetailInfo(string transportId)
        {
            return dal.GetReallDetailInfo(transportId);
        }

        public int ShurtOrder(UnitsModel unitsModel,string  userId,string userName)
        {
            return dal.ShurtOrder(unitsModel, userId, userName);
        }

        public DataTable ClientDoOrderList(string userId)
        {
            return dal.ClientDoOrderList(userId);
        }

        public DataTable GetOrderByUserIdDetails(string userId, string transportId)
        {
            return dal.GetOoderByUserDetails(userId,transportId);
        }

        public int KillOrder(UnitsModel unitsModel, string optionTransportId, string userId, string userName)
        {
            return dal.KillOrder(unitsModel, optionTransportId,userId, userName);
        }

        public DataTable ClientCompleteOrderList(string userId)
        {
            return dal.ClientCompleteOrderList(userId);
        }
    }
}
