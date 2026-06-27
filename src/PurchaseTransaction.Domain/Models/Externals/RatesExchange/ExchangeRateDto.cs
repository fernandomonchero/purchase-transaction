using System.Text.Json.Serialization;

namespace PurchaseTransaction.Domain.Models.Externals.RatesExchange
{
    public sealed class ExchangeRateDto
    {
        [JsonPropertyName("record_date")]
        public DateTime RecordDate { get; set; }

        [JsonPropertyName("effective_date")]
        public DateTime EffectiveDate { get; set; }

        [JsonPropertyName("country_currency_desc")]
        public string CountryCurrencyDescription { get; set; } = string.Empty;

        [JsonPropertyName("country")]
        public string Country { get; set; } = string.Empty;

        [JsonPropertyName("exchange_rate")]
        public decimal ExchangeRate { get; set; }
    }
}