using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebHelper;
using WebModel;

namespace WebDAL
{
    public class UploadDataDal
    {
        log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //测试可用
        public DataTable GetOrderList(string userId)
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
                string sql_new = string.Format("select * from dbo.DataOrder where logsticUserId='{0}' and orderStatus<>'已完成' and orderId in('{1}')", userId, ids);
                dt_new = SqlHelper.GetTableText(sql_new, null)[0];
            }
            return dt_new;
        }
        public DataTable GetLogiscOrderList(string userId)
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
                string sql_new = string.Format("select * from dbo.DataOrder where logsticUserId='{0}' and orderStatus='已完成' and orderId in('{1}')", userId, ids);
                dt_new = SqlHelper.GetTableText(sql_new, null)[0];
            }
            return dt_new;
        }
        //测试可用
        public int SaveSendDate(SendDataViewModel sendDataViewModel)
        {
            DateTime creatTime = Convert.ToDateTime(sendDataViewModel.sDateTime);//创建订单时间
            string sendPosition = sendDataViewModel.sPosition_longitude + "," + sendDataViewModel.sPosition_latitude;//经度和纬度
            string sql = "insert into DataOrder(clientUserId,clientName,clientTlp,visaInfoCountry,visaInfoNum,sendPosition,clientPosition,transportId,creatOrderTime,orderStatus,timeLimit,discription) values(@clientUserId,@clientName,@clientTlp,@visaInfoCountry,@visaInfoNum,@sendPosition,@clientPosition,@transportId,@creatOrderTime,@orderStatus,@timeLimit,@discription)";
            SqlParameter[] para = new SqlParameter[] {
                new SqlParameter("@clientUserId",sendDataViewModel.sUserId),
                new SqlParameter("@clientName",sendDataViewModel.sName),
                new SqlParameter("@clientTlp",sendDataViewModel.sPhone),
                new SqlParameter("@visaInfoCountry",sendDataViewModel.sCountry),
                new SqlParameter("@visaInfoNum",sendDataViewModel.sCount),
                new SqlParameter("@sendPosition",sendDataViewModel.sPosition),
                new SqlParameter("@clientPosition",sendPosition),
                new SqlParameter("@transportId",sendDataViewModel.sOrderId),
                new SqlParameter("@creatOrderTime",creatTime),
                new SqlParameter("@orderStatus","待接单"),
                new SqlParameter("@timeLimit",sendDataViewModel.sTimeLimit),
                new SqlParameter("@discription",sendDataViewModel.sDiscription)
            };
            try
            {
                LogModelSet ls = new LogModelSet(new LogModel(sendDataViewModel.sUserId, sendDataViewModel.sName, "发起一条资料收取请求，订单号："+ sendDataViewModel.sOrderId, 1), log);
            }
            catch (Exception ex)
            {
                LogModelSet ls = new LogModelSet(new LogModel(sendDataViewModel.sUserId, sendDataViewModel.sName, ex.ToString(), 3), log);
            }
            return SqlHelper.ExecteNonQueryText(sql, para);
        }

        public int SaveNextPosition(string optionTransportNextId, string postion, UnitsModel unitsModel)
        {
            string sql = string.Format("update dbo.DataOrder set logisticsPosition='{0}',recommendedDistance='{1}',recommendedTime='{2}',recommendedRoute='{3}' where transportId='{4}'", postion, unitsModel.recommendedDistance, unitsModel.recommendedTime, unitsModel.recommendedRoute, optionTransportNextId);
            int count = SqlHelper.ExecteNonQueryText(sql, null);
            return count;
        }

        public int ClientReciveOrder(string userId, string transportId)
        {
            int count = 0;
            try
            {
                string sql = string.Format("update dbo.DataOrder set orderStatus='已完成',orderCompleteTime='{0}' OUTPUT  Inserted.logsticUserId where transportId='{1}'", DateTime.Now, transportId);
                try
                {
                    LogModelSet ls = new LogModelSet(new LogModel(userId, "客户", "用户确认收取订单，订单号：" + transportId, 1), log);
                }
                catch (Exception ex)
                {
                    LogModelSet ls = new LogModelSet(new LogModel(userId, "客户", ex.ToString(), 3), log);
                }
                DataTable dt = SqlHelper.GetTableText(sql, null)[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    sql = string.Format("select * from dbo.OrderWaitData where logicsUserId='{0}' and orderStatus='待接单'", dt.Rows[0]["logsticUserId"].ToString());
                    DataTable dt_new = SqlHelper.GetTableText(sql, null)[0];
                    string ids = "";
                    int completeCount = 0;
                    for (int i = 0; i < dt_new.Rows.Count; i++)
                    {
                        ids = dt_new.Rows[i]["orderIds"].ToString().TrimEnd(',').Replace(",", "','"); ;
                        string sql_new = string.Format("select * from dbo.DataOrder where logsticUserId='{0}' and orderId in('{1}')", dt.Rows[0]["logsticUserId"].ToString(), ids);
                        DataTable dt_statusCheck = SqlHelper.GetTableText(sql_new, null)[0];
                        for (int j = 0; j < dt_statusCheck.Rows.Count; j++)
                        {
                            if (dt_statusCheck.Rows[j]["orderStatus"].ToString() != "已完成")
                            {
                                completeCount++;
                            }

                        }
                        if (completeCount == 0)
                        {
                            sql = string.Format("update dbo.OrderWaitData set orderStatus='已完成' where logicsUserId ='{0}' and orderIds='{1}'", dt.Rows[0]["logsticUserId"].ToString(), dt_new.Rows[i]["orderIds"].ToString());
                            count = SqlHelper.ExecteNonQueryText(sql, null);
                        }
                    }
                    count = 1;
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
        public DataTable GetDetailInfo(string targetTransportId)
        {
            string sql = string.Format("select * from dbo.DataOrder where transportId ='{0}'", targetTransportId);
            return SqlHelper.GetTableText(sql, null)[0];
        }

        //结束订单的逻辑
        public int ShurtOrder(UnitsModel unitsModel)
        {
          
            int i = 0;
            string sql = "update dbo.DataOrder set reallyDistance=@reallyDistance,orderReallyTime=@orderReallyTime,orderStatus=@orderStatus where transportId = @transportId";
            SqlParameter[] para = new SqlParameter[] {
                new SqlParameter("@reallyDistance",unitsModel.reallyDistance),
                new SqlParameter("@orderReallyTime",unitsModel.orderReallyTime),
                new SqlParameter("@orderStatus","待收货"),
                new SqlParameter("@transportId",unitsModel.transportId)
            };
            string uploadPath = "C:\\WebAPI\\Temp";
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }
            FileStream fs = new FileStream(uploadPath + "\\TestTxt1.txt", FileMode.Open, FileAccess.Write);
            StreamWriter sr = new StreamWriter(fs);
            sr.WriteLine(GlobalFuc.ModelToJson<UnitsModel>(unitsModel));//开始写入值
            sr.Close();
            fs.Close();
            try
            {
                i = SqlHelper.ExecteNonQueryText(sql, para);
                try
                {
                    LogModelSet ls = new LogModelSet(new LogModel(unitsModel.transportId, "后勤", "后勤结束了订单，订单号：" + unitsModel.transportId, 1), log);
                }
                catch (Exception ex)
                {
                    LogModelSet ls = new LogModelSet(new LogModel(unitsModel.transportId, "后勤", ex.ToString(), 3), log);
                }
            }
            catch (Exception ex)
            {
                i = 0;
            }
            return i;
        }

        public int UpdateStatus(string transportId, string startPosition, string clickOlder, UnitsModel unitsModel)
        {
            int count = 0;
            try
            {
                string sql = string.Format("update dbo.DataOrder set orderStatus='结束',receiveOrderTime='{0}',logisticsPosition='{1}',older='{2}',recommendedDistance='{3}',recommendedTime='{4}',recommendedRoute='{5}' where transportId='{6}'", DateTime.Now, startPosition, clickOlder, unitsModel.recommendedDistance, unitsModel.recommendedTime, unitsModel.recommendedRoute, transportId);
                count = SqlHelper.ExecteNonQueryText(sql, null);
            }
            catch (Exception)
            {
                count = 0;
            }
            return count;
        }

        public DataTable GetUserAddress(string userId)
        {
            string sql = string.Format("select * from dbo.VisaLogin where userId='{0}'", userId);
            DataTable dt = SqlHelper.GetTableText(sql, null)[0];
            return dt;
        }

        public DataTable userAddressManager(string userId, string positionAddress, string logiscPosition)
        {
            string sql = string.Format("update dbo.VisaLogin set position='{0}',addressName='{1}' OUTPUT  Inserted.position,Inserted.addressName,Inserted.userName,Inserted.userTlp where  userId='{2}'", logiscPosition, positionAddress, userId);
            DataTable dt = SqlHelper.GetTableText(sql, null)[0];
            return dt;
        }

        public DataTable GetCompleteOrderListByUserId(string userId)
        {
            string sql = string.Format("select * from dbo.DataOrder where clientUserId='{0}' and orderStatus= '已完成'", userId);
            DataTable dt = SqlHelper.GetTableText(sql, null)[0];
            return dt;
        }
        public DataTable GetOrderByUserId(string userId)
        {
            string sql = string.Format("select * from dbo.DataOrder where clientUserId='{0}' and orderStatus<> '已完成'", userId);
            DataTable dt = SqlHelper.GetTableText(sql, null)[0];
            return dt;
        }

        public DataTable GetOrderByUserIdDetails(string userId, string transportId)
        {
            string sql = string.Format("select a.*,b.positionSet from dbo.DataOrder as a inner join dbo.TravelDistance as b on a.transportId=b.transportId where clientUserId='{0}' and a.transportId='{1}'", userId, transportId);
            DataTable dt = SqlHelper.GetTableText(sql, null)[0];
            return dt;
        }

        //获取待确认的订单
        public DataTable GetCompleteOrderList(string transportId)
        {
            string sql = string.Format("select orderId,clientName,clientTlp,visaInfoCountry,visaInfoNum,sendPosition,clientPosition,transportId,discription from dbo.DataOrder where orderStatus='待收货' and transportId='{0}'", transportId);
            DataTable dt = SqlHelper.GetTableText(sql, null)[0];
            return dt;
        }
        public DataTable GetUserInfo(string userId)
        {
            string sql = string.Format("select userName,userTlp from dbo.VisaLogin where userId='{0}'", userId);
            DataTable dt = SqlHelper.GetTableText(sql, null)[0];
            return dt;
        }
        public DataTable isExsitOrder(string transportId)
        {
            string sql = string.Format("select transportId from TravelDistance where transportId='{0}'", transportId);
            DataTable dt = SqlHelper.GetTableText(sql, null)[0];
            return dt;
        }
        //已测试
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
                    result += SqlHelper.ExecteNonQueryText(sql, null);
                }
                else
                {
                    sql = string.Format("insert into TravelDistance(transportId,positionSet) values('{0}','{1}')", transportId, postion);
                    result += SqlHelper.ExecteNonQueryText(sql, null);
                }
            }
            catch (Exception)
            {
                result = -1;
            }
            return result;
        }
        /// <summary>
        /// 返回指定日期格式
        /// </summary>
        /// <param name="datetimestr">需要转换的时间</param>
        /// <param name="replacestr">指定格式</param>
        /// <returns>转换后的时间</returns>
        public static string GetDate(string datetimestr, string replacestr)
        {
            if (datetimestr == null) return replacestr;
            if (datetimestr.Equals("")) return replacestr;
            try
            {
                datetimestr = Convert.ToDateTime(datetimestr).ToString("yyyy-MM-dd").Replace("1900-01-01", replacestr);
            }
            catch
            {
                return replacestr;
            }
            return datetimestr;
        }
    }
}
