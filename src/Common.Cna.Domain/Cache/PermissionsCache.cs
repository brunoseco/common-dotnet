using System;
using System.Collections.Generic;

namespace Common.Cna.Domain.Cache
{
    [Serializable]
    public class PermissionsCache
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public int ToolId { get; set; }
        public bool CanRead { get; set; }
        public bool CanWrite { get; set; }
        public bool CanDelete { get; set; }
        public bool CanImpersonate { get; set; }
        public bool HasImpersonate { get; set; }
        public bool ByToolUser { get; set; }
        public int ToolCategoryId { get; set; }


    }
}
