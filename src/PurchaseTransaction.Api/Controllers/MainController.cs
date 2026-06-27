using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using PurchaseTransaction.Domain.Notifications;

namespace PurchaseTransaction.Api.Controllers
{
    [ApiController]
    public abstract class MainController : Controller
    {
        private readonly INotificationCollector _notificationCollector;

        protected MainController(INotificationCollector notificationCollector)
        {
            _notificationCollector = notificationCollector;
        }

        protected bool IsOkResponse()
        {
            return !_notificationCollector.HasNotification();
        }

        protected ActionResult ApiResponse(object result = null)
        {
            if (IsOkResponse())
            {
                return Ok(new
                {
                    success = true,
                    data = result
                });
            }

            return BadRequest(new
            {
                success = false,
                errors = _notificationCollector.GetAllNotifications()
            });
        }

        protected ActionResult ApiResponse(ModelStateDictionary modelState)
        {
            if (!modelState.IsValid) NotifyInvalidModel(modelState);
            return ApiResponse();
        }

        protected void NotifyInvalidModel (ModelStateDictionary modelState)
        {
            foreach (var error in modelState.Values.SelectMany(e => e.Errors))
            {
                var errorMsg = error.Exception == null ? error.ErrorMessage : error.Exception.Message;
                NotifyError(errorMsg);
            }
        }

        protected void NotifyError(string notification)
        {
            _notificationCollector.AddNotification(notification);
        }
    }
}