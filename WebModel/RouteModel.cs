using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebModel
{
    public class RouteModel
    {
            public string action { get; set; }
            public string assistant_action { get; set; }
            public int distance { get; set; }
            public int duration { get; set; }
            public string instruction { get; set; }
            public string orientation { get; set; }
            public string polyline { get; set; }
            public string road { get; set; }
    }
}
