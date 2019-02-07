using System;
using System.Configuration;
using System.Reflection;
using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using static log4net.LogManager;

namespace wsTransferToNeoLoad
{
    public sealed class LoggingService : ILoggingService
    {
        private static readonly ILog log;
        private static readonly Configuration Config;

        static LoggingService()
        {
            var currentAssembly = Assembly.GetCallingAssembly();
            try
            {
                Config = ConfigurationManager.OpenExeConfiguration(currentAssembly.Location);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            var hierarchy = (Hierarchy) GetRepository();
            hierarchy.Threshold = GetLogLevel();


            var patternLayout = new PatternLayout();
            patternLayout.ConversionPattern = "%date %logger %level: %message%newline";
            patternLayout.ActivateOptions();

            var appender = new RollingFileAppender();
            appender.Name = @"wsTransferToNeoLoad";
            appender.File = GetLogFileName();
            appender.AppendToFile = true;
            appender.MaxSizeRollBackups = 2;
            appender.RollingStyle = RollingFileAppender.RollingMode.Size;
            appender.MaximumFileSize = "10MB";
            appender.Layout = patternLayout;
            appender.LockingModel = new FileAppender.MinimalLock();
            appender.StaticLogFileName = true;
            appender.ActivateOptions();
            hierarchy.Root.AddAppender(appender);

            hierarchy.Configured = true;

            log = GetLogger(@"wsTransferToNeoLoad");
        }

        private LoggingService()
        {
        }

        public static LoggingService GetLogger { get; } = new LoggingService();


        public void Debug(object message)
        {
            log.Debug(message);
        }

        public void Debug(object message, Exception exception)
        {
            log.Debug(message, exception);
        }

        public void DebugFormat(string format, object arg0)
        {
            log.DebugFormat(format, arg0);
        }

        public void DebugFormat(string format, object arg0, object arg1)
        {
            log.DebugFormat(format, arg0, arg1);
        }

        public void DebugFormat(string format, object arg0, object arg1, object arg2)
        {
            log.DebugFormat(format, arg0, arg1, arg2);
        }

        public void DebugFormat(string format, params object[] args)
        {
            log.DebugFormat(format, args);
        }

        public void Info(object message)
        {
            log.Info(message);
        }

        public void Info(object message, Exception exception)
        {
            log.Info(message, exception);
        }

        public void Error(object message)
        {
            log.Error(message);
        }

        public void Error(object message, Exception exception)
        {
            log.Error(message, exception);
        }

        public void ErrorFormat(string format, object arg0)
        {
            log.ErrorFormat(format, arg0);
        }

        public void ErrorFormat(string format, object arg0, object arg1)
        {
            log.ErrorFormat(format, arg0, arg1);
        }

        public void ErrorFormat(string format, object arg0, object arg1, object arg2)
        {
            log.ErrorFormat(format, arg0, arg1, arg2);
        }

        public void ErrorFormat(string format, params object[] args)
        {
            log.ErrorFormat(format, args);
        }

        public void Warn(object message)
        {
            log.Warn(message);
        }

        public void Warn(object message, Exception exception)
        {
            log.Warn(message, exception);
        }


        public void InfoFormat(string format, object arg0)
        {
            log.InfoFormat(format, arg0);
        }

        public void InfoFormat(string format, object arg0, object arg1)
        {
            log.InfoFormat(format, arg0, arg1);
        }

        public void InfoFormat(string format, object arg0, object arg1, object arg2)
        {
            log.InfoFormat(format, arg0, arg1, arg2);
        }

        public void InfoFormat(string format, params object[] args)
        {
            log.InfoFormat(format, args);
        }

        public void WarnFormat(string format, object arg0)
        {
            log.WarnFormat(format, arg0);
        }

        public void WarnFormat(string format, object arg0, object arg1)
        {
            log.WarnFormat(format, arg0, arg1);
        }

        public void WarnFormat(string format, object arg0, object arg1, object arg2)
        {
            log.WarnFormat(format, arg0, arg1, arg2);
        }

        public void WarnFormat(string format, params object[] args)
        {
            log.WarnFormat(format, args);
        }

        private static string GetAppSetting(string key, string defaultValue)
        {
            var settings = Config.AppSettings.Settings;

            string value = null;
            if (settings != null)
            {
                value = settings[key].Value;
                if (string.IsNullOrEmpty(value))
                {
                    value = defaultValue;
                }
            }


            return value;
        }

        private static Level GetLogLevel()
        {
            var sLevel = GetAppSetting("loglevel", "debug");
            Level level;
            if (string.IsNullOrWhiteSpace(sLevel))
            {
                level = Level.Debug;
            }
            else
            {
                level = GetRepository().LevelMap[sLevel.ToUpper()];
            }

            return level;
        }


        private static string GetLogFileName()
        {
            return Environment.ExpandEnvironmentVariables(GetAppSetting("logfile", "wsTransferToNeoLoad.log"));
        }
    }
}