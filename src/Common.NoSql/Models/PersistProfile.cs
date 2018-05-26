using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.NoSql.Models
{
    [Serializable]
    class PersistProfile
    {
        public Guid Id { get; set; }
        public string RoleId { get; set; }
        public int ExternalId { get; set; }
        public string Name { get; set; }
        public object Value { get; set; }

    }
}
