using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.Payment.Models
{
    public class Pagador
    {
        public string CPF_CNPJ { get; set; }
        public string Nome { get; set; }
        public string TelefonePrefixo { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }

        public string CEP { get; set; }
        public string Rua { get; set; }
        public string Numero { get; set; }
        public string Cidade { get; set; }
        public string UF { get; set; }
        public string Pais { get; set; }
        

    }
    

}
