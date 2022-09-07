using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SC.App.Services.Notification.Client;

namespace SC.App.Services.Bill.Client.Notification
{
    public interface INotificationManager
    {
        Task<ResponseOfCreateNotificationResponse> CreateNotificationForChannelAsync(HttpRequest request, Guid channelId, EnumNotificationType type, EnumNotificationSubject subject, EnumNotificationDisplay display, string title, string content);

        Task<ResponseOfCreateNotificationResponse> CreateNotificationForUserAsync(HttpRequest request, Guid userId, EnumNotificationType type, EnumNotificationSubject subject, EnumNotificationDisplay display, string title, string content);
    }
}