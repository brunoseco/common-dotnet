using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Cna.Domain.Cache
{
    [Serializable]
    public class ColaboradorLogadoCache
    {
        //public IEnumerable<EscolaCache> Escolas { get; set; }
        public EscolaCache EscolaLogada { get; set; }
        public int ColaboradorId { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool Atualizado { get; set; }
        public bool TemColaboradorAdministrador { get; set; }
        public int AppId { get; set; }
    }
}
