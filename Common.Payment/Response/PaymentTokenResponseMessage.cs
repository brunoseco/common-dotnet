using Newtonsoft.Json;

namespace Common.Payment.Response
{
    internal class PaymentTokenResponseMessage
    {
        /// <summary>
        /// Token Criado
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Método de Pagamento (atualmente somente credit_card)
        /// </summary>
        [JsonProperty("method")]
        public string Method { get; set; }
    }

}
