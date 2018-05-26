using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Deploy
{

    public class PathsDeploy
    {
        public PathsDeploy()
        {
            this.BlackListFile = new List<string>();
        }

        public IEnumerable<string> BlackListFile { get; set; }

        public string Name { get; set; }
        public string DevelopmentPath { get; set; }
        public string DevelopmentPathBase { get; set; }
        public string HomologPath { get; set; }
        public string ValidationPath { get; set; }
        public string ImplantationPath { get; set; }
        public string PreProductionPath { get; set; }
        public string ProductionPath { get; set; }
        public string FileType { get; set; }
        public bool Recursive { get; set; }
        public bool WebConfig { get; set; }
        public bool AppConfig { get; set; }
        public string FileAppConfigName { get; set; }
        public bool WindowsService { get; set; }
        public string WindowsServiceName { get; set; }
        public List<string> ConnectionStringName { get; set; }

        public string DevelopmentPathComplete(string branch)
        {
            return string.Format("{0}{1}{2}", this.DevelopmentPathBase, branch, this.DevelopmentPath);
        }

    }

}
