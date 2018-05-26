using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.NoSql.Models
{
    [Serializable]
    class PersistKeyValue
    {
        public Guid Id { get; set; }

        public string key { get; set; }

        public object Value { get; set; }

    }
}
