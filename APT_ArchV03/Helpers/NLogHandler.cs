using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APT_ArchV03.Helpers
{
    public class NLogHandler
    {

        //private static Logger logger = LogManager.GetCurrentClassLogger();
        private static Logger loggeruser = LogManager.GetLogger("UserLog");
        private static Logger loggersystem = LogManager.GetLogger("SystemLog");

        public void APTLoggerUser(string logtext, string loglevel)
        {
            switch (loglevel) {
                case "Trace":
                    loggeruser.Trace(logtext);
                    break;
                case "Debug":
                    loggeruser.Debug(logtext);
                    break;
                case "Info":
                    loggeruser.Info(logtext);
                    break;
                case "Warn":
                    loggeruser.Warn(logtext);
                    break;
                case "Error":
                    loggeruser.Error(logtext);
                    break;
                case "Fatal":
                    loggeruser.Fatal(logtext);
                    break;
                default:
                    loggeruser.Info(logtext);
                    break;

            }            

            // alternatively you can call the Log() method
            // and pass log level as the parameter.
            //logger.Log(LogLevel.Info, "Sample informational message");
        }

        public void APTLoggerSystem(string logtext, string loglevel)
        {
            switch (loglevel)
            {
                case "Trace":
                    loggersystem.Trace(logtext);
                    break;
                case "Debug":
                    loggersystem.Debug(logtext);
                    break;
                case "Info":
                    loggersystem.Info(logtext);
                    break;
                case "Warn":
                    loggersystem.Warn(logtext);
                    break;
                case "Error":
                    loggersystem.Error(logtext);
                    break;
                case "Fatal":
                    loggersystem.Fatal(logtext);
                    break;
                default:
                    loggersystem.Info(logtext);
                    break;

            }

            // alternatively you can call the Log() method
            // and pass log level as the parameter.
            //logger.Log(LogLevel.Info, "Sample informational message");
        }

    }
}