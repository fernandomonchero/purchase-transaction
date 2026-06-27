using PurchaseTransaction.Domain.Extensions;
using PurchaseTransaction.Domain.Interfaces;
using PurchaseTransaction.Domain.Models;
using PurchaseTransaction.Domain.Notifications;
using PurchaseTransaction.Domain.Validations;

namespace PurchaseTransaction.Domain.Services
{
    public class TransactionService : DomainService, ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;

        public TransactionService(ITransactionRepository transactionRepository, INotificationCollector notificationCollector) : base(notificationCollector)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task Add(Transaction transaction)
        {
            if (!Validate(new TransactionValidation(), transaction))
                return;

            TruncateAmout(transaction);

            await _transactionRepository.Add(transaction);
        }

        private void TruncateAmout(Transaction transaction)
        {
            transaction.Amount = transaction.Amount.Truncate();
        }

        public void Dispose()
        {
            _transactionRepository?.Dispose();
        }
    }
}