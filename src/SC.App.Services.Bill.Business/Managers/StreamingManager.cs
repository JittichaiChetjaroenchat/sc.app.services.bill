using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SC.App.Services.Bill.Business.Exceptions;
using SC.App.Services.Bill.Business.Managers.Interface;
using Serilog;

namespace SC.App.Services.Bill.Business.Managers
{
    public class StreamingManager : IStreamingManager
    {
        private readonly Client.Streaming.IStreamingManager _streamingManager;

        public StreamingManager(
            Client.Streaming.IStreamingManager streamingManager)
        {
            _streamingManager = streamingManager;
        }

        public async Task UnlockBooking(HttpRequest request, Guid channelId, string name)
        {
            try
            {
                // Get latest live connected
                Streaming.Client.GetLiveResponse latestLiveConntected = null;
                var getLatestLiveConnectedResponse = await _streamingManager.GetLatestLiveConnected(request, channelId);
                if (!Client.Streaming.StreamingClientHelper.IsSuccess(getLatestLiveConnectedResponse))
                {
                    return;
                }
                else
                {
                    latestLiveConntected = getLatestLiveConnectedResponse.Data;
                }

                if (latestLiveConntected == null)
                {
                    return;
                }

                // Get live's commentor
                Streaming.Client.GetLiveCommentorResponse liveCommentor = null;
                var getLiveCommentorByFilterResponse = await _streamingManager.GetLiveCommentorByFilterAsync(request, channelId, name);
                if (!Client.Streaming.StreamingClientHelper.IsSuccess(getLiveCommentorByFilterResponse))
                {
                    return;
                }
                else
                {
                    liveCommentor = getLiveCommentorByFilterResponse.Data;
                }

                if (liveCommentor == null)
                {
                    return;
                }

                // Unlock booking
                var unlockBookingResponse = await _streamingManager.UnlockBookingAsync(request, latestLiveConntected.Id, liveCommentor.Id);
                if (!Client.Streaming.StreamingClientHelper.IsSuccess(unlockBookingResponse))
                {
                    throw new SkipProcessException("Unlock booking failed.");
                }
            }
            catch (SkipProcessException ex)
            {
                Log.Information(ex, string.Empty);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);
            }

            await Task.CompletedTask;
        }
    }
}