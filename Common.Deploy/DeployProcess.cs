using Common.Domain;
using Common.Infrastructure.Log;
using Common.Infrastructure.ORM.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Deploy
{
    public static class DeployProcess
    {
        public static void DevelopmentToPackagingServer(DeploySettings deploy)
        {

            var log = FactoryLog.GetInstace();
            var connectionStringDeploy = deploy.ConnectionStringDeploy;
            deploy.CreatePackagingName(deploy.DeployName);


            var filesToZip = new List<FileInfo>();
            var filesToDelete = new List<FileInfo>();
            
            foreach (var app in deploy.Get())
            {
                log.Debug(string.Format("Iniciando Deploy da aplicação {0}", app.Name));

                string developmentPathComplete = DevelopmentPathComplete(deploy, app);

                var pathCopySettings = GetFilesToProccess(app, Environment.Development, developmentPathComplete);

                if (app.WebConfig || app.AppConfig)
                    TransformationsConfig(log, filesToZip, app, developmentPathComplete);

                if (app.FileType == "*.js" && deploy.VersionEnable)
                    FileEnvironment(deploy, log, filesToZip, app, developmentPathComplete);

                foreach (var file in pathCopySettings.filesSource)
                {
                    var fileBlackListUser = FileBlackListUser(app);
                    if (fileBlackListUser.Contains(file.Name.ToLower()))
                        continue;

                    SqlFileCopy(deploy, app, file, filesToDelete);

                    if (filesToZip.Where(_ => _.FullName == file.FullName).NotIsAny())
                        filesToZip.Add(file);
                }

            }

            log.Debug(string.Format("Iniciando Compactação"));

            var pathPackagingClientName = Path.Combine(deploy.PackagingClientPathBase, deploy.GetPackagingName());
            if (filesToZip.Count() > 0)
            {
                if (!Directory.Exists(Path.GetDirectoryName(pathPackagingClientName)))
                    Directory.CreateDirectory(Path.GetDirectoryName(pathPackagingClientName));

                var files = filesToZip.Select(_ => _.FullName);
                ZipHelper.ZipFiles(pathPackagingClientName, files);
            }

            log.Debug(string.Format("Iniciando FTP"));

            if (File.Exists(pathPackagingClientName))
            {
                var ftpClient = new ftp(deploy.FtpHosting, deploy.FtpUser, deploy.FtpPassword);
                var serverFileName = Path.GetFileName(pathPackagingClientName);
                ftpClient.upload(serverFileName, pathPackagingClientName);
            }

            log.Debug(string.Format("FTP Finalizado"));

            DeleteLocalFiles(filesToDelete);

            var commandText = "Insert into DeployQueue";
            commandText += "(DeployName,Status,HomologToPreProductionAprove,PreProductionToProductionAprove,Flow,MajorVersion,MinorVersion,BuildNumber)";
            commandText += "values";
            commandText += "(@DeployName,@Status,@HomologToPreProductionAprove,@PreProductionToProductionAprove,@Flow,@MajorVersion,@MinorVersion,@BuildNumber)";

            AdoNetHelper.ExecuteNonQuery(commandText, connectionStringDeploy, new
            {
                DeployName = deploy.GetPackagingName(),
                Status = (int)EStatusDeploy.DevelopmentToHomolog,
                DevelopmentToHomolog = true,
                HomologToPreProductionAprove = false,
                PreProductionToProductionAprove = false,
                Flow = deploy.Flow,
                MajorVersion = deploy.MajorVersion,
                MinorVersion = deploy.MinorVersion,
                BuildNumber = deploy.BuildNumber
            }, commandType: CommandType.Text);

            log.Debug(string.Format("Pacote {0} está no servidor", deploy.GetPackagingName()));
            log.Debug(MessageSuccess());

        }

        public static string DevelopmentPathComplete(DeploySettings deploy, PathsDeploy app)
        {
            return string.Format("{0}{1}", deploy.GetDevelopmentPathBase(), app.DevelopmentPathComplete(deploy.DefineBranch()));
        }

        public static void PackagingServerStartWatcher(DeploySettings deploy)
        {
            ServerWatcher.StartWatcher(deploy);
        }

        public static void PackagingServerStartService(DeploySettings deploy)
        {
            ServerService.StartServiceDevelopmentToHomolog(deploy);
            ServerService.StartServicePreProductionToProduction(deploy);
        }

        public static string ReplaceAssemblysFoldersInPath(string path)
        {
            return path
                .Replace("bin", string.Empty)
                .Replace("Debug", string.Empty)
                .Replace("Release", string.Empty);
        }

        #region InternalProcess

        internal static void PackagingServerPathToValidation(DeploySettings deploy)
        {
            FactoryLog.GetInstace().Debug(string.Format("PackagingServerPathValidation - Descompactando arquivo {0}", deploy.GetPackagingName()));

            var pathPackagingServerName = Path.Combine(deploy.PackagingServerPathBase, deploy.GetPackagingName());
            if (!File.Exists(pathPackagingServerName))
            {
                FactoryLog.GetInstace().Debug(string.Format("O pacote {0} não foi encontrado", pathPackagingServerName));
                return;
            }


            ZipHelper.Unzip(pathPackagingServerName, deploy.PackagingServerPathBase, deploy.GetDevelopmentPathBase());
            BkpDatabase(deploy.ValidationSettings.ProcedureBkp, deploy.ValidationSettings.ConnectionString);
            DeployDatabase(deploy.ValidationSettings.ValidationPathBase, deploy.ValidationSettings.ScriptDatabasePath, deploy.ValidationSettings.ConnectionString);


            foreach (var app in deploy.Get())
            {
                FactoryLog.GetInstace().Debug(string.Format("PackagingServerPathToValidation - Deploy Aplicação {0}", app.Name));

                var pathSource = PackagingServerSource(deploy, app);
                var pathDestination = app.ValidationPath;

                var filesTransfer = GetFilesToProccess(app, Environment.Validation, pathSource, pathDestination, disableFileType: true);

                BkpFiles(Environment.Validation, deploy, app, filesTransfer.filesDestination, "app", "*.*");
                BkpFiles(Environment.Validation, deploy, app, filesTransfer.filesDestination, "log", "*.log");
                BkpFiles(Environment.Validation, deploy, app, filesTransfer.filesDestination, "report", "*.report");

                DeployFiles(deploy, Environment.Validation, pathSource, app.ValidationPath, app, filesTransfer.filesSource);

            }

            FactoryLog.GetInstace().Debug(string.Format("PackagingServerPathToValidation - Deploy em Validacao finalizado"));
        }
        internal static void PackagingServerPathToHomolog(DeploySettings deploy)
        {
            FactoryLog.GetInstace().Debug(string.Format("PackagingServerPathToHomolog - Descompactando arquivo {0}", deploy.GetPackagingName()));

            var pathPackagingServerName = Path.Combine(deploy.PackagingServerPathBase, deploy.GetPackagingName());
            if (!File.Exists(pathPackagingServerName))
            {
                FactoryLog.GetInstace().Debug(string.Format("O pacote {0} não foi encontrado", pathPackagingServerName));
                return;
            }


            ZipHelper.Unzip(pathPackagingServerName, deploy.PackagingServerPathBase, deploy.GetDevelopmentPathBase());
            BkpDatabase(deploy.HomologSettings.ProcedureBkp, deploy.HomologSettings.ConnectionString);
            DeployDatabase(deploy.HomologSettings.HomologPathBase, deploy.HomologSettings.ScriptDatabasePath, deploy.HomologSettings.ConnectionString);


            foreach (var app in deploy.Get())
            {
                FactoryLog.GetInstace().Debug(string.Format("PackagingServerPathToHomolog - Deploy Aplicação {0}", app.Name));

                var pathSource = PackagingServerSource(deploy, app);
                var pathDestination = app.HomologPath;

                var filesTransfer = GetFilesToProccess(app, Environment.Homolog, pathSource, pathDestination, disableFileType: true);

                BkpFiles(Environment.Homolog, deploy, app, filesTransfer.filesDestination, "app", "*.*");
                BkpFiles(Environment.Homolog, deploy, app, filesTransfer.filesDestination, "log", "*.log");
                BkpFiles(Environment.Homolog, deploy, app, filesTransfer.filesDestination, "report", "*.report");

                DeployFiles(deploy, Environment.Homolog, pathSource, app.HomologPath, app, filesTransfer.filesSource);

            }

            FactoryLog.GetInstace().Debug(string.Format("PackagingServerPathToHomolog - Deploy em Homologação finalizado"));
        }

        
        internal static void PackagingServerPathToPreProduction(DeploySettings deploy)
        {
            FactoryLog.GetInstace().Debug(string.Format("PackagingServerPathToPreProduction - Descompactando arquivo {0}", deploy.GetPackagingName()));

            var pathPackagingServerName = Path.Combine(deploy.PackagingServerPathBase, deploy.GetPackagingName());
            if (!File.Exists(pathPackagingServerName))
            {
                FactoryLog.GetInstace().Debug(string.Format("PackagingServerPathToPreProduction - O pacote {0} não foi encontrado", pathPackagingServerName));
                return;
            }

            ZipHelper.Unzip(pathPackagingServerName, deploy.PackagingServerPathBase, deploy.GetDevelopmentPathBase());
            BkpDatabase(deploy.PreProductionSettings.ProcedureBkp, deploy.PreProductionSettings.ConnectionString);
            DeployDatabase(deploy.PreProductionSettings.PreProductionPathBase, deploy.PreProductionSettings.ScriptDatabasePath, deploy.PreProductionSettings.ConnectionString);

            foreach (var app in deploy.Get())
            {
                var pathSource = string.Format("{0}\\{1}{2}", deploy.PackagingServerPathBase, Path.GetFileNameWithoutExtension(deploy.GetPackagingName()), app.DevelopmentPathComplete(deploy.DefineBranch()));
                var pathDestination = app.PreProductionPath;

                var filesTransfer = GetFilesToProccess(app, Environment.PreProduction, pathSource, pathDestination, disableFileType: true);

                BkpFiles(Environment.PreProduction, deploy, app, filesTransfer.filesDestination, "app", "*.*");
                BkpFiles(Environment.PreProduction, deploy, app, filesTransfer.filesDestination, "log", "*.log");
                BkpFiles(Environment.PreProduction, deploy, app, filesTransfer.filesDestination, "report", "*.report");

                DeployFiles(deploy, Environment.PreProduction, pathSource, app.PreProductionPath, app, filesTransfer.filesSource);
            }

            FactoryLog.GetInstace().Debug(string.Format("PackagingServerPathToPreProduction - Deploy em Pré produção finalizado finalizado"));

            var connectionStringDeploy = deploy.ConnectionStringDeploy;
            var deployName = deploy.DeployName;
            AdoNetHelper.ExecuteReader("update DeployQueue set Status=@StatusTo where deployName=@deployName and Status=@StatusFrom", connectionStringDeploy, new
            {
                StatusFrom = EStatusDeploy.DevelopmentToHomolog,
                StatusTo = EStatusDeploy.HomologToPreProduction,
                deployName = deploy.GetPackagingName(),
            }, CommandType.Text);
        }

        internal static void HomologToPreProduction(DeploySettings deploy)
        {
            var pathPackagingServerName = Path.Combine(deploy.PackagingServerPathBase, deploy.GetPackagingName());
            if (!File.Exists(pathPackagingServerName))
            {
                FactoryLog.GetInstace().Debug(string.Format("O pacote {0} não foi encontrado", pathPackagingServerName));
                return;
            }

            BkpDatabase(deploy.PreProductionSettings.ProcedureBkp, deploy.PreProductionSettings.ConnectionString);
            DeployDatabase(deploy.PreProductionSettings.PreProductionPathBase, deploy.PreProductionSettings.ScriptDatabasePath, deploy.PreProductionSettings.ConnectionString);

            foreach (var app in deploy.Get())
            {
                var pathSource = app.HomologPath;
                var pathDestination = app.PreProductionPath;

                var filesTransfer = GetFilesToProccess(app, Environment.PreProduction, pathSource, pathDestination, disableFileType: true);

                AddFilesEnvironment(app, deploy, filesTransfer, Environment.PreProduction);

                BkpFiles(Environment.PreProduction, deploy, app, filesTransfer.filesDestination, "app", "*.*");
                BkpFiles(Environment.PreProduction, deploy, app, filesTransfer.filesDestination, "log", "*.log");
                BkpFiles(Environment.PreProduction, deploy, app, filesTransfer.filesDestination, "report", "*.report");


                DeployFiles(deploy, Environment.PreProduction, pathSource, app.PreProductionPath, app, filesTransfer.filesSource);
            }

            var connectionStringDeploy = deploy.ConnectionStringDeploy;
            var deployName = deploy.DeployName;
            AdoNetHelper.ExecuteReader("update DeployQueue set Status=@StatusTo where deployName=@deployName and Status=@StatusFrom", connectionStringDeploy, new
            {
                StatusFrom = EStatusDeploy.DevelopmentToHomolog,
                StatusTo = EStatusDeploy.HomologToPreProduction,
                deployName = deploy.GetPackagingName(),
            }, CommandType.Text);


        }

        internal static void PreProductionToProduction(DeploySettings deploy)
        {
            var pathPackagingServerName = Path.Combine(deploy.PackagingServerPathBase, deploy.GetPackagingName());
            if (!File.Exists(pathPackagingServerName))
            {
                FactoryLog.GetInstace().Debug(string.Format("O pacote {0} não foi encontrado", pathPackagingServerName));
                return;
            }

            BkpDatabase(deploy.ProductionSettings.ProcedureBkp, deploy.ProductionSettings.ConnectionString);
            DeployDatabase(deploy.ProductionSettings.ProductionPathBase, deploy.ProductionSettings.ScriptDatabasePath, deploy.ProductionSettings.ConnectionString);

            foreach (var app in deploy.Get())
            {
                if (app.WindowsService)
                    WindowsServiceManager.StopService(app.WindowsServiceName, 5000);

                var pathSource = app.PreProductionPath;
                var pathDestination = app.ProductionPath;
                var filesTransfer = GetFilesToProccess(app, Environment.Production, pathSource, pathDestination);

                AddFilesEnvironment(app, deploy, filesTransfer, Environment.Production);

                BkpFiles(Environment.Production, deploy, app, filesTransfer.filesDestination, "app", "*.*");
                BkpFiles(Environment.Production, deploy, app, filesTransfer.filesDestination, "log", "*.log");
                BkpFiles(Environment.Production, deploy, app, filesTransfer.filesDestination, "report", "*.report");

                DeployFiles(deploy, Environment.Production, pathSource, app.ProductionPath, app, filesTransfer.filesSource);

                if (app.WindowsService)
                    WindowsServiceManager.StartService(app.WindowsServiceName, 2000);
            }

            var connectionStringDeploy = deploy.ConnectionStringDeploy;
            var deployName = deploy.DeployName;
            AdoNetHelper.ExecuteReader("update DeployQueue set Status=@StatusTo where deployName=@deployName and Status=@StatusFrom", connectionStringDeploy, new
            {
                StatusFrom = EStatusDeploy.HomologToPreProduction,
                StatusTo = EStatusDeploy.PreProductionToProduction,
                deployName = deploy.GetPackagingName(),
            }, CommandType.Text);
        }

        internal static void PreProductionToImplantation(DeploySettings deploy)
        {
            var pathPackagingServerName = Path.Combine(deploy.PackagingServerPathBase, deploy.GetPackagingName());
            if (!File.Exists(pathPackagingServerName))
            {
                FactoryLog.GetInstace().Debug(string.Format("Production to Implantation - O pacote {0} não foi encontrado", pathPackagingServerName));
                return;
            }

            BkpDatabase(deploy.ImplantationSettings.ProcedureBkp, deploy.ImplantationSettings.ConnectionString);
            DeployDatabase(deploy.ImplantationSettings.ImplantationPathBase, deploy.ImplantationSettings.ScriptDatabasePath, deploy.ImplantationSettings.ConnectionString);
            
            foreach (var app in deploy.Get())
            {
                var pathSource = app.ProductionPath;
                var pathDestination = app.ImplantationPath;

                var filesTransfer = GetFilesToProccess(app, Environment.Implantation, pathSource, pathDestination, disableFileType: true);
                AddFilesEnvironment(app, deploy, filesTransfer, Environment.Implantation);

                BkpFiles(Environment.Implantation, deploy, app, filesTransfer.filesDestination, "app", "*.*");
                BkpFiles(Environment.Implantation, deploy, app, filesTransfer.filesDestination, "log", "*.log");
                BkpFiles(Environment.Implantation, deploy, app, filesTransfer.filesDestination, "report", "*.report");

                DeployFiles(deploy, Environment.Implantation, pathSource, app.ImplantationPath, app, filesTransfer.filesSource);
            }

            FactoryLog.GetInstace().Debug(string.Format("Production to Implantation - Deploy em Implantation finalizado"));
        }

        #endregion

        #region helpers


        private static string PackagingServerSource(DeploySettings deploy, PathsDeploy app)
        {
            return string.Format("{0}\\{1}{2}", deploy.PackagingServerPathBase, Path.GetFileNameWithoutExtension(deploy.GetPackagingName()), app.DevelopmentPathComplete(deploy.DefineBranch()));
        }

        private static void DeleteLocalFiles(List<FileInfo> filesToDelete)
        {
            if (filesToDelete.IsAny())
            {
                foreach (var file in filesToDelete)
                    file.Delete();
            }
        }

        private static void SqlFileCopy(DeploySettings deploy, PathsDeploy app, FileInfo file, List<FileInfo> filesToDelete)
        {
            if (file.Extension.ToLower() == ".sql")
            {
                var parhDestinationSqlCopy = file.FullName.Replace("ProximoDeploy", string.Format("HistoricoDeploy\\{0}", deploy.GetPackagingNameWithoutExtension()));
                if (!Directory.Exists(Path.GetDirectoryName(parhDestinationSqlCopy)))
                    Directory.CreateDirectory(Path.GetDirectoryName(parhDestinationSqlCopy));

                file.CopyTo(parhDestinationSqlCopy);
                filesToDelete.Add(file);
            }
        }
        private static void FileEnvironment(DeploySettings deploy, Domain.Interfaces.ILog log, List<FileInfo> filesToZip, PathsDeploy app, string developmentPathComplete)
        {
            var fileEnvironmentHomolog = FileEnvironment(deploy, developmentPathComplete, Environment.Homolog);
            if (fileEnvironmentHomolog.Exists)
                filesToZip.Add(fileEnvironmentHomolog);

            var fileEnvironmentValidation = FileEnvironment(deploy, developmentPathComplete, Environment.Validation);
            if (fileEnvironmentValidation.Exists)
                filesToZip.Add(fileEnvironmentValidation);

            var fileEnvironmentImplantation = FileEnvironment(deploy, developmentPathComplete, Environment.Implantation);
            if (fileEnvironmentImplantation.Exists)
                filesToZip.Add(fileEnvironmentImplantation);

            var fileEnvironmentPreProduction = FileEnvironment(deploy, developmentPathComplete, Environment.PreProduction);
            if (fileEnvironmentPreProduction.Exists)
                filesToZip.Add(fileEnvironmentPreProduction);

            var fileEnvironmentProduction = FileEnvironment(deploy, developmentPathComplete, Environment.Production);
            if (fileEnvironmentProduction.Exists)
                filesToZip.Add(fileEnvironmentProduction);
        }
        private static void TransformationsConfig(Domain.Interfaces.ILog log, List<FileInfo> filesToZip, PathsDeploy app, string developmentPathComplete)
        {
            var developmentPathCompleteWithoutBin = ReplaceAssemblysFoldersInPath(developmentPathComplete);
            var webConfigHomolog = TransformationsConfig(app, developmentPathCompleteWithoutBin, Environment.Homolog);
            if (webConfigHomolog.Exists)
                filesToZip.Add(webConfigHomolog);
            else
                FactoryLog.GetInstace().Error(string.Format("Arquivo de config {0} não existe", webConfigHomolog));

            var webConfigValidation = TransformationsConfig(app, developmentPathCompleteWithoutBin, Environment.Validation);
            if (webConfigValidation.Exists)
                filesToZip.Add(webConfigValidation);
            else
                FactoryLog.GetInstace().Error(string.Format("Arquivo de config {0} não existe", webConfigValidation));


            var webConfigImplantation = TransformationsConfig(app, developmentPathCompleteWithoutBin, Environment.Implantation);
            if (webConfigImplantation.Exists)
                filesToZip.Add(webConfigImplantation);
            else
                FactoryLog.GetInstace().Error(string.Format("Arquivo de config {0} não existe", webConfigImplantation));

            var webConfigPreProduction = TransformationsConfig(app, developmentPathCompleteWithoutBin, Environment.PreProduction);
            if (webConfigPreProduction.Exists)
                filesToZip.Add(webConfigPreProduction);
            else
                FactoryLog.GetInstace().Error(string.Format("Arquivo de config {0} não existe", webConfigPreProduction));


            var webConfigProduction = TransformationsConfig(app, developmentPathCompleteWithoutBin, Environment.Production);
            if (webConfigProduction.Exists)
                filesToZip.Add(webConfigProduction);
            else
                FactoryLog.GetInstace().Error(string.Format("Arquivo de config {0} não existe", webConfigProduction));

        }
        private static string ReplaceEnvironmentInFile(string file, string environment)
        {
            return file.ToLower().Replace(environment.ToLower(), "").Replace("..", ".");
        }
        private static FileInfo TransformationsConfig(PathsDeploy app, string developmentPathComplete, string environment)
        {

            var fileConfig = "Web";
            var fileConfigOutput = "Web";
            if (app.AppConfig)
            {
                fileConfig = "App";
                fileConfigOutput = app.FileAppConfigName;
            }

            var pathTemplate = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "templates", "trans.tp");
            var fileContent = File.ReadAllText(pathTemplate);
            var pathTemp = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "filetemp");
            var pathTransform = string.Format("{0}\\{1}", developmentPathComplete, "transform");

            if (!Directory.Exists(pathTransform))
                Directory.CreateDirectory(pathTransform);

            var pathFileConfigOutput = string.Format("{0}\\{1}.{2}.config", pathTransform, fileConfigOutput, Environment.Config(environment));
            fileContent = fileContent.Replace("<#pathSource#>", string.Format("{0}\\{1}.config", developmentPathComplete, fileConfig));
            fileContent = fileContent.Replace("<#pathFileTransformation#>", string.Format("{0}\\{1}.{2}.config", developmentPathComplete, fileConfig, Environment.Config(environment)));
            fileContent = fileContent.Replace("<#pathDestination#>", pathFileConfigOutput);
            
            if (File.Exists(pathFileConfigOutput))
                File.Delete(pathFileConfigOutput);
            
            var transformationFile = string.Format("{0}.{1}", app.Name.Replace(" ", ""), "proj");
            var pathTransformationsFile = String.Format("{0}\\{1}", pathTemp, transformationFile);

            using (var stream = new StreamWriter(pathTransformationsFile))
            {
                stream.Write(fileContent);
            }

            var fileName = @"C:\windows\Microsoft.Net\Framework\v4.0.30319\MSBuild";
            var arguments = String.Format("{0}\\{1} /t:Deploy", pathTemp, transformationFile);

            HelperCmd.ExecuteApp(fileName, arguments);

            FactoryLog.GetInstace().Debug(string.Format("Arquivo de config {0} transformado", Path.GetFileName(pathFileConfigOutput)));

            var pathFileConfigOutputGenerated = new FileInfo(pathFileConfigOutput);
            if (!pathFileConfigOutputGenerated.Exists)
                throw new InvalidOperationException(string.Format("Arquivo de config {0} não pode ser transformado", Path.GetFileName(pathFileConfigOutput)));

            return pathFileConfigOutputGenerated;

        }
        private static FileInfo FileEnvironment(DeploySettings deploy, string developmentPathComplete, string environment)
        {
            var pathTemplate = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "templates", "deploysettings.tp");
            var fileContent = File.ReadAllText(pathTemplate);
            var pathTemp = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "filetemp");

            DeployVersion(deploy);

            fileContent = fileContent.Replace("<#DEPLOYENVIRONMENT#>", environment);
            fileContent = fileContent.Replace("<#DEPLOYVERSION#>", deploy.Version);
            fileContent = fileContent.Replace("<#DEPLOYAPP#>", deploy.DeployName);

            var fileEnvironmentTemp = string.Format("deploysettings.{0}.js", Environment.Config(environment));
            var pathFolderEnvironmentTemp = String.Format("{0}\\ini", developmentPathComplete);
            var pathFileEnvironmentTemp = String.Format("{0}\\{1}", pathFolderEnvironmentTemp, fileEnvironmentTemp);

            if (!Directory.Exists(pathFolderEnvironmentTemp))
                Directory.CreateDirectory(pathFolderEnvironmentTemp);

            using (var stream = new StreamWriter(pathFileEnvironmentTemp))
            {
                stream.Write(fileContent);
            }

            FactoryLog.GetInstace().Debug(string.Format("Arquivo de ambiente {0} transformado", Path.GetFileName(pathFileEnvironmentTemp)));
            return new FileInfo(pathFileEnvironmentTemp);

        }
        private static void DeployVersion(DeploySettings deploy)
        {
            var deployName = deploy.GetPackagingNameWithoutExtension();
            var connectionStringDeploy = deploy.ConnectionStringDeploy;

            var version = deploy.VersionInitial;
            var versionParts = version.Split('.');

            var MajorVersion = Convert.ToInt32(versionParts[0]);
            var MinorVersion = Convert.ToInt32(versionParts[1]);
            var BuildNumber = Convert.ToInt32(versionParts[2]);


            var resultDeploys = AdoNetHelper.ExecuteReader("Select top 1 * from DeployQueue where Status=@Status and MajorVersion=@MajorVersion order by id desc", connectionStringDeploy, new
            {
                DeployName = deployName,
                Status = (int)EStatusDeploy.PreProductionToProduction,
                MajorVersion = MajorVersion
            }, commandType: CommandType.Text);


            var lastDeployVersion = BuildNumber;
            if (resultDeploys.Count() > 0)
            {
                var lastDeploy = resultDeploys.LastOrDefault();
                if (lastDeploy != null)
                    lastDeploy.BuildNumber = lastDeploy.BuildNumber + 1;

                lastDeployVersion = lastDeploy.BuildNumber;
            }
            else
                lastDeployVersion = lastDeployVersion + 1;

            var DeployName = deploy.DeployName.Replace(" ", "");
            var deploypackging = deploy.GetPackagingNameWithoutExtension().Replace(DeployName, "").Replace("-Packaging-", "");
            deploy.Version = string.Format("{0}.{1}.{2} Atualizada em [<#DATA_TRANSFER_ENVIRONMENT#>] - Pacote [{3}]", MajorVersion, MinorVersion, lastDeployVersion, deploypackging);

            deploy.MajorVersion = Convert.ToInt32(versionParts[0]);
            deploy.MinorVersion = Convert.ToInt32(versionParts[1]);
            deploy.BuildNumber = lastDeployVersion;
        }
        private static PathCopySettings GetFilesToProccess(PathsDeploy app, string environment, string pathSource = null, string pathDestination = null, bool disableFileType = false)
        {
            var pathCopySettings = new PathCopySettings();
            var fileType = disableFileType ? "*.*" : app.FileType;

            if (app.Recursive)
            {
                if (!pathSource.IsNullOrEmpaty())
                {
                    var filesSourceTemp = new List<FileInfo>();
                    DirSearch(pathSource, fileType, filesSourceTemp);
                    pathCopySettings.filesSource = filesSourceTemp.ToArray();
                }

                if (!pathDestination.IsNullOrEmpaty())
                {
                    var filesDestinationTemp = new List<FileInfo>();
                    DirSearch(pathDestination, fileType, filesDestinationTemp);
                    pathCopySettings.filesDestination = filesDestinationTemp.ToArray();
                }


                return pathCopySettings;
            }

            if (!pathSource.IsNullOrEmpaty())
            {
                if (!Directory.Exists(pathSource))
                    Directory.CreateDirectory(pathSource);

                pathCopySettings.filesSource = new DirectoryInfo(pathSource).GetFiles(fileType);
            }

            if (!pathDestination.IsNullOrEmpaty())
            {
                if (!Directory.Exists(pathDestination))
                    Directory.CreateDirectory(pathDestination);

                pathCopySettings.filesDestination = new DirectoryInfo(pathDestination).GetFiles(fileType);
            }

            return pathCopySettings;
        }
        private static PathCopySettings AddFilesEnvironment(PathsDeploy app, DeploySettings deploy, PathCopySettings settings, string environment)
        {
            var pathSource = string.Format("{0}\\{1}{2}", deploy.PackagingServerPathBase, Path.GetFileNameWithoutExtension(deploy.GetPackagingName()), app.DevelopmentPathComplete(deploy.DefineBranch()));

            if (app.FileType == "*.js")
            {
                var pathDeploySettings = new FileInfo(string.Format("{0}\\ini\\DeploySettings.{1}.js", pathSource, Environment.Config(environment)));
                if (pathDeploySettings.Exists)
                {
                    settings.AddFileToSorce(pathDeploySettings);
                    FactoryLog.GetInstace().Debug(string.Format("Arquivo de ambiente {0} adicionado", pathDeploySettings));
                }
                else
                {
                    FactoryLog.GetInstace().Debug(string.Format("Arquivo de ambiente {0} não encontrado no package path", pathDeploySettings));
                }
            }
            return settings;
        }

        private static void SetDateTransferEnvironment(FileInfo pathDeploySettings)
        {
            var fileText = File.ReadAllText(pathDeploySettings.FullName);
            if (fileText.IsNotNull())
            {
                fileText = fileText.Replace("<#DATA_TRANSFER_ENVIRONMENT#>", DateTime.Now.ToString());

                using (var str = new StreamWriter(pathDeploySettings.FullName))
                {
                    str.Write(fileText);
                }
            }
        }

        private static void DirSearch(string directory, string fileType, List<FileInfo> filesFound)
        {
            try
            {
                foreach (var d in new DirectoryInfo(directory).GetDirectories())
                {
                    foreach (var f in d.GetFiles(fileType))
                    {
                        filesFound.Add(f);
                    }
                    DirSearch(d.FullName, fileType, filesFound);
                }
            }
            catch (System.Exception excpt)
            {
                Console.WriteLine(excpt.Message);
            }
        }
        private static void DeployDatabase(string basePath, string scriptDatabasePath, string connectionString)
        {
            if (connectionString.IsNullOrEmpaty())
                return;

            if (scriptDatabasePath.IsNullOrEmpaty())
                return;

            var scriptsPathBase = string.Format("{0}\\{1}", basePath, scriptDatabasePath);
            if (!Directory.Exists(scriptsPathBase))
                return;

            var files = new DirectoryInfo(scriptsPathBase).GetFiles();
            if (files.NotIsAny())
                return;

            FactoryLog.GetInstace().Debug(string.Format("Deploy Database Iniciado"));
            foreach (var file in files)
            {
                try
                {
                    AdoNetHelper.ExecuteNonQuery(file.FullName, connectionString, null, commandType: CommandType.Text);
                    FactoryLog.GetInstace().Debug(string.Format("Script {0} executado com sucesso", file.Name));
                }
                catch (Exception ex)
                {
                    FactoryLog.GetInstace().Error(string.Format("Erro ao executar o script {0} -[1]", file.Name, ex.Message), ex);
                }

            }
        }
        private static void BkpDatabase(string procedureBkp, string connectionString)
        {
            if (connectionString.IsNullOrEmpaty())
                return;

            if (procedureBkp.IsNullOrEmpaty())
                return;
            try
            {
                FactoryLog.GetInstace().Debug(string.Format("BkpDatabase Iniciado"));
                AdoNetHelper.ExecuteNonQuery(procedureBkp, connectionString, null, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                FactoryLog.GetInstace().Error(string.Format("Erro ao executar o bkp do banco {0} -[1]", procedureBkp, ex.Message), ex);
                throw ex;
            }

        }
        private static void BkpFiles(string environment, DeploySettings deploy, PathsDeploy app, FileInfo[] files, string folder, string filterExtensions)
        {
            if (files.NotIsAny())
                return;

            var filesFilter = filterExtensions == "*.*" ? files : files.Where(_ => _.Extension == filterExtensions.Replace("*", ""));
            var pathBase = string.Empty;
            var bkpPath = string.Empty;

            if (environment == Environment.Homolog)
            {
                pathBase = deploy.HomologSettings.HomologPathBase;
                bkpPath = deploy.HomologSettings.BkpPath;
            }

            if (environment == Environment.Validation)
            {
                pathBase = deploy.ValidationSettings.ValidationPathBase;
                bkpPath = deploy.ValidationSettings.BkpPath;
            }

            if (environment == Environment.Implantation)
            {
                pathBase = deploy.ImplantationSettings.ImplantationPathBase;
                bkpPath = deploy.ImplantationSettings.BkpPath;
            }

            if (environment == Environment.PreProduction)
            {
                pathBase = deploy.PreProductionSettings.PreProductionPathBase;
                bkpPath = deploy.PreProductionSettings.BkpPath;
            }

            if (environment == Environment.Production)
            {
                pathBase = deploy.ProductionSettings.ProductionPathBase;
                bkpPath = deploy.ProductionSettings.BkpPath;
            }

            FactoryLog.GetInstace().Debug(string.Format("Bkp da applicacao {0} Iniciado", app.Name));

            var destinationBkp = string.Format("{0}\\{1}\\{2}", bkpPath, folder, deploy.GetPackagingNameWithoutExtension());
            if (!Directory.Exists(destinationBkp))
                Directory.CreateDirectory(destinationBkp);

            foreach (var file in filesFilter)
            {
                var directoryDiff = file.Directory.ToString().Replace(pathBase, string.Empty);
                var destinationPathNew = string.Format("{0}\\{1}", destinationBkp, directoryDiff);

                if (!Directory.Exists(destinationPathNew))
                    Directory.CreateDirectory(destinationPathNew);

                TranferFile(destinationPathNew, file);
            }

            return;
        }
        private static FileInfo TranferFile(string destination, FileInfo file)
        {
            try
            {
                var destinationFile = string.Format("{0}\\{1}", destination, file.Name);
                return file.CopyTo(destinationFile, true);
            }
            catch (Exception ex)
            {
                FactoryLog.GetInstace().Error(ex.Message, ex);
            }
            return file;

        }
        private static FileInfo TranferFileEnvironment(string destination, FileInfo file, string environment)
        {
            SetDateTransferEnvironment(file);
            var destinationFile = string.Format("{0}\\{1}", destination, ReplaceEnvironmentInFile(file.Name, environment));
            FactoryLog.GetInstace().Debug(string.Format("Copiando arquivo de Ambiente {0} para {1}", file.FullName, destinationFile));
            return file.CopyTo(destinationFile, true);
        }
        private static FileInfo TranferFileConfig(string destination, FileInfo file, string environment)
        {
            var destinationFile = string.Format("{0}\\{1}", ReplaceAssemblysFoldersInPath(destination), ReplaceEnvironmentInFile(file.Name, environment));
            FactoryLog.GetInstace().Debug(string.Format("Copiando arquivo de Config {0} para {1}", file.FullName, destinationFile));
            return file.CopyTo(destinationFile, true);
        }
        private static void DeployFiles(DeploySettings deploy, string environment, string pathSource, string destinationPath, PathsDeploy app, FileInfo[] files)
        {
            FactoryLog.GetInstace().Debug(string.Format("Deploy da applicacao {0} Iniciado pathSource={1} , destinationPath={2}, Files={3}", app.Name, pathSource, destinationPath, files.Count()));

            if (files.NotIsAny())
                return;

            var destinationPathNew = destinationPath;

            foreach (var file in files)
            {
                if (app.Recursive)
                {

                    if (file.Name.ToLower() == String.Format("deploysettings.{0}.js", Environment.Config(environment)).ToLower())
                    {
                        destinationPathNew = string.Format("{0}\\{1}", destinationPath, "ini");
                    }
                    else
                    {
                        if (file.Directory.ToString().Contains(pathSource))
                        {
                            var directoryDiff = file.Directory.ToString().Replace(pathSource, string.Empty);
                            destinationPathNew = string.Format("{0}{1}", destinationPath, directoryDiff);
                        }
                    }

                    if (!Directory.Exists(destinationPathNew))
                        Directory.CreateDirectory(destinationPathNew);
                }



                if (file.Name.ToLower() == String.Format("deploysettings.{0}.js", Environment.Config(environment)).ToLower())
                    TranferFileEnvironment(destinationPathNew, file, Environment.Config(environment));
                else
                {
                    if (NotExistsInBlackList(app, file, environment))
                        TranferFile(destinationPathNew, file);
                }
            }
            var packagingServerSource = PackagingServerSource(deploy, app);
            if (app.WebConfig)
            {
                var webconfigPath = string.Format("{0}\\transform\\", ReplaceAssemblysFoldersInPath(packagingServerSource));
                var webconfigFile = String.Format("Web.{0}.config", Environment.Config(environment)).ToLower();
                var webconfig = string.Format("{0}{1}", webconfigPath, webconfigFile);

                if (File.Exists(webconfig))
                    TranferFileConfig(destinationPathNew, new FileInfo(webconfig), Environment.Config(environment));
                else
                    FactoryLog.GetInstace().Debug(string.Format("Não encontrado arquivo de config {0} no path {1}", webconfigFile, webconfigPath));
            }

            if (app.AppConfig)
            {
                var appconfigPath = string.Format("{0}\\transform\\", ReplaceAssemblysFoldersInPath(packagingServerSource));
                var appconfigFile = String.Format("{0}.{1}.config", app.FileAppConfigName, Environment.Config(environment)).ToLower();
                var appconfig = string.Format("{0}{1}", appconfigPath, appconfigFile);

                if (File.Exists(appconfig))
                    TranferFileConfig(destinationPathNew, new FileInfo(appconfig), Environment.Config(environment));
                else
                    FactoryLog.GetInstace().Debug(string.Format("Não encontrado arquivo de config {0} no path {1}", appconfigFile, appconfigPath));

            }
        }
        private static bool NotExistsInBlackList(PathsDeploy app, FileInfo file, string environment)
        {
            var filesBlackListEnvironment = BlackListEnvironment(environment);
            var filesBlackListAppConfig = BlackListAppConfig(app, environment);
            var fileBlackListUser = FileBlackListUser(app);

            return !filesBlackListEnvironment.Contains(file.Name.ToLower())
                && !filesBlackListAppConfig.Contains(file.Name.ToLower())
                && !fileBlackListUser.Contains(file.Name.ToLower());
        }

        private static IEnumerable<string> FileBlackListUser(PathsDeploy app)
        {
            return app.BlackListFile.Select(_ => _.ToLower());
        }

        private static string[] BlackListEnvironment(string environment)
        {
            var environmentPrincipal = String.Format("deploysettings.js").ToLower();
            var environmentActual = String.Format("deploysettings.{0}.js", Environment.Config(environment)).ToLower();
            var environmentHomolog = String.Format("deploysettings.{0}.js", Environment.Config(Environment.Homolog)).ToLower();
            var environmentImplantation = String.Format("deploysettings.{0}.js", Environment.Config(Environment.Implantation)).ToLower();
            var environmentPreProduction = String.Format("deploysettings.{0}.js", Environment.Config(Environment.PreProduction)).ToLower();
            var environmentRelease = String.Format("deploysettings.{0}.js", Environment.Config(Environment.Production)).ToLower();
            
            var environments = new string[]
                    {
                        environmentPrincipal,
                        environmentActual,
                        environmentHomolog,
                        environmentImplantation,
                        environmentPreProduction,
                        environmentRelease
                    };
            return environments;
        }
        private static string[] BlackListAppConfig(PathsDeploy app, string environment)
        {
            var appConfigPrincipal = String.Format("{0}.config", app.FileAppConfigName).ToLower();
            var appConfigActual = String.Format("{0}.{1}.config", app.FileAppConfigName, Environment.Config(environment)).ToLower();
            var appConfigHomolog = String.Format("{0}.{1}.config", app.FileAppConfigName, Environment.Config(Environment.Homolog)).ToLower();
            var appConfigImplantation = String.Format("{0}.{1}.config", app.FileAppConfigName, Environment.Config(Environment.Implantation)).ToLower();
            var appConfigPreProduction = String.Format("{0}.{1}.config", app.FileAppConfigName, Environment.Config(Environment.PreProduction)).ToLower();
            var appConfigRelease = String.Format("{0}.{1}.config", app.FileAppConfigName, Environment.Config(Environment.Production)).ToLower();

            var environments = new string[]
                    {
                       appConfigPrincipal,
                        appConfigActual,
                        appConfigHomolog,
                        appConfigImplantation,
                        appConfigPreProduction,
                        appConfigRelease
                    };
            return environments;
        }


        private static string MessageSuccess()
        {
            return @"
``.```-+s+:-..```.-:/:.``..````.::` `..  ```````..`     ..`````...::-`  ``````````--........-...`.--
..... `-:/:...`````---:/--:::--.:++syyyysso+///+++/:.`  .-`  `.---:-.`````````...`.`.--------.-::---
--.--..` `..`  `.....:+/:--./sdNNMMMMMMMMMMMMMMMMMNd+-...`   `://::-..`````...-:://--/++:----.-::-..
::-....```...`..-.`.::---/yNMMMMMMMMMMMMMMMMMMMMMMMMd/-.` ``./oshhs/:-..........-+o+:----....-.....`
--.``...---...-::---...-hNMMMMMMMMMMMMMMMMMMMMMMMMMMMd/-``..-+shdhs/-.```......-/++++++//:....``.::-
...``..::--...-:--.`.-:mMMMMMMMMMMMMMMMMMMMMMMMMMMMMMNd+----:+oo+/+/..`..--...-::/----::-:--:-......
-....---.----::-.....-yMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMNdo:--:::--.--`.///------:--.``.--:///+///:-..
--.:/++:...:/::-...-..mMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMmho:-..--..--../+/::--.---::::/+o/---:://:---
--.-:///::-::-::...../MMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMmyo/-..-----.`-+//:-...---:::+so/:-......`..
:---/+//--``.:/+/--..yMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMdso/-..----.`.//::-.``.-----/+::::.``..``..
----:-...-:....----..hMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMNdyo/-..-:--.-:--::-...---..-----..``..````
```..-:/+:--.....-...mMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMNdyo/-.----....-::-----:-.`..---...`..````
`.-..-/+/.`....---///NMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMNmhs+:.....---::---.----.`..--...`````````
.::...-:-.`.-.--.`-/oMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMNmhs/---:/++:--::..-:::-.-//-````````````
.-``.--.....-::-.-/+oMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMNdyo:-:://:-.---..:++-.--::-````````.```
/o+.:s+:--...-://///sMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMmhso+/:.-:/+/-...:/sddy:---::.````````.-.-
+osoooo/+syhdmmNNNmmNMMMMMMMMMMNMMMMMMMMMMMMMMMMMMMMMMMMNNmho/::-..:::::/oyhhs+/-:/oo:..````````.---
+sssydmNMMMMMMMMMMMMMMMMMMMMMMNNmmmmNNNNMMMMMMMMNmmhys++/:::---------:+yhs+/:/+syhy/-.``````````----
ydmNMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMNNNNNmmmmmmdyo+/////+//++++oooosssssyyhddmmdhso/-..`````````.---.
dNMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMNNMNMMNNNNmmmmmdddmddddddmmmNmmMMMMMMNNdy+:--://:..`````````.-..`
MMMMMMMMMMMMMMMMMMMMMMMMMMMMMMNmddddddddmmmmmmdddddhhhhhhhdmNNmmmdNMNNdy+:-..--::::-..`````````-:.``
MMMMMMMMMMMMMMMMMMMMMMMMMMMMMNmhhhhhhhhhhddddmmdhhhyyyyyhhdmmmdhhyho++:-:/:----:-----.````````.+/-.`
MMMMMMMMMMMMMMMMMMMMMMMMMMMMMNmhhyyysssysyhdhhyhyyysyyssyhdhhyyhyoo:-...::.`.--...-----..```.:/+:...
hNMMMMMMMMMMMMMMMMMMNNNMMMMMMNdhysssssssssssssssysyyyyyyssssso+oo++o+-.//.```.....------.``.-/o/-.-:
.:odNMMMMMMMMMMMMMMNmmmNNdmNNNdysssssssssooooooossssysyssoosooo++/-os/--.`...---...-:/:.```.-:/:--:+
....:shmNMMMMMMMMMMNhhyddhhdmmdyssssssooooooooooooosssssso/++++///.-/+-```.-.....-/++:-...--://::://
..--/+//oydmNNMMMMMMmsoyhyhhddhysssoooooooo+++++oossssyysso+oo+///.`./:.``.-----.--------://++::/:/:
.```-/++oo+/:/+osymMMmssyhdyhdhhysssoooooooo+++ooosyyyssssoo/oo+/:-:/-....-:/+//-://:----::/+/::::-.
-....-/oso+://:--:oNMMdsssyhhhhhysssssooooo++oooosssssoo+oo++sso+:-/:...-:+ooo++://:--...-:://://:..
-//:::+ysoo+o/:-../NMMMmysosyhhhhysssssosoooooooosyyyyyyyyyysyss+-.--:/+++//////o+:--::-..:+//++:-.-
.--/::shhyo----::::dMMMMMdyosyhhhhysssssooooooossyyyhyyhdhyhhhys/`...-+s/--/:/:::////:--::/+oo+:-..-
/...:+oos/:-.-:--..+NNMMMMNyyydhhhhyyssoooooosyyyhhhyyyhddhhhhs:.`-.--:oo..::----//+/::-:+ooo/:---:/
::..:o:-:.--.-+/-.:+yNNNNMNyyhddddhyyyyssooshhhhhyhhhyhyhysssss/-.//:--/+:-::---:////o++/oso:---://:
`.:++o:--.--.`-+//+ooyNMNMNsshdmhhdyyyyyssshyhyyyyyyyyyyyss+/oy+.----.-::::::-:::::+sso/+ss+:-://:::
`.:ss+so:..:/::/+oyo/+oddNNssyhddhhhhhhyyyshhhsoooo+ooooo++/-/s/-:-..-::--/o/::/::/oss+/+so////:::::
.-/yyyhs/::---:+shysoos+---ssssshddhhhhhhyyhhysssssooosyysso//:.-:::://---/s+/::::+oossyyo/:----:::/
-/oo+yy/+oso//+/oyo++soo++odssoooyhddddhdhhhhhhyyyyyhhdddhhyo-..-:///:---:+so+/:/oso++yhyo+/:---::/:
/--++soo+ossyyyoyys+/+++hm+hssoooosyhmdddddhhhhhhhhyhhhyyyy+-....:++/:---:os++/:/oso+/syys+/:///::--
//ooos/:+++yhs++so:.../yNd/odyssssooshdddmmddhhhyyyyyyyyyyso/-++..//:----+yooo+//+ss+oyhyo+////:-...
./oyhy/:-.+hhso+:-:-.-hNMN+/sdhyysssssyhdmNmmdhhhyyhhhhhhhysoooo+..::--.:+o+os+//oysoydhyo++/:--..--
-+/oso++/-:hdy+-..--:dMMMMo//sddhyyyyssssyhhddddddddddhdhhdmmdsos+.``...:+/+ss+/::+ooydyso++/::-----
+o+oo:/oyyshdhs/-:/omMMMMMy///+hdhhyyyyssyyyyyyyyhhhhoohhddmNdysso:/+``....-::::--:shdhyso+/+///::::
+:-/+:+sooso+syydmNMMMMMMMm//+//ohhhyyysssyyyyyyysyyyyyyhdddNdssoo/:o..`...-:..```.-:+oooo//+////+//
y+/shdysoo/::/dMMMMMMMMMMMMhoo+//sosyyyyssssyyyyyyyyhyyhhdddNhyso+/+ss+.`.-:/::..```......-://++o/::
smmmdy/---+ydNMMMMMMMMMMMMMNdhyooN+:/+syssssyyyyyyyyyyhhhhhdmyoo+++ymNhs+:.-/oo-...``./ooo:`.:/+/::/
dmd+:..-+hmNNMMMMMMMMMMMMMMNNmmhody:///+ssssyyyyyyyyyyhhhhhmd+ohs:s/odmmdh/:-/s+.::.```-+dms-+:..:+y
Nm+.-+ydNNNNMMMMMMMMMMMMMMMmhmddhodmo/+//ysssyyyyyyyyyyyyhdms/yh--/:-+mmmmhhs:+:.:hy/.```-odh/s+``:/
mhsydNNMMMNMMMMMMMMMMMMMMMMMhyhhhhshho////ossyyyyyssyyyyhhdo-/s/...:-/mmhyydmdyo+/oyysso/--/hd++:.++
mNMMMMMMMMNMMMMMMMMMMMMMMMMMhysssyo+ooo+///:+oyyyyyyyyyyhd+.-::.....:+mmyo:omNddhoo/yNMMNNmddNmo/`ym
MMMMMMMMNNMMMMMMMMMMMMMMMMMMhsys+ooso+oo+++//:/osyyyyyhhds....``----:oNNds+.-odmhyo+:+dMMMMMMMMNs.+m
MMMMMMMMNMMMMMMMMMMMMMMNNMMMMmsssosyysoo+++++++/+osyyyhhd:```..`--:::+mNNNdo-.:/hmmshmosNMMMMMMMNy-y
MMMMMMNNMMMMMMMMNNNmmNNmNMMMMMNsoysssys+oo//////+//oyhhdh`````.`--:/++yNMMMNy/.`.:+/MMNhdMMMMMMMMm+-
MMMMMhhdmmNNmmmmdmNdmNMNNMMMMMMNyoosooys++++///::////ohdy   `.``.-::+oomMMMMMms-```/NMMMNNMMMMMMMMh-
MMMMMMMNNNNNNNNMMMMNmNNMMMMMMMMMNy+os++yy///////:::-::+hh   `` `--://+syNMMMMMNms:.-+mMMMMMNmdNMMMNs
MMMMMMMMMMMMMMMMMMMMdmMhdMMMMMMMMNy++o+oyy/:-::///.`.::/y`   ` .//://++shNMMMMMMNm:o.:mMMNho/-:/ohNm
MMMMMMMMMMMMMMMMMMMMMdmhsNMMMMMMMMNy++o+oss/:---://::/::+-    `:++://++oydMMMMMMMm:/.`-hNh+ys-.-`.+m
MMMMMMMMMMMMMMMMMMMMMMdNMMMMMMMMMMMNs/++/+os/---:://++++o+`   -//+/:/+++oymMMMMMMysms-`.ysoys+/:/s`/
MMMMMMMMMMMMMMMMMMMMMMNmMMMMMMMMMMMMNo+++/+oo:---:::/++osh..`.::/+/://+++oyNNydMM/dMMd/`.ssshy+so/./
MMMMMMMMMMMMMMMMMMMMMMMmNMMMMMMMMMMMMmo++//+oo:---::::::/s:.--::/+////++oooyNhmMN:NMMMNs--sy//++//od
MMMMMMMMMMMMMMMMMMMMMMMMmNMMMMMMMMMMMMmo/+//+o+----::--:/o+--::/:///::/+oooshNMMm/NMMMMmh/`:o/shdymh
MMMMMMMMMMMMMMMMMMMMMMMMMNNMMMMMMMMMMMMm+///+oo:.---::-:/+y--:::///::://oososdNNm+:dNNmh+++.:/++dmNM
MMMMMMMMMMMMMMMMMMMMMMMMMMmNMMMMMMMMMMMNd////++:-----:-:/+h--:::///::::/+ooooyNNmh/.sNddmmdoo/.`/mNN
MMMMMMMMMMMMMMMMMMMMMMMMMMMNNMMNMNMMMMMMNd+/:://-..-----:oh/-::::::/::::/+oooohNNm/-+mNNNNmysy/:`.dN
NMMMMMMMMMMMMMMMMMMMMMMMMMMMMNNmdmMMNMMMNNh+:-//-..------:oo-:::---::::::/+oo++hNNy`.oNNhs+//.-..``+
";
        }


        #endregion

    }
}
