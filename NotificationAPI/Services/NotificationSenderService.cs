using System.Net.Http.Headers;
using CorePush.Firebase;
using NotificationAPI.Models;

namespace NotificationAPI.Services
{
    public interface INotificationSenderService
    {
        Task<ResponseModel> SendNotification(NotificationModel notification);
    }
    public class NotificationSenderService : INotificationSenderService
    {
        private readonly FirebaseSettings _firebaseSettings;
        public NotificationSenderService(FirebaseSettings firebaseSettings)
        {
            _firebaseSettings = firebaseSettings;
        }

        public async Task<ResponseModel> SendNotification(NotificationModel notification)
        {
            var client = new HttpClient();
            var authorizationKey = $"key={_firebaseSettings.PrivateKey}";
            var payload = new GoogleNotification.DataPayload();
            payload.Title = notification.Title;
            payload.Body = notification.Body;
            var notificationModel = new GoogleNotification()
            {
                Data = payload,
                Notification = payload
            };
            var sender = new FirebaseSender(_firebaseSettings, client);
            var response = await sender.SendAsync(notificationModel);

            return new ResponseModel();
        }
    }
}
