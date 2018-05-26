using Common.Infrastructure.Log;
using Common.Infrastructure.ORM.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Deploy
{
    public static class ServerWatcher
    {
        private static DeploySettings _deploy;
        public static void StartWatcher(DeploySettings deploy)
        {
            _deploy = deploy;
            var watcher = new FileSystemWatcher();
            watcher.Path = deploy.PackagingServerPathBase;

            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
               | NotifyFilters.FileName | NotifyFilters.DirectoryName;


            watcher.Filter = "*.zip";


            watcher.Created += new FileSystemEventHandler(OnChanged);
            watcher.EnableRaisingEvents = true;
        }

        private static void OnChanged(object source, FileSystemEventArgs e)
        {
            try
            {
                FactoryLog.GetInstace().Debug(string.Format("Arquivo {0} {1}", e.FullPath, e.ChangeType));
                deployStart(e.Name, e.FullPath);

            }
            catch (Exception ex)
            {
                FactoryLog.GetInstace().Error(ex.Message, ex);
            }
        }
        private static void OnRenamed(object source, RenamedEventArgs e)
        {
            try
            {
                FactoryLog.GetInstace().Debug(string.Format("Arquivo {0} renomeado para {1}", e.OldFullPath, e.FullPath));
                deployStart(e.OldName, e.FullPath);
            }
            catch (Exception ex)
            {
                FactoryLog.GetInstace().Error(ex.Message, ex);
            }
        }

        private static void deployStart(string Name, string FullPath)
        {

            if (_deploy.IsNotNull())
            {
                WaitForComplete(FullPath);

                _deploy.SetPackagingName(Name);

                var connectionStringDeploy = _deploy.ConnectionStringDeploy;

                var resultDeploysToDo = AdoNetHelper.ExecuteReader("Select * from DeployQueue where deployName=@deployName", connectionStringDeploy, new
                {
                    DeployName = _deploy.GetPackagingName(),
                }, commandType: CommandType.Text);

                FactoryLog.GetInstace().Debug(string.Format("Packaging {0}, obtido do banco", _deploy.GetPackagingName()));

                foreach (var resultDeployToDo in resultDeploysToDo)
                {
                    if (resultDeployToDo.Flow == (int)EFlow.Updates)
                        _deploy.Flow = EFlow.Updates;
                    if (resultDeployToDo.Flow == (int)EFlow.Validation)
                        _deploy.Flow = EFlow.Validation;
                    if (resultDeployToDo.Flow == (int)EFlow.FixInProduction)
                        _deploy.Flow = EFlow.FixInProduction;
                    
                    if (_deploy.Flow == EFlow.Updates)
                    {
                        FactoryLog.GetInstace().Debug(string.Format("fluxo de {0} selecionado", EFlow.Updates.ToString()));
                        DeployProcess.PackagingServerPathToHomolog(_deploy);
                    }

                    if (_deploy.Flow == EFlow.Validation)
                    {
                        FactoryLog.GetInstace().Debug(string.Format("fluxo de {0} selecionado", EFlow.Validation.ToString()));
                        DeployProcess.PackagingServerPathToValidation(_deploy);
                    }

                    if (_deploy.Flow == EFlow.FixInProduction)
                    {
                        FactoryLog.GetInstace().Debug(string.Format("fluxo de {0} selecionado", EFlow.FixInProduction.ToString()));
                        DeployProcess.PackagingServerPathToPreProduction(_deploy);
                    }

                }
            }
        }

        private static void WaitForComplete(string filename)
        {
            while (IsFileInUse(new FileInfo(filename)))
                System.Threading.Thread.Sleep(10000);

            FactoryLog.GetInstace().Debug(string.Format("Arquivo {0} ,Liberado", filename));
            System.Threading.Thread.Sleep(2000);
        }


        private static bool IsFileInUse(FileInfo file)
        {
            FileStream stream = null;
            var fileInUse = false;
            try
            {
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }

            catch (IOException ex)
            {
                FactoryLog.GetInstace().Debug(string.Format("Erro ao ler Arquivo {0} , tamanho {1} bytes [{2}]", file.Name, file.Length, ex.Message));
                fileInUse = true;
            }

            finally
            {
                if (stream != null)
                    stream.Close();
            }

            return fileInUse;
        }

    }
}
