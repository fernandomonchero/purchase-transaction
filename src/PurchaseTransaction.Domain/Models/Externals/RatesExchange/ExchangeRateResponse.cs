using System.Text.Json.Serialization;

namespace PurchaseTransaction.Domain.Models.Externals.RatesExchange
{
    public sealed class ExchangeRateResponse
    {
        [JsonPropertyName("data")]
        public List<ExchangeRateDto> Data { get; set; } = [];

        [JsonPropertyName("meta")]
        public MetaDto? Meta { get; set; }

        [JsonPropertyName("links")]
        public LinksDto? Links { get; set; }
    }
}