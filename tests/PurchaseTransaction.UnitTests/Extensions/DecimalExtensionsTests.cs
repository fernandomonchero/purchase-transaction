using PurchaseTransaction.Domain.Extensions;

namespace PurchaseTransaction.UnitTests.Extensions
{
    [TestFixture]
    public class DecimalExtensionsTests
    {
        [Test]
        public void Truncate_ShouldReturnSameValue_WhenAlreadyHasTwoDecimalPlaces()
        {
            // Arrange
            decimal value = 99.99m;

            // Act
            decimal result = value.Truncate();

            // Assert
            Assert.That(result, Is.EqualTo(99.99m));
        }

        [Test]
        public void Truncate_ShouldReturnSameValue_WhenHasLessThanTwoDecimalPlaces()
        {
            decimal value = 99.9m;

            decimal result = value.Truncate();

            Assert.That(result, Is.EqualTo(99.9m));
        }

        [Test]
        public void Truncate_ShouldReturnSameValue_WhenIsInteger()
        {
            decimal value = 99m;

            decimal result = value.Truncate();

            Assert.That(result, Is.EqualTo(99m));
        }

        [Test]
        public void Truncate_ShouldTruncateExtraDecimalPlaces()
        {
            decimal value = 99.99999m;

            decimal result = value.Truncate();

            Assert.That(result, Is.EqualTo(99.99m));
        }

        [Test]
        public void Truncate_ShouldNotRoundUp()
        {
            decimal value = 99.995m;

            decimal result = value.Truncate();

            Assert.That(result, Is.EqualTo(99.99m));
        }

        [Test]
        public void Truncate_ShouldHandleSmallDecimal()
        {
            decimal value = 0.009m;

            decimal result = value.Truncate();

            Assert.That(result, Is.EqualTo(0.00m));
        }

        [Test]
        public void Truncate_ShouldHandleZero()
        {
            decimal value = 0m;

            decimal result = value.Truncate();

            Assert.That(result, Is.EqualTo(0m));
        }

        [Test]
        public void Truncate_ShouldHandleNegativeNumber()
        {
            decimal value = -99.999m;

            decimal result = value.Truncate();

            Assert.That(result, Is.EqualTo(-99.99m));
        }

        [Test]
        public void Truncate_ShouldHandleNegativeSmallNumber()
        {
            decimal value = -0.019m;

            decimal result = value.Truncate();

            Assert.That(result, Is.EqualTo(-0.01m));
        }

        [Test]
        public void Truncate_ShouldHandleLargeNumber()
        {
            decimal value = 1234567890.129999m;

            decimal result = value.Truncate();

            Assert.That(result, Is.EqualTo(1234567890.12m));
        }
    }
}