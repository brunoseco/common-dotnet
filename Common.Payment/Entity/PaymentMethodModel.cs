using Newtonsoft.Json;

namespace Common.Payment.Entity
{
    // TODO: Precisa de  documentação
    internal class PaymentMethodModel
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("id")]
        public string ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("item_type")]
        public string ItemType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("data")]
        public PaymentMethodData Data { get; set; }
    }

    // TODO: Precisa de  documentação
    internal class PaymentMethodData
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("token")]
        public string Token { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("display_number")]
        public string DisplayNumber { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("brand")]
        public string Brand { get; set; }
    }
}
