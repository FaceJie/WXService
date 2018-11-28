using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using WebHelper;
using WebModel;

namespace WebDAL
{
    public class ErorrOrderDal
    {
       
        public int UpAction(string userId, string type)
        {

            string sql = "";
            if (type=="yes")
            {
                sql = string.Format("update VisaLogin set isInner=1 where userId='{0}'", userId);
            }
            else
            {
                sql = string.Format("delete from VisaLogin where userId='{0}'", userId);
            }
            try
            {
                return SqlHelper.ExecteNonQueryText(sql, null);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public int UpErorrData(string transportId, string erorrData)
        {
            string sql = string.Format("update QuickOrder set erorrData='{0}' where transportId='{1}'", erorrData, transportId);
            try
            {
                return SqlHelper.ExecteNonQueryText(sql, null);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public DataTable GetOrderInfo(TableOrgModel tableOrgModel)
        {
            string sql = "SELECT [transportId],[oName],[oPhone],[sCreateTime],[sName],[sPhone],[sAddress],[rName],[rPhone],[rAddress],[pName],[pPhone],[pSendTime],[lName],[lPhone],[lAddress],[lReceiveTime],[recommendedDistance],[recommendedTime],[recommendedRoute],[reallyDistance],[orderReallyTime],[lCompleteTime],[orderStatus],[erorrData] FROM [TravelAgency].[dbo].[QuickOrder] WHERE 1=1";
            List<SqlParameter> para = new List<SqlParameter>();
            if (!string.IsNullOrEmpty(tableOrgModel.transportId))
            {
                sql += " and transportId=@transportId ";
                para.Add(new SqlParameter("@transportId", tableOrgModel.transportId));
            }
            if (!string.IsNullOrEmpty(tableOrgModel.sName))
            {
                sql += " and sName=@sName ";
                para.Add(new SqlParameter("@sName", tableOrgModel.sName));
            }
            if (!string.IsNullOrEmpty(tableOrgModel.lName))
            {
                sql += " and lName=@lName ";
                para.Add(new SqlParameter("@lName", tableOrgModel.lName));
            }
            switch (tableOrgModel.lCompleteTime)
            {
                case "1":
                    sql += " and datediff(day,lCompleteTime,getdate())=0";
                    break;
                case "2":
                    sql += " and  datediff(week,lCompleteTime,getdate())=0";
                    break;
                case "3":
                    sql += " and datediff(month,lCompleteTime,getdate())=0";
                    break;
                case "4":
                    sql += " and datediff(qq,lCompleteTime,getdate())=0";
                    break;
                case "5":
                    break;
                default:
                    break;
            }
            sql += " ORDER BY lCompleteTime DESC";
            return SqlHelper.GetTableTextByParams(sql, para)[0];
        }

        public DataTable GetAction()
        {
            string sql = string.Format("select * from VisaLogin where isInner=0");
            DataTable dt = SqlHelper.GetTableText(sql, null)[0];
            return dt;
        }
    }
}