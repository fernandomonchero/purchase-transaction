using FluentValidation;
using FluentValidation.Results;
using PurchaseTransaction.Domain.Models;
using PurchaseTransaction.Domain.Notifications;

namespace PurchaseTransaction.Domain.Services
{
    public abstract class DomainService
    {
        private readonly INotificationCollector _notificationCollector;

        protected DomainService(INotificationCollector notificationCollector)
        {
            _notificationCollector = notificationCollector;
        }

        protected void Notify(ValidationResult validationResult)
        {
            foreach (var error in validationResult.Errors)
            {
                Notify(error.ErrorMessage);
            }
        }

        protected void Notify(string errorMesage)
        {
            _notificationCollector.AddNotification(errorMesage);
        }

        protected bool Validate<TV, TE>(TV validacao, TE entidade) where TV : AbstractValidator<TE> where TE : Entity
        {
            var validator = validacao.Validate(entidade);

            if (validator.IsValid) return true;

            Notify(validator);

            return false;
        }
    }
}