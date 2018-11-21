using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebModel
{

    public class LogModelSet
    {

        public ILog Logger { get; set; }
        public LogModelSet(LogModel logModel, ILog logger)
        {
            log4net.GlobalContext.Properties["userid"] = logModel.UserId;
            log4net.GlobalContext.Properties["username"] = logModel.UserName;
            switch (logModel.LogType)
            {
                case 1: logger.Info(logModel.Msg); break;
                case 2: logger.Warn(logModel.Msg); break;
                case 3: logger.Error(logModel.Msg); break;
                case 4: logger.Fatal(logModel.Msg); break;
                default: logger.Fatal("记录日志出错！"); break;
            }
        }
    }
    public class LogModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Msg { get; set; }
        public int LogType { get; set; }
        public LogModel(string userId, string userName, string msg, int logType)
        {
            this.UserId = userId;
            this.UserName = userName;
            this.Msg = msg;
            this.LogType = logType;
        }

    }
}
