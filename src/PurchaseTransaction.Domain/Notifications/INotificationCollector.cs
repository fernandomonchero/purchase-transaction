namespace PurchaseTransaction.Domain.Notifications
{
    public interface INotificationCollector
    {
        bool HasNotification();
        
        List<string> GetAllNotifications();

        void AddNotification(string notification);
    }
}