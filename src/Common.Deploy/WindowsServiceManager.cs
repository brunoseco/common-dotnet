using Common.Infrastructure.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Common.Deploy
{
    public static class WindowsServiceManager
    {

        public static void StartService(string serviceName, int timeoutMilliseconds)
        {
            var service = new ServiceController(serviceName);

            try
            {
                FactoryLog.GetInstace().Debug(string.Format("Start service:{0}", serviceName));

                var timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);
                service.Start();
                service.WaitForStatus(ServiceControllerStatus.Running, timeout);
            }
            catch (Exception ex)
            {
                FactoryLog.GetInstace().Error(ex.Message, ex);
            }
        }

        public static void StopService(string serviceName, int timeoutMilliseconds)
        {
            var service = new ServiceController(serviceName);
            try
            {
                FactoryLog.GetInstace().Debug(string.Format("Stop service:{0}", serviceName));

                var timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);
                service.Stop();
                service.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
                System.Threading.Thread.Sleep(5000);
            }
            catch (Exception ex)
            {
                FactoryLog.GetInstace().Error(ex.Message, ex);
            }
        }
    }
}
