using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Cna.Domain.Cache
{
    [Serializable]
    public class CurrentUserCache
    {
        public int UserId { get; set; }
        public int ClientId { get; set; }
        public bool OnlyUser { get; set; }

    }
}
