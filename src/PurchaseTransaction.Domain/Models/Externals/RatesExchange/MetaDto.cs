using System.Text.Json.Serialization;

namespace PurchaseTransaction.Domain.Models.Externals.RatesExchange
{
    public sealed class MetaDto
    {
        [JsonPropertyName("count")]
        public int Count { get; set; }

        [JsonPropertyName("total-count")]
        public int TotalCount { get; set; }
    }
}