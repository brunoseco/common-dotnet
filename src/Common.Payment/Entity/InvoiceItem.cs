using Newtonsoft.Json;

namespace Common.Payment.Entity
{
    /// <summary>
    /// Representa um item de uma fatura
    /// </summary>
    internal class InvoiceItem
    {
        /// <summary>
        /// Descrição do Item
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// Quantidade
        /// </summary>
        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        /// <summary>
        /// Preço em Centavos. Valores negativos entram como desconto no total
        /// </summary>
        [JsonProperty("price_cents")]
        public int PriceCents { get; set; }
    }
}
