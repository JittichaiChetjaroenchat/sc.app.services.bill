using System;
using System.Collections.Generic;
using System.Linq;
using SC.App.Services.Bill.Business.Enums;
using SC.App.Services.Bill.Business.Helpers;
using SC.App.Services.Bill.Business.Queries.Bill;
using SC.App.Services.Bill.Database.Models;
using SC.App.Services.Bill.Lib.Extensions;
using SC.App.Services.Bill.Lib.Helpers;

namespace SC.App.Services.Bill.Business.Mappers
{
    public class SearchBillMapper
    {
        public static SearchBillResponse Map(int page, int pageSize, int numberOfItems, int numberOfPages, List<SearchBillItem> items)
        {
            return new SearchBillResponse
            {
                Page = page,
                PageSize = pageSize,
                NumberOfItems = numberOfItems,
                NumberOfPages = numberOfPages,
                Items = items
            };
        }

        public static SearchBillItem Map(
            string baseUrl,
            Database.Models.Bill bill,
            Customer.Client.GetCustomerResponse customer,
            ICollection<Order.Client.GetOrderResponse> orders,
            ICollection<Courier.Client.GetOrderResponse> courierOrders,
            ICollection<Area.Client.GetAreaResponse> areas,
            Setting.Client.GetPreferencesResponse preferences)
        {
            return new SearchBillItem
            {
                Bill = BillMapper.Map(baseUrl, bill, bill.BillStatus, bill.BillDiscount, bill.BillPayment, preferences),
                Notification = NotificationMapper.Map(bill.BillNotification),
                Recipient = RecipientMapper.Map(bill.BillRecipient, customer, areas),
                Order = OrderMapper.Map(orders),
                Payment = PaymentMapper.Map(bill.Payments),
                Shipping = ShippingMapper.Map(orders, bill.BillShipping, bill.Parcels, courierOrders),
                Cost = CostMapper.Map(orders, bill.BillShipping, bill.Payments, bill.BillDiscount, bill.BillPayment),
                Tags = TagMapper.Map(bill.BillTags)
            };
        }

        private class BillMapper
        {
            public static SearchBill Map(string baseUrl, Database.Models.Bill bill, BillStatus status, BillDiscount discount, BillPayment billPayment, Setting.Client.GetPreferencesResponse preferences)
            {
                return new SearchBill
                {
                    Id = bill.Id,
                    BillNo = bill.BillNo,
                    RunningNo = bill.RunningNo,
                    IsDeposit = bill.IsDeposit,
                    IsNewCustomer = bill.IsNewCustomer,
                    Link = BillHelper.GetLink(baseUrl, bill.Key, preferences.Language),
                    Discount = DiscountMapper.Map(discount),
                    Payment = BillPaymentMapper.Map(billPayment),
                    Status = Map(status),
                    CreatedOn = Map(bill.CreatedOn)
                };
            }

            private static SearchBillStatus Map(BillStatus status)
            {
                return new SearchBillStatus
                {
                    Code = status.Code
                };
            }

            private static SearchBillDate Map(DateTime dateTime)
            {
                return new SearchBillDate
                {
                    Date = dateTime,
                    IsPresentDate = DateTimeHelper.IsPresentDate(dateTime),
                    IsPresentMonth = DateTimeHelper.IsPresentMonth(dateTime),
                    IsPresentYear = DateTimeHelper.IsPresentYear(dateTime)
                };
            }
        }

        private class NotificationMapper
        {
            public static SearchBillNotification Map(BillNotification notification)
            {
                if (notification == null)
                {
                    return null;
                }

                var status = BillNotificationHelper.GetStatus(notification);

                return new SearchBillNotification
                {
                    Id = notification.Id,
                    Status = Map(status)
                };
            }

            private static SearchBillNotificationStatus Map(EnumBillNotificationStatus status)
            {
                return new SearchBillNotificationStatus
                {
                    Code = status.GetDescription()
                };
            }
        }

        private class OrderMapper
        {
            public static SearchBillOrder Map(ICollection<Order.Client.GetOrderResponse> orders)
            {
                var nonCancelledOrders = orders
                    .Where(x => x.Status.Code != EnumOrderStatus.Cancelled.GetDescription())
                    .ToList();

                return new SearchBillOrder
                {
                    Amount = OrderHelper.GetAmount(nonCancelledOrders),
                    Price = OrderHelper.GetPrice(nonCancelledOrders),
                };
            }
        }

        private class PaymentMapper
        {
            public static SearchPayment Map(ICollection<Payment> payments)
            {
                var latestPayment = PaymentHelper.GetLatest(payments);

                return new SearchPayment
                {
                    Amount = Amount(payments),
                    Status = Status(latestPayment),
                    Latest = Latest(latestPayment),
                    Accepted = Accepted(payments)
                };
            }

            private static decimal Amount(ICollection<Payment> payments)
            {
                return payments.Sum(x => x.Amount);
            }

            private static SearchPaymentStatus Status(Payment latestPayment)
            {
                if (latestPayment == null || latestPayment.PaymentStatus == null)
                {
                    return null;
                }

                return new SearchPaymentStatus
                {
                    Code = latestPayment.PaymentStatus.Code
                };
            }

            private static SearchPaymentLatest Latest(Payment latestPayment)
            {
                if (latestPayment == null)
                {
                    return null;
                }

                return new SearchPaymentLatest
                {
                    Id = latestPayment.Id,
                    HasEvidence = latestPayment.EvidenceId.HasValue,
                    Amount = latestPayment.Amount,
                    Payon = PayOn(latestPayment.PayOn),
                    Verification = PaymentVerificationMapper.Map(latestPayment.PaymentVerification)
                };
            }

            private static SearchPaymentAccepted Accepted(ICollection<Payment> payments)
            {
                var accepts = PaymentHelper.GetAccepted(payments);

                return new SearchPaymentAccepted
                {
                    Time = accepts.Count,
                    Amount = accepts.Sum(x => x.Amount)
                };
            }

            private static SearchPaymentDate PayOn(DateTime dateTime)
            {
                return new SearchPaymentDate
                {
                    Date = dateTime,
                    IsPresentDate = DateTimeHelper.IsPresentDate(dateTime),
                    IsPresentMonth = DateTimeHelper.IsPresentMonth(dateTime),
                    IsPresentYear = DateTimeHelper.IsPresentYear(dateTime)
                };
            }
        }

        private class PaymentVerificationMapper
        {
            public static SearchPaymentVerification Map(PaymentVerification paymentVerification)
            {
                if (paymentVerification == null)
                {
                    return null;
                }

                return new SearchPaymentVerification
                {
                    Id = paymentVerification.Id,
                    Remark = paymentVerification.Remark,
                    Status = Map(paymentVerification.PaymentVerificationStatus)
                };
            }

            private static SearchPaymentVerificationStatus Map(PaymentVerificationStatus paymentVerificationStatus)
            {
                if (paymentVerificationStatus == null)
                {
                    return null;
                }

                return new SearchPaymentVerificationStatus
                {
                    Code = paymentVerificationStatus.Code
                };
            }
        }

        private class RecipientMapper
        {
            public static SearchBillRecipient Map(BillRecipient recipient, Customer.Client.GetCustomerResponse customer, ICollection<Area.Client.GetAreaResponse> areas)
            {
                return new SearchBillRecipient
                {
                    Id = recipient.Id,
                    AliasName = recipient.AliasName,
                    Name = recipient.Name,
                    Contact = RecipientContactMapper.Map(recipient.BillRecipientContact, areas),
                    Customer = CustomerMapper.Map(customer)
                };
            }
        }

        private class RecipientContactMapper
        {
            public static SearchBillRecipientContact Map(BillRecipientContact recipientContact, ICollection<Area.Client.GetAreaResponse> areas)
            {
                if (recipientContact == null)
                {
                    return null;
                }

                var recipientContactArea = areas
                    .FirstOrDefault(x => x.Id == recipientContact.AreaId);
                var fullAddress = BillRecipientContactHelper.GetFullAddress(recipientContact.Address, recipientContactArea);

                return new SearchBillRecipientContact
                {
                    FullAddress = fullAddress,
                    PrimaryPhone = recipientContact.PrimaryPhone,
                    SecondaryPhone = recipientContact.SecondaryPhone,
                    Email = recipientContact.Email
                };
            }
        }

        private class CustomerMapper
        {
            public static SearchBillCustomer Map(Customer.Client.GetCustomerResponse customer)
            {
                if (customer == null)
                {
                    return null;
                }

                return new SearchBillCustomer
                {
                    Id = customer.Id,
                    Name = customer.Name,
                    IsNew = customer.Is_new,
                    IsBlocked = customer.Is_blocked,
                    Tags = CustomerTagMapper.Map(customer.Tags)
                };
            }
        }

        private class CustomerTagMapper
        {
            public static List<SearchBillCustomerTag> Map(ICollection<Customer.Client.GetCustomerTag> customerTags)
            {
                return customerTags
                    .Select(Map)
                    .ToList();
            }

            public static SearchBillCustomerTag Map(Customer.Client.GetCustomerTag customerTag)
            {
                if (customerTag == null)
                {
                    return null;
                }

                return new SearchBillCustomerTag
                {
                    Id = customerTag.Id,
                    TagId = customerTag.Tag_id,
                    Color = customerTag.Color,
                    Name = customerTag.Name,
                    Description = customerTag.Description
                };
            }
        }

        private class DiscountMapper
        {
            public static SearchBillDiscount Map(BillDiscount discount)
            {
                if (discount == null || (!discount.Amount.HasValue && !discount.Percentage.HasValue))
                {
                    return null;
                }

                return new SearchBillDiscount
                {
                    Amount = discount.Amount,
                    Percentage = discount.Percentage
                };
            }

            public static SearchBillCostDiscount Map(decimal total, BillDiscount billDiscount)
            {
                var hasDiscount = billDiscount.Amount.HasValue || billDiscount.Percentage.HasValue;

                return new SearchBillCostDiscount
                {
                    HasDiscount = hasDiscount,
                    Amount = billDiscount.Amount,
                    Percentage = billDiscount.Percentage,
                    Total = DiscountHelper.Calculate(total, billDiscount)
                };
            }
        }

        private class BillPaymentMapper
        {
            public static SearchBillPayment Map(BillPayment billPayment)
            {
                if (billPayment == null)
                {
                    return null;
                }

                return new SearchBillPayment
                {
                    Type = Map(billPayment.BillPaymentType),
                    HasCodAddOn = billPayment.HasCodAddOn,
                    CodAddOnAmount = billPayment.CodAddOnAmount,
                    CodAddOnPercentage = billPayment.CodAddOnPercentage,
                    HasVat = billPayment.HasVat,
                    IncludedVat = billPayment.IncludedVat
                };
            }

            private static SearchBillPaymentType Map(BillPaymentType billPaymentType)
            {
                return new SearchBillPaymentType
                {
                    Code = billPaymentType.Code
                };
            }
        }

        private class ShippingMapper
        {
            public static SearchBillShipping Map(ICollection<Order.Client.GetOrderResponse> orders, BillShipping billShipping, ICollection<Parcel> parcels, ICollection<Courier.Client.GetOrderResponse> courierOrders)
            {
                var nonCancelledOrders = orders
                    .Where(x => x.Status.Code != EnumOrderStatus.Cancelled.GetDescription())
                    .ToList();
                var cost = ShippingHelper.CalculateCost(nonCancelledOrders, billShipping);

                return new SearchBillShipping
                {
                    Cost = cost,
                    Parcels = ParcelMapper.Map(parcels, courierOrders)
                };
            }
        }

        private class ParcelMapper
        {
            public static List<SearchBillParcel> Map(ICollection<Parcel> parcels, ICollection<Courier.Client.GetOrderResponse> courierOrders)
            {
                return parcels
                    .Select(x => Map(x, courierOrders))
                    .ToList();
            }

            public static SearchBillParcel Map(Parcel parcel, ICollection<Courier.Client.GetOrderResponse> courierOrders)
            {
                var courierOrder = courierOrders
                    .FirstOrDefault(x => x.Ref_id == parcel.Id);

                return Map(parcel, courierOrder);
            }

            public static SearchBillParcel Map(Parcel parcel, Courier.Client.GetOrderResponse courierOrder)
            {
                return new SearchBillParcel
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

            private static SearchBillParcelStatus Map(ParcelStatus parcelStatus)
            {
                return new SearchBillParcelStatus
                {
                    Code = parcelStatus.Code
                };
            }

            private static SearchBillParcelDate Map(DateTime dateTime)
            {
                return new SearchBillParcelDate
                {
                    Date = dateTime,
                    IsPresentDate = DateTimeHelper.IsPresentDate(dateTime),
                    IsPresentMonth = DateTimeHelper.IsPresentMonth(dateTime),
                    IsPresentYear = DateTimeHelper.IsPresentYear(dateTime)
                };
            }
        }

        private class CostMapper
        {
            public static SearchBillCost Map(ICollection<Order.Client.GetOrderResponse> orders, BillShipping billShipping, ICollection<Payment> payments, BillDiscount billDiscount, BillPayment billPayment)
            {
                var nonCancelledOrders = orders
                    .Where(x => x.Status.Code != EnumOrderStatus.Cancelled.GetDescription())
                    .ToList();
                var goods = OrderHelper.GetPrice(nonCancelledOrders);
                var shipping = ShippingHelper.CalculateCost(nonCancelledOrders, billShipping);
                var beforeDiscount = goods + shipping;
                var discount = DiscountMapper.Map(beforeDiscount, billDiscount);
                var afterDiscount = beforeDiscount - discount.Total;
                var cod = CodMapper.Map(afterDiscount, billPayment);
                var afterCod = afterDiscount + cod.Total;
                var gross = afterCod;
                var vat = VatMapper.Map(gross, billPayment);
                var afterVat = gross + vat.Total;
                var net = afterVat;
                var paid = OrderHelper.GetAcceptedAmount(payments);
                var payExtra = net - paid;

                return new SearchBillCost
                {
                    Goods = goods,
                    Shipping = shipping,
                    Discount = discount,
                    Cod = cod,
                    Vat = vat,
                    Gross = gross,
                    Net = net,
                    Paid = paid,
                    PayExtra = payExtra
                };
            }

            private class CodMapper
            {
                public static SearchBillCostCod Map(decimal total, BillPayment billPayment)
                {
                    return new SearchBillCostCod
                    {
                        HasAddOn = billPayment.HasCodAddOn,
                        Amount = billPayment.CodAddOnAmount,
                        Percentage = billPayment.CodAddOnPercentage,
                        Total = CodHelper.Calculate(total, billPayment)
                    };
                }
            }

            private class VatMapper
            {
                public static SearchBillCostVat Map(decimal total, BillPayment billPayment)
                {
                    return new SearchBillCostVat
                    {
                        HasVat = billPayment.HasVat,
                        IncludedVat = billPayment.IncludedVat,
                        VatPercentage = billPayment.VatPercentage,
                        Total = VatHelper.Calculate(total, billPayment)
                    };
                }
            }
        }

        private class TagMapper
        {
            public static List<SearchBillTag> Map(ICollection<BillTag> billTags)
            {
                return billTags
                    .Select(Map)
                    .ToList();
            }

            private static SearchBillTag Map(BillTag billTag)
            {
                if (billTag == null || billTag.Tag == null)
                {
                    return null;
                }

                return new SearchBillTag
                {
                    Id = billTag.Id,
                    TagId = billTag.Tag.Id,
                    Name = billTag.Tag.Name,
                    Color = billTag.Tag.Color,
                    Description = billTag.Tag.Description
                };
            }
        }
    }
}