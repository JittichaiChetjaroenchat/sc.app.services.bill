using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SC.App.Services.Bill.Common.Extensions;
using SC.App.Services.Notification.Client;

namespace SC.App.Services.Bill.Client.Notification
{
    public class NotificationManager : INotificationManager
    {
        private readonly INotificationClient _client;

        public NotificationManager(
            INotificationClient client)
        {
            _client = client;
        }

        public async Task<ResponseOfCreateNotificationResponse> CreateNotificationForChannelAsync(HttpRequest request, Guid channelId, EnumNotificationType type, EnumNotificationSubject subject, EnumNotificationDisplay display, string title, string content)
        {
            _client.SetAuthorization(request.GetAuthorization());
            _client.SetAcceptLanguage(request.GetAcceptLanguage());

            var payload = new CreateNotification
            {
                Channel_id = channelId,
                User_id = null,
                Type = type,
                Subject = subject,
                Display = display,
                Title = title,
                Content = content
            };

            return await _client.Notifications_CreateAsync(payload);
        }

        public async Task<ResponseOfCreateNotificationResponse> CreateNotificationForUserAsync(HttpRequest request, Guid userId, EnumNotificationType type, EnumNotificationSubject subject, EnumNotificationDisplay display, string title, string content)
        {
            _client.SetAuthorization(request.GetAuthorization());
            _client.SetAcceptLanguage(request.GetAcceptLanguage());

            var payload = new CreateNotification
            {
                Channel_id = null,
                User_id = userId,
                Type = type,
                Subject = subject,
                Display = display,
                Title = title,
                Content = content
            };

            return await _client.Notifications_CreateAsync(payload);
        }
    }
}