using PurchaseTransaction.Domain.Notifications;

namespace PurchaseTransaction.UnitTests.Notifications
{
    [TestFixture]
    public class NotificationCollectorTests
    {
        [Test]
        public void Constructor_Should_StartWithoutNotifications()
        {
            // Arrange & Act
            var collector = new NotificationCollector();

            // Assert
            Assert.That(collector.HasNotification(), Is.False);
            Assert.That(collector.GetAllNotifications(), Is.Not.Null);
            Assert.That(collector.GetAllNotifications(), Is.Empty);
        }

        [Test]
        public void AddNotification_Should_AddNotificationToCollection()
        {
            // Arrange
            var collector = new NotificationCollector();

            // Act
            collector.AddNotification("Error");

            // Assert
            Assert.That(collector.GetAllNotifications(), Has.Count.EqualTo(1));
            Assert.That(collector.GetAllNotifications()[0], Is.EqualTo("Error"));
        }

        [Test]
        public void AddNotification_Should_AllowMultipleNotifications()
        {
            // Arrange
            var collector = new NotificationCollector();

            // Act
            collector.AddNotification("Error 1");
            collector.AddNotification("Error 2");
            collector.AddNotification("Error 3");

            // Assert
            Assert.That(collector.GetAllNotifications(), Has.Count.EqualTo(3));
            CollectionAssert.AreEqual(new[] { "Error 1", "Error 2", "Error 3" }, collector.GetAllNotifications());
        }

        [Test]
        public void HasNotification_Should_ReturnFalseWhenCollectionIsEmpty()
        {
            // Arrange
            var collector = new NotificationCollector();

            // Act
            var result = collector.HasNotification();

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void HasNotification_Should_ReturnTrueWhenCollectionHasNotifications()
        {
            // Arrange
            var collector = new NotificationCollector();
            collector.AddNotification("Error");

            // Act
            var result = collector.HasNotification();

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void GetAllNotifications_Should_ReturnSameListInstance()
        {
            // Arrange
            var collector = new NotificationCollector();

            // Act
            var list1 = collector.GetAllNotifications();
            var list2 = collector.GetAllNotifications();

            // Assert
            Assert.That(ReferenceEquals(list1, list2), Is.True);
        }
    }
}