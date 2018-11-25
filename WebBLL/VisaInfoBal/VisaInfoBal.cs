using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebDAL;

namespace WebBLL
{
    public class VisaInfoBal
    {
        VisaInfoDal dal = new VisaInfoDal();
        public DataTable GetVisaInfoParams(string serviceName, string sendDataTime, string passportNo, string country,string tihuoWay)
        {
            return dal.GetVisaInfoParams(serviceName, sendDataTime, passportNo, country,tihuoWay);
        }

        public string GetNameByPhone(string bindPhone)
        {
            return dal.GetNameByPhone(bindPhone);
        }
    }
}
