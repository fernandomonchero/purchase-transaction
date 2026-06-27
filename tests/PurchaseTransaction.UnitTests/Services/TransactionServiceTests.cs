using Moq;
using PurchaseTransaction.Domain.Interfaces;
using PurchaseTransaction.Domain.Models;
using PurchaseTransaction.Domain.Notifications;
using PurchaseTransaction.Domain.Services;

namespace PurchaseTransaction.UnitTests.Services;

[TestFixture]
public class TransactionServiceTests
{
    private Mock<ITransactionRepository> _repository;
    private Mock<INotificationCollector> _notifications;

    private TransactionService _service;

    [SetUp]
    public void Setup()
    {
        _repository = new Mock<ITransactionRepository>();
        _notifications = new Mock<INotificationCollector>();

        _service = new TransactionService(
            _repository.Object,
            _notifications.Object);
    }

    private static Transaction ValidTransaction()
    {
        return new Transaction
        {
            Description = "Salary",
            Date = DateTime.Today,
            Amount = 123.4567m
        };
    }

    [Test]
    public async Task Add_Should_AddTransactionWhenTransactionIsValid()
    {
        var transaction = ValidTransaction();

        await _service.Add(transaction);

        _repository.Verify(r => r.Add(It.IsAny<Transaction>()), Times.Once);

        _notifications.Verify(n => n.AddNotification(It.IsAny<string>()), Times.Never);
    }

    [Test]
    public async Task Add_Should_TruncateAmountBeforeSave()
    {
        var transaction = ValidTransaction();

        await _service.Add(transaction);

        _repository.Verify(r => r.Add(It.Is<Transaction>(t => t.Amount == 123.45m)), Times.Once);
    }

    [Test]
    public async Task Add_ShouldNot_SaveWhenDescriptionIsEmpty()
    {
        var transaction = ValidTransaction();
        transaction.Description = "";

        await _service.Add(transaction);

        _repository.Verify(r => r.Add(It.IsAny<Transaction>()), Times.Never);

        // Validar mensagem adicionada
        _notifications.Verify(n => n.AddNotification(It.IsAny<string>()), Times.Once);
    }

    [Test]
    public async Task Add_ShouldNot_SaveWhenDescriptionIsGreaterThan50()
    {
        var transaction = ValidTransaction();
        transaction.Description = new string('A', 51);

        await _service.Add(transaction);

        _repository.Verify(r => r.Add(It.IsAny<Transaction>()), Times.Never);

        // Validar mensagem adicionada
        _notifications.Verify(n => n.AddNotification(It.IsAny<string>()), Times.Once);
    }

    [Test]
    public async Task Add_ShouldNot_SaveWhenDateIsDefault()
    {
        var transaction = ValidTransaction();
        transaction.Date = default;

        await _service.Add(transaction);

        _repository.Verify(r => r.Add(It.IsAny<Transaction>()), Times.Never);

        // Validar mensagem adicionada
        _notifications.Verify(n => n.AddNotification(It.IsAny<string>()), Times.Once);
    }

    [Test]
    public async Task Add_ShouldNot_SaveWhenAmountIsZero()
    {
        var transaction = ValidTransaction();
        transaction.Amount = 0;

        await _service.Add(transaction);

        _repository.Verify(r => r.Add(It.IsAny<Transaction>()), Times.Never);

        // Validar mensagem adicionada
        _notifications.Verify(n => n.AddNotification(It.IsAny<string>()), Times.Exactly(2));
    }

    [Test]
    public async Task Add_ShouldNot_SaveWhenAmountIsNegative()
    {
        var transaction = ValidTransaction();
        transaction.Amount = -15;

        await _service.Add(transaction);

        _repository.Verify(r => r.Add(It.IsAny<Transaction>()), Times.Never);

        // Validar mensagem adicionada
        _notifications.Verify(n => n.AddNotification(It.IsAny<string>()), Times.Once);
    }

    [Test]
    public void Dispose_Should_DisposeRepository()
    {
        _service.Dispose();

        _repository.Verify(r => r.Dispose(), Times.Once);
    }

    [TearDown]
    public void TearDown()
    {
        _service.Dispose();
    }
}