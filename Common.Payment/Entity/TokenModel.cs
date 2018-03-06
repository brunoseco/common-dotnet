using Newtonsoft.Json;

namespace Common.Payment.Entity
{
    internal class TokenModel
    {
        /// <summary>
        /// ID
        /// </summary>
        [JsonProperty("id")]
        public string ID { get; set; }

        /// <summary>
        /// Método
        /// </summary>
        [JsonProperty("method")]
        public string Method { get; set; }
    }
}
