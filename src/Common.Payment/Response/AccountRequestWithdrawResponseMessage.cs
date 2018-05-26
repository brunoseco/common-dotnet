using Newtonsoft.Json;

namespace Common.Payment.Response
{
    /// <summary>
    /// Resposta da Api de pedido de saque
    /// </summary>
    internal class AccountRequestWithdrawResponseMessage
    {
        /// <summary>
        /// Id que identifica o pedido de saque efetuado
        /// </summary>
        [JsonProperty("account_id")]
        public string OperationId { get; set; }

        /// <summary>
        /// Valor solicitado para saque
        /// </summary>
        [JsonProperty("amount")]
        public decimal WithdrawValue { get; set; }
    }
}
