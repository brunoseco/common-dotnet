using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Deploy
{
    public class DeploySettings
    {
        private List<PathsDeploy> destinantionPaths;
        private string _packagingNamePath;
        private string _developmentPathBase;

        public DeploySettings()
        {
            this.destinantionPaths = new List<PathsDeploy>();

        }

        public void CreatePackagingName(string deployName)
        {
            this._packagingNamePath = string.Format("{0}-Packaging-{1}.zip", deployName.Replace(" ", ""), DateTime.Now.ToString("dd-MM-yyyy-HH-mm"));
        }

        public string DefineBranch()
        {

            if (this.Flow == EFlow.FixInProduction)
                return ConfigurationManager.AppSettings["BranchFixInProduction"];

            if (this.Flow == EFlow.Validation)
                return ConfigurationManager.AppSettings["BranchValidation"];


            return ConfigurationManager.AppSettings["BranchUpdates"];

        }
        public string GetPackagingName()
        {
            return this._packagingNamePath;
        }

        public string GetPackagingNameWithoutExtension()
        {
            return Path.GetFileNameWithoutExtension(this._packagingNamePath);
        }

        public void SetPackagingName(string packagingNamePath)
        {
            this._packagingNamePath = packagingNamePath;
        }

        public DeploySettings(EPathBase pathBase, IEnumerable<PathsDeploy> destinantionPaths)
            : this()
        {

            var developmentPathBase = ConfigurationManager.AppSettings["developmentPathBase"];

            if (pathBase == EPathBase.publish)
                developmentPathBase = ConfigurationManager.AppSettings["publishPathBase"];

            this.SetDevelopmentPathBase(developmentPathBase);

            foreach (var item in destinantionPaths)
            {
                this.destinantionPaths.Add(new PathsDeploy
                {
                    Name = item.Name,
                    FileType = item.FileType,
                    Recursive = item.Recursive,
                    WebConfig = item.WebConfig,
                    AppConfig = item.AppConfig,
                    FileAppConfigName = item.FileAppConfigName,
                    DevelopmentPath = item.DevelopmentPath,
                    DevelopmentPathBase = item.DevelopmentPathBase,
                    ImplantationPath = this.ImplantationSettings.ImplantationPathBase + item.ImplantationPath,
                    HomologPath = this.HomologSettings.HomologPathBase + item.HomologPath,
                    ValidationPath = this.ValidationSettings.ValidationPathBase + item.ValidationPath,
                    PreProductionPath = this.PreProductionSettings.PreProductionPathBase + item.PreProductionPath,
                    ProductionPath = this.ProductionSettings.ProductionPathBase + item.ProductionPath,
                    WindowsService = item.WindowsService,
                    WindowsServiceName = item.WindowsServiceName,
                    BlackListFile = item.BlackListFile,
                    ConnectionStringName = item.ConnectionStringName,
                });

            }

        }

        private void SetDevelopmentPathBase(string developmentPathBase)
        {
            this._developmentPathBase = developmentPathBase;
        }
        public string GetDevelopmentPathBase()
        {
            return this._developmentPathBase;
        }

        public string PackagingClientPathBase
        {
            get
            {
                return ConfigurationManager.AppSettings["packagingClientPathBase"];
            }
        }

        public string PackagingServerPathBase
        {
            get
            {
                return ConfigurationManager.AppSettings["packagingServerPathBase"];
            }
        }

        public string FtpHosting
        {
            get
            {
                return ConfigurationManager.AppSettings["ftpHosting"];
            }
        }

        public string FtpUser
        {
            get
            {
                return ConfigurationManager.AppSettings["ftpUser"];
            }
        }

        public string FtpPassword
        {
            get
            {
                return ConfigurationManager.AppSettings["ftpPassword"];
            }
        }

        public string DeployName
        {
            get
            {
                return ConfigurationManager.AppSettings["deployName"];
            }
        }

        public EFlow Flow { get; set; }

        public string ConnectionStringDeploy
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["connectionStringDeploy"].ToString();
            }
        }

        public string VersionInitial
        {
            get
            {
                return ConfigurationManager.AppSettings["VersionInitial"].ToString();
            }
        }

        public bool VersionEnable
        {
            get
            {
                return Convert.ToBoolean(ConfigurationManager.AppSettings["VersionEnable"]);
            }
        }

        public string Version { get; set; }
        public int MajorVersion { get; set; }
        public int MinorVersion { get; set; }
        public int BuildNumber { get; set; }

        public IEnumerable<PathsDeploy> Get()
        {
            return this.destinantionPaths;
        }

        public Implantation ImplantationSettings
        {
            get
            {
                return new Implantation();
            }
        }

        public Homolog HomologSettings
        {
            get
            {
                return new Homolog();
            }
        }

        public Validation ValidationSettings
        {
            get
            {
                return new Validation();
            }
        }

        public PreProduction PreProductionSettings
        {
            get
            {
                return new PreProduction();
            }
        }

        public Production ProductionSettings
        {
            get
            {
                return new Production();
            }
        }

        public class Implantation
        {
            public string BkpPathBase
            {
                get
                {
                    return ConfigurationManager.AppSettings["bkpPathBase"];
                }
            }
            public string BkpPath
            {
                get
                {
                    return string.Format("{0}\\{1}", this.BkpPathBase, "Implantation");
                }
            }

            public string ConnectionString
            {
                get
                {
                    return ConfigurationManager.ConnectionStrings["Implantation"].ToString();
                }
            }

            public string ProcedureBkp
            {
                get
                {
                    return ConfigurationManager.AppSettings["procedureBkpImplantation"].ToString();
                }
            }

            public string ScriptDatabasePath
            {
                get
                {
                    return ConfigurationManager.AppSettings["scriptDatabasePathImplantation"].ToString();
                }
            }

            public string ImplantationPathBase
            {
                get
                {
                    return ConfigurationManager.AppSettings["implantationPathBase"].ToString();
                }
            }


        }

        public class Homolog
        {
            public string BkpPathBase
            {
                get
                {
                    return ConfigurationManager.AppSettings["bkpPathBase"];
                }
            }
            public string BkpPath
            {
                get
                {
                    return string.Format("{0}\\{1}", this.BkpPathBase, "Homolog");
                }
            }

            public string ConnectionString
            {
                get
                {
                    return ConfigurationManager.ConnectionStrings["Homolog"].ToString();
                }
            }

            public string ProcedureBkp
            {
                get
                {
                    return ConfigurationManager.AppSettings["procedureBkpHomolog"].ToString();
                }
            }

            public string ScriptDatabasePath
            {
                get
                {
                    return ConfigurationManager.AppSettings["scriptDatabasePathHomolog"].ToString();
                }
            }

            public string HomologPathBase
            {
                get
                {
                    return ConfigurationManager.AppSettings["homologPathBase"].ToString();
                }
            }


        }


        public class Validation
        {
            public string BkpPathBase
            {
                get
                {
                    return ConfigurationManager.AppSettings["bkpPathBase"];
                }
            }
            public string BkpPath
            {
                get
                {
                    return string.Format("{0}\\{1}", this.BkpPathBase, "Validation");
                }
            }

            public string ConnectionString
            {
                get
                {
                    return ConfigurationManager.ConnectionStrings["Validation"].ToString();
                }
            }

            public string ProcedureBkp
            {
                get
                {
                    return ConfigurationManager.AppSettings["procedureBkpValidation"].ToString();
                }
            }

            public string ScriptDatabasePath
            {
                get
                {
                    return ConfigurationManager.AppSettings["scriptDatabasePathValidation"].ToString();
                }
            }

            public string ValidationPathBase
            {
                get
                {
                    return ConfigurationManager.AppSettings["validationPathBase"].ToString();
                }
            }


        }

        public class PreProduction
        {
            public string BkpPathBase
            {
                get
                {
                    return ConfigurationManager.AppSettings["bkpPathBase"];
                }
            }
            public string BkpPath
            {
                get
                {
                    return string.Format("{0}\\{1}", this.BkpPathBase, "PreProduction");
                }
            }

            public string ConnectionString
            {
                get
                {
                    return ConfigurationManager.ConnectionStrings["PreProduction"].ToString();
                }
            }

            public string ProcedureBkp
            {
                get
                {
                    return ConfigurationManager.AppSettings["ProcedureBkpPreProduction"].ToString();
                }
            }

            public string ScriptDatabasePath
            {
                get
                {
                    return ConfigurationManager.AppSettings["scriptDatabasePathPreProduction"].ToString();
                }
            }

            public string PreProductionPathBase
            {
                get
                {
                    return ConfigurationManager.AppSettings["preProductionPathBase"].ToString();
                }
            }



        }

        public class Production
        {
            public string BkpPathBase
            {
                get
                {
                    return ConfigurationManager.AppSettings["bkpPathBase"];
                }
            }
            public string BkpPath
            {
                get
                {
                    return string.Format("{0}\\{1}", this.BkpPathBase, "Production");
                }
            }

            public string ConnectionString
            {
                get
                {
                    return ConfigurationManager.ConnectionStrings["Production"].ToString();
                }
            }

            public string ProcedureBkp
            {
                get
                {
                    return ConfigurationManager.AppSettings["procedureBkpProduction"].ToString();
                }
            }

            public string ScriptDatabasePath
            {
                get
                {
                    return ConfigurationManager.AppSettings["scriptDatabasePathProduction"].ToString();
                }
            }

            public string ProductionPathBase
            {
                get
                {
                    return ConfigurationManager.AppSettings["productionPathBase"].ToString();
                }
            }

        }

    }
}
