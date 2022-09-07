using System;
using MediatR;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SC.App.Services.Bill.Common.Requests;
using SC.App.Services.Bill.Common.Responses;

namespace SC.App.Services.Bill.Business.Queries.BillNotification
{
    public class GetBillNotificationById
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        public GetBillNotificationById(Guid id)
        {
            Id = id;
        }
    }

    public class GetBillNotificationByIdQuery : BaseQuery, IRequest<Response<GetBillNotificationResponse>>
    {
        public GetBillNotificationById Payload { get; private set; }

        public GetBillNotificationByIdQuery(HttpRequest request, GetBillNotificationById payload)
            : base(request)
        {
            Payload = payload;
        }
    }
}