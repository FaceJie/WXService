using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebModel
{
    public class ReciveDataViewModel
    {
        public string rUserId { get; set; }
        public string rName { get; set; }
        public string rPhone { get; set; }
        public DateTime rDateTime { get; set; }
        public string recommendedDistance { get; set; }
        public string recommendedTime { get; set; }
        public string recommendedRoute { get; set; }
        public string rAddress { get; set; }
        public string sAddress { get; set; }
        public string rTransportId { get; set; }
        public string sPosition { get; set; }//收取位置
        public string rPosition { get; set; }//出发位置
    }
}
