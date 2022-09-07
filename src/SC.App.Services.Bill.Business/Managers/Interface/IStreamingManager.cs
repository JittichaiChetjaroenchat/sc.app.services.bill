using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace SC.App.Services.Bill.Business.Managers.Interface
{
    public interface IStreamingManager
    {
        Task UnlockBooking(HttpRequest request, Guid channelId, string name);
    }
}