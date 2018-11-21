using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebModel
{
    public class VisaLoginLog
    {
        private string userId;
        private string userName;
        private string loginTimes;
        private string lastLoginDate;
        private string iPAddress;

        public string UserId
        {
            get
            {
                return userId;
            }

            set
            {
                userId = value;
            }
        }

        public string UserName
        {
            get
            {
                return userName;
            }

            set
            {
                userName = value;
            }
        }

        public string LoginTimes
        {
            get
            {
                return loginTimes;
            }

            set
            {
                loginTimes = value;
            }
        }

        public string LastLoginDate
        {
            get
            {
                return lastLoginDate;
            }

            set
            {
                lastLoginDate = value;
            }
        }

        public string IPAddress
        {
            get
            {
                return iPAddress;
            }

            set
            {
                iPAddress = value;
            }
        }
    }
}
