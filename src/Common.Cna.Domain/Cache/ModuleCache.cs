using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Cna.Domain.Cache
{
    [Serializable]
    public class ModuleCache
    {

        public IEnumerable<ModuleToolCache> CollectionModuleTool { get; set; }
        public int ModuloId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string IconClass { get; set; }
        public int? ModuleAppId { get; set; }
    }
}
