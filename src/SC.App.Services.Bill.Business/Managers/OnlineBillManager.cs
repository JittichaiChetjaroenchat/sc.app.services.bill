using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SC.App.Services.Bill.Business.Commands.Bill;
using SC.App.Services.Bill.Business.Commands.BillConfiguration;
using SC.App.Services.Bill.Business.Enums;
using SC.App.Services.Bill.Business.Helpers;
using SC.App.Services.Bill.Business.Managers.Interface;
using SC.App.Services.Bill.Business.Queries.BillConfiguration;
using SC.App.Services.Bill.Business.Repositories.Interface;
using SC.App.Services.Bill.Business.Resources;
using SC.App.Services.Bill.Business.Validators.Bill;
using SC.App.Services.Bill.Client.Credit;
using SC.App.Services.Bill.Client.Customer;
using SC.App.Services.Bill.Client.Order;
using SC.App.Services.Bill.Client.Setting;
using SC.App.Services.Bill.Common.Helpers;
using SC.App.Services.Bill.Common.Managers;
using SC.App.Services.Bill.Common.Responses;
using SC.App.Services.Bill.Lib.Extensions;
using SC.App.Services.Bill.Lib.Helpers;
using Serilog;

namespace SC.App.Services.Bill.Business.Managers
{
    public class OnlineBillManager : BaseManager<IBillRepository>, IOnlineBillManager
    {
        private readonly IBillChannelRepository _billChannelRepository;
        private readonly IBillDiscountRepository _billDiscountRepository;
        private readonly IBillNotificationRepository _billNotificationRepository;
        private readonly IBillPaymentRepository _billPaymentRepository;
        private readonly IBillPaymentTypeRepository _billPaymentTypeRepository;
        private readonly IBillRecipientRepository _billRecipientRepository;
        private readonly IBillRecipientContactRepository _billRecipientContactRepository;
        private readonly IBillShippingRepository _billShippingRepository;
        private readonly IBillStatusRepository _billStatusRepository;

        private readonly IBillConfigurationManager _billConfigurationManager;
        private readonly ICreditManager _creditManager;
        private readonly ICustomerManager _customerManager;
        private readonly IOrderManager _orderManager;
        private readonly ISettingManager _settingManager;

        public OnlineBillManager(
            IBillRepository repository,
            IBillChannelRepository billChannelRepository,
            IBillNotificationRepository billNotificationRepository,
            IBillDiscountRepository billDiscountRepository,
            IBillPaymentRepository billPaymentRepository,
            IBillPaymentTypeRepository billPaymentTypeRepository,
            IBillRecipientRepository billRecipientRepository,
            IBillRecipientContactRepository billRecipientContactRepository,
            IBillShippingRepository billShippingRepository,
            IBillStatusRepository billStatusRepository,

            IBillConfigurationManager billConfigurationManager,
            ICreditManager creditManager,
            ICustomerManager customerManager,
            IOrderManager orderManager,
            ISettingManager settingManager)
            : base(repository)
        {
            _billChannelRepository = billChannelRepository;
            _billNotificationRepository = billNotificationRepository;
            _billDiscountRepository = billDiscountRepository;
            _billPaymentRepository = billPaymentRepository;
            _billPaymentTypeRepository = billPaymentTypeRepository;
            _billRecipientRepository = billRecipientRepository;
            _billRecipientContactRepository = billRecipientContactRepository;
            _billShippingRepository = billShippingRepository;
            _billStatusRepository = billStatusRepository;

            _billConfigurationManager = billConfigurationManager;
            _creditManager = creditManager;
            _customerManager = customerManager;
            _orderManager = orderManager;
            _settingManager = settingManager;
        }

        public async Task<Response<CreateBillResponse>> CreateAsync(IConfiguration configuration, CreateOnlineBillCommand command)
        {
            CreateBillResponse data = null;
            List<ResponseError> errors = new List<ResponseError>();
            Response<CreateBillResponse> response = null;

            try
            {
                // Validate
                await new CreateOnlineBillValidator().ValidateAndThrowAsync(command.Payload);

                // Create/update customer
                var customerId = await CreateCustomer(command.Request, command.Payload.ChannelId, command.Payload.Recipient.Name, command.Payload.UserId);

                // Get customer
                Customer.Client.GetCustomerResponse customer = null;
                var getCustomerByIdResponse = await _customerManager.GetCustomerByIdAsync(command.Request, customerId);
                if (!CustomerClientHelper.IsSuccess(getCustomerByIdResponse))
                {
                    var error = CustomerClientHelper.GetError(getCustomerByIdResponse.Errors);

                    errors.Add(new ResponseError { Code = error.Code, Message = error.Message });
                    response = ResponseHelper.Error<CreateBillResponse>(errors);

                    return await Task.FromResult(response);
                }
                else
                {
                    customer = getCustomerByIdResponse.Data;
                }

                // Check bill not exist or ended
                var bill = Repository.GetLatest(command.Payload.ChannelId, customer.Id, null, null);
                if (bill == null || BillHelper.IsEndState(bill))
                {
                    // Get bill's configuration
                    var billConfiguration = await GetBillConfigurationAsync(configuration, command.Request, command.Payload.ChannelId, command.Payload.UserId);

                    // Get billing
                    Setting.Client.GetBillingResponse billing = null;
                    var getBillingResponse = await _settingManager.GetBillingByChannelIdAsync(command.Request, command.Payload.ChannelId);
                    if (!SettingClientHelper.IsSuccess(getBillingResponse))
                    {
                        var error = SettingClientHelper.GetError(getBillingResponse.Errors);

                        errors.Add(new ResponseError { Code = error.Code, Message = error.Message });
                        response = ResponseHelper.Error<CreateBillResponse>(errors);

                        return await Task.FromResult(response);
                    }
                    else
                    {
                        billing = getBillingResponse.Data;
                    }

                    // Check credit's balance available
                    var checkBalanceAvailableResponse = await _creditManager.CheckCreditBalanceAvailableAsync(command.Request, command.Payload.ChannelId, billing.Billing_transaction_fee);
                    if (!CreditClientHelper.IsSuccess(checkBalanceAvailableResponse))
                    {
                        var error = CreditClientHelper.GetError(checkBalanceAvailableResponse.Errors);

                        errors.Add(new ResponseError { Code = error.Code, Message = error.Message });
                        response = ResponseHelper.Error<CreateBillResponse>(errors);

                        return await Task.FromResult(response);
                    }

                    if (!checkBalanceAvailableResponse.Data.Is_available)
                    {
                        errors.Add(new ResponseError { Code = EnumErrorCode._102080048.GetDescription(), Message = ErrorMessage._102080048 });
                        response = ResponseHelper.Error<CreateBillResponse>(errors);

                        return await Task.FromResult(response);
                    }

                    // Get shipping
                    Setting.Client.GetShippingResponse shipping = null;
                    var getShippingResponse = await _settingManager.GetShippingByChannelIdAsync(command.Request, command.Payload.ChannelId);
                    if (!SettingClientHelper.IsSuccess(getShippingResponse))
                    {
                        var error = SettingClientHelper.GetError(getShippingResponse.Errors);

                        errors.Add(new ResponseError { Code = error.Code, Message = error.Message });
                        response = ResponseHelper.Error<CreateBillResponse>(errors);

                        return await Task.FromResult(response);
                    }
                    else
                    {
                        shipping = getShippingResponse.Data;
                    }

                    // Create bill
                    bill = CreateBill(billConfiguration, command.Payload.ChannelId, command.Payload.BillChannel, customer.Is_new, command.Payload.UserId);

                    // Update bill's configuration
                    UpdateBillConfiguration(command.Payload.ChannelId);

                    // Create bill's discount
                    _ = CreateBillDiscount(bill.Id, null, null, command.Payload.UserId);

                    // Create bill's notification
                    _ = CreateBillNotification(bill.Id, command.Payload.UserId);

                    // Create bill's recipient
                    var billRecipient = CreateBillRecipient(bill.Id, customer.Id, command.Payload.Recipient.Name, command.Payload.Recipient.AliasName, command.Payload.UserId);

                    // Create bill recipient's contact
                    _ = CreateBillRecipientContact(
                        billRecipient.Id,
                        customer.Contact?.Address,
                        customer.Contact?.Area_id,
                        customer.Contact?.Primary_phone,
                        customer.Contact?.Secondary_phone,
                        null,
                        command.Payload.UserId);

                    // Create bill's payment
                    _ = CreateBillPayment(bill.Id, billing, command.Payload.UserId);

                    // Create bill's shipping
                    _ = CreateBillShipping(bill.Id, shipping, command.Payload.UserId);

                    // Update credit's balance
                    var updateCreditBalanceResponse = await _creditManager.UpdateCreditAsync(
                        command.Request,
                        bill.ChannelId,
                        Credit.Client.EnumCreditExpenseType.Billing,
                        -billing.Billing_transaction_fee, $"ค่าออกบิล (บิลเลขที่ {bill.BillNo})",
                        command.Payload.UserId);
                    if (!CreditClientHelper.IsSuccess(updateCreditBalanceResponse))
                    {
                        var error = CreditClientHelper.GetError(checkBalanceAvailableResponse.Errors);

                        errors.Add(new ResponseError { Code = error.Code, Message = error.Message });
                        response = ResponseHelper.Error<CreateBillResponse>(errors);

                        return await Task.FromResult(response);
                    }
                }

                // Refresh bill
                bill = Repository.GetById(bill.Id);

                // Update bill's status
                UpdateBillStatus(bill);

                // Create orders
                foreach (var order in command.Payload.Orders)
                {
                    // Create order
                    var createBillOrderResponse = await CreateBillOrderAsync(command.Request, command.Payload.ChannelId, command.Payload.LiveId, command.Payload.PostId, bill.Id, order, command.Payload.UserId);
                    if (!OrderClientHelper.IsSuccess(createBillOrderResponse))
                    {
                        var error = OrderClientHelper.GetError(createBillOrderResponse.Errors);

                        errors.Add(new ResponseError { Code = error.Code, Message = error.Message });
                        response = ResponseHelper.Error<CreateBillResponse>(errors);

                        return await Task.FromResult(response);
                    }
                }

                // Build response
                data = new CreateBillResponse(bill.Id);
                response = ResponseHelper.Ok(data);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);

                errors.Add(new ResponseError { Code = null, Message = ex.Message });
                response = ResponseHelper.Error<CreateBillResponse>(errors);
            }

            return await Task.FromResult(response);
        }

        #region Create

        private async Task<GetBillConfigurationResponse> GetBillConfigurationAsync(IConfiguration configuration, HttpRequest request, Guid channelId, Guid userId)
        {
            // Get bill's configuration
            var getBillConfigurationPayload = new GetBillConfigurationByFilter(channelId);
            var getBillConfigurationQuery = new GetBillConfigurationByFilterQuery(request, getBillConfigurationPayload);
            var getBillConfigurationResponse = await _billConfigurationManager.GetAsync(configuration, getBillConfigurationQuery);
            if (getBillConfigurationResponse.Errors.IsEmpty())
            {
                return await Task.FromResult(getBillConfigurationResponse.Data);
            }
            else
            {
                // Create bill's configuration
                var createBillConfigurationPayload = new CreateBillConfiguration
                {
                    ChannelId = channelId,
                    UserId = userId,
                };
                var createBillConfigurationCommand = new CreateBillConfigurationCommand(request, createBillConfigurationPayload);
                var createBillConfigurationResponse = await _billConfigurationManager.CreateAsync(configuration, createBillConfigurationCommand);
                if (!createBillConfigurationResponse.Errors.IsEmpty())
                {
                    throw new Exception(ErrorMessage._102080006);
                }
            }

            // Refresh bill's configuration
            getBillConfigurationResponse = await _billConfigurationManager.GetAsync(configuration, getBillConfigurationQuery);

            return await Task.FromResult(getBillConfigurationResponse.Data);
        }

        private async Task<Guid> CreateCustomer(HttpRequest request, Guid channelId, string name, Guid userId)
        {
            // Get customer
            Guid customerId = GuidHelper.Generate($"{channelId}_{name}");
            var getCustomerByIdResponse = await _customerManager.GetCustomerByIdAsync(request, customerId);
            if (!CustomerClientHelper.IsSuccess(getCustomerByIdResponse))
            {
                // Create new customer
                var createCustomerResponse = await _customerManager.CreateCustomerAsync(
                    request,
                    channelId,
                    name,
                    null,
                    null,
                    null,
                    null,
                    null,
                    userId);
                if (CustomerClientHelper.IsSuccess(createCustomerResponse))
                {
                    customerId = createCustomerResponse.Data.Id;
                }
            }
            else
            {
                customerId = getCustomerByIdResponse.Data.Id;
            }

            return await Task.FromResult(customerId);
        }

        private Database.Models.Bill CreateBill(GetBillConfigurationResponse billConfiguration, Guid channelId, EnumBillChannel billChannel, bool isNewCustomer, Guid userId)
        {
            var channel = _billChannelRepository.GetByCode(billChannel.GetDescription());
            var status = _billStatusRepository.GetByCode(EnumBillStatus.Pending.GetDescription());
            var billNo = BillHelper.GetNextBillNo(billConfiguration.CurrentNo);
            var runningNo = BillHelper.GetNextRunningNo(billConfiguration.CurrentNo);
            var key = BillHelper.GetKey(channelId + billNo);
            var bill = new Database.Models.Bill
            {
                ChannelId = channelId,
                BillNo = billNo,
                RunningNo = runningNo,
                BillChannelId = channel.Id,
                BillStatusId = status.Id,
                IsDeposit = false,
                IsNewCustomer = isNewCustomer,
                Remark = null,
                Key = key,
                CreatedBy = userId,
                UpdatedBy = userId
            };
            Repository.Add(bill);

            return bill;
        }

        private void UpdateBillConfiguration(Guid channelId)
        {
            _billConfigurationManager.IncreaseCurrentNo(channelId);
        }

        private Guid CreateBillNotification(Guid billId, Guid userId)
        {
            var billNotifiation = new Database.Models.BillNotification
            {
                BillId = billId,

                IsIssueNotified = false,
                IssueNotifiedOn = null,
                CanNotifyIssue = null,

                IsBeforeCancelNotified = false,
                BeforeCancelNotifiedOn = null,
                CanNotifyBeforeCancel = null,

                IsCancelNotified = false,
                CancelNotifiedOn = null,
                CanNotifyCancel = null,

                IsSummaryNotified = false,
                SummaryNotifiedOn = null,
                CanNotifySummary = null,

                CreatedBy = userId,
                UpdatedBy = userId
            };

            return _billNotificationRepository.Add(billNotifiation);
        }

        private Database.Models.BillRecipient CreateBillRecipient(Guid billId, Guid customerId, string customerName, string customerAliasName, Guid userId)
        {
            // Create bill recipient
            var billRecipient = new Database.Models.BillRecipient
            {
                BillId = billId,
                CustomerId = customerId,
                Name = customerName,
                AliasName = customerAliasName,
                CreatedBy = userId,
                UpdatedBy = userId
            };
            _billRecipientRepository.Add(billRecipient);

            return billRecipient;
        }

        private Database.Models.BillRecipientContact CreateBillRecipientContact(Guid billRecipientId, string address, Guid? areaId, string primaryPhone, string secondaryPhone, string email, Guid userId)
        {
            if (!address.IsEmpty())
            {
                var billRecipientContact = new Database.Models.BillRecipientContact
                {
                    BillRecipientId = billRecipientId,
                    Address = address,
                    AreaId = areaId,
                    PrimaryPhone = primaryPhone,
                    SecondaryPhone = secondaryPhone,
                    Email = email,
                    CreatedBy = userId,
                    UpdatedBy = userId
                };
                _billRecipientContactRepository.Add(billRecipientContact);

                return billRecipientContact;
            }

            return null;
        }

        private Database.Models.BillDiscount CreateBillDiscount(Guid billId, decimal? amount, decimal? percentage, Guid userId)
        {
            var billDiscount = new Database.Models.BillDiscount
            {
                BillId = billId,
                Amount = amount,
                Percentage = percentage,
                CreatedBy = userId,
                UpdatedBy = userId
            };
            _billDiscountRepository.Add(billDiscount);

            return billDiscount;
        }

        private Database.Models.BillPayment CreateBillPayment(Guid billId, Setting.Client.GetBillingResponse billing, Guid userId)
        {
            var enumBillPaymentType = EnumBillPaymentType.Unknown;
            if (billing.Can_transfer && billing.Can_cod)
            {
                enumBillPaymentType = EnumBillPaymentType.PrePaid;
            }
            else if (billing.Can_transfer && !billing.Can_cod)
            {
                enumBillPaymentType = EnumBillPaymentType.PrePaid;
            }
            else if (!billing.Can_transfer && billing.Can_cod)
            {
                enumBillPaymentType = EnumBillPaymentType.PostPaid;
            }
            else
            {
                enumBillPaymentType = EnumBillPaymentType.PrePaid;
            }

            var billPaymentType = _billPaymentTypeRepository.GetByCode(enumBillPaymentType.GetDescription());
            var billPayment = new Database.Models.BillPayment
            {
                BillId = billId,
                BillPaymentTypeId = billPaymentType.Id,
                HasCodAddOn = enumBillPaymentType == EnumBillPaymentType.PostPaid ? billing.Has_cod_addon : false,
                CodAddOnAmount = billing.Cod_addon_amount,
                CodAddOnPercentage = billing.Cod_addon_percentage,
                HasVat = billing.Has_vat,
                IncludedVat = billing.Included_vat,
                VatPercentage = billing.Vat_percentage,
                CreatedBy = userId,
                UpdatedBy = userId
            };
            _billPaymentRepository.Add(billPayment);

            return billPayment;
        }

        private Database.Models.BillShipping CreateBillShipping(Guid billId, Setting.Client.GetShippingResponse shipping, Guid userId)
        {
            var shippingRangeRuleEnabled = shipping.Range_rule.Enabled;
            var shippingTotalRuleEnabled = shipping.Total_rule.Enabled;
            var shippingFreeRuleEnabled = shipping.Free_rule.Enabled;

            // Prepare bill's shipping range rule
            var billShippingRanges = new List<Database.Models.BillShippingRange>();
            foreach (var range in shipping.Range_rule.Ranges)
            {
                var billShippingRange = new Database.Models.BillShippingRange
                {
                    Cost = range.Cost,
                    Begin = range.Begin,
                    End = range.End,
                };
                billShippingRanges.Add(billShippingRange);
            }

            var billShippingRangeRule = new Database.Models.BillShippingRangeRule
            {
                BillShippingRanges = billShippingRanges,
                Enabled = shippingRangeRuleEnabled
            };

            // Prepare bill's shipping total rule
            var billShippingTotalRule = new Database.Models.BillShippingTotalRule
            {
                Cost = shipping.Total_rule.Cost,
                Enabled = shippingTotalRuleEnabled,
            };

            // Prepare bill's shipping free rule
            var billShippingFreeRule = new Database.Models.BillShippingFreeRule
            {
                Price = shipping.Free_rule.Cost,
                Amount = shipping.Free_rule.Amount,
                Enabled = shippingFreeRuleEnabled
            };

            // Create bill's shipping
            var billShippingId = GuidHelper.Generate(billId.ToString());
            var billShipping = new Database.Models.BillShipping
            {
                Id = billShippingId,
                BillId = billId,
                BillShippingRangeRule = billShippingRangeRule,
                BillShippingTotalRule = billShippingTotalRule,
                BillShippingFreeRule = billShippingFreeRule,
                CreatedBy = userId,
                UpdatedBy = userId,
            };
            _ = _billShippingRepository.Add(billShipping);

            return billShipping;
        }

        private async Task<Order.Client.ResponseOfCreateOrderResponse> CreateBillOrderAsync(HttpRequest request, Guid channelId, Guid? liveId, Guid? postId, Guid billId, CreateOnlineBillOrder order, Guid userId)
        {
            var item = new Order.Client.CreateOrdersItem
            {
                Booking_id = order.BookingId,
                Product_id = order.ProductId,
                Code = order.Code,
                Amount = order.Amount,
                Unit_price = order.UnitPrice
            };

            return await _orderManager.CreateOrderAsync(request, channelId, liveId, postId, billId, item, userId);
        }

        private void UpdateBillStatus(Database.Models.Bill bill)
        {
            var billStatuses = new string[] { EnumBillStatus.Confirmed.GetDescription(), EnumBillStatus.Rejected.GetDescription() };
            if (billStatuses.Contains(bill.BillStatus.Code))
            {
                var paymentStatuses = new string[] { EnumPaymentStatus.Accepted.GetDescription(), EnumPaymentStatus.Rejected.GetDescription() };
                var latestPayment = PaymentHelper.GetLatest(bill.Payments);
                if (paymentStatuses.Contains(latestPayment.PaymentStatus.Code))
                {
                    // Update bill's status to pending
                    var billStatus = _billStatusRepository.GetByCode(EnumBillStatus.Pending.GetDescription());

                    bill.BillStatusId = billStatus.Id;
                    Repository.Update(bill);
                }
            }
        }

        #endregion
    }
}