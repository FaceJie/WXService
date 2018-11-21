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
            string sql = string.Format("update DataOrder set erorrData='{0}' where transportId='{1}'", erorrData, transportId);
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
            string sql = "SELECT [orderId],[clientName],[clientTlp],[visaInfoCountry],[visaInfoNum],[sendPosition],[recivePosition],[recipientsName],[recipientsTlp],[transportId],[creatOrderTime],[receiveOrderTime],[orderStatus],[recommendedDistance],[recommendedTime],[reallyDistance],[orderReallyTime],[orderCompleteTime],[timeLimit],[discription],[erorrData] FROM [TravelAgency].[dbo].[DataOrder] WHERE 1=1";
            List<SqlParameter> para = new List<SqlParameter>();
            if (!string.IsNullOrEmpty(tableOrgModel.transportId))
            {
                sql += " and transportId=@transportId ";
                para.Add(new SqlParameter("@transportId", tableOrgModel.transportId));
            }
            if (!string.IsNullOrEmpty(tableOrgModel.clientName))
            {
                sql += " and clientName=@clientName ";
                para.Add(new SqlParameter("@clientName", tableOrgModel.clientName));
            }
            if (!string.IsNullOrEmpty(tableOrgModel.recipientsName))
            {
                sql += " and recipientsName=@recipientsName ";
                para.Add(new SqlParameter("@recipientsName", tableOrgModel.recipientsName));
            }
            if (!string.IsNullOrEmpty(tableOrgModel.country))
            {
                para.Add(new SqlParameter("@visaInfoCountry", tableOrgModel.country));
                sql += " and visaInfoCountry=@visaInfoCountry ";
            }
            sql += " ORDER BY creatOrderTime DESC";
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