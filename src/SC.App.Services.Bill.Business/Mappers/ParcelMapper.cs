using System;
using System.Collections.Generic;
using System.Linq;
using SC.App.Services.Bill.Business.Queries.Parcel;
using SC.App.Services.Bill.Database.Models;
using SC.App.Services.Bill.Lib.Helpers;

namespace SC.App.Services.Bill.Business.Mappers
{
    public class ParcelMapper
    {
        public static List<GetParcelResponse> Map(List<Parcel> parcels, List<Courier.Client.GetOrderResponse> courierOrders)
        {
            return parcels
                .Select(x => Map(x, courierOrders))
                .ToList();
        }

        public static GetParcelResponse Map(Parcel parcel, List<Courier.Client.GetOrderResponse> courierOrders)
        {
            var courierOrder = courierOrders
                .FirstOrDefault(x => x.Ref_id == parcel.Id);

            return Map(parcel, courierOrder);
        }

        public static GetParcelResponse Map(Parcel parcel, Courier.Client.GetOrderResponse courierOrder)
        {
            if (parcel == null)
            {
                return null;
            }

            return new GetParcelResponse
            {
                Id = parcel.Id,
                BillId = parcel.BillId,
                Number = courierOrder?.Feedback?.Ref_1,
                Link = courierOrder?.Feedback?.Tracking?.Url,
                Remark = parcel.Remark,
                Status = Map(parcel.ParcelStatus),
                CreatedOn = Map(parcel.CreatedOn),
                IsPrinted = parcel.IsPrinted,
                IsPacked = parcel.IsPacked
            };
        }

        private static GetParcelStatus Map(ParcelStatus parcelStatus)
        {
            if (parcelStatus == null)
            {
                return null;
            }

            return new GetParcelStatus
            {
                Code = parcelStatus.Code
            };
        }

        private static GetParcelDate Map(DateTime dateTime)
        {
            return new GetParcelDate
            {
                Date = dateTime,
                IsPresentDate = DateTimeHelper.IsPresentDate(dateTime),
                IsPresentMonth = DateTimeHelper.IsPresentMonth(dateTime),
                IsPresentYear = DateTimeHelper.IsPresentYear(dateTime)
            };
        }
    }
}