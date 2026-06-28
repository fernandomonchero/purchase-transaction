using PurchaseTransaction.IntegrationTests.Infrastructure;
using System.Net;
using System.Net.Http.Json;

namespace PurchaseTransaction.IntegrationTests.Tests
{
    [TestFixture]
    public class CountryControllerTests : IntegrationTestBase
    {
        [Test]
        public async Task Get_Should_Return_All_Countries()
        {
            CreateFactory<FakeExchangeRateAdapter>();

            var response = await Client.GetAsync("/api/countries");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var countries = await response.Content.ReadFromJsonAsync<List<string>>();

            Assert.NotNull(countries);
            Assert.IsNotEmpty(countries);
            Assert.Contains("Brazil", countries);
            Assert.Contains("Canada", countries);
            Assert.Contains("United Kingdom", countries);
            Assert.Contains("South Africa", countries);
            Assert.Contains("Japan", countries);
            Assert.Contains("Australia", countries);
            Assert.That(countries!.Distinct().Count(), Is.EqualTo(countries.Count));
        }
    }
}