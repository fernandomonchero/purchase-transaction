using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PurchaseTransaction.IntegrationTests
{
    public abstract class IntegrationTestBase
    {
        protected HttpClient Client;

        [SetUp]
        public void Setup()
        {
            var factory = new IntegrationTestFactory();

            Client = factory.CreateClient();
        }

        [TearDown]
        public void TearDown()
        {
            Client.Dispose();
        }
    }
}