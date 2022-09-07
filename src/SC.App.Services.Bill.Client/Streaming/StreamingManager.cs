using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SC.App.Services.Bill.Common.Extensions;
using SC.App.Services.Streaming.Client;

namespace SC.App.Services.Bill.Client.Streaming
{
    public class StreamingManager : IStreamingManager
    {
        private readonly IStreamingClient _streamingClient;

        public StreamingManager(
            IStreamingClient streamingClient)
        {
            _streamingClient = streamingClient;
        }

        public async Task<ResponseOfGetLiveResponse> GetLatestLiveConnected(HttpRequest request, Guid channelId)
        {
            _streamingClient.SetAuthorization(request.GetAuthorization());
            _streamingClient.SetAcceptLanguage(request.GetAcceptLanguage());

            return await _streamingClient.Lives_GetLatestConnectedAsync(channelId);
        }

        public async Task<ResponseOfGetLiveCommentorResponse> GetLiveCommentorByFilterAsync(HttpRequest request, Guid channelId, string name)
        {
            _streamingClient.SetAuthorization(request.GetAuthorization());
            _streamingClient.SetAcceptLanguage(request.GetAcceptLanguage());

            return await _streamingClient.LiveCommentors_GetByFilterAsync(channelId, name);
        }

        public async Task<ResponseOfUnlockBookingResponse> UnlockBookingAsync(HttpRequest request, Guid liveId, Guid liveCommentorId)
        {
            _streamingClient.SetAuthorization(request.GetAuthorization());
            _streamingClient.SetAcceptLanguage(request.GetAcceptLanguage());

            var payload = new UnlockBooking { Live_id = liveId, Live_commentor_id = liveCommentorId };
            return await _streamingClient.BookingUnlocks_UnlockAsync(payload);
        }
    }
}