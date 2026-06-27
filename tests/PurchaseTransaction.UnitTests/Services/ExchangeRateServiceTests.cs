using Moq;
using PurchaseTransaction.Domain.Interfaces;
using PurchaseTransaction.Domain.Models.Externals.RatesExchange;
using PurchaseTransaction.Domain.Services;

namespace PurchaseTransaction.UnitTests.Services;

[TestFixture]
public class ExchangeRateServiceTests
{
    private Mock<IExchangeRateAdapter> _adapterMock = null!;
    private ExchangeRateService _service = null!;

    [SetUp]
    public void Setup()
    {
        _adapterMock = new Mock<IExchangeRateAdapter>();

        _service = new ExchangeRateService(_adapterMock.Object);
    }

    [Test]
    public async Task GetValidExchangeRate_ShouldCallAdapterWithCorrectDateFilters()
    {
        // Arrange
        var purchaseDate = new DateTime(2025, 05, 20);
        var expectedMinDate = purchaseDate.AddMonths(-6);

        _adapterMock.Setup(x => x.GetExchangeRates("Brazil", expectedMinDate, purchaseDate, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Enumerable.Empty<ExchangeRateDto>());

        // Act
        await _service.GetValidExchangeRate("Brazil", purchaseDate);

        // Assert
        _adapterMock.Verify(x => x.GetExchangeRates("Brazil", expectedMinDate, purchaseDate, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    public async Task GetValidExchangeRate_Should_ReturnMostRecentExchangeRateWithinSixMonths()
    {
        // Arrange
        var purchaseDate = new DateTime(2025, 05, 20);

        var exchangeRates = new[]
        {
            new ExchangeRateDto { RecordDate = new DateTime(2025, 05, 10), ExchangeRate = 5.10m },
            new ExchangeRateDto { RecordDate = new DateTime(2025, 05, 18), ExchangeRate = 5.25m },
            new ExchangeRateDto { RecordDate = new DateTime(2025, 03, 15), ExchangeRate = 5.00m }
        };

        _adapterMock.Setup(x => x.GetExchangeRates(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(exchangeRates);

        // Act
        var result = await _service.GetValidExchangeRate("Brazil", purchaseDate);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.RecordDate, Is.EqualTo(new DateTime(2025, 05, 18)));
        Assert.That(result.ExchangeRate, Is.EqualTo(5.25m));
    }

    [Test]
    public async Task GetValidExchangeRate_Should_ReturnNullWhenNoExchangeRatesAreReturned()
    {
        // Arrange
        _adapterMock.Setup(x => x.GetExchangeRates(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Enumerable.Empty<ExchangeRateDto>());

        // Act
        var result = await _service.GetValidExchangeRate("Brazil", DateTime.Today);

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GetValidExchangeRate_Should_IgnoreExchangeRatesOlderThanSixMonths()
    {
        // Arrange
        var purchaseDate = new DateTime(2025, 08, 20);

        var exchangeRates = new[]
        {
            new ExchangeRateDto { RecordDate = new DateTime(2024, 12, 01), ExchangeRate = 4.90m }
        };

        _adapterMock.Setup(x => x.GetExchangeRates(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(exchangeRates);

        // Act
        var result = await _service.GetValidExchangeRate("Brazil", purchaseDate);

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GetValidExchangeRate_Should_IgnoreExchangeRatesAfterPurchaseDate()
    {
        // Arrange
        var purchaseDate = new DateTime(2025, 05, 20);

        var exchangeRates = new[]
        {
            new ExchangeRateDto { RecordDate = new DateTime(2025, 05, 22), ExchangeRate = 5.50m },
            new ExchangeRateDto { RecordDate = new DateTime(2025, 05, 18), ExchangeRate = 5.20m }
        };

        _adapterMock.Setup(x => x.GetExchangeRates(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(exchangeRates);

        // Act
        var result = await _service.GetValidExchangeRate("Brazil", purchaseDate);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.RecordDate, Is.EqualTo(new DateTime(2025, 05, 18)));
    }

    [Test]
    public async Task GetValidExchangeRate_Should_ReturnExchangeRateWhenRecordDateIsExactlySixMonthsOld()
    {
        // Arrange
        var purchaseDate = new DateTime(2025, 08, 20);

        var exchangeRate = new ExchangeRateDto
        {
            RecordDate = purchaseDate.AddMonths(-6),
            ExchangeRate = 5.15m
        };

        _adapterMock.Setup(x => x.GetExchangeRates(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new[] { exchangeRate });

        // Act
        var result = await _service.GetValidExchangeRate("Brazil", purchaseDate);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.ExchangeRate, Is.EqualTo(5.15m));
    }

    [Test]
    public async Task GetValidExchangeRate_Should_FilterInvalidDatesReturnedByAdapter()
    {
        // Arrange
        var purchaseDate = new DateTime(2025, 05, 20);

        var exchangeRates = new[]
        {
            new ExchangeRateDto { RecordDate = new DateTime(2024, 01, 10), ExchangeRate = 4.90m },
            new ExchangeRateDto { RecordDate = new DateTime(2025, 05, 18), ExchangeRate = 5.20m },
            new ExchangeRateDto { RecordDate = new DateTime(2025, 05, 25), ExchangeRate = 5.30m }
        };

        _adapterMock.Setup(x => x.GetExchangeRates(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(exchangeRates);

        // Act
        var result = await _service.GetValidExchangeRate("Brazil", purchaseDate);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.RecordDate, Is.EqualTo(new DateTime(2025, 05, 18)));
    }

    [Test]
    public void GetValidExchangeRate_Should_ThrowWhenAdapterThrowsException()
    {
        _adapterMock.Setup(x => x.GetExchangeRates(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new HttpRequestException());

        Assert.ThrowsAsync<HttpRequestException>(async () => await _service.GetValidExchangeRate("Brazil", DateTime.Today));
    }
}