using System;
using MediatR;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SC.App.Services.Bill.Common.Requests;
using SC.App.Services.Bill.Common.Responses;

namespace SC.App.Services.Bill.Business.Commands.Parcel
{
    public class UpdateParcelPrinted
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        public UpdateParcelPrinted(Guid id)
        {
            Id = id;
        }
    }

    public class UpdateParcelPrintedCommand : BaseCommand, IRequest<Response<UpdateParcelPrintedResponse>>
    {
        public UpdateParcelPrinted Payload { get; set; }

        public UpdateParcelPrintedCommand(HttpRequest request, UpdateParcelPrinted payload)
            : base(request)
        {
            Payload = payload;
        }
    }
}