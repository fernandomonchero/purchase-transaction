using Microsoft.AspNetCore.Mvc;
using PurchaseTransaction.Api.Dtos;
using PurchaseTransaction.Api.Mappings;
using PurchaseTransaction.Domain.Interfaces;
using PurchaseTransaction.Domain.Notifications;
using PurchaseTransaction.Domain.Services;

namespace PurchaseTransaction.Api.Controllers
{
    [Route("api/transactions")]
    public class TransactionController : MainController
    {
        private readonly ITransactionService _transactionService;
        private readonly ITransactionRepository _transactionRepository;

        public TransactionController(ITransactionService transactionService, ITransactionRepository transactionRepository, INotificationCollector notificationCollector) : base(notificationCollector)
        {
            _transactionService = transactionService;
            _transactionRepository = transactionRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<TransactionDto>> Get()
        {
            var transactions = await _transactionRepository.All();

            return transactions.Select(t => new TransactionDto
            {
                Id = t.Id,
                Description = t.Description,
                Date = t.Date,
                Amount = t.Amount
            });
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<TransactionDto>> Get(Guid id)
        {
            var transaction = await _transactionRepository.Get(id);

            if (transaction == null) return NotFound();

            var transactionDto = TransactionMapper.ToDto(transaction);

            return transactionDto;
        }

        [HttpPost]
        public async Task<ActionResult<TransactionDto>> Add(TransactionDto transactionDto)
        {
            if (!ModelState.IsValid) return ApiResponse(ModelState);

            var transaction = TransactionMapper.ToEntity(transactionDto);

            await _transactionService.Add(transaction);

            return ApiResponse(transactionDto);
        }
    }
}