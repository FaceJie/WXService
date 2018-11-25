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
    public class QuickOrderDal
    {
        log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public DataTable QuickSendData(string sUserId)
        {
            string sql = string.Format("select userName,userTlp,position,addressName from dbo.VisaLogin where userId='{0}'", sUserId);
            DataTable dt = SqlHelper.GetTableText(sql, null)[0];
            return dt;
        }

        public int SaveQuickSendData(QuickSendDataViewModel quickSendDataViewModel)
        {
            int result = 0;
            string sql = "insert into QuickOrder(transportId,oUserId,oName,oPhone,sCreateTime,sName,sPhone,sAddress,sPositionSet,rName,rPhone,rAddress,rPositionSet,orderStatus) values(@transportId,@oUserId,@oName,@oPhone,@sCreateTime,@sName,@sPhone,@sAddress,@sPositionSet,@rName,@rPhone,@rAddress,@rPositionSet,@orderStatus)";
            SqlParameter[] para = new SqlParameter[] {
                new SqlParameter("@transportId",quickSendDataViewModel.transportId),
                new SqlParameter("@oUserId",quickSendDataViewModel.oUserId),
                new SqlParameter("@oName",quickSendDataViewModel.oName),
                new SqlParameter("@oPhone",quickSendDataViewModel.oPhone),
                new SqlParameter("@sCreateTime", Convert.ToDateTime(quickSendDataViewModel.sCreateTime)),
                new SqlParameter("@sName",quickSendDataViewModel.sName),
                new SqlParameter("@sPhone",quickSendDataViewModel.sPhone),
                new SqlParameter("@sAddress",quickSendDataViewModel.sAddress),
                new SqlParameter("@sPositionSet",quickSendDataViewModel.sPositionSet),
                new SqlParameter("@rName",quickSendDataViewModel.rName),
                new SqlParameter("@rPhone",quickSendDataViewModel.rPhone),
                new SqlParameter("@rAddress",quickSendDataViewModel.rAddress),
                new SqlParameter("@rPositionSet",quickSendDataViewModel.rPositionSet),
                new SqlParameter("@orderStatus", "待接单"),
            };
            try
            {
                result= SqlHelper.ExecteNonQueryText(sql, para);
                LogModelSet ls = new LogModelSet(new LogModel(quickSendDataViewModel.oUserId, quickSendDataViewModel.oName, quickSendDataViewModel.oName+"发起一条资料收取请求，订单号：" + quickSendDataViewModel.transportId, 1), log);
            }
            catch (Exception ex)
            {
                LogModelSet ls = new LogModelSet(new LogModel(quickSendDataViewModel.oUserId, quickSendDataViewModel.oName, "插入订单时："+ex.ToString(), 3), log);
            }
            return result;
        }

       

        public int KillOrder(UnitsModel unitsModel, string optionTransportId, string userId, string userName)
        {
            int i = 0;
            string sql = "update dbo.QuickOrder set recommendedDistance=@recommendedDistance,recommendedTime=@recommendedTime,recommendedRoute=@recommendedRoute,reallyDistance=@reallyDistance,orderReallyTime=@orderReallyTime,orderStatus=@orderStatus,lCompleteTime=@lCompleteTime where transportId = @transportId";
            SqlParameter[] para = new SqlParameter[] {
                 new SqlParameter("@recommendedDistance",unitsModel.recommendedDistance),
                new SqlParameter("@recommendedTime",unitsModel.recommendedTime),
                 new SqlParameter("@recommendedRoute",unitsModel.recommendedRoute),
                new SqlParameter("@reallyDistance",unitsModel.reallyDistance),
                new SqlParameter("@orderReallyTime",unitsModel.orderReallyTime),
                new SqlParameter("@orderStatus","已完成"),
                new SqlParameter("@lCompleteTime",DateTime.Now),
                new SqlParameter("@transportId",unitsModel.transportId)
            };
            try
            {
                DataTable dt = GetReallDetailInfo(optionTransportId);
                i = SqlHelper.ExecteNonQueryText(sql, para);
                //插入
                sql = string.Format("insert into dbo.TravelDistance (transportId,positionSet) values('{0}','{1}')", unitsModel.transportId, dt.Rows[0]["positionSet"].ToString());
                i = SqlHelper.ExecteNonQueryText(sql, null);
                //删除
                sql = string.Format("delete from dbo.TravelDistance where transportId = '{0}'", optionTransportId);
                i = SqlHelper.ExecteNonQueryText(sql, null);
                ClientReciveOrder(userId);
            }
            catch (Exception ex)
            {
                i = 0;
                LogModelSet ls = new LogModelSet(new LogModel(userId, userName, "结束订单" + optionTransportId + "时：" + ex.ToString(), 3), log);
            }
            return i;
        }

        public DataTable GetOoderByUserDetails(string userId, string transportId)
        {
            string sql = string.Format("select a.*,b.positionSet from dbo.QuickOrder as a inner join dbo.TravelDistance as b on a.transportId=b.transportId where a.oUserId='{0}' and a.transportId='{1}'", userId, transportId);
            DataTable dt = SqlHelper.GetTableText(sql, null)[0];
            return dt;
        }

        public DataTable ClientDoOrderList(string userId)
        {
            string sql = string.Format("select * from dbo.QuickOrder where oUserId='{0}' and orderStatus<>'已完成'", userId);
            DataTable dt = SqlHelper.GetTableText(sql, null)[0];
            return dt;
        }
        public DataTable ClientCompleteOrderList(string userId)
        {
            string sql = string.Format("select * from dbo.QuickOrder where oUserId='{0}' and orderStatus='已完成'", userId);
            DataTable dt = SqlHelper.GetTableText(sql, null)[0];
            return dt;
        }
        //结束订单的逻辑
        public int ShurtOrder(UnitsModel unitsModel,string userId,string userName)
        {
            int i = 0;
            string sql = "update dbo.QuickOrder set recommendedDistance=@recommendedDistance,recommendedTime=@recommendedTime,recommendedRoute=@recommendedRoute,reallyDistance=@reallyDistance,orderReallyTime=@orderReallyTime,orderStatus=@orderStatus,lCompleteTime=@lCompleteTime where transportId = @transportId";
            SqlParameter[] para = new SqlParameter[] {
                 new SqlParameter("@recommendedDistance",unitsModel.recommendedDistance),
                new SqlParameter("@recommendedTime",unitsModel.recommendedTime),
                new SqlParameter("@recommendedRoute",unitsModel.recommendedRoute),
                new SqlParameter("@reallyDistance",unitsModel.reallyDistance),
                new SqlParameter("@orderReallyTime",unitsModel.orderReallyTime),
                new SqlParameter("@orderStatus","已完成"),
                new SqlParameter("@lCompleteTime",DateTime.Now),
                new SqlParameter("@transportId",unitsModel.transportId)
            };
            try
            {
                i = SqlHelper.ExecteNonQueryText(sql, para);
                ClientReciveOrder(userId);
             
            }
            catch (Exception ex)
            {
                i = 0;
                LogModelSet ls = new LogModelSet(new LogModel(userId, userName, "结束订单" + unitsModel.transportId + "时：" + ex.ToString(), 3), log);
            }
            return i;
        }
        //结束订单

        public int ClientReciveOrder(string userId)
        {
            int count = 0;
            try
            {
                string sql = string.Format("select * from dbo.OrderWaitData where logicsUserId='{0}' and orderStatus='待接单'", userId);
                DataTable dt_new = SqlHelper.GetTableText(sql, null)[0];
                string ids = "";
                int completeCount = 0;
                for (int i = 0; i < dt_new.Rows.Count; i++)
                {
                    ids = dt_new.Rows[i]["orderIds"].ToString().TrimEnd(',').Replace(",", "','"); ;
                    string sql_new = string.Format("select orderId,lUserId,orderStatus from dbo.QuickOrder where lUserId='{0}' and orderId in('{1}')", userId, ids);
                    DataTable dt_statusCheck = SqlHelper.GetTableText(sql_new, null)[0];
                    for (int j = 0; j < dt_statusCheck.Rows.Count; j++)
                    {
                        if (dt_statusCheck.Rows[j]["orderStatus"].ToString() != "已完成")
                        {
                            completeCount++;
                            break;
                        }

                    }
                    if (completeCount == 0)
                    {
                        sql = string.Format("update dbo.OrderWaitData set orderStatus='已完成' where logicsUserId ='{0}' and orderIds='{1}'", userId, dt_new.Rows[i]["orderIds"].ToString());
                        count = SqlHelper.ExecteNonQueryText(sql, null);
                    }
                }
            }
            catch (Exception ex)
            {
                count = 0;
            }
            return count;
        }

        public DataTable GetReallDetailInfo(string targetTransportId)
        {
            string sql = string.Format("select * from TravelDistance where transportId in('{0}')", targetTransportId);
            return SqlHelper.GetTableText(sql, null)[0];
        }

        public int SaveNextPosition(string optionTransportNextId, string position, UnitsModel unitsModel)
        {
            string sql = string.Format("update dbo.QuickOrder set rPositionSet='{0}',recommendedDistance='{1}',recommendedTime='{2}',recommendedRoute='{3}' where transportId='{4}'", position, unitsModel.recommendedDistance, unitsModel.recommendedTime, unitsModel.recommendedRoute, optionTransportNextId);
            int count = SqlHelper.ExecteNonQueryText(sql, null);
            return count;
        }

        public DataTable userAddressManager(string userId, string positionAddress, string position)
        {
            string sql = string.Format("update dbo.VisaLogin set position='{0}',addressName='{1}' OUTPUT  Inserted.position,Inserted.addressName,Inserted.userName,Inserted.userTlp where  userId='{2}'", position, positionAddress, userId);
            DataTable dt = SqlHelper.GetTableText(sql, null)[0];
            return dt;
        }

        public int UpdateStatus(string transportId, string startPosition, string clickOlder, UnitsModel unitsModel)
        {
            int count = 0;
            try
            {
                string sql = string.Format("update dbo.QuickOrder set orderStatus='结束',lReceiveTime='{0}',lPositionSet='{1}',older='{2}',recommendedDistance='{3}',recommendedTime='{4}',recommendedRoute='{5}' where transportId='{6}'", DateTime.Now, startPosition, clickOlder, unitsModel.recommendedDistance, unitsModel.recommendedTime, unitsModel.recommendedRoute, transportId);
                count = SqlHelper.ExecteNonQueryText(sql, null);
            }
            catch (Exception)
            {
                count = 0;
            }
            return count;
        }


        public int SavaLocationAll(string userId, string transportId, string postion)
        {
            string sql = "";
            int result = 0;
            try
            {
                DataTable dt = isExsitOrder(transportId);
                if (dt != null && dt.Rows.Count > 0)
                {
                    sql = string.Format("update TravelDistance set positionSet=positionSet+'{0}' where transportId='{1}'", postion, transportId);
                    result = SqlHelper.ExecteNonQueryText(sql, null);
                }
                else
                {
                    sql = string.Format("insert into TravelDistance(transportId,positionSet) values('{0}','{1}')", transportId, postion);
                    result = SqlHelper.ExecteNonQueryText(sql, null);
                }
            }
            catch (Exception)
            {
                result = -1;
            }
            return result;
        }

        private DataTable isExsitOrder(string transportId)
        {
            string sql = string.Format("select transportId from TravelDistance where transportId='{0}'", transportId);
            DataTable dt = SqlHelper.GetTableText(sql, null)[0];
            return dt;
        }

        public DataTable GetDetailInfo(string transportId)
        {
            string sql = string.Format("select * from dbo.QuickOrder where transportId ='{0}'", transportId);
            return SqlHelper.GetTableText(sql, null)[0];
        }

        public DataTable GetWaitOrderList(string userId)
        {
            string ids = "";
            DataTable dt_new = new DataTable();
            string sql = string.Format("select * from dbo.OrderWaitData where logicsUserId='{0}' and orderStatus='待接单'", userId);
            DataTable dt = SqlHelper.GetTableText(sql, null)[0];
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ids += dt.Rows[i]["orderIds"].ToString() + ",";
                }
                ids = ids.TrimEnd(',').Replace(",", "','");
                string sql_new = string.Format("select * from dbo.QuickOrder where lUserId='{0}' and orderStatus<>'已完成' and orderId in('{1}')", userId, ids);
                dt_new = SqlHelper.GetTableText(sql_new, null)[0];
            }
            return dt_new;
        }
        public DataTable GetCompleteOrderList(string userId)
        {
            string ids = "";
            DataTable dt_new = new DataTable();
            string sql = string.Format("select * from dbo.OrderWaitData where logicsUserId='{0}'", userId);
            DataTable dt = SqlHelper.GetTableText(sql, null)[0];
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ids += dt.Rows[i]["orderIds"].ToString() + ",";
                }
                ids = ids.TrimEnd(',').Replace(",", "','");
                string sql_new = string.Format("select * from dbo.QuickOrder where lUserId='{0}' and orderStatus='已完成' and orderId in('{1}')", userId, ids);
                dt_new = SqlHelper.GetTableText(sql_new, null)[0];
            }
            return dt_new;
        }
    }
}
