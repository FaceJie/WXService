﻿using System;
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
    public class VisaLoginDal
    {
        //1
        public VisaLoginModel UserExsit(string userNikeName)
        {
            string sql = "select * from VisaLogin where userNikeName=@userNikeName and isInner=1";
            SqlParameter[] pars ={
                                    new SqlParameter("@userNikeName",userNikeName)
                                };
            DataTable dt = SqlHelper.GetTableText(sql, pars)[0];
            VisaLoginModel visaLoginModel = null;
            if (dt != null && dt.Rows.Count > 0 )
            {
                visaLoginModel = new VisaLoginModel();
                loadEntity(dt.Rows[0], visaLoginModel);
                string sqllog = "insert into VisaLoginLog(UserName,EntryTime, Account, LoginState,LoginCheck) values ('" + dt.Rows[0]["userName"] + "','" + DateTime.Now + "','" + dt.Rows[0]["userTlp"] + "','账号登陆成功！','微信验证登录')";
                SqlHelper.ExecteNonQueryText(sqllog, null);
            }
            return visaLoginModel;
        }

        public VisaLoginModel UserLogin(string openid)
        {
            DataTable dt = new DataTable();
            VisaLoginModel visaLoginModel = new VisaLoginModel();
            string sql = string.Format("select * from VisaLogin where openid='{0}' and isInner=0", openid);
            dt = SqlHelper.GetTableText(sql, null)[0];
            if (dt != null && dt.Rows.Count > 0)
            {
                visaLoginModel.Status= "0";
            }
            else
            {
                sql = string.Format("select * from VisaLogin where openid='{0}' and isInner=1", openid);
                dt = SqlHelper.GetTableText(sql, null)[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    visaLoginModel.Status = "1";
                    loadEntity(dt.Rows[0], visaLoginModel);
                    DataCache.SetCache(dt.Rows[0]["userId"].ToString(), visaLoginModel);
                }
                else
                {
                    visaLoginModel.Status = "-1";
                }
            }
            return visaLoginModel;
        }

        public bool CheckUserName(string userName, string userType)
        {
            string sql = "select * from VisaLogin where userName=@userName";
            SqlParameter[] pars ={
                                    new SqlParameter("@userName",userName)
                                };
            DataTable dt = SqlHelper.GetTableText(sql, pars)[0];
            
            if (dt != null && dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string BindMobilPhone(string userId, string tel)
        {
            try
            {
                string sql = string.Format("update VisaLogin set bindPhone='{0}' where userId='{1}'", tel, userId);
                if (SqlHelper.ExecteNonQueryText(sql, null) == 1)
                {
                    return "0";
                }
                else
                {
                    return "1";
                }
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }


        }


        //是否在审核中
        public bool Checking(string userName, string pwd)
        {
            bool isInner = false;
            string sql = "select * from VisaLogin where userName=@userName and password=@password and isInner=0";
            SqlParameter[] pars ={
                                    new SqlParameter("@userName",userName),
                                    new SqlParameter("@password",pwd)
                                };
            DataTable dt = SqlHelper.GetTableText(sql, pars)[0];
            if (dt!=null&&dt.Rows.Count>0)
            {
                isInner = true;
            }
            else
            {
                isInner = false;
            }
            return isInner;
        }
       
        //2
        public string UserRegister(string openid,string nickName, string avatarUrl, string userName, string userType,string userTlp,string pwd)
        {
            try
            {
                string sql = "insert into VisaLogin(openid,userName,password,userType,userTlp,userNikeName,headUrl,entryTime) output inserted.userId  values(@openid,@userName,@password,@userType, @userTlp,@userNikeName,@headUrl,@entryTime)";
                SqlParameter[] pars ={
                                        new SqlParameter("@openid",openid),
                                    new SqlParameter("@userName",userName),
                                      new SqlParameter("@password",pwd),
                                     new SqlParameter("@userType",userType),
                                    new SqlParameter("@userTlp",userTlp),
                                    new SqlParameter("@userNikeName", nickName),
                                    new SqlParameter("@headUrl",avatarUrl),
                                     new SqlParameter("@entryTime",DateTime.Now)
                                };

                string result = SqlHelper.ExecuteScalarText(sql, pars).ToString();
                if (!string.IsNullOrEmpty(result))
                {
                    string sqllog = "insert into VisaLoginLog(UserName,EntryTime, Account, LoginState,LoginCheck) values ('" + userName + "','" + DateTime.Now + "','" + userTlp + "','账号登陆成功！','账号注册登录')";
                    SqlHelper.ExecteNonQueryText(sqllog, null);
                    return result;
                }
                else
                {
                    string sqllog = "insert into VisaLoginLog(UserName,EntryTime, Account, LoginState,LoginCheck) values ('" + userName + "','" + DateTime.Now + "','" + userTlp + "','账号登陆失败！','账号注册登录')";
                    SqlHelper.ExecteNonQueryText(sqllog, null);
                    return "";
                }
            }
            catch (Exception)
            {
                return "erorr";
            }

        }

        //获取手机的IP
        public string GetIPRecord(string mobile, string ShowIp)
        {
            string IPSate = "00";
            string sql = "select * from VisaLogin where userTlp=@userTlp";
            SqlParameter[] pars ={
                                    new SqlParameter("@userTlp",mobile)
                                };
            DataTable dt = SqlHelper.GetTableText(sql, pars)[0];
            if (dt != null && dt.Rows.Count > 0)
            {
                string sqlLog = "select * from VisaLoginLog where UserName='" + dt.Rows[0]["userName"].ToString() + "'";
                DataTable dt1 = SqlHelper.GetTableText(sqlLog, null)[0];
                if (dt1!=null&&dt1.Rows.Count > 0)
                {
                    if (dt1.Rows[0]["iPAddress"].ToString() == ShowIp)
                    {
                        DateTime date1 = Convert.ToDateTime(dt1.Rows[0]["EntryTime"].ToString());
                        DateTime date2 = DateTime.Now;
                        TimeSpan timeSpan = date2 - date1;
                        double span = timeSpan.TotalMinutes;
                        if (span > 1)
                        {
                            IPSate = "11";
                        }
                        else
                        {
                            IPSate = "0011";
                        }
                    }
                }
            }
            return IPSate;
        }
        public void loadEntity(DataRow row, VisaLoginModel model)
        {
            model.UserId = row["userId"] != DBNull.Value ? row["userId"].ToString() : string.Empty;
            model.Openid = row["openid"] != DBNull.Value ? row["openid"].ToString() : string.Empty;
            model.Password = row["password"] != DBNull.Value ? row["password"].ToString() : string.Empty;
            model.UserName = row["userName"] != DBNull.Value ? row["userName"].ToString() : string.Empty;
            model.UserType = row["userType"] != DBNull.Value ? row["userType"].ToString() : string.Empty;
            model.UserTlp = row["userTlp"] != DBNull.Value ? row["userTlp"].ToString() : string.Empty;
            model.HeadUrl = row["headUrl"] != DBNull.Value ? row["headUrl"].ToString() : string.Empty;
            model.UserNikeName = row["userNikeName"] != DBNull.Value ? row["userNikeName"].ToString() : string.Empty;
            model.BindPhone = row["bindPhone"] != DBNull.Value ? row["bindPhone"].ToString() : string.Empty;
            if (Convert.ToDateTime(row["entryTime"]) != null)
            {
                model.EntryTime = Convert.ToDateTime(row["entryTime"]);
            }
        }
    }
}
