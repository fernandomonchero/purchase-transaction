using Microsoft.AspNetCore.Mvc;
using PurchaseTransaction.Api.Dtos;
using PurchaseTransaction.Api.Mappings;
using PurchaseTransaction.Domain.Interfaces;
using PurchaseTransaction.Domain.Notifications;
using System.Text.Json;

namespace PurchaseTransaction.Api.Controllers
{
    [Route("api/converted-transactions")]
    public class ConvertedTransactionController : MainController
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IExchangeRateService _exchangeRateService;

        public ConvertedTransactionController(ITransactionRepository transactionRepository, IExchangeRateService exchangeRateService, INotificationCollector notificationCollector) : base(notificationCollector)
        {
            _transactionRepository = transactionRepository;
            _exchangeRateService = exchangeRateService;
        }

        [HttpGet]
        public async Task<ActionResult<ConvertedTransactionDto>> GetConverted(Guid id, string country)
        {
            var transaction = await _transactionRepository.Get(id);

            if (transaction == null) return NotFound();

            try
            {
                var mostRecentExchangeRate = await _exchangeRateService.GetValidExchangeRate(country, transaction.Date);

                if (mostRecentExchangeRate == null)
                    return NotFound("The purchase can not be converted to currency for this country");

                return ConvertedTransactionMapper.ToDto(transaction, mostRecentExchangeRate);
            }
            catch (TaskCanceledException)
            {
                return StatusCode(StatusCodes.Status504GatewayTimeout, new { message = "The Treasury API took too long to respond" });
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, new { message = "The Treasury API is currently unreachable" });
            }
            catch (JsonException ex)
            {
                return StatusCode(StatusCodes.Status502BadGateway, new { message = "Received an invalid response from the Treasury API" });
            }
        }
    }
}