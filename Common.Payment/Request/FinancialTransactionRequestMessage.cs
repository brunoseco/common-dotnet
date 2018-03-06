using Newtonsoft.Json;
using System.Collections.Generic;

namespace Common.Payment.Request
{
    /// <summary>
    /// Requisição para a API de contas
    /// </summary>
    internal class FinancialTransactionRequestMessage
    {
        /// <summary>
        ///  Variáveis Personalizadas
        /// </summary>
        [JsonProperty("transactions")]
        public List<Transactions> Transactions { get; set; }

    }

    internal class Transactions
    {
        [JsonProperty("id")]
        public int Id { get; set; }
    }
}
