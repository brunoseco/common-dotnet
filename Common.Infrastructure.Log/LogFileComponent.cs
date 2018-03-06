using Common.Domain.Interfaces;
using Common.StorageBase;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Infrastructure.Log
{
    public class LogFileComponent : ILog
    {
        public LogFileComponent()
        {

        }
        public void Debug(string message)
        {
            this.WriteLine(ETipoLog.Debug, "Debug {0} - {1}", DateTime.Now, message.ToString());
        }

        public void Debug(string message, Exception exception)
        {
            this.WriteLine(ETipoLog.Debug, "Debug {0}-{1}-{2}-{3}", DateTime.Now, message, exception.Message, exception.StackTrace);
        }

        public void Info(string message)
        {
            this.WriteLine(ETipoLog.Info, "Info {0} - {1}", DateTime.Now, message.ToString());
        }

        public void Info(string message, Exception exception)
        {
            this.WriteLine(ETipoLog.Info, "Info {0}-{1}-{2}-{3}", DateTime.Now, message, exception.Message, exception.StackTrace);
        }

        public void Warn(string message)
        {
            this.WriteLine(ETipoLog.Warn, "Warn {0} - {1}", DateTime.Now, message.ToString());
        }

        public void Warn(string message, Exception exception)
        {
            this.WriteLine(ETipoLog.Warn, "Warn {0}-{1}-{2}-{3}", DateTime.Now, message, exception.Message, exception.StackTrace);
        }

        public void Error(string message)
        {
            this.WriteLine(ETipoLog.Error, "Error {0} - {1}", DateTime.Now, message.ToString());
        }

        public void Error(string message, Exception exception)
        {
            this.WriteLine(ETipoLog.Error, "Error {0}-{1}-{2}-{3}", DateTime.Now, message, exception.Message, exception.StackTrace);
        }

        public void Fatal(string message)
        {
            this.WriteLine(ETipoLog.Fatal, "Fatal {0} - {1}", DateTime.Now, message.ToString());
        }

        public void Fatal(string message, Exception exception)
        {
            this.WriteLine(ETipoLog.Fatal, "Fatal {0}-{1}-{2}-{3}", DateTime.Now, message, exception.Message, exception.StackTrace);
        }

        public void Debug(string format, params object[] arg)
        {
            this.WriteLine(ETipoLog.Debug, format, arg);
        }

        public void Info(string format, params object[] arg)
        {
            this.WriteLine(ETipoLog.Info, format, arg);
        }

        public void Warn(string format, params object[] arg)
        {
            this.WriteLine(ETipoLog.Warn, format, arg);
        }

        public void Error(string format, params object[] arg)
        {
            this.WriteLine(ETipoLog.Error, format, arg);
        }

        public void Fatal(string format, params object[] arg)
        {
            this.WriteLine(ETipoLog.Fatal, format, arg);
        }

        private void WriteLine(ETipoLog tipoLog, string format, params object[] arg)
        {
            WriteLine(tipoLog.ToString(), format, arg);
        }
        private void WriteLine(string tipoLog, string format, params object[] arg)
        {

            try
            {

                var log = ConfigurationManager.AppSettings["logs"].ToUpper();
                var message = string.Format(format, arg);
                if (tipoLog == ETipoLog.Error.ToString() || tipoLog == ETipoLog.Fatal.ToString())
                    Console.ForegroundColor = ConsoleColor.Red;
                else
                    Console.ForegroundColor = ConsoleColor.White;

                if (log == "CONSOLE")
                {
                    if (tipoLog.ToString().ToUpper() == ETipoLog.Debug.ToString().ToUpper())
                        Console.WriteLine(message);
                }

                if (tipoLog.ToString().ToUpper() == log || tipoLog == ETipoLog.Error.ToString() || tipoLog == ETipoLog.Fatal.ToString() || log == ETipoLog.True.ToString().ToUpper())
                {
                    Console.WriteLine(message);

                    if (GetStorageConfig().ToUpper() == "CLOUD")
                        CloadLog(tipoLog, message);
                    else
                        FileLog(tipoLog, message);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private static string GetStorageConfig()
        {
            return ConfigurationManager.AppSettings["Storage"] ?? string.Empty;
        }

        private static void FileLog(string tipoLog, string message)
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, GetFileName(tipoLog));
            using (var write = new StreamWriter(path, true))
            {
                write.WriteLine(message);
            }
        }

        private static void CloadLog(string tipoLog, string message)
        {

            var fileName = GetFileNameCload(tipoLog);
            var ms = new MemoryStream();
            var write = new StreamWriter(ms);
            write.WriteLine(message);
            HelperStorageBase.SaveBytesInFile(ms.ToArray(), fileName, "logs");

        }

        private static string GetFileName(string tipoLog)
        {
            return String.Format("log-{0}-{1}.log", tipoLog, DateTime.Now.ToString("dd-MM-yyyy"));
        }

        private static string GetFileNameCload(string tipoLog)
        {
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory
             .Replace(":", "")
             .Replace("\\", "-");

            return String.Format("log-{0}-{1}-{2}.log", baseDirectory, tipoLog, DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss"));
        }
    }
}
