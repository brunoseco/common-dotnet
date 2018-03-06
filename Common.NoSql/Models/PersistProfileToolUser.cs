using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.NoSql.Models
{
    [Serializable]
    class PersistProfileToolUser
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public int ExternalId { get; set; }
        public int UserGroupId { get; set; }
        public object Value { get; set; }
    }
}
