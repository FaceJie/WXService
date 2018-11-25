using System;
using System.Data;
using WebDAL;
using WebModel;

namespace WebBLL
{
    public class FilesUpBal
    {
        FilesUpDal dal = new FilesUpDal();

        public bool Save(FilesUpViewModel model)
        {
            return dal.Save(model);
        }

        public DataTable GetVisaNeedInfo()
        {
            return dal.GetVisaNeedInfo();
        }

        public bool SaveName(FilesUpViewModel filesUpViewModel)
        {
            return dal.SaveName(filesUpViewModel);
        }

        public DataTable GetVisaNeedInfoByCountry(string countryName)
        {
            return dal.GetVisaNeedInfoByCountry(countryName);
        }

        public DataTable GetCountry()
        {
            return dal.GetCountry();
        }

        public DataTable GetVisaTableList(string fileName)
        {
            return dal.GetVisaTableList(fileName);
        }
    }
}