using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Content.WXViewModel
{
    public class UserInfoViewModel
    {
        private string avatarUrl;//微信头像

        private string country;//国家

        private string province;//省市

        private string city;//城市

        private string gender;//性别

        private string language;//语言

        private string nickName;//微信名称

        public string AvatarUrl
        {
            get
            {
                return avatarUrl;
            }

            set
            {
                avatarUrl = value;
            }
        }

        public string Country
        {
            get
            {
                return country;
            }

            set
            {
                country = value;
            }
        }

        public string Province
        {
            get
            {
                return province;
            }

            set
            {
                province = value;
            }
        }

        public string City
        {
            get
            {
                return city;
            }

            set
            {
                city = value;
            }
        }

        public string Gender
        {
            get
            {
                return gender;
            }

            set
            {
                gender = value;
            }
        }

        public string Language
        {
            get
            {
                return language;
            }

            set
            {
                language = value;
            }
        }

        public string NickName
        {
            get
            {
                return nickName;
            }

            set
            {
                nickName = value;
            }
        }
    }
}