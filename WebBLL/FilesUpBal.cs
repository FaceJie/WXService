using System;
using System.Data;
using WebDAL;
using WebModel;

namespace WebBLL
{
    public class FilesUpBal
    {
        FilesUpDal dal = new FilesUpDal();

        public bool Save(CountryVisaInfoNeed model)
        {
            return dal.Save(model);
        }

        public DataTable GetVisaNeedInfo()
        {
            return dal.GetVisaNeedInfo();
        }

        public bool SaveName(string fileReallyName,string tempNeedId, string where)
        {
            return dal.SaveName(fileReallyName, tempNeedId, where);
        }

        public DataTable GetVisaNeedInfoByCountry(string countryName)
        {
            return dal.GetVisaNeedInfoByCountry(countryName);
        }

        public DataTable GetCountry()
        {
            return dal.GetCountry();
        }
    }
}