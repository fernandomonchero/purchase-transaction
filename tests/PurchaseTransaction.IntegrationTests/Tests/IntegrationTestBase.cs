using PurchaseTransaction.Domain.Interfaces;
using PurchaseTransaction.IntegrationTests.Infrastructure;

namespace PurchaseTransaction.IntegrationTests.Tests
{
    public abstract class IntegrationTestBase
    {
        protected IntegrationTestFactory Factory = null!;
        protected HttpClient Client = null!;

        protected void CreateFactory<TAdapter>() where TAdapter : class, IExchangeRateAdapter
        {
            Factory = new IntegrationTestFactory(typeof(TAdapter));
            Client = Factory.CreateClient();
        }
    }
}