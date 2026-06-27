using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PurchaseTransaction.Api.Dtos;
using PurchaseTransaction.Infrastructure.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace PurchaseTransaction.IntegrationTests.Transaction
{
    public class TransactionControllerTests : IntegrationTestBase
    {
        [Test]
        public async Task Add_Should_Save_Transaction()
        {
            var dto = new TransactionDto
            {
                Description = "Compra Mercado",
                Date = DateTime.Today,
                Amount = 350.75m
            };

            var response = await Client.PostAsJsonAsync("/api/transaction", dto);

            response.EnsureSuccessStatusCode();

            using var scope = new IntegrationTestFactory().CreateScope();

            var db = scope.ServiceProvider.GetRequiredService<PurchaseTransactionContext>();

            var transaction = await db.Transactions.FirstOrDefaultAsync();

            Assert.NotNull(transaction);
            Assert.That(transaction.Description, Is.EqualTo(dto.Description));
            Assert.That(transaction.Amount, Is.EqualTo(dto.Amount));
            Assert.That(transaction.Date, Is.EqualTo(dto.Date));
        }
    }
}