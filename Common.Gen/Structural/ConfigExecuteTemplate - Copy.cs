using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Gen
{
    public class ConfigExecutetemplate
    {

        public TableInfo tableInfo { get; set; }
        public Context configContext { get; set; }
        public IEnumerable<Info> infos { get; set; }
        public string pathOutput { get; set; }
        public string template { get; set; }


    }
}
