using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebModel
{
    public class VisaInfoModel
    {
        string _VisaInfo_id, _GroupNo, _Name, _EnglishName, _Sex, _Birthday, _PassportNo, _LicenceTime, _ExpiryDate, _Birthplace, _IssuePlace, _Post, _Phone, _GuideNo, _Client, _Salesperson, _Types, _Sale_id, _Tips, _InTime, _OutTime, _Call, _EmbassyTime, _RealOutTime, _OutState;

        public string OutState
        {
            get { return _OutState; }
            set { _OutState = value; }
        }

        public string RealOutTime
        {
            get { return _RealOutTime; }
            set { _RealOutTime = value; }
        }

        public string EmbassyTime
        {
            get { return _EmbassyTime; }
            set { _EmbassyTime = value; }
        }

        public string Call
        {
            get { return _Call; }
            set { _Call = value; }
        }

        public string OutTime
        {
            get { return _OutTime; }
            set { _OutTime = value; }
        }

        public string InTime
        {
            get { return _InTime; }
            set { _InTime = value; }
        }

        public string Tips
        {
            get { return _Tips; }
            set { _Tips = value; }
        }
        int _no, _Number;

        public int Number
        {
            get { return _Number; }
            set { _Number = value; }
        }

        public int No
        {
            get { return _no; }
            set { _no = value; }
        }

        public string Sale_id
        {
            get { return _Sale_id; }
            set { _Sale_id = value; }
        }

        public string Types
        {
            get { return _Types; }
            set { _Types = value; }
        }

        public string Salesperson
        {
            get { return _Salesperson; }
            set { _Salesperson = value; }
        }

        public string Client
        {
            get { return _Client; }
            set { _Client = value; }
        }

        public string GuideNo
        {
            get { return _GuideNo; }
            set { _GuideNo = value; }
        }

        public string Phone
        {
            get { return _Phone; }
            set { _Phone = value; }
        }

        public string Post
        {
            get { return _Post; }
            set { _Post = value; }
        }

        public string IssuePlace
        {
            get { return _IssuePlace; }
            set { _IssuePlace = value; }
        }

        public string Birthplace
        {
            get { return _Birthplace; }
            set { _Birthplace = value; }
        }

        public string ExpiryDate
        {
            get { return _ExpiryDate; }
            set { _ExpiryDate = value; }
        }

        public string LicenceTime
        {
            get { return _LicenceTime; }
            set { _LicenceTime = value; }
        }

        public string PassportNo
        {
            get { return _PassportNo; }
            set { _PassportNo = value; }
        }

        public string Birthday
        {
            get { return _Birthday; }
            set { _Birthday = value; }
        }

        public string Sex
        {
            get { return _Sex; }
            set { _Sex = value; }
        }

        public string EnglishName
        {
            get { return _EnglishName; }
            set { _EnglishName = value; }
        }

        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        public string GroupNo
        {
            get { return _GroupNo; }
            set { _GroupNo = value; }
        }

        public string VisaInfo_id
        {
            get { return _VisaInfo_id; }
            set { _VisaInfo_id = value; }
        }
    }
}
