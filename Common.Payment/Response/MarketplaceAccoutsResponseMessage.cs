using Newtonsoft.Json;
using System.Collections.Generic;

namespace Common.Payment.Response
{
    internal class MarketplaceAccoutsResponseMessage
    {
        [JsonProperty("totalItems")]
        public int TotalItems { get; set; }

        [JsonProperty("items")]
        public List<MarketPlaceAccountItem> Accounts { get; set; }

    }

    internal class MarketPlaceAccountItem
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("Name")]
        public string Name { get; set; }
        [JsonProperty("verified", NullValueHandling = NullValueHandling.Ignore)]
        public bool Verified { get; set; }
    }
}
