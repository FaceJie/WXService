using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebModel
{
    public class UploadFilesData
    {
        private string workId;
        private string userName;
        private string userHeader;
        private string remark;
        private string uploadId;
        private string groupId;
        private int uploadType;
        private DateTime uploadTime;
        private Boolean display;
        private string uploadTitle;

        public string WorkId
        {
            get
            {
                return workId;
            }

            set
            {
                workId = value;
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

        public string Remark
        {
            get
            {
                return remark;
            }

            set
            {
                remark = value;
            }
        }

        public string UploadId
        {
            get
            {
                return UploadId1;
            }

            set
            {
                UploadId1 = value;
            }
        }

        public DateTime UploadTime
        {
            get
            {
                return uploadTime;
            }

            set
            {
                uploadTime = value;
            }
        }

        public bool Display
        {
            get
            {
                return display;
            }

            set
            {
                display = value;
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

        public string UploadId1
        {
            get
            {
                return uploadId;
            }

            set
            {
                uploadId = value;
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

        public int UploadType
        {
            get
            {
                return uploadType;
            }

            set
            {
                uploadType = value;
            }
        }

        public string UploadTitle
        {
            get
            {
                return uploadTitle;
            }

            set
            {
                uploadTitle = value;
            }
        }

       
    }
    public class FilesUpViewModel
    {
        public string needId { get; set; }
        public string country { get; set; }

        public string fileName { get; set; }
        public string fileUrl { get; set; }
        public string fileType { get; set; }
        public string fileImg { get; set; }
        public string discription { get; set; }
    }
}
