using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebHelper;

namespace WebDAL
{
    public class VisaSerachDal
    {
        //1
        public DataTable GetCountList()
        {
            string sql = "SELECT DISTINCT Country FROM VisaInfoWeChatView where Country is not null or Country <> ''";
            return SqlHelper.GetTableText(sql, null)[0];
        }
    }
}
