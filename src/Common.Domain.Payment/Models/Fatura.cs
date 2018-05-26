using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.Payment.Models
{
    public class Fatura
    {
        public string Id { get; set; }
        public string ClienteId { get; set; }
        public string UsuarioID { get; set; }

        public string Email { get; set; }
        public string Status { get; set; }

        public string SecureID { get; set; }
        public string SecureUrl { get; set; }

        public decimal ValorPago { get; set; }
        public decimal ValorTotal { get; set; }
        public decimal ValorImpostos { get; set; }
        public decimal ValorMulta { get; set; }
        public decimal ValorDesconto { get; set; }
        public decimal ValorDescontoAteVencimento { get; set; }

        public bool Reembolsavel { get; set; }
        public bool TemDescontoAteVencimento { get; set; }
        public bool CobrarJuros { get; set; }

        public ETipoPagamento TipoPagamento { get; set; }

        public DateTime DataVencimento { get; set; }
        public DateTime DataPagamento { get; set; }

        public DateTime DataCriacao { get; set; }
        public DateTime DataAtualizacao { get; set; }
        
        public Boleto Boleto { get; set; }

        public Pagador Pagador { get; set; }

        public List<FaturaItem> FaturaItems { get; set; }
        public List<Log> Logs { get; set; }

        public Fatura(string email, DateTime dataVencimento, List<FaturaItem> faturaItems)
        {
            this.Email = email;
            this.DataVencimento = dataVencimento;
            this.FaturaItems = faturaItems;
        }
    }

    public enum ETipoPagamento
    {
        TODOS = 0,
        CREDITO = 1,
        BOLETO = 2
    }

}
