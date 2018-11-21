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
    public class DTDal
    {
        //获取所有订单信息
        public DataTable GetAllOrderLocationData()
        {
            string sql = string.Format("select * from dbo.QuickOrder");
            DataTable dt = SqlHelper.GetTableText(sql, null)[0];
            return dt;
        }

        public MsgModel CompleteOrderData(string logicsId, string orderDatas, UserInfo user)
        {
            MsgModel msg = new MsgModel();
            string sql = "insert into OrderWaitData(logicsUserId,orderIds,orderStatus,creatDate) values(@logicsUserId,@orderIds,@orderStatus,@creatDate)";
            SqlParameter[] para = new SqlParameter[] {
                new SqlParameter("@logicsUserId",logicsId),
                new SqlParameter("@orderIds", orderDatas.TrimEnd(',')),
                new SqlParameter("@orderStatus", "待接单"),
                new SqlParameter("@creatDate",DateTime.Now)
                };
            try
            {
                //开启事务，如遇到错误回滚,那就
                int result = SqlHelper.ExecteNonQueryText(sql, para);
                if (result > 0)
                {
                    ReciveDataViewModel reciveDataViewModel = GetLogiscData(logicsId);
                    if (reciveDataViewModel != null)
                    {
                        string ids =orderDatas.TrimEnd(',').Replace(",", "','") ;
                       
                        //更新订单得数据
                        string sql_update = string.Format("update dbo.QuickOrder set pUserId='{0}', pName='{1}',pPhone='{1}',pSendTime='{3}', lUserId='{4}',lName='{5}',lPhone='{6}',lAddress='{7}',lPositionSet='{8}',orderStatus='待处理' where orderId in ('{9}')", user.userId,user.userName,user.userTlp,DateTime.Now, logicsId, reciveDataViewModel.rName, reciveDataViewModel.rPhone, reciveDataViewModel.rAddress, reciveDataViewModel.rPosition, ids);
                        int result_update = SqlHelper.ExecteNonQueryText(sql_update, null);
                        if (result_update > 0)
                        {
                            msg.scu = true;
                            msg.msg = "派单成功";
                        }
                        else
                        {
                            msg.scu = false;
                            msg.msg = "更新订单失败";
                        }
                    }
                    else
                    {
                        msg.scu = false;
                        msg.msg = "后勤信息为空";
                    }
                }
                else
                {
                    msg.scu = false;
                    msg.msg = "插入订单失败";
                }
            }
            catch (Exception ex)
            {
                msg.scu = false;
                msg.msg = ex.Message.ToString();
            }
            return msg;
        }

        public DataTable Login(string username, string password)
        {
            string sql = string.Format("select * from dbo.VisaLogin where userName='{0}' and userTlp='{1}' and userType='0'", username, password);//0代表超级管理员
            DataTable dt = SqlHelper.GetTableText(sql, null)[0];
            return dt;
        }

        public DataTable GetAllLogiscLocationData()
        {
            string sql = string.Format("select * from dbo.VisaLogin where userType='3'");
            DataTable dt = SqlHelper.GetTableText(sql, null)[0];
            return dt;
        }

        //获取信息
        private ReciveDataViewModel GetLogiscData(string userId)
        {
            ReciveDataViewModel model = new ReciveDataViewModel();
            string sql = string.Format("select * from dbo.VisaLogin where userId='{0}'", userId);
            DataTable dt = SqlHelper.GetTableText(sql, null)[0];
            if (dt != null && dt.Rows.Count > 0)
            {
                model.rName = dt.Rows[0]["userName"] != DBNull.Value ? dt.Rows[0]["userName"].ToString() : string.Empty;
                model.rPhone = dt.Rows[0]["userTlp"] != DBNull.Value ? dt.Rows[0]["userTlp"].ToString() : string.Empty;
                model.rPosition = dt.Rows[0]["position"] != DBNull.Value ? dt.Rows[0]["position"].ToString() : string.Empty;
                model.rAddress = dt.Rows[0]["addressName"] != DBNull.Value ? dt.Rows[0]["addressName"].ToString() : string.Empty;
            }
            return model;
        }
    }
}
