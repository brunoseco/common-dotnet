using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Infrastructure.Log
{
    public class Logger
    {
        public static void Setup()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, GetFileName("all"));

            Hierarchy hierarchy = (Hierarchy)LogManager.GetRepository();

            PatternLayout patternLayout = new PatternLayout();
            patternLayout.ConversionPattern = "%date [%thread] %-5level %logger - %message%newline";
            patternLayout.ActivateOptions();

            var roller = new RollingFileAppender();
            roller.AppendToFile = true;
            roller.File = path;
            roller.Layout = patternLayout;
            roller.MaxSizeRollBackups = 5;
            roller.MaximumFileSize = "1GB";
            roller.RollingStyle = RollingFileAppender.RollingMode.Size;
            roller.StaticLogFileName = true;
            roller.ActivateOptions();
            hierarchy.Root.AddAppender(roller);

            var consoleAppender = new ConsoleAppender();
            consoleAppender.Layout = patternLayout;
            consoleAppender.ActivateOptions();
            hierarchy.Root.AddAppender(consoleAppender);

            var level = ConfigLevel();

            hierarchy.Root.Level = level;
            hierarchy.Configured = true;
        }

        private static Level ConfigLevel()
        {
            var log = ConfigurationManager.AppSettings["logs"].ToUpper();
            var level = Level.Error;

            if (log == ETipoLog.True.ToString().ToUpper()) level = Level.All;
            if (log == ETipoLog.False.ToString().ToUpper()) level = Level.Error;
            if (log == ETipoLog.Error.ToString().ToUpper()) level = Level.Error;
            if (log == ETipoLog.Debug.ToString().ToUpper()) level = Level.Debug;
            if (log == ETipoLog.Fatal.ToString().ToUpper()) level = Level.Fatal;
            if (log == ETipoLog.Info.ToString().ToUpper()) level = Level.Info;
            if (log == ETipoLog.Warn.ToString().ToUpper()) level = Level.Warn;
            if (log == ETipoLog.Console.ToString().ToUpper()) level = Level.Debug;

            return level;
        }

        private static string GetFileName(string tipoLog)
        {
            return String.Format("log-{0}-{1}.log", tipoLog, DateTime.Now.ToString("dd-MM-yyyy"));
        }

    }
}
