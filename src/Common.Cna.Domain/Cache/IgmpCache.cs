using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Cna.Domain.Cache
{
    [Serializable]
    public class IgmpCache
    {
        public DateTime Data { get; set; }
        public Decimal Valor { get; set; }
        public Decimal ValorProRata { get; set; }

    }
}
