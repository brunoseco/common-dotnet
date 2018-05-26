using Common.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;



namespace Common.Gen
{
    public class FixRename
    {

        private static List<string> _typeFilesExt;

        static FixRename()
        {
            _typeFilesExt = new List<string>
            {
                "Repository.cs",
                "OrderByCustomExtension.cs",
                "FilterCustomExtension.cs",
                "AptoParaCadastroValidation.cs",
                "AptoParaCadastroWarning.cs",
                "EstaConsistenteValidation.cs"
            };
        }

        public static void Fix(HelperSysObjectsBase sysObject)
        {
            foreach (var item in sysObject.Contexts)
            {
                FixFileInFolder(item.OutputClassDomain);
                FixFileInFolder(item.OutputClassApp);
                FixFileInFolder(item.OutputClassApi);
                FixFileInFolder(item.OutputClassDto);
                FixFileInFolder(item.OutputClassFilter);
            }
        }

        private static void FixFileInFolder(string root)
        {
            var dirs = Directory.GetDirectories(root);
            foreach (var item in dirs)
            {
                var subDirs = Directory.GetDirectories(item);
                if (subDirs.IsAny())
                    FixFileInFolder(item);

                var files = Directory.GetFiles(item);
                foreach (var file in files)
                {
                    var found = _typeFilesExt.Where(_ => _ == file).IsAny();
                    if (found)
                    {

                    }
                }
            }

        }
        
    }
}
