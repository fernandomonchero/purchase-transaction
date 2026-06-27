using PurchaseTransaction.Domain.Interfaces;
using PurchaseTransaction.Domain.Models.Externals.RatesExchange;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PurchaseTransaction.Infrastructure.Adapters
{
    public sealed class ExchangeRateAdapter : IExchangeRateAdapter
    {
        private readonly HttpClient _httpClient;
        private const string ROUTE_PATH = "accounting/od/rates_of_exchange";
        private const string QUERY_FIELDS = "fields=record_date,effective_date,country_currency_desc,country,exchange_rate";
        private const string QUERY_SORT = "sort=-record_date";

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true,
            NumberHandling = JsonNumberHandling.AllowReadingFromString
        };

        public ExchangeRateAdapter(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<ExchangeRateDto>> GetExchangeRates(string countryFilter, DateTime minDateFilter, DateTime maxDateFilter, CancellationToken cancellationToken = default)
        {
            using var response = await _httpClient.GetAsync($"v1/{ROUTE_PATH}?{QUERY_FIELDS}&{QUERY_SORT}&{GetQueryFilter(countryFilter, minDateFilter, maxDateFilter)}",
                cancellationToken);

            if (response.StatusCode == HttpStatusCode.NotFound)
                return [];

            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException($"Treasury API retornou {(int)response.StatusCode}");

            await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);

            var teste = await response.Content.ReadAsStringAsync(cancellationToken);

            var result = await JsonSerializer.DeserializeAsync<ExchangeRateResponse>(stream, JsonOptions, cancellationToken);

            if (result is null)
                throw new InvalidOperationException("Invalis answer from Treasury API");

            return result.Data;
        }

        private string GetQueryFilter(string countryFilter, DateTime minDateFilter, DateTime maxDateFilter)
        {
            return $"filter=country:in:({countryFilter}),record_date:gte:{minDateFilter:yyyy-MM-dd},record_date:lte:{maxDateFilter:yyyy-MM-dd}";
        }
    }
}