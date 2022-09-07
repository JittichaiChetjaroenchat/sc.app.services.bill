using System.Collections.Generic;
using System.Linq;
using SC.App.Services.Bill.Business.Enums;
using SC.App.Services.Bill.Business.Helpers;
using SC.App.Services.Bill.Business.Queries.Bill;
using SC.App.Services.Bill.Database.Models;
using SC.App.Services.Bill.Lib.Extensions;

namespace SC.App.Services.Bill.Business.Mappers
{
    public class BillManifestMapper
    {
        public static List<GetBillManifestResponse> Map(List<Database.Models.Bill> bills, ICollection<Order.Client.GetOrderResponse> orders)
        {
            return bills
                .Select(x => Map(x, orders))
                .ToList();
        }

        public static GetBillManifestResponse Map(Database.Models.Bill bill, ICollection<Order.Client.GetOrderResponse> orders)
        {
            var billOrders = orders
                .Where(x => x.Bill != null && x.Bill.Id == bill.Id)
                .ToList();

            return new GetBillManifestResponse
            {
                Id = bill.Id,
                IsDeposit = bill.IsDeposit,
                Customer = Map(bill.BillRecipient),
                Goods = Map(billOrders),
                Shipping = Map(billOrders, bill.BillShipping),
                LatestPayment = Map(bill.Payments),
                Status = Map(bill.BillStatus)
            };
        }

        private static GetBillManifestCustomer Map(BillRecipient billRecipient)
        {
            return new GetBillManifestCustomer
            {
                Id = billRecipient.CustomerId
            };
        }

        private static GetBillManifestGoods Map(List<Order.Client.GetOrderResponse> orders)
        {
            var nonCancelledOrders = orders
                .Where(x => x.Status.Code != EnumOrderStatus.Cancelled.GetDescription())
                .ToList();
            var amount = OrderHelper.GetAmount(nonCancelledOrders);
            var cost = OrderHelper.GetPrice(nonCancelledOrders);

            return new GetBillManifestGoods
            {
                Amount = amount,
                Cost = cost
            };
        }

        private static GetBillManifestShipping Map(List<Order.Client.GetOrderResponse> orders, BillShipping billShipping)
        {
            var nonCancelledOrders = orders
                .Where(x => x.Status.Code != EnumOrderStatus.Cancelled.GetDescription())
                .ToList();
            var cost = ShippingHelper.CalculateCost(nonCancelledOrders, billShipping);

            return new GetBillManifestShipping
            {
                Cost = cost
            };
        }

        private static GetBillManifestLatestPayment Map(ICollection<Payment> payments)
        {
            if (payments.IsEmpty())
            {
                return null;
            }

            var latestPayment = payments
                .OrderByDescending(x => x.PaymentNo)
                .FirstOrDefault();
            return new GetBillManifestLatestPayment
            {
                Id = latestPayment.Id
            };
        }

        private static GetBillManifestStatus Map(BillStatus billStatus)
        {
            return new GetBillManifestStatus
            {
                Code = billStatus.Code
            };
        }
    }
}