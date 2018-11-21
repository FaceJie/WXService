using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    public class ListViewModel
    { 
        public string uploadId
        {
            get;
            set;
        }
        public DataTable dt
        {
            get;
            set;
        }
    }
}