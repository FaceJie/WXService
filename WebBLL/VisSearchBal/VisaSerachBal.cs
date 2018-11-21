using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebDAL;

namespace WebBLL
{
    public class VisaSerachBal
    {
        VisaSerachDal dal = new VisaSerachDal();
        public DataTable GetCountList()
        {
            return dal.GetCountList();
        }
    }
}
