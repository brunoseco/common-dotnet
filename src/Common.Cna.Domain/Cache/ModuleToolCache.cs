using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Cna.Domain.Cache
{
    [Serializable]
    public class ModuleToolCache
    {
        public ModuleCache Module { get; set; }
        public ToolCache Tool { get; set; }
        public int ToolId { get; set; }
        public int ModuleId { get; set; }

    }
}
