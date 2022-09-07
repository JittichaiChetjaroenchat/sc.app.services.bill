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
    public class OfflineBillManager : BaseManager<IBillRepository>, IOfflineBillManager
    {
        private readonly IBillChannelRepository _billChannelRepository;
        private readonly IBillDiscountRepository _billDiscountRepository;
        private readonly IBillPaymentRepository _billPaymentRepository;
        private readonly IBillPaymentTypeRepository _billPaymentTypeRepository;
        private readonly IBillRecipientRepository _billRecipientRepository;
        private readonly IBillRecipientContactRepository _billRecipientContactRepository;
        private readonly IBillShippingRepository _billShippingRepository;
        private readonly IBillShippingRangeRuleRepository _billShippingRangeRuleRepository;
        private readonly IBillShippingRangeRepository _billShippingRangeRepository;
        private readonly IBillShippingTotalRuleRepository _billShippingTotalRuleRepository;
        private readonly IBillShippingFreeRuleRepository _billShippingFreeRuleRepository;
        private readonly IBillStatusRepository _billStatusRepository;

        private readonly IBillConfigurationManager _billConfigurationManager;
        private readonly ICreditManager _creditManager;
        private readonly ICustomerManager _customerManager;
        private readonly IOrderManager _orderManager;
        private readonly ISettingManager _settingManager;

        public OfflineBillManager(
            IBillRepository repository,
            IBillChannelRepository billChannelRepository,
            IBillDiscountRepository billDiscountRepository,
            IBillPaymentRepository billPaymentRepository,
            IBillPaymentTypeRepository billPaymentTypeRepository,
            IBillRecipientRepository billRecipientRepository,
            IBillRecipientContactRepository billRecipientContactRepository,
            IBillShippingRepository billShippingRepository,
            IBillShippingRangeRuleRepository billShippingRangeRuleRepository,
            IBillShippingRangeRepository billShippingRangeRepository,
            IBillShippingTotalRuleRepository billShippingTotalRuleRepository,
            IBillShippingFreeRuleRepository billShippingFreeRuleRepository,
            IBillStatusRepository billStatusRepository,

            IBillConfigurationManager billConfigurationManager,
            ICreditManager creditManager,
            ICustomerManager customerManager,
            IOrderManager orderManager,
            ISettingManager settingManager)
            : base(repository)
        {
            _billChannelRepository = billChannelRepository;
            _billDiscountRepository = billDiscountRepository;
            _billPaymentRepository = billPaymentRepository;
            _billPaymentTypeRepository = billPaymentTypeRepository;
            _billRecipientRepository = billRecipientRepository;
            _billRecipientContactRepository = billRecipientContactRepository;
            _billShippingRepository = billShippingRepository;
            _billShippingRangeRuleRepository = billShippingRangeRuleRepository;
            _billShippingRangeRepository = billShippingRangeRepository;
            _billShippingTotalRuleRepository = billShippingTotalRuleRepository;
            _billShippingFreeRuleRepository = billShippingFreeRuleRepository;
            _billStatusRepository = billStatusRepository;

            _billConfigurationManager = billConfigurationManager;
            _creditManager = creditManager;
            _customerManager = customerManager;
            _orderManager = orderManager;
            _settingManager = settingManager;
        }

        public async Task<Response<CreateBillResponse>> CreateAsync(IConfiguration configuration, CreateOfflineBillCommand command)
        {
            CreateBillResponse data = null;
            List<ResponseError> errors = new List<ResponseError>();
            Response<CreateBillResponse> response = null;

            try
            {
                // Validate
                await new CreateOfflineBillValidator().ValidateAndThrowAsync(command.Payload);

                // Create/update customer
                var customerId = await CreateOrUpdateCustomerAsync(command.Request, command.Payload.ChannelId, command.Payload.Recipient, command.Payload.UserId);

                // Check bill exist
                var bill = Repository.GetLatest(command.Payload.ChannelId, customerId, null, null);
                if (bill != null)
                {
                    // Check bill still pending
                    var isEndState = BillHelper.IsEndState(bill);
                    if (!isEndState)
                    {
                        errors.Add(new ResponseError { Code = EnumErrorCode._102080028.GetDescription(), Message = ErrorMessage._102080028 });
                        response = ResponseHelper.Error<CreateBillResponse>(errors);

                        return await Task.FromResult(response);
                    }
                }

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
                var isNewCustomer = command.Payload.Recipient.Id.HasValue ? false : true;
                bill = CreateBill(billConfiguration, command.Payload.ChannelId, isNewCustomer, command.Payload.Remark, command.Payload.UserId);

                // Update bill's configuration
                UpdateBillConfiguration(command.Payload.ChannelId);

                // Create bill's recipient
                var billRecipient = CreateBillRecipient(bill.Id, customerId, command.Payload.Recipient.Name, command.Payload.Recipient.AliasName, command.Payload.UserId);

                // Create bill recipient's contact
                var billRecipientContact = CreateBillRecipientContact(
                    billRecipient.Id,
                    command.Payload.Recipient.Contact?.Address,
                    command.Payload.Recipient.Contact?.AreaId,
                    command.Payload.Recipient.Contact?.PrimaryPhone,
                    command.Payload.Recipient.Contact?.SecondaryPhone,
                    null,
                    command.Payload.UserId);

                // Create bill's discount
                var billDiscount = CreateBillDiscount(bill.Id, command.Payload.Discount, command.Payload.UserId);

                // Create bill's payment
                var billPayment = CreateBillPayment(bill.Id, command.Payload.Payment, billing, command.Payload.UserId);

                // Create bill's shipping
                var billShipping = CreateBillShipping(bill.Id, command.Payload.Shipping, shipping, command.Payload.UserId);

                // Create order
                await CreateBillOrderAsync(command.Request, command.Payload.ChannelId, bill.Id, command.Payload.Orders, command.Payload.UserId);

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

        public async Task<Response<UpdateBillResponse>> UpdateAsync(IConfiguration configuration, UpdateOfflineBillCommand command)
        {
            UpdateBillResponse data = null;
            List<ResponseError> errors = new List<ResponseError>();
            Response<UpdateBillResponse> response = null;

            try
            {
                // Validate
                await new UpdateOfflineBillValidator().ValidateAndThrowAsync(command.Payload);

                // Check bill exist
                var bill = Repository.GetById(command.Payload.BillId);
                if (bill == null)
                {
                    errors.Add(new ResponseError { Code = EnumErrorCode._102080001.GetDescription(), Message = ErrorMessage._102080001 });
                    response = ResponseHelper.Error<UpdateBillResponse>(errors);

                    return await Task.FromResult(response);
                }

                // Create/update customer
                var customerId = await CreateOrUpdateCustomerAsync(command.Request, command.Payload.ChannelId, command.Payload.Recipient, command.Payload.UserId);

                // Get billing
                Setting.Client.GetBillingResponse billing = null;
                var getBillingResponse = await _settingManager.GetBillingByChannelIdAsync(command.Request, bill.ChannelId);
                if (!SettingClientHelper.IsSuccess(getBillingResponse))
                {
                    var error = SettingClientHelper.GetError(getBillingResponse.Errors);

                    errors.Add(new ResponseError { Code = error.Code, Message = error.Message });
                    response = ResponseHelper.Error<UpdateBillResponse>(errors);

                    return await Task.FromResult(response);
                }
                else
                {
                    billing = getBillingResponse.Data;
                }

                // Get shipping
                Setting.Client.GetShippingResponse shipping = null;
                var getShippingResponse = await _settingManager.GetShippingByChannelIdAsync(command.Request, bill.ChannelId);
                if (!SettingClientHelper.IsSuccess(getShippingResponse))
                {
                    var error = SettingClientHelper.GetError(getShippingResponse.Errors);

                    errors.Add(new ResponseError { Code = error.Code, Message = error.Message });
                    response = ResponseHelper.Error<UpdateBillResponse>(errors);

                    return await Task.FromResult(response);
                }
                else
                {
                    shipping = getShippingResponse.Data;
                }

                // Update bill
                bill = UpdateBill(bill, command.Payload.Remark, command.Payload.UserId);

                // Update bill's recipient
                var billRecipient = UpdateBillRecipient(bill.Id, command.Payload.Recipient, command.Payload.UserId);

                // Update bill recipient's contact
                var billRecipientContact = UpdateBillRecipientContact(billRecipient.Id, command.Payload.Recipient.Contact, command.Payload.UserId);

                // Update bill's discount
                var billDiscount = UpdateBillDiscount(bill.Id, command.Payload.Discount, command.Payload.UserId);

                // Update bill's payment
                var billPayment = UpdateBillPayment(bill.Id, command.Payload.Payment, billing, command.Payload.UserId);

                // Update bill's shipping
                var billShipping = UpdateBillShipping(bill.Id, command.Payload.Shipping, shipping, command.Payload.UserId);

                // Update order
                var updateBillOrderResponse = await UpdateBillOrderAsync(command.Request, bill.ChannelId, bill.Id, command.Payload.Orders, command.Payload.UserId);
                if (!OrderClientHelper.IsSuccess(updateBillOrderResponse))
                {
                    var error = OrderClientHelper.GetError(updateBillOrderResponse.Errors);

                    errors.Add(new ResponseError { Code = error.Code, Message = error.Message });
                    response = ResponseHelper.Error<UpdateBillResponse>(errors);

                    return await Task.FromResult(response);
                }

                // Build response
                data = new UpdateBillResponse();
                response = ResponseHelper.Ok(data);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);

                errors.Add(new ResponseError { Code = null, Message = ex.Message });
                response = ResponseHelper.Error<UpdateBillResponse>(errors);
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

        private async Task<Guid> CreateOrUpdateCustomerAsync(HttpRequest request, Guid channelId, CreateOfflineBillRecipient customer, Guid userId)
        {
            // Create/Update customer
            Guid customerId = Guid.Empty;
            if (!customer.Id.HasValue)
            {
                // Create new customer
                var createCustomerResponse = await _customerManager.CreateCustomerAsync(
                    request,
                    channelId,
                    customer.Name,
                    customer.Contact?.Address,
                    customer.Contact?.AreaId,
                    customer.Contact?.PrimaryPhone,
                    customer.Contact?.SecondaryPhone,
                    customer.Contact?.Email,
                    userId);
                if (CustomerClientHelper.IsSuccess(createCustomerResponse))
                {
                    customerId = createCustomerResponse.Data.Id;
                }
            }
            else
            {
                // Update customer
                var updateCustomerContactResponse = await _customerManager.UpdateCustomerContactAsync(
                    request,
                    customer.Id.Value,
                    customer.Contact?.Address,
                    customer.Contact?.AreaId,
                    customer.Contact?.PrimaryPhone,
                    customer.Contact?.SecondaryPhone,
                    customer.Contact?.Email);
                if (CustomerClientHelper.IsSuccess(updateCustomerContactResponse))
                {
                    customerId = customer.Id.Value;
                }
            }

            return await Task.FromResult(customerId);
        }

        private async Task<Guid> CreateOrUpdateCustomerAsync(HttpRequest request, Guid channelId, UpdateOfflineBillRecipient customer, Guid userId)
        {
            // Create/Update customer
            Guid customerId = Guid.Empty;
            if (!customer.Id.HasValue)
            {
                // Create new customer
                var createCustomerResponse = await _customerManager.CreateCustomerAsync(
                    request,
                    channelId,
                    customer.Name,
                    customer.Contact?.Address,
                    customer.Contact?.AreaId,
                    customer.Contact?.PrimaryPhone,
                    customer.Contact?.SecondaryPhone,
                    customer.Contact?.Email,
                    userId);
                if (CustomerClientHelper.IsSuccess(createCustomerResponse))
                {
                    customerId = createCustomerResponse.Data.Id;
                }
            }
            else
            {
                // Update customer
                var updateCustomerContactResponse = await _customerManager.UpdateCustomerContactAsync(
                    request,
                    customer.Id.Value,
                    customer.Contact?.Address,
                    customer.Contact?.AreaId,
                    customer.Contact?.PrimaryPhone,
                    customer.Contact?.SecondaryPhone,
                    customer.Contact?.Email);
                if (CustomerClientHelper.IsSuccess(updateCustomerContactResponse))
                {
                    customerId = customer.Id.Value;
                }
            }

            return await Task.FromResult(customerId);
        }

        private Database.Models.Bill CreateBill(GetBillConfigurationResponse billConfiguration, Guid channelId, bool isNewCustomer, string remark, Guid userId)
        {
            var channel = _billChannelRepository.GetByCode(EnumBillChannel.Offline.GetDescription());
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
                Remark = remark,
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

        private Database.Models.BillDiscount CreateBillDiscount(Guid billId, CreateOfflineBillDiscount data, Guid userId)
        {
            var billDiscount = new Database.Models.BillDiscount
            {
                BillId = billId,
                Amount = data.Amount,
                Percentage = data.Percentage,
                CreatedBy = userId,
                UpdatedBy = userId
            };
            _billDiscountRepository.Add(billDiscount);

            return billDiscount;
        }

        private Database.Models.BillPayment CreateBillPayment(Guid billId, CreateOfflineBillPayment data, Setting.Client.GetBillingResponse billing, Guid userId)
        {
            var enumBillPaymentType = BillPaymentTypeHelper.GetByCode(data.Type);
            var billPaymentType = _billPaymentTypeRepository.GetByCode(enumBillPaymentType.GetDescription());
            var billPayment = new Database.Models.BillPayment
            {
                BillId = billId,
                BillPaymentTypeId = billPaymentType.Id,
                HasCodAddOn = data.HasCodAddOn,
                CodAddOnAmount = data.CodAddOnAmount,
                CodAddOnPercentage = data.CodAddOnPercentage,
                HasVat = data.HasVat,
                IncludedVat = data.IncludedVat,
                VatPercentage = billing.Vat_percentage,
                CreatedBy = userId,
                UpdatedBy = userId
            };
            _billPaymentRepository.Add(billPayment);

            return billPayment;
        }

        private Database.Models.BillShipping CreateBillShipping(Guid billId, CreateOfflineBillShipping data, Setting.Client.GetShippingResponse shipping, Guid userId)
        {
            var shippingRangeRuleEnabled = false;
            var shippingTotalRuleEnabled = false;
            var shippingFreeRuleEnabled = false;
            switch (data.CostType)
            {
                case EnumShippingCostType.Range:
                    shippingRangeRuleEnabled = true;
                    break;
                case EnumShippingCostType.Total:
                    shippingTotalRuleEnabled = true;
                    break;
                case EnumShippingCostType.Free:
                    shippingFreeRuleEnabled = true;
                    break;
            }

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

        private async Task CreateBillOrderAsync(HttpRequest request, Guid channelId, Guid billId, List<CreateOfflineBillOrder> orders, Guid userId)
        {
            foreach (var order in orders)
            {
                var item = new Order.Client.CreateOrdersItem
                {
                    Booking_id = null,
                    Product_id = order.ProductId,
                    Code = order.Code,
                    Amount = order.Amount,
                    Unit_price = order.UnitPrice
                };

                // Create order
                _ = await _orderManager.CreateOrderAsync(request, channelId, null, null, billId, item, userId);
            }
        }

        #endregion

        #region Update

        private Database.Models.Bill UpdateBill(Database.Models.Bill bill, string remark, Guid userId)
        {
            bill.Remark = remark;
            bill.UpdatedBy = userId;
            bill.UpdatedOn = DateTime.Now;

            Repository.Update(bill);

            return bill;
        }

        private Database.Models.BillRecipient UpdateBillRecipient(Guid billId, UpdateOfflineBillRecipient customer, Guid userId)
        {
            var billRecipient = _billRecipientRepository.GetByBillId(billId);
            billRecipient.Name = customer.Name;
            billRecipient.AliasName = customer.Name;
            billRecipient.UpdatedBy = userId;
            billRecipient.UpdatedOn = DateTime.Now;

            _billRecipientRepository.Update(billRecipient);

            return billRecipient;
        }

        private Database.Models.BillRecipientContact UpdateBillRecipientContact(Guid billRecipientId, UpdateOfflineBillRecipientContact contact, Guid userId)
        {
            var billRecipientContact = _billRecipientContactRepository.GetByBillRecipientId(billRecipientId);
            if (billRecipientContact == null)
            {
                if (contact != null && !contact.Address.IsEmpty())
                {
                    // Create new contact
                    billRecipientContact = new Database.Models.BillRecipientContact
                    {
                        BillRecipientId = billRecipientId,
                        Address = contact?.Address,
                        AreaId = contact?.AreaId,
                        PrimaryPhone = contact?.PrimaryPhone,
                        SecondaryPhone = contact?.SecondaryPhone,
                        Email = contact?.Email,
                        CreatedBy = userId,
                        UpdatedBy = userId
                    };
                    _billRecipientContactRepository.Add(billRecipientContact);

                    return billRecipientContact;
                }
            }
            else
            {
                if (contact != null && !contact.Address.IsEmpty())
                {
                    // Update contact
                    billRecipientContact.Address = contact?.Address;
                    billRecipientContact.AreaId = contact?.AreaId;
                    billRecipientContact.PrimaryPhone = contact?.PrimaryPhone;
                    billRecipientContact.SecondaryPhone = contact?.SecondaryPhone;
                    billRecipientContact.Email = contact?.Email;
                    billRecipientContact.UpdatedBy = userId;
                    billRecipientContact.UpdatedOn = DateTime.Now;

                    _billRecipientContactRepository.Update(billRecipientContact);
                }
            }

            return billRecipientContact;
        }

        private Database.Models.BillDiscount UpdateBillDiscount(Guid billId, UpdateOfflineBillDiscount discount, Guid userId)
        {
            var billDiscount = _billDiscountRepository.GetByBillId(billId);

            billDiscount.Amount = discount?.Amount;
            billDiscount.Percentage = discount?.Percentage;
            billDiscount.UpdatedBy = userId;
            billDiscount.UpdatedOn = DateTime.Now;

            _billDiscountRepository.Update(billDiscount);

            return billDiscount;
        }

        private Database.Models.BillPayment UpdateBillPayment(Guid billId, UpdateOfflineBillPayment payment, Setting.Client.GetBillingResponse billing, Guid userId)
        {
            var enumBillPaymentType = BillPaymentTypeHelper.GetByCode(payment.Type);
            var billPaymentType = _billPaymentTypeRepository.GetByCode(enumBillPaymentType.GetDescription());
            var billPayment = _billPaymentRepository.GetByBillId(billId);

            billPayment.BillPaymentTypeId = billPaymentType.Id;
            billPayment.HasCodAddOn = payment.HasCodAddOn;
            billPayment.CodAddOnAmount = payment.CodAddOnAmount;
            billPayment.CodAddOnPercentage = payment.CodAddOnPercentage;
            billPayment.HasVat = payment.HasVat;
            billPayment.IncludedVat = payment.IncludedVat;
            billPayment.VatPercentage = billing.Vat_percentage;
            billPayment.UpdatedBy = userId;

            _billPaymentRepository.Update(billPayment);

            return billPayment;
        }

        private Database.Models.BillShipping UpdateBillShipping(Guid billId, UpdateOfflineBillShipping shipping, Setting.Client.GetShippingResponse shippingSetting, Guid userId)
        {
            var billShipping = _billShippingRepository.GetByBillId(billId);

            // Update range rule
            var billShippingRangeRule = _billShippingRangeRuleRepository.GetByBillShippingId(billShipping.Id);
            billShippingRangeRule.Enabled = shippingSetting.Range_rule.Enabled;

            _billShippingRangeRuleRepository.Update(billShippingRangeRule);

            // Update range rule (ranges)
            // Remove
            var billShippingRanges = _billShippingRangeRepository.GetByBillShippingRangeRuleId(billShippingRangeRule.Id);

            _billShippingRangeRepository.Removes(billShippingRanges);

            // Add
            var shippingSettingRangeRuleRanges = shippingSetting.Range_rule.Ranges
                .OrderBy(x => x.Begin)
                .ToArray();
            var newBillShippingRanges = new List<Database.Models.BillShippingRange>();
            foreach (var shippingSettingRangeRuleRange in shippingSettingRangeRuleRanges)
            {
                var billShippingRange = new Database.Models.BillShippingRange
                {
                    BillShippingRangeRuleId = billShippingRangeRule.Id,
                    Begin = shippingSettingRangeRuleRange.Begin,
                    End = shippingSettingRangeRuleRange.End,
                    Cost = shippingSettingRangeRuleRange.Cost
                };

                newBillShippingRanges.Add(billShippingRange);
            }

            _billShippingRangeRepository.Adds(newBillShippingRanges);

            // Update total rule
            var billShippingTotalRule = _billShippingTotalRuleRepository.GetByBillShippingId(billShipping.Id);
            billShippingTotalRule.Cost = shippingSetting.Total_rule.Cost;
            billShippingTotalRule.Enabled = shippingSetting.Total_rule.Enabled;

            _billShippingTotalRuleRepository.Update(billShippingTotalRule);

            // Update free rule
            var billShippingFreeRule = _billShippingFreeRuleRepository.GetByBillShippingId(billShipping.Id);
            billShippingFreeRule.Price = shippingSetting.Free_rule.Cost;
            billShippingFreeRule.Amount = shippingSetting.Free_rule.Amount;
            billShippingFreeRule.Enabled = shippingSetting.Free_rule.Enabled;

            _billShippingFreeRuleRepository.Update(billShippingFreeRule);

            // Update bill's shipping
            var shippingRangeRuleEnabled = false;
            var shippingTotalRuleEnabled = false;
            var shippingFreeRuleEnabled = false;
            switch (shipping.CostType)
            {
                case EnumShippingCostType.Range:
                    shippingRangeRuleEnabled = true;
                    break;
                case EnumShippingCostType.Total:
                    shippingTotalRuleEnabled = true;
                    break;
                case EnumShippingCostType.Free:
                    shippingFreeRuleEnabled = true;
                    break;
            }

            billShipping.BillShippingRangeRule.Enabled = shippingRangeRuleEnabled;
            billShipping.BillShippingTotalRule.Enabled = shippingTotalRuleEnabled;
            billShipping.BillShippingFreeRule.Enabled = shippingFreeRuleEnabled;
            billShipping.UpdatedBy = userId;
            billShipping.UpdatedOn = DateTime.Now;

            _billShippingRepository.Update(billShipping);

            return billShipping;
        }

        private async Task<Order.Client.ResponseOfUpdateOrdersResponse> UpdateBillOrderAsync(HttpRequest request, Guid channelId, Guid billId, List<UpdateOfflineBillOrder> orders, Guid userId)
        {
            var items = new List<Order.Client.UpdateOrdersItem>();
            foreach (var order in orders)
            {
                var item = new Order.Client.UpdateOrdersItem
                {
                    Product_id = order.ProductId,
                    Code = order.Code,
                    Amount = order.Amount,
                    Unit_price = order.UnitPrice
                };

                items.Add(item);
            }

            return await _orderManager.UpdateOrderAsync(request, channelId, billId, items, userId);
        }

        #endregion
    }
}