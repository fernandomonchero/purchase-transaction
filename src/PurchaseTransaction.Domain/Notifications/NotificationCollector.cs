namespace PurchaseTransaction.Domain.Notifications
{
    public class NotificationCollector : INotificationCollector
    {
        private List<string> _notifications;

        public NotificationCollector()
        {
            _notifications = new List<string>();
        }

        public void AddNotification(string notification)
        {
            _notifications.Add(notification);
        }

        public List<string> GetAllNotifications()
        {
            return _notifications;
        }

        public bool HasNotification()
        {
            return _notifications.Any();
        }
    }
}