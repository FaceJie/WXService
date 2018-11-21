using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebModel
{
    public class VisaLoginModel
    {
        private string userId;
        private string userName;
        private string userType;
        private string headUrl;
        private string userTlp;
        private string userNikeName;
        private DateTime entryTime;
        private string bindPhone;

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

       

        public string UserTlp
        {
            get
            {
                return userTlp;
            }

            set
            {
                userTlp = value;
            }
        }

        public string UserNikeName
        {
            get
            {
                return userNikeName;
            }

            set
            {
                userNikeName = value;
            }
        }

        public DateTime EntryTime
        {
            get
            {
                return entryTime;
            }

            set
            {
                entryTime = value;
            }
        }

        public string HeadUrl
        {
            get
            {
                return headUrl;
            }

            set
            {
                headUrl = value;
            }
        }

        public string BindPhone
        {
            get
            {
                return bindPhone;
            }

            set
            {
                bindPhone = value;
            }
        }

        public string UserType
        {
            get
            {
                return userType;
            }

            set
            {
                userType = value;
            }
        }
    }
}
