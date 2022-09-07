using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SC.App.Services.Streaming.Client;

namespace SC.App.Services.Bill.Client.Streaming
{
    public interface IStreamingManager
    {
        Task<ResponseOfGetLiveResponse> GetLatestLiveConnected(HttpRequest request, Guid channelId);

        Task<ResponseOfGetLiveCommentorResponse> GetLiveCommentorByFilterAsync(HttpRequest request, Guid channelId, string name);

        Task<ResponseOfUnlockBookingResponse> UnlockBookingAsync(HttpRequest request, Guid liveId, Guid liveCommentorId);
    }
}