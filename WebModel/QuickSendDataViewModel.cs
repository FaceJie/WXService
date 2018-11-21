using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebModel
{
    public class QuickSendDataViewModel
    {
        //发件订单部分
        public string transportId { get; set; }//订单的编号
        public string oUserId { get; set; }//操作人Id
        public string oName { get; set; }//操作人姓名
        public string oPhone { get; set; }//操作人电话
        public DateTime sCreateTime { get; set; }//订单创建时间
        public string sName { get; set; }//发件人
        public string sPhone { get; set; }//发件人电话
        public string sAddress { get; set; }//发件人的地址
        public string sPositionSet { get; set; }//发件人的坐标
        public string rName { get; set; }//收件人
        public string rPhone { get; set; }//收件人电话
        public string rAddress { get; set; }//收件人的地址
        public string rPositionSet { get; set; }//收件人的坐标

        //派单部分
        public string pUserId { get; set; }//派单人的Id
        public string pName { get; set; }//后勤姓名
        public string pPhone { get; set; }//后勤电话
        public DateTime pSendTime { get; set; }//后勤接单的事件

        //后勤收件部分
        public string lUserId { get; set; }//收件人的Id
        public string lName { get; set; }//后勤姓名
        public string lPhone { get; set; }//后勤电话
        public string lAddress { get; set; }//后勤地址
        public string lPositionSet { get; set; }//后勤的坐标
        public DateTime lReceiveTime { get; set; }//后勤接单的时间

        //订单结算部分
        public string recommendedDistance { get; set; }//高德推荐行程距离
        public string recommendedTime { get; set; }//高德规划路径用时
        public string recommendedRoute { get; set; }//高德推荐具体路线
        public string reallyDistance { get; set; }//后勤取件实际距离
        public string orderReallyTime { get; set; }//后勤取件实际用时
        public DateTime lCompleteTime { get; set; }//后勤完成订单的时间
    }
}
