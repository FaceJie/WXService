using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebModel
{
    public class UnitsModel
    {
        public string transportId { get; set; }
        public string recommendedDistance { get; set; }
        public string recommendedTime { get; set; }
        public string recommendedRoute { get; set; }
        public string reallyDistance { get; set; }
        public string orderReallyTime { get; set; }
        public DateTime orderCompleteTime { get; set; }
    }
}
