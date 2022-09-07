using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SC.App.Services.Bill.Business.Commands.Bill;
using SC.App.Services.Bill.Business.Commands.Parcel;
using SC.App.Services.Bill.Business.Enums;
using SC.App.Services.Bill.Business.Helpers;
using SC.App.Services.Bill.Business.Managers.Interface;
using SC.App.Services.Bill.Business.Mappers;
using SC.App.Services.Bill.Business.Queries.Parcel;
using SC.App.Services.Bill.Business.Repositories.Interface;
using SC.App.Services.Bill.Business.Resources;
using SC.App.Services.Bill.Client.Area;
using SC.App.Services.Bill.Client.Courier;
using SC.App.Services.Bill.Client.Notification;
using SC.App.Services.Bill.Client.Order;
using SC.App.Services.Bill.Client.Setting;
using SC.App.Services.Bill.Common.Helpers;
using SC.App.Services.Bill.Common.Managers;
using SC.App.Services.Bill.Common.Responses;
using SC.App.Services.Bill.Lib.Extensions;
using SC.App.Services.Bill.Queue.Managers.Interface;
using Serilog;

namespace SC.App.Services.Bill.Business.Managers
{
    public class ParcelManager : BaseManager<IParcelRepository>, IParcelManager
    {
        private readonly IBillRepository _billRepository;
        private readonly IBillStatusRepository _billStatusRepository;
        private readonly IBillShippingRepository _billShippingRepository;
        private readonly IParcelStatusRepository _parcelStatusRepository;
        private readonly INotificationManager _notificationManager;
        private readonly IOrderManager _orderManager;
        private readonly ISettingManager _settingManager;
        private readonly ICourierManager _courierManager;
        private readonly IAreaManager _areaManager;
        private readonly IQueueManager _queueManager;

        public ParcelManager(
            IParcelRepository repository,
            IBillRepository billRepository,
            IBillStatusRepository billStatusRepository,
            IBillShippingRepository billShippingRepository,
            IParcelStatusRepository parcelStatusRepository,
            INotificationManager notificationManager,
            IOrderManager orderManager,
            ISettingManager settingManager,
            ICourierManager courierManager,
            IAreaManager areaManager,
            IQueueManager queueManager)
            : base(repository)
        {
            _billRepository = billRepository;
            _billStatusRepository = billStatusRepository;
            _billShippingRepository = billShippingRepository;
            _parcelStatusRepository = parcelStatusRepository;
            _notificationManager = notificationManager;
            _orderManager = orderManager;
            _settingManager = settingManager;
            _courierManager = courierManager;
            _areaManager = areaManager;
            _queueManager = queueManager;
        }

        public async Task<Response<GetParcelResponse>> GetAsync(IConfiguration configuration, GetParcelByIdQuery query)
        {
            GetParcelResponse data = null;
            List<ResponseError> errors = new List<ResponseError>();
            Response<GetParcelResponse> response = null;

            try
            {
                // Get parcel
                var parcel = Repository.GetById(query.Payload.Id);
                if (parcel == null)
                {
                    errors.Add(new ResponseError { Code = EnumErrorCode._102080029.GetDescription(), Message = ErrorMessage._102080029 });
                    response = ResponseHelper.Error<GetParcelResponse>(errors);

                    return await Task.FromResult(response);
                }

                // Get courier's order
                var getCourierOrderResponse = await _courierManager.GetOrderByFilterAsync(query.Request, parcel.Id);
                if (!CourierClientHelper.IsSuccess(getCourierOrderResponse))
                {
                    var error = CourierClientHelper.GetError(getCourierOrderResponse.Errors);

                    errors.Add(new ResponseError { Code = error.Code, Message = error.Message });
                    response = ResponseHelper.Error<GetParcelResponse>(errors);

                    return await Task.FromResult(response);
                }

                // Build response
                data = ParcelMapper.Map(parcel, getCourierOrderResponse.Data);
                response = ResponseHelper.Ok(data);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);

                errors.Add(new ResponseError { Code = null, Message = ex.Message });
                response = ResponseHelper.Error<GetParcelResponse>(errors);
            }

            return await Task.FromResult(response);
        }

        public async Task<Response<List<GetParcelResponse>>> GetAsync(IConfiguration configuration, GetParcelByFilterQuery query)
        {
            List<GetParcelResponse> data = null;
            List<ResponseError> errors = new List<ResponseError>();
            Response<List<GetParcelResponse>> response = null;

            try
            {
                // Get parcels
                var parcels = new List<Database.Models.Parcel>();
                if (query.Payload.BillId.HasValue)
                {
                    parcels = Repository.GetByBillId(query.Payload.BillId.Value);
                }
                else if (query.Payload.Ids.Length > 0)
                {
                    parcels = Repository.GetByIds(query.Payload.Ids);
                }

                // Get courier's orders
                var courierOrders = new List<Courier.Client.GetOrderResponse>();
                foreach (var parcel in parcels)
                {
                    var getCourierOrderResponse = await _courierManager.GetOrderByFilterAsync(query.Request, parcel.Id);
                    if (!CourierClientHelper.IsSuccess(getCourierOrderResponse))
                    {
                        var error = CourierClientHelper.GetError(getCourierOrderResponse.Errors);

                        errors.Add(new ResponseError { Code = error.Code, Message = error.Message });
                        response = ResponseHelper.Error<List<GetParcelResponse>>(errors);

                        return await Task.FromResult(response);
                    }

                    courierOrders.Add(getCourierOrderResponse.Data);
                }

                // Build response
                data = ParcelMapper.Map(parcels, courierOrders);
                response = ResponseHelper.Ok(data);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);

                errors.Add(new ResponseError { Code = null, Message = ex.Message });
                response = ResponseHelper.Error<List<GetParcelResponse>>(errors);
            }

            return await Task.FromResult(response);
        }

        public async Task<Response<CreateParcelResponse>> CreateAsync(IConfiguration configuration, CreateParcelCommand command)
        {
            CreateParcelResponse data = null;
            List<ResponseError> errors = new List<ResponseError>();
            Response<CreateParcelResponse> response = null;

            try
            {
                // Get bill
                var bill = _billRepository.GetById(command.Payload.BillId);
                if (bill == null)
                {
                    errors.Add(new ResponseError { Code = EnumErrorCode._102080001.GetDescription(), Message = ErrorMessage._102080001 });
                    response = ResponseHelper.Error<CreateParcelResponse>(errors);

                    return await Task.FromResult(response);
                }

                if (bill.BillRecipient == null)
                {
                    errors.Add(new ResponseError { Code = EnumErrorCode._102080004.GetDescription(), Message = ErrorMessage._102080004 });
                    response = ResponseHelper.Error<CreateParcelResponse>(errors);

                    return await Task.FromResult(response);
                }

                if (bill.BillRecipient.BillRecipientContact == null)
                {
                    errors.Add(new ResponseError { Code = EnumErrorCode._102080005.GetDescription(), Message = ErrorMessage._102080005 });
                    response = ResponseHelper.Error<CreateParcelResponse>(errors);

                    return await Task.FromResult(response);
                }

                // Get bill's shipping
                var billShipping = _billShippingRepository.GetByBillId(bill.Id);
                if (billShipping == null)
                {
                    errors.Add(new ResponseError { Code = EnumErrorCode._102080031.GetDescription(), Message = ErrorMessage._102080031 });
                    response = ResponseHelper.Error<CreateParcelResponse>(errors);

                    return await Task.FromResult(response);
                }

                // Get courier's account
                Setting.Client.GetCourierAccountResponse courierAccount = null;
                var getCourierAccountResponse = await _settingManager.GetCourierAccountByChannelIdAsync(command.Request, bill.ChannelId);
                if (!SettingClientHelper.IsSuccess(getCourierAccountResponse))
                {
                    var error = SettingClientHelper.GetError(getCourierAccountResponse.Errors);

                    errors.Add(new ResponseError { Code = error.Code, Message = error.Message });
                    response = ResponseHelper.Error<CreateParcelResponse>(errors);

                    return await Task.FromResult(response);
                }
                else
                {
                    courierAccount = getCourierAccountResponse.Data;
                }

                // Check has courier's account
                if (!courierAccount.Has_account || courierAccount.Courier == null)
                {
                    errors.Add(new ResponseError { Code = EnumErrorCode._102080040.GetDescription(), Message = ErrorMessage._102080040 });
                    response = ResponseHelper.Error<CreateParcelResponse>(errors);

                    return await Task.FromResult(response);
                }

                // Get bill recipient's area
                Area.Client.GetAreaResponse billRecipientArea = null;
                var getAreaByIdResponse = await _areaManager.GetAreaByIdAsync(command.Request, bill.BillRecipient.BillRecipientContact.AreaId ?? Guid.Empty);
                if (!AreaClientHelper.IsSuccess(getAreaByIdResponse))
                {
                    var error = AreaClientHelper.GetError(getAreaByIdResponse.Errors);

                    errors.Add(new ResponseError { Code = error.Code, Message = error.Message });
                    response = ResponseHelper.Error<CreateParcelResponse>(errors);

                    return await Task.FromResult(response);
                }
                else
                {
                    billRecipientArea = getAreaByIdResponse.Data;
                }

                // Get shop's account
                Setting.Client.GetShopAccountResponse shopAccount = null;
                var getShopAcccountResponse = await _settingManager.GetShopAccountByChannelIdAsync(command.Request, bill.ChannelId);
                if (!SettingClientHelper.IsSuccess(getShopAcccountResponse))
                {
                    var error = SettingClientHelper.GetError(getShopAcccountResponse.Errors);

                    errors.Add(new ResponseError { Code = error.Code, Message = error.Message });
                    response = ResponseHelper.Error<CreateParcelResponse>(errors);

                    return await Task.FromResult(response);
                }
                else
                {
                    shopAccount = getShopAcccountResponse.Data;
                }

                // Get orders
                ICollection<Order.Client.GetOrderResponse> orders = new List<Order.Client.GetOrderResponse>();
                var getOrdersResponse = await _orderManager.GetOrderByBillIdAsync(command.Request, bill.Id, Order.Client.EnumOrderStatus.Unknown);
                if (!OrderClientHelper.IsSuccess(getOrdersResponse))
                {
                    var error = OrderClientHelper.GetError(getOrdersResponse.Errors);

                    errors.Add(new ResponseError { Code = error.Code, Message = error.Message });
                    response = ResponseHelper.Error<CreateParcelResponse>(errors);

                    return await Task.FromResult(response);
                }
                else
                {
                    orders = getOrdersResponse.Data;
                }

                if (!command.Payload.Orders.IsEmpty())
                {
                    var orderIds = command.Payload.Orders
                        .Select(x => x.Id)
                        .ToArray();
                    orders = orders
                        .Where(x => orderIds.Contains(x.Id))
                        .ToList();
                }

                // Get courier's shipping
                ICollection<Setting.Client.GetCourierShippingResponse> courierShippings = new List<Setting.Client.GetCourierShippingResponse>();
                Setting.Client.GetCourierShippingResponse courierShipping = null;
                var getCourierShippingsResponse = await _settingManager.GetCourierShippingsByCourierIdAsync(command.Request, courierAccount.Courier.Id);
                if (!SettingClientHelper.IsSuccess(getCourierShippingsResponse))
                {
                    var error = SettingClientHelper.GetError(getCourierShippingsResponse.Errors);

                    errors.Add(new ResponseError { Code = error.Code, Message = error.Message });
                    response = ResponseHelper.Error<CreateParcelResponse>(errors);

                    return await Task.FromResult(response);
                }
                else
                {
                    courierShippings = getCourierShippingsResponse.Data;
                    courierShipping = courierShippings.FirstOrDefault(x => x.Courier_shipping_type_id == command.Payload.Option.ShippingTypeId);
                }

                // Get courier's velocity
                ICollection<Setting.Client.GetCourierVelocityResponse> courierVelocities = new List<Setting.Client.GetCourierVelocityResponse>();
                Setting.Client.GetCourierVelocityResponse courierVelocity = null;
                var getCourierVelocitiesResponse = await _settingManager.GetCourierVelocitiesByCourierIdAsync(command.Request, courierAccount.Courier.Id);
                if (!SettingClientHelper.IsSuccess(getCourierVelocitiesResponse))
                {
                    var error = SettingClientHelper.GetError(getCourierVelocitiesResponse.Errors);

                    errors.Add(new ResponseError { Code = error.Code, Message = error.Message });
                    response = ResponseHelper.Error<CreateParcelResponse>(errors);

                    return await Task.FromResult(response);
                }
                else
                {
                    courierVelocities = getCourierVelocitiesResponse.Data;
                    courierVelocity = courierVelocities.FirstOrDefault(x => x.Courier_velocity_type_id == command.Payload.Option.VelocityTypeId);
                }

                // Get courier's payment
                ICollection<Setting.Client.GetCourierPaymentResponse> courierPayments = new List<Setting.Client.GetCourierPaymentResponse>();
                Setting.Client.GetCourierPaymentResponse courierPayment = null;
                var getCourierPaymentsResponse = await _settingManager.GetCourierPaymentsByCourierIdAsync(command.Request, courierAccount.Courier.Id);
                if (!SettingClientHelper.IsSuccess(getCourierPaymentsResponse))
                {
                    var error = SettingClientHelper.GetError(getCourierPaymentsResponse.Errors);

                    errors.Add(new ResponseError { Code = error.Code, Message = error.Message });
                    response = ResponseHelper.Error<CreateParcelResponse>(errors);

                    return await Task.FromResult(response);
                }
                else
                {
                    courierPayments = getCourierPaymentsResponse.Data;
                    courierPayment = courierPayments.FirstOrDefault(x => x.Courier_payment_type_id == command.Payload.Option.PaymentTypeId);
                }

                // Get courier's insurance
                ICollection<Setting.Client.GetCourierInsuranceResponse> courierInsurances = new List<Setting.Client.GetCourierInsuranceResponse>();
                Setting.Client.GetCourierInsuranceResponse courierInsurance = null;
                var getCourierInsurancesResponse = await _settingManager.GetCourierInsurancesByCourierIdAsync(command.Request, courierAccount.Courier.Id);
                if (!SettingClientHelper.IsSuccess(getCourierInsurancesResponse))
                {
                    var error = SettingClientHelper.GetError(getCourierInsurancesResponse.Errors);

                    errors.Add(new ResponseError { Code = error.Code, Message = error.Message });
                    response = ResponseHelper.Error<CreateParcelResponse>(errors);

                    return await Task.FromResult(response);
                }
                else
                {
                    courierInsurances = getCourierInsurancesResponse.Data;
                    courierInsurance = courierInsurances.FirstOrDefault(x => x.Courier_insurance_type_id == command.Payload.Option.InsuranceTypeId);
                }

                // Get cost
                var cost = BillMapper.CostMapper.Map(orders, billShipping, new List<Database.Models.Payment>(), bill.BillDiscount, bill.BillPayment);
                var paymentType = CourierSettingHelper.GetPaymentType(courierPayment.Code);
                var codAmount = ParcelHelper.GetCodAmount(command.Payload.Option.CodAmount, paymentType);
                var insuranceType = CourierSettingHelper.GetInsuranceType(courierInsurance.Code);
                var insuranceAmount = paymentType == Courier.Client.EnumPaymentType.Cod ?
                    ParcelHelper.GetCodAmount(command.Payload.Option.CodAmount, paymentType) :
                    ParcelHelper.GetInsuranceAmount(cost.Gross, insuranceType);
                var option = new CreateParcelOption
                {
                    ShippingTypeId = command.Payload.Option.ShippingTypeId,
                    VelocityTypeId = command.Payload.Option.VelocityTypeId,
                    PaymentTypeId = command.Payload.Option.PaymentTypeId,
                    CodAmount = codAmount,
                    InsuranceTypeId = command.Payload.Option.InsuranceTypeId,
                    InsuranceAmount = insuranceAmount
                };

                // Create courier's order
                var createCourierOrderResponse = await CreateCourierOrderAsync(
                    command.Request,
                    bill,
                    billRecipientArea,
                    orders,
                    shopAccount,
                    courierAccount,
                    courierShipping,
                    courierVelocity,
                    courierPayment,
                    courierInsurance,
                    option,
                    command.Payload.UserId);
                if (!CourierClientHelper.IsSuccess(createCourierOrderResponse))
                {
                    var error = CourierClientHelper.GetError(createCourierOrderResponse.Errors);

                    errors.Add(new ResponseError { Code = error.Code, Message = error.Message });
                    response = ResponseHelper.Error<CreateParcelResponse>(errors);

                    return await Task.FromResult(response);
                }

                var getCourierOrderResponse = await _courierManager.GetOrderByIdAsync(command.Request, createCourierOrderResponse.Data.Id);
                if (!CourierClientHelper.IsSuccess(getCourierOrderResponse))
                {
                    var error = CourierClientHelper.GetError(getCourierOrderResponse.Errors);

                    errors.Add(new ResponseError { Code = error.Code, Message = error.Message });
                    response = ResponseHelper.Error<CreateParcelResponse>(errors);

                    return await Task.FromResult(response);
                }

                // Create bill's parcel
                var parcelStatus = _parcelStatusRepository.GetByCode(EnumParcelStatus.Active.GetDescription());
                var parcel = new Database.Models.Parcel
                {
                    Id = getCourierOrderResponse.Data.Ref_id,
                    BillId = bill.Id,
                    ParcelStatusId = parcelStatus.Id
                };
                Repository.Add(parcel);

                // Update order's parcel
                var updateParcelResponse = await UpdateParcelAsync(command.Request, orders, getCourierOrderResponse.Data.Ref_id, command.Payload.UserId);
                if (!OrderClientHelper.IsSuccess(updateParcelResponse))
                {
                    var error = OrderClientHelper.GetError(updateParcelResponse.Errors);

                    errors.Add(new ResponseError { Code = error.Code, Message = error.Message });
                    response = ResponseHelper.Error<CreateParcelResponse>(errors);

                    return await Task.FromResult(response);
                }

                // Done bill
                var billStatus = _billStatusRepository.GetByCode(EnumBillStatus.Done.GetDescription());

                bill.IsDeposit = false;
                bill.BillStatusId = billStatus.Id;
                bill.UpdatedBy = command.Payload.UserId;
                bill.UpdatedOn = DateTime.Now;

                _billRepository.Update(bill);

                // Send notification
                var createNotificationParcelCreatedResponse = await CreateNotificationParcelCreatedAsync(command.Request, bill, getCourierOrderResponse.Data);
                if (!NotificationClientHelper.IsSuccess(createNotificationParcelCreatedResponse))
                {
                    var error = NotificationClientHelper.GetError(createNotificationParcelCreatedResponse.Errors);

                    errors.Add(new ResponseError { Code = error.Code, Message = error.Message });
                    response = ResponseHelper.Error<CreateParcelResponse>(errors);

                    return await Task.FromResult(response);
                }

                // Send response message
                await _queueManager.NotifyParcelIssueAsync(parcel.Id);

                // Build response
                data = new CreateParcelResponse(parcel.Id);
                response = ResponseHelper.Ok(data);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);

                errors.Add(new ResponseError { Code = null, Message = ex.Message });
                response = ResponseHelper.Error<CreateParcelResponse>(errors);
            }

            return await Task.FromResult(response);
        }

        public async Task<Response<CreateParcelsResponse>> CreateAsync(IConfiguration configuration, CreateParcelsCommand command)
        {
            CreateParcelsResponse data = null;
            List<ResponseError> errors = new List<ResponseError>();
            Response<CreateParcelsResponse> response = null;

            try
            {
                // Get bills
                var bills = _billRepository.GetByIds(command.Payload.BillIds);
                if (bills.IsEmpty())
                {
                    errors.Add(new ResponseError { Code = EnumErrorCode._102080001.GetDescription(), Message = ErrorMessage._102080001 });
                    response = ResponseHelper.Error<CreateParcelsResponse>(errors);

                    return await Task.FromResult(response);
                }

                foreach (var bill in bills)
                {
                    // Get bill's shipping
                    var billShipping = _billShippingRepository.GetByBillId(bill.Id);
                    if (billShipping == null)
                    {
                        errors.Add(new ResponseError { Code = EnumErrorCode._102080031.GetDescription(), Message = ErrorMessage._102080031 });
                        response = ResponseHelper.Error<CreateParcelsResponse>(errors);

                        return await Task.FromResult(response);
                    }

                    // Get orders
                    ICollection<Order.Client.GetOrderResponse> orders = new List<Order.Client.GetOrderResponse>();
                    var getOrdersResponse = await _orderManager.GetOrderByBillIdAsync(command.Request, bill.Id, Order.Client.EnumOrderStatus.Unknown);
                    if (!OrderClientHelper.IsSuccess(getOrdersResponse))
                    {
                        var error = OrderClientHelper.GetError(getOrdersResponse.Errors);

                        errors.Add(new ResponseError { Code = error.Code, Message = error.Message });
                        response = ResponseHelper.Error<CreateParcelsResponse>(errors);

                        return await Task.FromResult(response);
                    }
                    else
                    {
                        orders = getOrdersResponse.Data;
                    }

                    // Get courier's account
                    Setting.Client.GetCourierAccountResponse courierAccount = null;
                    var getCourierAccountResponse = await _settingManager.GetCourierAccountByChannelIdAsync(command.Request, bill.ChannelId);
                    if (!SettingClientHelper.IsSuccess(getCourierAccountResponse))
                    {
                        var error = SettingClientHelper.GetError(getCourierAccountResponse.Errors);

                        errors.Add(new ResponseError { Code = error.Code, Message = error.Message });
                        response = ResponseHelper.Error<CreateParcelsResponse>(errors);

                        return await Task.FromResult(response);
                    }
                    else
                    {
                        courierAccount = getCourierAccountResponse.Data;
                    }

                    // Get courier's payment
                    ICollection<Setting.Client.GetCourierPaymentResponse> courierPayments = new List<Setting.Client.GetCourierPaymentResponse>();
                    var getCourierPaymentsResponse = await _settingManager.GetCourierPaymentsByCourierIdAsync(command.Request, courierAccount.Courier.Id);
                    if (!SettingClientHelper.IsSuccess(getCourierPaymentsResponse))
                    {
                        var error = SettingClientHelper.GetError(getCourierPaymentsResponse.Errors);

                        errors.Add(new ResponseError { Code = error.Code, Message = error.Message });
                        response = ResponseHelper.Error<CreateParcelsResponse>(errors);

                        return await Task.FromResult(response);
                    }
                    else
                    {
                        courierPayments = getCourierPaymentsResponse.Data;
                    }

                    // Get courier's insurance
                    ICollection<Setting.Client.GetCourierInsuranceResponse> courierInsurances = new List<Setting.Client.GetCourierInsuranceResponse>();
                    var getCourierInsurancesResponse = await _settingManager.GetCourierInsurancesByCourierIdAsync(command.Request, courierAccount.Courier.Id);
                    if (!SettingClientHelper.IsSuccess(getCourierInsurancesResponse))
                    {
                        var error = SettingClientHelper.GetError(getCourierInsurancesResponse.Errors);

                        errors.Add(new ResponseError { Code = error.Code, Message = error.Message });
                        response = ResponseHelper.Error<CreateParcelsResponse>(errors);

                        return await Task.FromResult(response);
                    }
                    else
                    {
                        courierInsurances = getCourierInsurancesResponse.Data;
                    }

                    // Get courier's settings
                    var billPaymentType = BillPaymentTypeHelper.GetByCode(bill.BillPayment.BillPaymentType.Code);
                    var courierPaymentType = CourierSettingHelper.GetPaymentType(billPaymentType);
                    var courierPayment = CourierSettingHelper.GetPaymentType(courierPayments, courierPaymentType);
                    var courierInsurance = CourierSettingHelper.GetInsuranceType(courierInsurances, command.Payload.Option.InsuranceTypeId);
                    var courierInsuranceType = CourierSettingHelper.GetInsuranceType(courierInsurance.Code);

                    // Get cost
                    var cost = BillMapper.CostMapper.Map(orders, billShipping, new List<Database.Models.Payment>(), bill.BillDiscount, bill.BillPayment);
                    var codAmount = ParcelHelper.GetCodAmount(cost.Gross, courierPaymentType);
                    var insuranceAmount = ParcelHelper.GetInsuranceAmount(cost.Gross, courierInsuranceType);

                    // Create parcel
                    var createParcelOrders = new List<CreateParcelOrder>();
                    var createParcelOption = new CreateParcelOption
                    {
                        ShippingTypeId = command.Payload.Option.ShippingTypeId,
                        VelocityTypeId = command.Payload.Option.VelocityTypeId,
                        PaymentTypeId = courierPayment.Courier_payment_type_id,
                        CodAmount = codAmount,
                        InsuranceTypeId = command.Payload.Option.InsuranceTypeId,
                        InsuranceAmount = insuranceAmount
                    };
                    var createParcelPayload = new CreateParcel
                    {
                        BillId = bill.Id,
                        Orders = createParcelOrders,
                        Option = createParcelOption,
                        UserId = command.Payload.UserId
                    };
                    var createParcelCommand = new CreateParcelCommand(command.Request, createParcelPayload);
                    var createParcelResponse = await CreateAsync(configuration, createParcelCommand);
                    if (!createParcelResponse.Errors.IsEmpty())
                    {
                        var error = createParcelResponse.Errors.FirstOrDefault();

                        errors.Add(new ResponseError { Code = error.Code, Message = error.Message });
                        response = ResponseHelper.Error<CreateParcelsResponse>(errors);

                        return await Task.FromResult(response);
                    }
                }

                // Build response
                data = new CreateParcelsResponse();
                response = ResponseHelper.Ok(data);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);

                errors.Add(new ResponseError { Code = null, Message = ex.Message });
                response = ResponseHelper.Error<CreateParcelsResponse>(errors);
            }

            return await Task.FromResult(response);
        }

        public async Task<Response<UpdateParcelResponse>> UpdateAsync(IConfiguration configuration, UpdateParcelCommand command)
        {
            UpdateParcelResponse data = null;
            List<ResponseError> errors = new List<ResponseError>();
            Response<UpdateParcelResponse> response = null;

            try
            {
                // Update
                var orderIds = command.Payload.Orders
                    .Select(x => x.Id)
                    .ToArray();
                var updateParcelResponse = await _orderManager.UpdateParcelAsync(command.Request, orderIds, command.Payload.Id, command.Payload.UserId);
                if (!OrderClientHelper.IsSuccess(updateParcelResponse))
                {
                    var error = OrderClientHelper.GetError(updateParcelResponse.Errors);

                    errors.Add(new ResponseError { Code = error.Code, Message = error.Message });
                    response = ResponseHelper.Error<UpdateParcelResponse>(errors);

                    return await Task.FromResult(response);
                }

                // Build response
                data = new UpdateParcelResponse();
                response = ResponseHelper.Ok(data);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);

                errors.Add(new ResponseError { Code = null, Message = ex.Message });
                response = ResponseHelper.Error<UpdateParcelResponse>(errors);
            }

            return await Task.FromResult(response);
        }

        public async Task<Response<UpdateParcelPrintedResponse>> UpdateAsync(IConfiguration configuration, UpdateParcelPrintedCommand command)
        {
            UpdateParcelPrintedResponse data = null;
            List<ResponseError> errors = new List<ResponseError>();
            Response<UpdateParcelPrintedResponse> response = null;

            try
            {
                // Get parcel
                var parcel = Repository.GetById(command.Payload.Id);
                if (parcel == null)
                {
                    errors.Add(new ResponseError { Code = EnumErrorCode._102080029.GetDescription(), Message = ErrorMessage._102080029 });
                    response = ResponseHelper.Error<UpdateParcelPrintedResponse>(errors);

                    return await Task.FromResult(response);
                }

                // Update
                parcel.IsPrinted = true;
                Repository.Update(parcel);

                // Build response
                data = new UpdateParcelPrintedResponse();
                response = ResponseHelper.Ok(data);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);

                errors.Add(new ResponseError { Code = null, Message = ex.Message });
                response = ResponseHelper.Error<UpdateParcelPrintedResponse>(errors);
            }

            return await Task.FromResult(response);
        }

        public async Task<Response<CancelParcelResponse>> UpdateAsync(IConfiguration configuration, CancelParcelCommand command)
        {
            CancelParcelResponse data = null;
            List<ResponseError> errors = new List<ResponseError>();
            Response<CancelParcelResponse> response = null;

            try
            {
                // Get parcel
                var parcel = Repository.GetById(command.Payload.Id);
                if (parcel == null)
                {
                    errors.Add(new ResponseError { Code = EnumErrorCode._102080029.GetDescription(), Message = ErrorMessage._102080029 });
                    response = ResponseHelper.Error<CancelParcelResponse>(errors);

                    return await Task.FromResult(response);
                }

                // Get bill
                var bill = _billRepository.GetById(parcel.BillId);
                if (bill == null)
                {
                    errors.Add(new ResponseError { Code = EnumErrorCode._102080001.GetDescription(), Message = ErrorMessage._102080001 });
                    response = ResponseHelper.Error<CancelParcelResponse>(errors);

                    return await Task.FromResult(response);
                }

                // Get courier's account
                Setting.Client.GetCourierAccountResponse courierAccount = null;
                var getCourierAccountResponse = await _settingManager.GetCourierAccountByChannelIdAsync(command.Request, bill.ChannelId);
                if (!SettingClientHelper.IsSuccess(getCourierAccountResponse))
                {
                    var error = SettingClientHelper.GetError(getCourierAccountResponse.Errors);

                    errors.Add(new ResponseError { Code = error.Code, Message = error.Message });
                    response = ResponseHelper.Error<CancelParcelResponse>(errors);

                    return await Task.FromResult(response);
                }
                else
                {
                    courierAccount = getCourierAccountResponse.Data;
                }

                // Cancel courier's order
                var cancelCourierOrderResponse = await CancelCourierOrderAsync(command.Request, parcel.Id, command.Payload.UserId);
                if (!CourierClientHelper.IsSuccess(cancelCourierOrderResponse))
                {
                    var error = CourierClientHelper.GetError(cancelCourierOrderResponse.Errors);

                    errors.Add(new ResponseError { Code = error.Code, Message = error.Message });
                    response = ResponseHelper.Error<CancelParcelResponse>(errors);

                    return await Task.FromResult(response);
                }

                // Update parcel
                var parcelStatus = _parcelStatusRepository.GetByCode(EnumParcelStatus.Cancelled.GetDescription());

                parcel.ParcelStatusId = parcelStatus.Id;
                Repository.Update(parcel);

                // Update parcel_id to null on order
                var cancelParcelResponse = await CancelParcelAsync(command.Request, parcel, command.Payload.UserId);
                if (!OrderClientHelper.IsSuccess(cancelParcelResponse))
                {
                    var error = OrderClientHelper.GetError(cancelParcelResponse.Errors);

                    errors.Add(new ResponseError { Code = error.Code, Message = error.Message });
                    response = ResponseHelper.Error<CancelParcelResponse>(errors);

                    return await Task.FromResult(response);
                }

                // Get courier's order
                var getCourierOrderResponse = await _courierManager.GetOrderByFilterAsync(command.Request, parcel.Id);
                if (!CourierClientHelper.IsSuccess(getCourierOrderResponse))
                {
                    var error = CourierClientHelper.GetError(getCourierOrderResponse.Errors);

                    errors.Add(new ResponseError { Code = error.Code, Message = error.Message });
                    response = ResponseHelper.Error<CancelParcelResponse>(errors);

                    return await Task.FromResult(response);
                }

                // Send notification
                var createNotificationParcelCancelledResponse = await CreateNotificationParcelCancelledAsync(command.Request, bill, getCourierOrderResponse.Data);
                if (!NotificationClientHelper.IsSuccess(createNotificationParcelCancelledResponse))
                {
                    var error = NotificationClientHelper.GetError(createNotificationParcelCancelledResponse.Errors);

                    errors.Add(new ResponseError { Code = error.Code, Message = error.Message });
                    response = ResponseHelper.Error<CancelParcelResponse>(errors);

                    return await Task.FromResult(response);
                }

                // Build response
                data = new CancelParcelResponse();
                response = ResponseHelper.Ok(data);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);

                errors.Add(new ResponseError { Code = null, Message = ex.Message });
                response = ResponseHelper.Error<CancelParcelResponse>(errors);
            }

            return await Task.FromResult(response);
        }

        private async Task<Order.Client.ResponseOfUpdateParcelResponse> UpdateParcelAsync(HttpRequest request, ICollection<Order.Client.GetOrderResponse> orders, Guid parcelId, Guid userId)
        {
            var ids = orders
                .Select(x => x.Id)
                .ToArray();

            return await _orderManager.UpdateParcelAsync(request, ids, parcelId, userId);
        }

        private async Task<Courier.Client.ResponseOfCreateOrderResponse> CreateCourierOrderAsync(
            HttpRequest request,
            Database.Models.Bill bill,
            Area.Client.GetAreaResponse billRecipientArea,
            ICollection<Order.Client.GetOrderResponse> orders,
            Setting.Client.GetShopAccountResponse shopAccount,
            Setting.Client.GetCourierAccountResponse courierAccount,
            Setting.Client.GetCourierShippingResponse courierShipping,
            Setting.Client.GetCourierVelocityResponse courierVelocity,
            Setting.Client.GetCourierPaymentResponse courierPayment,
            Setting.Client.GetCourierInsuranceResponse courierInsurance,
            CreateParcelOption option,
            Guid userId)
        {
            // Get courier's settings
            var courierShippingCode = courierShipping != null ? courierShipping.Code : courierAccount.Courier_shipping_type.Code;
            var courierVelocityCode = courierVelocity != null ? courierVelocity.Code : courierAccount.Courier_velocity_type.Code;
            var courierPaymentCode = courierPayment != null ? courierPayment.Code : courierAccount.Courier_payment_type.Code;
            var courierInsuranceCode = courierPayment != null ? courierInsurance.Code : courierAccount.Courier_insurance_type.Code;

            // Prepare data
            var courier = CourierHelper.GetByCode(courierAccount.Courier.Code);
            var ownershipType = OwnershipTypeHelper.Get(courierAccount);
            var shippingType = CourierSettingHelper.GetShippingType(courierShippingCode);
            var velocityType = CourierSettingHelper.GetVelocityType(courierVelocityCode);
            var paymentType = CourierSettingHelper.GetPaymentType(courierPaymentCode);
            var codAmount = option.CodAmount;
            var insuranceType = CourierSettingHelper.GetInsuranceType(courierInsuranceCode);
            var insuranceAmount = option.InsuranceAmount;
            var orderManifest = new Courier.Client.CreateOrderManifest
            {
                Shop_id = courierAccount.Merchant_id,
                Courier = courier,
                Ownership_type = ownershipType,
                Shipping_type = shippingType,
                Velocity_type = velocityType,
                Payment_type = paymentType,
                Cod_amount = codAmount,
                Insurance_type = insuranceType,
                Insurance_amount = insuranceAmount,
                Weight = null,
                Width = null,
                Length = null,
                Height = null
            };
            var orderSender = new Courier.Client.CreateOrderSender
            {
                Name = shopAccount.Name,
                Address = shopAccount.Address,
                Area_id = shopAccount.Area.Id,
                Sub_district = shopAccount.Area.Sub_district,
                District = shopAccount.Area.District,
                Province = shopAccount.Area.Province,
                Postal_code = shopAccount.Area.Postal_code,
                Phone_no = shopAccount.Primary_phone
            };
            var orderRecipient = new Courier.Client.CreateOrderRecipient
            {
                Name = bill.BillRecipient.Name,
                Address = bill.BillRecipient.BillRecipientContact.Address,
                Area_id = billRecipientArea.Id,
                Sub_district = billRecipientArea.Sub_district,
                District = billRecipientArea.District,
                Province = billRecipientArea.Province,
                Postal_code = billRecipientArea.Postal_code,
                Phone_no = bill.BillRecipient.BillRecipientContact.PrimaryPhone
            };
            var orderItems = new List<Courier.Client.CreateOrderItem>();
            foreach (var order in orders)
            {
                var orderItem = new Courier.Client.CreateOrderItem
                {
                    Sku = order.Product.Sku,
                    Code = order.Product.Code,
                    Alias_code = order.Code,
                    Name = order.Product.Name,
                    Color = order.Product.Color,
                    Size = order.Product.Size,
                    Description = order.Product.Description,
                    Unit_price = order.Unit_price,
                    Amount = order.Amount
                };

                orderItems.Add(orderItem);
            }

            var courierOrder = new Courier.Client.CreateOrder
            {
                Channel_id = bill.ChannelId,
                Ref_id = Guid.NewGuid(),
                Manifest = orderManifest,
                Sender = orderSender,
                Recipient = orderRecipient,
                Items = orderItems,
                Remark = bill.Remark,
                User_id = userId
            };

            return await _courierManager.CreateOrderAsync(request, courierOrder);
        }

        private async Task<Courier.Client.ResponseOfCancelOrderResponse> CancelCourierOrderAsync(HttpRequest request, Guid refId, Guid userId)
        {
            var cancelCourierOrderPayload = new Courier.Client.CancelOrder
            {
                Ref_id = refId,
                Remark = null,
                User_id = userId
            };

            return await _courierManager.CancelOrderAsync(request, cancelCourierOrderPayload);
        }

        private async Task<Order.Client.ResponseOfCancelParcelResponse> CancelParcelAsync(
            HttpRequest request,
            Database.Models.Parcel parcel,
            Guid userId)
        {
            return await _orderManager.CancelParcelAsync(request, parcel.Id, userId);
        }

        private async Task<Notification.Client.ResponseOfCreateNotificationResponse> CreateNotificationParcelCreatedAsync(
            HttpRequest request,
            Database.Models.Bill bill,
            Courier.Client.GetOrderResponse courierOrder)
        {
            var channelId = bill.ChannelId;
            var type = Notification.Client.EnumNotificationType.Information;
            var subject = Notification.Client.EnumNotificationSubject.ParcelCreated;
            var display = Notification.Client.EnumNotificationDisplay.Silent;
            var title = $"สร้างเลขพัสดุ {courierOrder.Feedback.Ref_1} สำเร็จ";
            var content = $"เลขพัสดุ {courierOrder.Feedback.Ref_1} สำหรับบิล {bill.BillNo} ถูกสร้างแล้ว";

            return await _notificationManager.CreateNotificationForChannelAsync(request, channelId, type, subject, display, title, content);
        }

        private async Task<Notification.Client.ResponseOfCreateNotificationResponse> CreateNotificationParcelCancelledAsync(
            HttpRequest request,
            Database.Models.Bill bill,
            Courier.Client.GetOrderResponse courierOrder)
        {
            var channelId = bill.ChannelId;
            var type = Notification.Client.EnumNotificationType.Information;
            var subject = Notification.Client.EnumNotificationSubject.ParcelCreated;
            var display = Notification.Client.EnumNotificationDisplay.Silent;
            var title = $"ยกเลิกเลขพัสดุ {courierOrder.Feedback.Ref_1} สำเร็จ";
            var content = $"เลขพัสดุ {courierOrder.Feedback.Ref_1} สำหรับบิล {bill.BillNo} ถูกยกเลิกแล้ว";

            return await _notificationManager.CreateNotificationForChannelAsync(request, channelId, type, subject, display, title, content);
        }
    }
}