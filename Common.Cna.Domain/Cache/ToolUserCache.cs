using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Cna.Domain.Cache
{
    [Serializable]
    public class ToolUserCache
    {
        public string UserId { get; set; }
        public int UserIdExternal { get; set; }
        public int UserGroupId { get; set; }
        public int ToolId { get; set; }
        public string Name { get; set; }
        public int ToolCategoryId { get; set; }
        public string Url { get; set; }
        public bool CanRead { get; set; }
        public bool CanWrite { get; set; }
        public bool CanDelete { get; set; }
        public int Environment { get; set; }

    }
}
