using Common.Deploy;
using Common.Domain.CustomExceptions;
using Common.Infrastructure.Log;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Common.Deploy
{
    public class RunDeploy
    {
        private static Thread thread;
        private static bool StopRequest { get; set; }

        public RunDeploy()
        {
            StopRequest = false;
        }

        public void Run()
        {
            FactoryLog.GetInstace().Debug("Serviço de Deploy Inicializado");
            StopRequest = false;
            ProcessSync();

        }
        public void RunAsync()
        {
            StopRequest = false;
            thread = new Thread(new ThreadStart(ProcessAsync));
            thread.Start();
            FactoryLog.GetInstace().Debug("Serviço de Deploy Inicializado");
        }

        public void Stop()
        {
            StopRequest = true;
            FactoryLog.GetInstace().Debug("Serviço de Deploy Finalizado");

        }

        private void ProcessAsync()
        {
            Process(ciclical: true);
        }
        private void ProcessSync()
        {
            Process(ciclical: true);
        }


        private void Process(bool ciclical = true)
        {
            try
            {
                var settings = DefineSettings();
                var deploySettings = new DeploySettings(EPathBase.development, settings);
                DeployProcess.PackagingServerStartWatcher(deploySettings);

                while (!StopRequest)
                {
                    try
                    {
                        System.Threading.Thread.Sleep(2000);
                        DeployProcess.PackagingServerStartService(deploySettings);

                    }
                    catch (Exception ex)
                    {
                        FactoryLog.GetInstace().Error(ex.Message, ex);
                        throw;
                    }

                    if (!ciclical)
                        break;
                };

                if (thread != null)
                    thread.Abort();


            }
            catch (Exception ex)
            {
                if (thread != null)
                    thread.Abort();

                FactoryLog.GetInstace().Error("Erro geral servico esta sendo parado", ex);
            }

        }

        public virtual IEnumerable<PathsDeploy> DefineSettings()
        {
            var config = true;
            var settings = new DeploySettingsFactory().CreateSettings(config);
            return settings;
        }


    }
}
