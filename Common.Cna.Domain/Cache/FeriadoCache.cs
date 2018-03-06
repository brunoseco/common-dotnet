using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Cna.Domain.Cache
{
    [Serializable]
    public class FeriadoCache
    {

        public int CalendarioExcecaoId { get; set; }
        public int EscolaId { get; set; }
        public bool Feriado { get; set; }
        public int Dia { get; set; }
        public int Mes { get; set; }
        public int? Ano { get; set; } 

    }
}
