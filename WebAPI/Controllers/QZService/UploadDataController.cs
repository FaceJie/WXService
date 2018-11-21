using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBLL;
using WebHelper;
using WebModel;

namespace WebAPI.Controllers.QZService
{
    public class UploadDataController : Controller
    {
        UploadDataBal bal = new UploadDataBal();
        // GET: UploadData
        public ActionResult Index()
        {
            return View();
        }

        //获取派送任务
        [HttpPost]
        public string GetOrderList(string userId)
        {
            string json = "";
            DataTable dt = bal.GetOrderList(userId);
            if (dt != null && dt.Rows.Count > 0)
            {
                json = DatatableToJson.Dtb2Json(dt);
            }
            return json;
        }
        //获历史订单
        [HttpPost]
        public string GetLogiscOrderList(string userId)
        {
            string json = "";
            DataTable dt = bal.GetLogiscOrderList(userId);
            if (dt != null && dt.Rows.Count > 0)
            {
                json = DatatableToJson.Dtb2Json(dt);
            }
            return json;
        }
       

        //下单
        public string SendData(SendDataViewModel sendDataViewModel)
        {
            MsgModel ret = new MsgModel();
            //根据userId去查询客户的姓名和电话
            //获取用户的简要信息

            DataTable dt = bal.GetUserInfo(sendDataViewModel.sUserId);
            if (dt != null && dt.Rows.Count > 0)
            {
                sendDataViewModel.sName = dt.Rows[0]["userName"].ToString();
                sendDataViewModel.sPhone = dt.Rows[0]["userTlp"].ToString();
                sendDataViewModel.sDateTime = DateTime.Now;
                sendDataViewModel.sTimeLimit = Convert.ToInt32(sendDataViewModel.sDiscription);
                sendDataViewModel.sDiscription = "预计" + DateTime.Now.AddHours(Convert.ToInt32(sendDataViewModel.sDiscription)).ToString("HH:mm") + "收件";
                sendDataViewModel.sOrderId = GenerateOrderNo();//运单号
                try
                {
                    int result = bal.SaveSendDate(sendDataViewModel);
                    if (result > 0)
                    {
                        ret.scu = true;
                        ret.msg = sendDataViewModel.sOrderId;
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
                ret.msg = "用户不存在！";
            }
            return GlobalFuc.ModelToJson<MsgModel>(ret);
        }

        /**
     * 根据当前系统时间加随机序列来生成订单号
      * @return 订单号
     */
        public static string GenerateOrderNo()
        {
            Random ran = new Random();
            return string.Format("U{0}{1}", DateTime.Now.ToString("yyyyMMddHHmmss"), ran.Next(999));
        }
        //操作当前订单
        public string UpdateStatus(string transportId, string startPosition, string clickOlder)
        {
            MsgModel ret = new MsgModel();
            DataTable dtDetail = bal.GetDetailInfo(transportId);
            RouteCommandModel routeCommandModel = new RouteCommandModel();
            UnitsModel unitsModel = new UnitsModel();
            routeCommandModel = AmapUtil.GetRouteCommand(startPosition, dtDetail.Rows[0]["clientPosition"].ToString(), 1000);
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

        //持续保存接单人员的位置信息
        public string SavaLocationAll(string userId, string transportId, string postion)
        {
            string json = "erorr";
            int i = bal.SavaLocationAll(userId, transportId, postion);
            if (i != -1)
            {
                json = "scu";
            }
            return json;
        }
        public double MinOfList(double[] doubleNum)
        {
            double minValue = doubleNum.ToArray().Max();
            for (int i = 0; i < doubleNum.Length; ++i)
            {
                if ((doubleNum[i] > -9999.0d) && (minValue > doubleNum[i]))
                {
                    minValue = doubleNum[i];
                }
            }
            return minValue;
        }





        //结束订单
        /// <summary>
        /// 测试地址：http://localhost:56242/QZService/UploadData/ShurtOrder?userId=c3b71b66-ca5f-4f74-8d24-977914d805db&targetTransportIds=4814ba5f47154e9b98bc72906bd11b02,596735f3404241ff8732b9172be9c689
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="targetTransportIds"></param>
        /// <param name="postion"></param>
        /// <returns></returns>
        public string ShurtOrder(string userId, string transportId, string optionTransportNextId, string postion)//, 
        {
            MsgModel ret = new MsgModel();
            int old = bal.SavaLocationAll(userId, transportId, postion);
            if (old < 0)
            {
                ret.scu = false;
                ret.msg = "更新数据出错";
                return GlobalFuc.ModelToJson<MsgModel>(ret);
            }
            if (string.IsNullOrEmpty(optionTransportNextId))
            {
                DataTable dt_start = bal.GetDetailInfo(optionTransportNextId);
                RouteCommandModel routeCommandModel = new RouteCommandModel();
                UnitsModel unitsModel = new UnitsModel();
                routeCommandModel = AmapUtil.GetRouteCommand(postion, dt_start.Rows[0]["clientPosition"].ToString(), 1000);
                //两点之间的真实和推荐距离、时间、线路
                unitsModel.recommendedDistance = routeCommandModel.distance;
                unitsModel.recommendedTime = routeCommandModel.duration;
                unitsModel.recommendedRoute = GlobalFuc.ModelToJson<RouteModel>(routeCommandModel.steps);
                int new_position = bal.SaveNextPosition(optionTransportNextId, postion, unitsModel);
                if (new_position < 0)
                {
                    ret.scu = false;
                    ret.msg = "更新位置出错";
                    return GlobalFuc.ModelToJson<MsgModel>(ret);
                }
            }
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
                    distance = AmapUtil.OnlyGetDistance(positionData[0], positionData[positionData.Length - 1]);

                    string uploadPath = "C:\\WebAPI\\Temp";
                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }
                    FileStream fs = new FileStream(uploadPath+ "\\TestTxt2.txt", FileMode.Open, FileAccess.Write);
                    StreamWriter sr = new StreamWriter(fs);
                    sr.WriteLine("起点：" + positionData[0]+",终点：" +positionData[positionData.Length - 1] + "高德地图计算出来的距离：" + distance);//开始写入值
                    sr.Close();
                    fs.Close();

                }
                else
                {
                    distance = 0;
                }
                DateTime end = Convert.ToDateTime(dtDetail.Rows[0]["receiveOrderTime"]);
                timeSpan = strat.Subtract(end).TotalMinutes.ToString("0");
                unitsModel.orderReallyTime = timeSpan + "min";
                unitsModel.reallyDistance = distance.ToString("0.00") + "km";
                unitsModel.orderCompleteTime = strat;
                unitsModel.transportId = dtDetail.Rows[0]["transportId"].ToString();
                //把数据更新到数据库
                int q = bal.ShurtOrder(unitsModel);
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

        //客户方法
        public string GetCompleteOrderListByUserId(string userId)
        {
            string json = "";
            DataTable dt = bal.GetCompleteOrderListByUserId(userId);
            if (dt != null && dt.Rows.Count > 0)
            {
                json = DatatableToJson.Dtb2Json(dt);
            }
            return json;
        }
        //完成订单
        public string ClientReciveOrder(string userId, string transportId)
        {
            MsgModel ret = new MsgModel();
            try
            {
                int result = bal.ClientReciveOrder(userId, transportId);
                if (result > 0)
                {
                    ret.scu = true;
                    ret.msg = "订单完成";//运单凭证
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
            return GlobalFuc.ModelToJson<MsgModel>(ret);
        }
        [HttpPost]
        public string GetOrderByUserId(string userId)
        {
            string json = "";
            DataTable dt = bal.GetOrderByUserId(userId);
            if (dt != null && dt.Rows.Count > 0)
            {
                json = DatatableToJson.Dtb2Json(dt);
            }
            return json;
        }
        [HttpPost]
        public string GetOrderByUserIdDetails(string userId, string transportId)
        {
            string json = "";
            DataTable dt = bal.GetOrderByUserIdDetails(userId, transportId);
            if (dt != null && dt.Rows.Count > 0)
            {
                json = DatatableToJson.Dtb2Json(dt);
            }
            return json;
        }

        //过时
        //查询待处理订单列表（客户可见的列表，后勤可见的列表）
        [HttpPost]
        public string GetCompleteOrderList(string transportId)
        {
            string json = "";
            DataTable dt = bal.GetCompleteOrderList(transportId);
            if (dt != null && dt.Rows.Count > 0)
            {
                json = DatatableToJson.Dtb2Json(dt);
            }
            return json;
        }







        //后勤结算
        public string CompleteOrder(CompleteDataViewModel completeDataViewModel)
        {
            MsgModel ret = new MsgModel();
            try
            {
                int result = bal.CompleteDate(completeDataViewModel);
                if (result > 0)
                {
                    ret.scu = true;
                    ret.msg = "订单完成";//运单凭证
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
            return GlobalFuc.ModelToJson<MsgModel>(ret);
        }

        //根据经纬度确定地址
        public string GetAddressInfo(string LngLatStr)
        {
            //经度在前，纬度在后
            string str = AmapUtil.GetLocationByLngLat(LngLatStr, 10000);
            return str;
        }

        //获取推荐路线
        public string GetRouteCommand(string origin, string destination)
        {
            //经度在前，纬度在后,起点终点
            RouteCommandModel routeCommandModel = AmapUtil.GetRouteCommand(origin, destination, 1000);
            return GlobalFuc.ModelToJson<RouteCommandModel>(routeCommandModel);
        }
        //根据两个地点的经纬度确定距离
        public string GetDistance()
        {
            Hashtable hr = new Hashtable();
            //经度在前，纬度在后
            string[] strArr = AmapUtil.GetDistance("104.0679350000,30.6316680000", "104.0665600000,30.6335200000", 1000).Split('-');
            hr.Add("duration", strArr[0]);
            hr.Add("distance", strArr[1]);
            return GlobalFuc.HashtableToJson(hr);
        }
        //获取地址
        public string GetUserAddress(string userId)
        {
            string json = "";
            DataTable dt = bal.GetUserAddress(userId);
            if (dt != null && dt.Rows.Count > 0)
            {
                json = DatatableToJson.Dtb2Json(dt);
            }
            return json;
        }
        //更新地址
        public string userAddressManager(string userId, string logiscPosition)
        {
            string json = "";
            string positionAddress = AmapUtil.GetLocationByLngLat(logiscPosition, 10000);
            if (!string.IsNullOrEmpty(positionAddress))
            {
                DataTable result = bal.userAddressManager(userId, positionAddress, logiscPosition);

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
        
    }
    public class CalcMode
    {
        public int index
        {
            get;
            set;
        }
        public decimal AllNum
        {
            get;
            set;
        }
        public decimal OKNum
        {
            get;
            set;
        }
    }
}