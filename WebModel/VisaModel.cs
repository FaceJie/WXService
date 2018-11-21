using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebModel
{
    public class VisaModel
    {
        string _Visa_id, _GroupNo, _Name, _Types, _PredictTime, _RealTime, _FinishTime, _Satus, _Person, _List, _SalesPerson, _ExpressNo, _Call, _Country, _Documenter, _Tips, _Tips2, _WorkId;

        public string WorkId
        {
            get { return _WorkId; }
            set { _WorkId = value; }
        }
        float _ListCount, _Picture, _Receipt, _Quidco, _Cost, _OtherCost, _ConsulateCost, _VisaPersonCost, _MailCost, _Price, _GroupPrice, _InvitationCost;

        public float InvitationCost
        {
            get { return _InvitationCost; }
            set { _InvitationCost = value; }
        }

        public float GroupPrice
        {
            get { return _GroupPrice; }
            set { _GroupPrice = value; }
        }
        int _SubmitFlag;

        public int SubmitFlag
        {
            get { return _SubmitFlag; }
            set { _SubmitFlag = value; }
        }

        public float Price
        {
            get { return _Price; }
            set { _Price = value; }
        }
        public string Tips2
        {
            get { return _Tips2; }
            set { _Tips2 = value; }
        }

        public string Tips
        {
            get { return _Tips; }
            set { _Tips = value; }
        }

        public string Documenter
        {
            get { return _Documenter; }
            set { _Documenter = value; }
        }
        public float MailCost
        {
            get { return _MailCost; }
            set { _MailCost = value; }
        }

        public float VisaPersonCost
        {
            get { return _VisaPersonCost; }
            set { _VisaPersonCost = value; }
        }

        public float ConsulateCost
        {
            get { return _ConsulateCost; }
            set { _ConsulateCost = value; }
        }
        int _Number;


        public string Country
        {
            get { return _Country; }
            set { _Country = value; }
        }
        public float OtherCost
        {
            get { return _OtherCost; }
            set { _OtherCost = value; }
        }

        public float Cost
        {
            get { return _Cost; }
            set { _Cost = value; }
        }

        public float Quidco
        {
            get { return _Quidco; }
            set { _Quidco = value; }
        }

        public float Receipt
        {
            get { return _Receipt; }
            set { _Receipt = value; }
        }

        public float Picture
        {
            get { return _Picture; }
            set { _Picture = value; }
        }

        public float ListCount
        {
            get { return _ListCount; }
            set { _ListCount = value; }
        }


        public string Call
        {
            get { return _Call; }
            set { _Call = value; }
        }

        public string ExpressNo
        {
            get { return _ExpressNo; }
            set { _ExpressNo = value; }
        }



        public string SalesPerson
        {
            get { return _SalesPerson; }
            set { _SalesPerson = value; }
        }

        public string List
        {
            get { return _List; }
            set { _List = value; }
        }


        public int Number
        {
            get { return _Number; }
            set { _Number = value; }
        }

        public string Person
        {
            get { return _Person; }
            set { _Person = value; }
        }

        public string Satus
        {
            get { return _Satus; }
            set { _Satus = value; }
        }

        public string FinishTime
        {
            get { return _FinishTime; }
            set { _FinishTime = value; }
        }

        public string RealTime
        {
            get { return _RealTime; }
            set { _RealTime = value; }
        }

        public string PredictTime
        {
            get { return _PredictTime; }
            set { _PredictTime = value; }
        }

        public string Types
        {
            get { return _Types; }
            set { _Types = value; }
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

        public string Visa_id
        {
            get { return _Visa_id; }
            set { _Visa_id = value; }
        }
    }
}
