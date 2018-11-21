using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebModel
{
    public class RouteCommandModel
    {
        public string duration { get; set; }
        public string distance { get; set; }
        public List<RouteModel> steps { get; set; }
    }
}
