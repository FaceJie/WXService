using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using WebBLL;
using WebHelper;
using WebModel;

namespace WebAPI.Controllers.QZService
{
    public class QuickOrderController : Controller
    {
        QuickOrderBal bal = new QuickOrderBal();

        /// <summary>
        /// 后勤端的方法
        /// </summary>

        #region 下单部分

        //一键下单
        public string QuickSendData(QuickSendDataViewModel quickSendDataViewModel)
        {
            MsgModel ret = new MsgModel();
            //根据userId去查询客户的姓名和电话
            //获取用户的简要信息

            DataTable dt = bal.QuickSendData(quickSendDataViewModel.oUserId);
            if (dt != null && dt.Rows.Count > 0)
            {
                quickSendDataViewModel.transportId = GenerateOrderNo();
                quickSendDataViewModel.oName = dt.Rows[0]["userName"].ToString();
                quickSendDataViewModel.oPhone = dt.Rows[0]["userTlp"].ToString();
                quickSendDataViewModel.sCreateTime = DateTime.Now;
                try
                {
                    int result = bal.SaveQuickSendData(quickSendDataViewModel);
                    if (result > 0)
                    {
                        ret.scu = true;
                        ret.msg = quickSendDataViewModel.transportId;
                    }
                    else
                    {
                        ret.scu = false;
                        ret.msg = "提交失败！";
                    }
                }
                catch (Exception ex)
                {
                    ret.scu = false;
                    ret.msg = ex.ToString();
                }
            }
            else
            {
                ret.scu = false;
                ret.msg = "当前用户不存在！";
            }
            return GlobalFuc.ModelToJson<MsgModel>(ret);
        }

        /**
         * 根据当前系统时间加随机序列来生成订单号
         * @return 订单号
        **/
        public static string GenerateOrderNo()
        {
            Random ran = new Random();
            return string.Format("U{0}{1}", DateTime.Now.ToString("yyyyMMddHHmmss"), ran.Next(999));
        }

        #endregion

        #region  后勤获取订单部分 

        //获取当前后勤的指定订单
        [HttpPost]
        public string GetWaitOrderList(string userId)
        {
            string json = "";
            DataTable dt = bal.GetWaitOrderList(userId);
            if (dt != null && dt.Rows.Count > 0)
            {
                json = DatatableToJson.Dtb2Json(dt);
            }
            return json;
        }
        //获取已完成订单
        [HttpPost]
        public string GetCompleteOrderList(string userId)
        {
            string json = "";
            DataTable dt = bal.GetCompleteOrderList(userId);
            if (dt != null && dt.Rows.Count > 0)
            {
                json = DatatableToJson.Dtb2Json(dt);
            }
            return json;
        }

        #endregion

        #region  后勤接单部分 

        //操作当前订单
        public string UpdateStatus(string userId,string transportId, string startPosition, string clickOlder)
        {
            MsgModel ret = new MsgModel();
            //更新地址
            string update_address = userAddressManager(userId, startPosition);
            if (update_address == "erorr")
            {
                ret.scu = false;
                ret.msg = "更新位置出错";
                return GlobalFuc.ModelToJson<MsgModel>(ret);
            }
            DataTable dtDetail = bal.GetDetailInfo(transportId);
            RouteCommandModel routeCommandModel = new RouteCommandModel();
            UnitsModel unitsModel = new UnitsModel();
            routeCommandModel = AmapUtil.GetRouteCommand(dtDetail.Rows[0]["sPositionSet"].ToString(), dtDetail.Rows[0]["rPositionSet"].ToString(), 1000);
            //两点之间的真实和推荐距离、时间、线路
            unitsModel.recommendedDistance = routeCommandModel.distance;
            unitsModel.recommendedTime = routeCommandModel.duration;
            unitsModel.recommendedRoute = GlobalFuc.ModelToJson<RouteModel>(routeCommandModel.steps);
            int result = bal.UpdateStatus(transportId, startPosition, clickOlder, unitsModel);
            if (result > 0)
            {
                ret.scu = true;
                ret.msg = "接单成功";
            }
            else
            {
                ret.scu = false;
                ret.msg = "接单失败";
            }
            return GlobalFuc.ModelToJson<MsgModel>(ret);
        }

        //采集位置
        public string SavaLocationAll(string userId, string transportId, string postion)
        {
            string json = "erorr";
            //更新地址
            string update_address = userAddressManager(userId, postion);
            if (update_address == "erorr")
            {
                return json;
            }
            int i = bal.SavaLocationAll(userId, transportId, postion);
            if (i != -1)
            {
                json = "scu";
            }
            return json;
        }
        #endregion

        #region 后勤结束订单部分

        //结束订单
        public string ShurtOrder(string userId, string transportId, string optionTransportNextId, string position)//, 
        {
            MsgModel ret = new MsgModel();
            //更新地址
            string update_address=userAddressManager(userId, position);
            if (update_address=="erorr")
            {
                ret.scu = false;
                ret.msg = "更新位置出错";
                return GlobalFuc.ModelToJson<MsgModel>(ret);
            }
            //存储结束位置
            int old = bal.SavaLocationAll(userId, transportId, position);
            if (old < 0)
            {
                ret.scu = false;
                ret.msg = "更新数据出错";
                return GlobalFuc.ModelToJson<MsgModel>(ret);
            }

            //更新实际路线信息
            string[] positionData;
            string timeSpan = "";
            int k = 0;//更新成功的条数
            DateTime strat = DateTime.Now;
            //1、插入从推荐距离，修改详细数据
            DataTable dtDetail = bal.GetDetailInfo(transportId);
            DataTable dtReallDetail = bal.GetReallDetailInfo(transportId);
            if (dtDetail != null && dtDetail.Rows.Count > 0)
            {
                RouteCommandModel routeCommandModel = new RouteCommandModel();
                UnitsModel unitsModel = new UnitsModel();
                DataRow data = dtReallDetail.Select("transportId='" + dtDetail.Rows[0]["transportId"].ToString() + "'")[0];
                positionData = data["positionSet"].ToString().TrimEnd(';').Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                double distance = 0;
                if (positionData.Length > 1)
                {
                    routeCommandModel= AmapUtil.GetRouteCommand(positionData[0], dtDetail.Rows[0]["sPositionSet"].ToString(), 1000);
                    //更新一下推荐距离数据
                    unitsModel.recommendedDistance = routeCommandModel.distance;
                    unitsModel.recommendedTime = routeCommandModel.duration;
                    string distanceRex = @"[+-]?\d+[\.]?\d*";
                    double DistanceSpan = Math.Round(double.Parse(Regex.Match(routeCommandModel.distance, distanceRex).Value), 0) + Math.Round(double.Parse(Regex.Match(dtDetail.Rows[0]["recommendedDistance"].ToString(), distanceRex).Value), 0);
                    double TimeSpan = Math.Round(double.Parse(Regex.Match(routeCommandModel.duration, distanceRex).Value), 0) + Math.Round(double.Parse(Regex.Match(dtDetail.Rows[0]["recommendedTime"].ToString(), distanceRex).Value), 0); 
                    unitsModel.recommendedDistance = DistanceSpan+ "km";
                    unitsModel.recommendedTime = TimeSpan + "min";

                    distance = AmapUtil.OnlyGetDistance(positionData[0], positionData[positionData.Length - 1]);
                }
                else
                {
                    distance = 0;
                }
                DateTime end = Convert.ToDateTime(dtDetail.Rows[0]["lReceiveTime"]);
                timeSpan = strat.Subtract(end).TotalMinutes.ToString("0");
                unitsModel.orderReallyTime = timeSpan + "min";
                unitsModel.reallyDistance = distance.ToString("0.00") + "km";
                unitsModel.orderCompleteTime = strat;
                unitsModel.transportId = dtDetail.Rows[0]["transportId"].ToString();
                string userName= dtDetail.Rows[0]["rName"].ToString();
                //把数据更新到数据库
                int q = bal.ShurtOrder(unitsModel, userId,userName);
                if (q > 0)
                {
                    k++;
                }
            }
            if (k == dtDetail.Rows.Count)
            {
                ret.scu = true;
                ret.msg = "共结算" + k + "条订单";
            }
            else
            {
                ret.scu = false;
                ret.msg = "结算订单出错";
            }
            return GlobalFuc.ModelToJson<MsgModel>(ret);
        }

        //更新地址
        public string userAddressManager(string userId, string position)
        {
            string json = "";
            string positionAddress = AmapUtil.GetLocationByLngLat(position, 10000);
            if (!string.IsNullOrEmpty(positionAddress))
            {
                DataTable result = bal.userAddressManager(userId, positionAddress, position);

                if (result != null && result.Rows.Count > 0)
                {
                    json = DatatableToJson.Dtb2Json(result);
                }
            }
            else
            {
                json = "erorr";
            }
            return json;
        }
        #endregion

        #region 后勤不按顺序结束订单

        public string KillOrder(string userId, string transportId, string optionTransportId, string position)
        {
            MsgModel ret = new MsgModel();
            //更新地址
            string update_address = userAddressManager(userId, position);
            if (update_address == "erorr")
            {
                ret.scu = false;
                ret.msg = "更新位置出错";
                return GlobalFuc.ModelToJson<MsgModel>(ret);
            }
            //存储第一次位置
            int old = bal.SavaLocationAll(userId, transportId, position);
            if (old < 0)
            {
                ret.scu = false;
                ret.msg = "更新数据出错";
                return GlobalFuc.ModelToJson<MsgModel>(ret);
            }

            //更新实际路线信息
            string[] positionData;
            string timeSpan = "";
            int k = 0;//更新成功的条数
            DateTime strat = DateTime.Now;
            //1、插入从推荐距离，修改详细数据
            DataTable dtDetail = bal.GetDetailInfo(transportId);
            DataTable dtReallDetail = bal.GetReallDetailInfo(transportId);
            if (dtDetail != null && dtDetail.Rows.Count > 0)
            {
                RouteCommandModel routeCommandModel = new RouteCommandModel();
                UnitsModel unitsModel = new UnitsModel();
                DataRow data = dtReallDetail.Select("transportId='" + dtDetail.Rows[0]["transportId"].ToString() + "'")[0];
                positionData = data["positionSet"].ToString().TrimEnd(';').Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                double distance = 0;
                if (positionData.Length > 1)
                {
                    routeCommandModel = AmapUtil.GetRouteCommand(positionData[0], dtDetail.Rows[0]["sPositionSet"].ToString(), 1000);
                    //更新一下推荐距离数据
                    unitsModel.recommendedDistance = routeCommandModel.distance;
                    unitsModel.recommendedTime = routeCommandModel.duration;
                    string distanceRex = @"[+-]?\d+[\.]?\d*";
                    double DistanceSpan = Math.Round(double.Parse(Regex.Match(routeCommandModel.distance, distanceRex).Value), 0) + Math.Round(double.Parse(Regex.Match(dtDetail.Rows[0]["recommendedDistance"].ToString(), distanceRex).Value), 0);
                    double TimeSpan = Math.Round(double.Parse(Regex.Match(routeCommandModel.duration, distanceRex).Value), 0) + Math.Round(double.Parse(Regex.Match(dtDetail.Rows[0]["recommendedTime"].ToString(), distanceRex).Value), 0);
                    unitsModel.recommendedDistance = DistanceSpan + "km";
                    unitsModel.recommendedTime = TimeSpan + "min";
                    distance = AmapUtil.OnlyGetDistance(positionData[0], positionData[positionData.Length - 1]);
                }
                else
                {
                    distance = 0;
                }
                DateTime end = Convert.ToDateTime(dtDetail.Rows[0]["lReceiveTime"]);
                timeSpan = strat.Subtract(end).TotalMinutes.ToString("0");
                unitsModel.orderReallyTime = timeSpan + "min";
                unitsModel.reallyDistance = distance.ToString("0.00") + "km";
                unitsModel.orderCompleteTime = strat;
                unitsModel.transportId = dtDetail.Rows[0]["transportId"].ToString();
                string userName = dtDetail.Rows[0]["rName"].ToString();
                //把数据更新到数据库
                int q = bal.KillOrder(unitsModel, optionTransportId, userId, userName);
                if (q > 0)
                {
                    k++;
                }
            }
            if (k == dtDetail.Rows.Count)
            {
                ret.scu = true;
                ret.msg = "共结算" + k + "条订单";
            }
            else
            {
                ret.scu = false;
                ret.msg = "结算订单出错";
            }
            return GlobalFuc.ModelToJson<MsgModel>(ret);
        }

        #endregion


        /// <summary>
        /// 客户用到的方法
        /// </summary>

        #region 获取客户的订单列表

        //客户获取处理中的订单
        public string ClientDoOrderList(string userId)
        {
            string json = "";
            DataTable dt = bal.ClientDoOrderList(userId);
            if (dt != null && dt.Rows.Count > 0)
            {
                json = DatatableToJson.Dtb2Json(dt);
            }
            return json;
        }
        //客户获取已完成的订单
        public string ClientCompleteOrderList(string userId)
        {
            string json = "";
            DataTable dt = bal.ClientCompleteOrderList(userId);
            if (dt != null && dt.Rows.Count > 0)
            {
                json = DatatableToJson.Dtb2Json(dt);
            }
            return json;
        }

        //[HttpPost]
        public string GetOrderByUserIdDetails(string userId, string transportId)
        {
            string json = "";
            DataTable dt = bal.GetOrderByUserIdDetails(userId, transportId);
            if (dt != null && dt.Rows.Count > 0)
            {
                DataTable dt_userInfo = bal.QuickSendData(dt.Rows[0]["lUserId"].ToString());
                dt.Columns.Add("userPosition", Type.GetType("System.String"));
                dt.Columns.Add("userAddress", Type.GetType("System.String"));
                //这里要查询实时位置
                dt.Rows[0]["userPosition"] = dt_userInfo.Rows[0]["position"].ToString();
                dt.Rows[0]["userAddress"] = dt_userInfo.Rows[0]["addressName"].ToString();
                json = DatatableToJson.Dtb2Json(dt);
            }
            return json;
        }

        #endregion

    }
}