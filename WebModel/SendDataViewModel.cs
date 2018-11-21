using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebModel
{
    public class SendDataViewModel
    {
        public string sUserId { get; set; }
        public string sOrderId { get; set; }
        public string sName { get; set; }
        public string sPhone { get; set; }
        public string sCount { get; set; }
        public string sCountry { get; set; }
        public int sTimeLimit { get; set; }
        public DateTime sDateTime { get; set; }
        public string sPosition { get; set; }
        public string sPosition_longitude { get; set; }
        public string sPosition_latitude { get; set; }
        public string sDiscription { get; set; }
        
    }
}
