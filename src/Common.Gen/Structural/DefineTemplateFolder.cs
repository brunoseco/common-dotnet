using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Gen
{
    public class DefineTemplateFolder
    {
        private string _templatePathBase;

        public DefineTemplateFolder()
        {
            _templatePathBase = GetDefaultTemplateFolder();
        }

        public string GetDefaultTemplateFolder()
        {
            return "Templates";
        }

        public void SetTemplatePathBase(string templatePathBase)
        {
            _templatePathBase = templatePathBase;
        }

        public string Define()
        {
            if (!new DirectoryInfo(_templatePathBase).Exists)
                throw new InvalidOperationException("Templates folder not Found, remember mark copy allways");

            return _templatePathBase;
        }

        public string Define(TableInfo tableInfo)
        {
            return Define();

        }

    }
}
