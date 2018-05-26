using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Gen
{
    static class PathOutputVue
    {
        public static bool UsePathProjects { get; set; }

        public static string PathOutputVueViewComponent(TableInfo tableInfo, Context configContext, string viewName)
        {
            var pathOutput = string.Empty;
            var className = tableInfo.ClassName.ToLower();

            var pathBase = configContext.OutputFrontend;
            pathOutput = Path.Combine(pathBase, "views", className, string.Format("{0}.vue", viewName));
            PathOutputBase.MakeDirectory(pathBase, "views", tableInfo.ClassName.ToLower());

            return pathOutput;
        }
        
        public static string PathOutputVueRouterViewComponent(TableInfo tableInfo, Context configContext)
        {
            var pathOutput = string.Empty;
            var className = tableInfo.ClassName.ToLower();

            var pathBase = configContext.OutputFrontend;
            pathOutput = Path.Combine(pathBase, "router", string.Format("generated.js"));
            PathOutputBase.MakeDirectory(pathBase, "router");

            return pathOutput;
        }
                
        
    }
}
