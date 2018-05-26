using Common.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Common.Infrastructure.Log
{
    public class HelperElapsedLog : IElapsedLog
    {
        private string requestToken;
        private Stopwatch stopWatch;
        public HelperElapsedLog()
        {
            this.requestToken = Guid.NewGuid().ToString();
            this.stopWatch = new Stopwatch();
        }
        public void LogRequestIni(string layer, string className)
        {
            stopWatch.Start();
            var warn = string.Format("[{0}] - Start - {1} - {2}", layer, this.requestToken, className);
            FactoryLog.GetInstace().Debug(warn);
        }

        public void LogRequestEnd(string layer, string className)
        {
            stopWatch.Stop();
            var ts = stopWatch.Elapsed;
            var elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:000}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds);
            var warn = string.Format("[{0}] - End - {1} - {2} - Tempo :{3}", layer, this.requestToken, className, elapsedTime);
            FactoryLog.GetInstace().Debug(warn);
        }
        

    }
}
