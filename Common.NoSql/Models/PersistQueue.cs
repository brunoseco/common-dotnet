using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.NoSql.Models
{
    [Serializable]
    public class PersistQueue
    {
        public Guid Id { get; set; }
        public string IdentifyId { get; set; }
        public int QueueTypeId { get; set; }
        public bool Proccessed { get; set; }
        public bool Peeked { get; set; }
        public IDictionary<string, object> Value { get; set; }
    }
}
