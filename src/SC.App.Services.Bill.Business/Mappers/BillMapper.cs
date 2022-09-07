using System;
using System.Collections.Generic;
using System.Linq;
using SC.App.Services.Bill.Business.Enums;
using SC.App.Services.Bill.Business.Helpers;
using SC.App.Services.Bill.Business.Queries.Bill;
using SC.App.Services.Bill.Business.Queries.Payment;
using SC.App.Services.Bill.Database.Models;
using SC.App.Services.Bill.Lib.Extensions;
using SC.App.Services.Bill.Lib.Helpers;

namespace SC.App.Services.Bill.Business.Mappers
{
    public class BillMapper
    {
        public static GetBillResponse Map(
            string baseUrl,
            Database.Models.Bill bill,
            Customer.Client.GetCustomerResponse customer,
            Area.Client.GetAreaResponse contactArea,
            ICollection<Order.Client.GetOrderResponse> orders,
            List<Courier.Client.GetOrderResponse> courierOrders,
            Setting.Client.GetPreferencesResponse preferences)
        {
            return new GetBillResponse
            {
                Bill = Map(baseUrl, bill, preferences),
                Recipient = RecipientMapper.Map(bill.BillRecipient, contactArea, customer),
                Orders = OrderMapper.Map(orders),
                Payment = PaymentMapper.Map(bill.Payments),
                Shipping = ShippingMapper.Map(bill.BillShipping, bill.Parcels, courierOrders),
                Cost = CostMapper.Map(orders, bill.BillShipping, bill.Payments, bill.BillDiscount, bill.BillPayment),
                Tags = TagMapper.Map(bill.BillTags)
            };
        }

        private static GetBill Map(string baseUrl, Database.Models.Bill bill, Setting.Client.GetPreferencesResponse preferences)
        {
            return new GetBill
            {
                Id = bill.Id,
                ChannelId = bill.ChannelId,
                BillNo = bill.BillNo,
                RunningNo = bill.RunningNo,
                IsDeposit = bill.IsDeposit,
                IsNewCustomer = bill.IsNewCustomer,
                Remark = bill.Remark,
                Link = BillHelper.GetLink(baseUrl, bill.Key, preferences.Language),
                Discount = DiscountMapper.Map(bill.BillDiscount),
                Payment = BillPaymentMapper.Map(bill.BillPayment),
                Status = Map(bill.BillStatus),
                CreatedOn = Map(bill.CreatedOn)
            };
        }

        private static GetBillStatus Map(BillStatus status)
        {
            return new GetBillStatus
            {
                Code = status.Code
            };
        }

        private static GetBillDate Map(DateTime dateTime)
        {
            return new GetBillDate
            {
                Date = dateTime,
                IsPresentDate = DateTimeHelper.IsPresentDate(dateTime),
                IsPresentMonth = DateTimeHelper.IsPresentMonth(dateTime),
                IsPresentYear = DateTimeHelper.IsPresentYear(dateTime)
            };
        }

        private class RecipientMapper
        {
            public static GetBillRecipient Map(BillRecipient recipient, Area.Client.GetAreaResponse contactArea, Customer.Client.GetCustomerResponse customer)
            {
                return new GetBillRecipient
                {
                    Id = recipient.Id,
                    AliasName = recipient.AliasName,
                    Name = recipient.Name,
                    Contact = RecipientContactMapper.Map(recipient?.BillRecipientContact, contactArea, customer?.Contact),
                    Customer = CustomerMapper.Map(customer)
                };
            }
        }

        private class RecipientContactMapper
        {
            public static GetBillRecipientContact Map(BillRecipientContact billContact, Area.Client.GetAreaResponse billContactArea, Customer.Client.GetCustomerContact customerContact)
            {
                if (billContact == null && customerContact == null)
                {
                    return null;
                }

                var address = billContact != null ? billContact.Address : customerContact.Address;
                var areaId = billContact != null ? billContact.AreaId : customerContact.Area_id;
                var subDistrict = billContact != null ? billContactArea.Sub_district : customerContact.Sub_district;
                var district = billContact != null ? billContactArea.District : customerContact.District;
                var province = billContact != null ? billContactArea.Province : customerContact.Province;
                var postalCode = billContact != null ? billContactArea.Postal_code : customerContact.Postal_code;
                var primaryPhone = billContact != null ? billContact.PrimaryPhone : customerContact.Primary_phone;
                var secondaryPhone = billContact != null ? billContact.SecondaryPhone : customerContact.Secondary_phone;
                var email = billContact != null ? billContact.Email : customerContact.Email;

                return new GetBillRecipientContact
                {
                    Address = address,
                    AreaId = areaId,
                    SubDistrict = subDistrict,
                    District = district,
                    Province = province,
                    PostalCode = postalCode,
                    PrimaryPhone = primaryPhone,
                    SecondaryPhone = secondaryPhone,
                    Email = email
                };
            }
        }

        private class CustomerMapper
        {
            public static GetBillRecipientCustomer Map(Customer.Client.GetCustomerResponse customer)
            {
                if (customer == null)
                {
                    return null;
                }

                return new GetBillRecipientCustomer
                {
                    Id = customer.Id,
                    Name = customer.Name,
                    IsNew = customer.Is_new,
                    IsBlocked = customer.Is_blocked,
                    Facebook = CustomerFacebookMapper.Map(customer.Facebook),
                    Tags = CustomerTagMapper.Map(customer.Tags)
                };
            }
        }

        private class CustomerFacebookMapper
        {
            public static GetBillRecipientCustomerFacebook Map(Customer.Client.GetCustomerFacebook customerFacebook)
            {
                if (customerFacebook == null)
                {
                    return null;
                }

                return new GetBillRecipientCustomerFacebook
                {
                    Id = customerFacebook.Facebook_id,
                    Name = customerFacebook.Facebook_name
                };
            }
        }

        private class CustomerTagMapper
        {
            public static List<GetBillRecipientCustomerTag> Map(ICollection<Customer.Client.GetCustomerTag> customerTags)
            {
                return customerTags
                    .Select(Map)
                    .ToList();
            }

            public static GetBillRecipientCustomerTag Map(Customer.Client.GetCustomerTag customerTag)
            {
                if (customerTag == null)
                {
                    return null;
                }

                return new GetBillRecipientCustomerTag
                {
                    Id = customerTag.Id,
                    TagId = customerTag.Tag_id,
                    Color = customerTag.Color,
                    Name = customerTag.Name,
                    Description = customerTag.Description
                };
            }
        }

        private class OrderMapper
        {
            public static List<GetBillOrder> Map(ICollection<Order.Client.GetOrderResponse> orders)
            {
                return orders
                    .OrderByDescending(x => x.Created_on.Date)
                    .Select(Map)
                    .ToList();
            }

            private static GetBillOrder Map(Order.Client.GetOrderResponse order)
            {
                return new GetBillOrder
                {
                    Id = order.Id,
                    Parcel = Map(order.Parcel),
                    Product = Map(order.Product),
                    Code = order.Code,
                    Amount = order.Amount,
                    UnitPrice = order.Unit_price,
                    Total = order.Total,
                    Paid = order.Paid,
                    Status = Map(order.Status),
                    CreatedOn = Map(order.Created_on)
                };
            }

            private static GetBillOrderParcel Map(Order.Client.GetOrderParcel parcel)
            {
                if (parcel == null || 
                    !parcel.Id.HasValue)
                {
                    return null;
                }

                return new GetBillOrderParcel
                {
                    Id = parcel.Id
                };
            }

            private static GetBillOrderProduct Map(Order.Client.GetOrderProduct product)
            {
                return new GetBillOrderProduct
                {
                    Id = product.Id,
                    Code = product.Code,
                    Name = product.Name,
                    Color = product.Color,
                    Size = product.Size
                };
            }

            private static GetBillOrderStatus Map(Order.Client.GetOrderStatus status)
            {
                if (status == null)
                {
                    return null;
                }

                return new GetBillOrderStatus
                {
                    Code = status.Code
                };
            }

            private static GetBillOrderDate Map(Order.Client.GetOrderDate orderDate)
            {
                return new GetBillOrderDate
                {
                    Date = orderDate.Date,
                    IsPresentDate = orderDate.Is_present_date,
                    IsPresentMonth = orderDate.Is_present_month,
                    IsPresentYear = orderDate.Is_present_year
                };
            }
        }

        private class PaymentMapper
        {
            public static GetPayment Map(ICollection<Payment> payments)
            {
                var latestPayment = PaymentHelper.GetLatest(payments);

                GetPayment payment = null;
                if (latestPayment != null)
                {
                    payment = new GetPayment
                    {
                        Status = new GetPaymentStatus
                        {
                            Code = latestPayment.PaymentStatus.Code
                        }
                    };
                }

                return payment;
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

        public class DiscountMapper
        {
            public static GetBillDiscount Map(BillDiscount discount)
            {
                if (discount == null || (!discount.Amount.HasValue && !discount.Percentage.HasValue))
                {
                    return null;
                }

                return new GetBillDiscount
                {
                    Amount = discount.Amount,
                    Percentage = discount.Percentage
                };
            }

            public static GetBillCostDiscount Map(decimal total, BillDiscount billDiscount)
            {
                var hasDiscount = billDiscount.Amount.HasValue || billDiscount.Percentage.HasValue;

                return new GetBillCostDiscount
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
            public static GetBillPayment Map(BillPayment billPayment)
            {
                if (billPayment == null)
                {
                    return null;
                }

                return new GetBillPayment
                {
                    Type = Map(billPayment.BillPaymentType),
                    HasCodAddOn = billPayment.HasCodAddOn,
                    CodAddOnAmount = billPayment.CodAddOnAmount,
                    CodAddOnPercentage = billPayment.CodAddOnPercentage,
                    HasVat = billPayment.HasVat,
                    IncludedVat = billPayment.IncludedVat,
                    VatPercentage = billPayment.VatPercentage
                };
            }

            private static GetBillPaymentType Map(BillPaymentType billPaymentType)
            {
                return new GetBillPaymentType
                {
                    Code = billPaymentType.Code
                };
            }
        }

        private class ShippingMapper
        {
            public static GetBillShipping Map(BillShipping shipping, ICollection<Parcel> parcels, List<Courier.Client.GetOrderResponse> courierOrders)
            {
                var costType = ShippingHelper.GetCostType(shipping);
                
                return new GetBillShipping
                {
                    CostType = costType.GetDescription(),
                    Parcels = ParcelMapper.Map(parcels, courierOrders)
                };
            }
        }

        private class ParcelMapper
        {
            public static List<GetBillParcel> Map(ICollection<Parcel> parcels, List<Courier.Client.GetOrderResponse> courierOrders)
            {
                return parcels
                    .Select(x => Map(x, courierOrders))
                    .ToList();
            }

            public static GetBillParcel Map(Parcel parcel, List<Courier.Client.GetOrderResponse> courierOrders)
            {
                var courierOrder = courierOrders
                    .FirstOrDefault(x => x.Ref_id == parcel.Id);

                return Map(parcel, courierOrder);
            }

            public static GetBillParcel Map(Parcel parcel, Courier.Client.GetOrderResponse courierOrder)
            {
                return new GetBillParcel
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

            private static GetBillParcelStatus Map(ParcelStatus parcelStatus)
            {
                return new GetBillParcelStatus
                {
                    Code = parcelStatus.Code
                };
            }

            private static GetBillParcelDate Map(DateTime dateTime)
            {
                return new GetBillParcelDate
                {
                    Date = dateTime,
                    IsPresentDate = DateTimeHelper.IsPresentDate(dateTime),
                    IsPresentMonth = DateTimeHelper.IsPresentMonth(dateTime),
                    IsPresentYear = DateTimeHelper.IsPresentYear(dateTime)
                };
            }
        }

        public class CostMapper
        {
            public static GetBillCost Map(ICollection<Order.Client.GetOrderResponse> orders, BillShipping billShipping, ICollection<Payment> payments, BillDiscount billDiscount, BillPayment billPayment)
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

                return new GetBillCost
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

            public class CodMapper
            {
                public static GetBillCostCod Map(decimal total, BillPayment billPayment)
                {
                    return new GetBillCostCod
                    {
                        HasAddOn = billPayment.HasCodAddOn,
                        Amount = billPayment.CodAddOnAmount,
                        Percentage = billPayment.CodAddOnPercentage,
                        Total = CodHelper.Calculate(total, billPayment)
                    };
                }
            }

            public class VatMapper
            {
                public static GetBillCostVat Map(decimal total, BillPayment billPayment)
                {
                    return new GetBillCostVat
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
            public static List<GetBillTag> Map(ICollection<BillTag> billTags)
            {
                return billTags
                    .Select(Map)
                    .ToList();
            }

            private static GetBillTag Map(BillTag billTag)
            {
                if (billTag == null || billTag.Tag == null)
                {
                    return null;
                }

                return new GetBillTag
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