using PurchaseTransaction.Api.Dtos;
using PurchaseTransaction.Domain.Models;
using PurchaseTransaction.IntegrationTests.Infrastructure;
using System.Net;
using System.Net.Http.Json;

namespace PurchaseTransaction.IntegrationTests.Tests
{
    [TestFixture]
    public class ConvertedTransactionControllerTests : IntegrationTestBase
    {
        [Test]
        public async Task Get_Should_ReturnOkWhenExchangeRateExists()
        {
            CreateFactory<FakeExchangeRateAdapter>();

            var transaction = new Transaction
            {
                Id = new Guid("0ea39c29-580e-4a80-a4c9-798b9e31fd0e"),
                Description = "Red Shirt",
                Amount = 10,
                Date = new DateTime(2026, 1, 1)
            };

            var response = await Client.GetAsync($"api/converted-transactions?id={transaction.Id}&country=Brazil");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var result = await response.Content.ReadFromJsonAsync<ConvertedTransactionDto>();

            Assert.NotNull(result);
            Assert.That(result.Id, Is.EqualTo(transaction.Id));
            Assert.That(result.Description, Is.EqualTo(transaction.Description));
            Assert.That(result.PurchaseDate, Is.EqualTo(transaction.Date));
            Assert.That(result.OriginalAmount, Is.EqualTo(transaction.Amount));
            Assert.That(result.ExchangeRate, Is.EqualTo(5.25m));
            Assert.That(result.ConvertedValue, Is.EqualTo(transaction.Amount * 5.25m));

            using var context = Factory.CreateDbContext();
            Assert.That(context.Transactions.Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task Get_Should_ReturnNotFoundWhenTransactionDoesNotExist()
        {
            CreateFactory<FakeExchangeRateAdapter>();

            var id = Guid.NewGuid();

            var response = await Client.GetAsync($"/api/converted-transactions?id={id}&country=Brazil");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));

            using var context = Factory.CreateDbContext();

            Assert.That(context.Transactions.Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task Get_Should_ReturnNotFoundWhenAnyExchangeRateReturnedFromExternalApi()
        {
            CreateFactory<EmptyExchangeRateAdapter>();

            var transaction = new Transaction
            {
                Id = new Guid("0ea39c29-580e-4a80-a4c9-798b9e31fd0e"),
                Description = "Red Shirt",
                Amount = 10,
                Date = new DateTime(2026, 1, 1)
            };

            var response = await Client.GetAsync($"api/converted-transactions?id={transaction.Id}&country=Brazil");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));

            using var context = Factory.CreateDbContext();

            Assert.That(context.Transactions.Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task Get_Should_ReturnServiceUnavailableWhenExternalApiDontAnswer()
        {
            CreateFactory<ServiceUnavailableExchangeRateAdapter>();

            var transaction = new Transaction
            {
                Id = new Guid("0ea39c29-580e-4a80-a4c9-798b9e31fd0e"),
                Description = "Red Shirt",
                Amount = 10,
                Date = new DateTime(2026, 1, 1)
            };

            var response = await Client.GetAsync($"/api/converted-transactions?id={transaction.Id}&country=Brazil");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.ServiceUnavailable));

            using var context = Factory.CreateDbContext();

            Assert.That(context.Transactions.Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task Get_Should_ReturnGatewayTimeoutWhenExternalApiIsExperiencingLatency()
        {
            CreateFactory<TimeoutExchangeRateAdapter>();

            var transaction = new Transaction
            {
                Id = new Guid("0ea39c29-580e-4a80-a4c9-798b9e31fd0e"),
                Description = "Red Shirt",
                Amount = 10,
                Date = new DateTime(2026, 1, 1)
            };

            var response = await Client.GetAsync($"/api/converted-transactions?id={transaction.Id}&country=Brazil");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.GatewayTimeout));

            using var context = Factory.CreateDbContext();

            Assert.That(context.Transactions.Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task Get_Should_ReturnBadGatewayWhenExternalApiResponseWithInvalidJson()
        {
            CreateFactory<InvalidJsonExchangeRateAdapter>();

            var transaction = new Transaction
            {
                Id = new Guid("0ea39c29-580e-4a80-a4c9-798b9e31fd0e"),
                Description = "Red Shirt",
                Amount = 10,
                Date = new DateTime(2026, 1, 1)
            };

            var response = await Client.GetAsync($"/api/converted-transactions?id={transaction.Id}&country=Brazil");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadGateway));

            using var context = Factory.CreateDbContext();

            Assert.That(context.Transactions.Count(), Is.EqualTo(1));
        }
    }
}