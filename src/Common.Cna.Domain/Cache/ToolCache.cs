using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Cna.Domain.Cache
{
    [Serializable]
    public class ToolCache
    {

        public IEnumerable<ToolCache> CollectionReletedTool { get; set; }
        public IEnumerable<ToolRoleCache> CollectionToolRole { get; set; }

        public int ToolId { get; set; }
        public string Nome { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public int? ReletedToolId { get; set; }
        public int ToolCategoryId { get; set; }
        public bool IsRoot { get; set; }
        public int Environment { get; set; }
        public string IconClass { get; set; }

    }
}
