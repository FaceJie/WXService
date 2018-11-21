using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebModel
{
    public class UploadUserModel
    {
        private string userId;
        private string groupId;
        private string userHeader;
        private string userType;
        private string userName;
        private string userTlp;
        private string userNikeName;
        private DateTime entryTime;

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

        public string UserHeader
        {
            get
            {
                return userHeader;
            }

            set
            {
                userHeader = value;
            }
        }

        public string GroupId
        {
            get
            {
                return groupId;
            }

            set
            {
                groupId = value;
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
    }

    public class LocationViewModel
    {
        public string sTransportId { get; set; }
        public string sLocationSet { get; set; }//经度纬度
        public string sPlaceSet { get; set; }//地点集合
        public string isEnd { get; set; }//是否结束订单

    }
}
