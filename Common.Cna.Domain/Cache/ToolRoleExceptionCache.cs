using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Cna.Domain.Cache
{
    [Serializable]
    public class ToolRoleExceptionCache
    {
        public string RoleId { get; set; }
        public int ToolId { get; set; }
        public int ExceptionGroupId { get; set; }
        public bool CanRead { get; set; }
        public bool CanWrite { get; set; }
        public bool CanDelete { get; set; }
        public bool CanImpersonate { get; set; }
        public bool HasImpersonate { get; set; }

    }
}
