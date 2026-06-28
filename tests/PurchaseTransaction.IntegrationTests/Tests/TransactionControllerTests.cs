using PurchaseTransaction.Api.Dtos;
using PurchaseTransaction.IntegrationTests.Infrastructure;
using System.Net;
using System.Net.Http.Json;

namespace PurchaseTransaction.IntegrationTests.Tests
{
    [TestFixture]
    public class TransactionControllerTests : IntegrationTestBase
    {
        [Test]
        public async Task Post_Should_CreateTransaction()
        {
            CreateFactory<FakeExchangeRateAdapter>();

            var request = new TransactionDto
            {
                Description = "Blue Jeans",
                Amount = 150,
                Date = new DateTime(2026,6,5)
            };

            var response = await Client.PostAsJsonAsync("api/transactions", request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            using var context = Factory.CreateDbContext();

            Assert.That(context.Transactions.Count(), Is.EqualTo(2));

            var transaction = context.Transactions.OrderByDescending(t => t.CreatedAt).First();

            Assert.That(transaction.Description, Is.EqualTo(request.Description));
            Assert.That(transaction.Amount, Is.EqualTo(request.Amount));
            Assert.That(transaction.Date, Is.EqualTo(request.Date));
        }

        [Test]
        public async Task Post_Should_ReturnBadRequestWhenModelStateIsInvalid()
        {
            CreateFactory<FakeExchangeRateAdapter>();

            var request = new TransactionDto
            {
                Amount = 150,
                Date = new DateTime(2026, 6, 5)
            };

            var response = await Client.PostAsJsonAsync("api/transactions", request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));

            using var context = Factory.CreateDbContext();

            Assert.That(context.Transactions.Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task Get_Should_ReturnNotFoundWhenTransactionDoesNotExist()
        {
            CreateFactory<FakeExchangeRateAdapter>();

            var transactionId = Guid.NewGuid();

            var response = await Client.GetAsync($"api/transactions/{transactionId}");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));

            using var context = Factory.CreateDbContext();

            Assert.That(context.Transactions.Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task Get_Should_ReturnTheTransaction()
        {
            CreateFactory<FakeExchangeRateAdapter>();

            var transactionId = Guid.NewGuid();

            var response = await Client.GetAsync($"api/transactions/0ea39c29-580e-4a80-a4c9-798b9e31fd0e");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            using var context = Factory.CreateDbContext();

            Assert.That(context.Transactions.Count(), Is.EqualTo(1));

            var transaction = context.Transactions.Find(new Guid("0ea39c29-580e-4a80-a4c9-798b9e31fd0e"));

            Assert.That(transaction.Id.ToString(), Is.EqualTo("0ea39c29-580e-4a80-a4c9-798b9e31fd0e"));
            Assert.That(transaction.Description, Is.EqualTo("Red Shirt"));
            Assert.That(transaction.Amount, Is.EqualTo(10));
            Assert.That(transaction.Date, Is.EqualTo(new DateTime(2026, 1, 1)));
        }

        [Test]
        public async Task Get_Should_ReturnAllTransactions()
        {
            CreateFactory<FakeExchangeRateAdapter>();

            var response = await Client.GetAsync("api/transactions");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var result = await response.Content.ReadFromJsonAsync<List<TransactionDto>>();

            Assert.NotNull(result);
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].Id.ToString(), Is.EqualTo("0ea39c29-580e-4a80-a4c9-798b9e31fd0e"));
            Assert.That(result[0].Description, Is.EqualTo("Red Shirt"));
            Assert.That(result[0].Amount, Is.EqualTo(10));
            Assert.That(result[0].Date, Is.EqualTo(new DateTime(2026, 1, 1)));

            using var context = Factory.CreateDbContext();

            Assert.That(context.Transactions.Count(), Is.EqualTo(1));
        }
    }
}