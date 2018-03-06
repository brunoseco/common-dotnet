using Newtonsoft.Json;

namespace Common.Payment.Response
{
    internal class SimpleResponseMessage
    {
        /// <summary>
        /// Result of request
        /// </summary>
        [JsonProperty("success")]
        public bool Success { get; set; }
    }
}
