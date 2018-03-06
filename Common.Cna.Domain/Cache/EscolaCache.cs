using System;
using System.Collections.Generic;

namespace Common.Cna.Domain.Cache
{
    [Serializable]
    public class EscolaCache
    {
        public string Nome { get; set; }
        public string RazaoSocial { get; set; }
        public string CNPJ { get; set; }
        public string Endereco { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public int EscolaId { get; set; }
        public int? CargoId { get; set; }
        public IEnumerable<GrupoCache> Grupos { get; set; }
        public bool TemModuloConsultivo { get; set; }
    }
}
