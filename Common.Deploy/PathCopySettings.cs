using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Deploy
{

    public class PathCopySettings
    {

        public FileInfo[] filesSource { get; set; }

        public FileInfo[] filesDestination { get; set; }


        public void AddFileToSorce(FileInfo fileInfo) {


            var listFilesTemp_ = new List<FileInfo>();
            listFilesTemp_.AddRange(this.filesSource);
            listFilesTemp_.Add(fileInfo);
            filesSource = listFilesTemp_.ToArray();

        }

    }

}
