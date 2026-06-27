using System.Text.Json.Serialization;

namespace PurchaseTransaction.Domain.Models.Externals.RatesExchange
{
    public sealed class LinksDto
    {
        [JsonPropertyName("self")]
        public string? Self { get; set; }

        [JsonPropertyName("next")]
        public string? Next { get; set; }
    }
}