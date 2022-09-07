using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using SC.App.Services.Bill.Business.Commands.Bill;
using SC.App.Services.Bill.Business.Enums;
using SC.App.Services.Bill.Business.Helpers;
using SC.App.Services.Bill.Business.Managers.Interface;
using SC.App.Services.Bill.Business.Mappers;
using SC.App.Services.Bill.Business.Queries.Bill;
using SC.App.Services.Bill.Business.Repositories.Interface;
using SC.App.Services.Bill.Business.Resources;
using SC.App.Services.Bill.Business.Validators.Bill;
using SC.App.Services.Bill.Client.Area;
using SC.App.Services.Bill.Client.Courier;
using SC.App.Services.Bill.Client.Customer;
using SC.App.Services.Bill.Client.Inventory;
using SC.App.Services.Bill.Client.Order;
using SC.App.Services.Bill.Client.Setting;
using SC.App.Services.Bill.Common.Constants;
using SC.App.Services.Bill.Common.Helpers;
using SC.App.Services.Bill.Common.Managers;
using SC.App.Services.Bill.Common.Responses;
using SC.App.Services.Bill.Lib.Extensions;
using SC.App.Services.Bill.Queue.Managers.Interface;
using Serilog;

namespace SC.App.Services.Bill.Business.Managers
{
    public class BillManager : BaseManager<IBillRepository>, IBillManager
    {
        private readonly IBillStatusRepository _billStatusRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IOrderManager _orderManager;
        private readonly ICustomerManager _customerManager;
        private readonly ICourierManager _courierManager;
        private readonly IAreaManager _areaManager;
        private readonly ISettingManager _settingManager;
        private readonly IInventoryManager _inventoryManager;
        private readonly IQueueManager _queueManager;

        public BillManager(
            IBillRepository repository,
            IBillStatusRepository billStatusRepository,
            IPaymentRepository paymentRepository,
            IOrderManager orderManager,
            ICustomerManager customerManager,
            ICourierManager courierManager,
            IAreaManager areaManager,
            ISettingManager settingManager,
            IInventoryManager inventoryManager,
            IQueueManager queueManager)
            : base(repository)
        {
            _billStatusRepository = billStatusRepository;
            _paymentRepository = paymentRepository;
            _orderManager = orderManager;
            _customerManager = customerManager;
            _courierManager = courierManager;
            _areaManager = areaManager;
            _settingManager = settingManager;
            _inventoryManager = inventoryManager;
            _queueManager = queueManager;
        }

        public async Task<Response<GetBillResponse>> GetAsync(IConfiguration configuration, GetBillByIdQuery query)
        {
            GetBillResponse data = null;
            List<ResponseError> errors = new List<ResponseError>();
            Response<GetBillResponse> response = null;

            try
            {
                // Get bill
                var bill = Repository.GetById(query.Payload.Id);
                if (bill == null)
                {
                    errors.Add(new ResponseError { Code = EnumErrorCode._102080001.GetDescription(), Message = ErrorMessage._102080001 });
                    response = ResponseHelper.Error<GetBillResponse>(errors);

                    return await Task.FromResult(response);
                }

                // Get bill recipient contact's area
                Area.Client.GetAreaResponse contactArea = null;
                if (bill.BillRecipient != null && bill.BillRecipient.BillRecipientContact != null)
                {
                    var getAreaByIdResponse = await _areaManager.GetAreaByIdAsync(query.Request, bill.BillRecipient.BillRecipientContact.AreaId ?? Guid.Empty);
                    if (!AreaClientHelper.IsSuccess(getAreaByIdResponse))
                    {
                        var error = AreaClientHelper.GetError(getAreaByIdResponse.Errors);

                        errors.Add(new ResponseError { Code = error.Code, Message = error.Message });
                        response = ResponseHelper.Error<GetBillResponse>(errors);

                        return await Task.FromResult(response);
                    }
                    else
                    {
                        contactArea = getAreaByIdResponse.Data;
                    }
                }

                // Get customer
                Customer.Client.GetCustomerResponse customer = null;
                var getCustomerByIdResponse = await _customerManager.GetCustomerByIdAsync(query.Request, bill.BillRecipient.CustomerId);
                if (!CustomerClientHelper.IsSuccess(getCustomerByIdResponse))
                {
                    var error = CustomerClientHelper.GetError(getCustomerByIdResponse.Errors);

                    errors.Add(new ResponseError { Code = error.Code, Message = error.Message });
                    response = ResponseHelper.Error<GetBillResponse>(errors);

                    return await Task.FromResult(response);
                }
                else
                {
                    customer = getCustomerByIdResponse.Data;
                }

                // Get orders
                ICollection<Order.Client.GetOrderResponse> orders = new List<Order.Client.GetOrderResponse>();
                var getOrdersResponse = await _orderManager.GetOrderByBillIdAsync(query.Request, bill.Id, Order.Client.EnumOrderStatus.Unknown);
                if (!OrderClientHelper.IsSuccess(getOrdersResponse))
                {
                    var error = OrderClientHelper.GetError(getOrdersResponse.Errors);

                    errors.Add(new ResponseError { Code = error.Code, Message = error.Message });
                    response = ResponseHelper.Error<GetBillResponse>(errors);

                    return await Task.FromResult(response);
                }
                else
                {
                    orders = getOrdersResponse.Data;
                }

                // Get courier's orders
                var courierOrders = new List<Courier.Client.GetOrderResponse>();
                foreach (var parcel in bill.Parcels)
                {
                    var getCourierOrderResponse = await _courierManager.GetOrderByFilterAsync(query.Request, parcel.Id);
                    if (CourierClientHelper.IsSuccess(getCourierOrderResponse))
                    {
                        courierOrders.Add(getCourierOrderResponse.Data);
                    }
                }

                // Get preferences
                Setting.Client.GetPreferencesResponse preferences = null;
                var getPreferencesResponse = await _settingManager.GetPreferencesByChannelIdAsync(query.Request, bill.ChannelId);
                if (!SettingClientHelper.IsSuccess(getPreferencesResponse))
                {
                    var error = SettingClientHelper.GetError(getPreferencesResponse.Errors);

                    errors.Add(new ResponseError { Code = error.Code, Message = error.Message });
                    response = ResponseHelper.Error<GetBillResponse>(errors);

                    return await Task.FromResult(response);
                }
                else
                {
                    preferences = getPreferencesResponse.Data;
                }

                // Get buyer's base url
                var baseUrl = configuration.GetValue<string>(AppSettings.Applications.Buyer.BaseUrl);

                // Build response
                data = BillMapper.Map(baseUrl, bill, customer, contactArea, orders, courierOrders, preferences);
                response = ResponseHelper.Ok(data);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);

                errors.Add(new ResponseError { Code = null, Message = ex.Message });
                response = ResponseHelper.Error<GetBillResponse>(errors);
            }

            return await Task.FromResult(response);
        }

        public async Task<Response<GetBillResponse>> GetAsync(IConfiguration configuration, GetLatestBillByFilterQuery query)
        {
            GetBillResponse data = null;
            List<ResponseError> errors = new List<ResponseError>();
            Response<GetBillResponse> response = null;

            try
            {
                // Get latest bill
                var latestBill = Repository.GetLatest(query.Payload.ChannelId, query.Payload.CustomerId, null, null);
                if (latestBill == null)
                {
                    errors.Add(new ResponseError { Code = EnumErrorCode._102080001.GetDescription(), Message = ErrorMessage._102080001 });
                    response = ResponseHelper.Error<GetBillResponse>(errors);

                    return await Task.FromResult(response);
                }

                // Get bill recipient contact's area
                Area.Client.GetAreaResponse contactArea = null;
                if (latestBill.BillRecipient != null && latestBill.BillRecipient.BillRecipientContact != null)
                {
                    var getAreaByIdResponse = await _areaManager.GetAreaByIdAsync(query.Request, latestBill.BillRecipient.BillRecipientContact.AreaId ?? Guid.Empty);
                    if (!AreaClientHelper.IsSuccess(getAreaByIdResponse))
                    {
                        var error = AreaClientHelper.GetError(getAreaByIdResponse.Errors);

                        errors.Add(new ResponseError { Code = error.Code, Message = error.Message });
                        response = ResponseHelper.Error<GetBillResponse>(errors);

                        return await Task.FromResult(response);
                    }
                    else
                    {
                        contactArea = getAreaByIdResponse.Data;
                    }
                }

                // Get customer
                Customer.Client.GetCustomerResponse customer = null;
                var getCustomerByIdResponse = await _customerManager.GetCustomerByIdAsync(query.Request, latestBill.BillRecipient.CustomerId);
                if (!CustomerClientHelper.IsSuccess(getCustomerByIdResponse))
                {
                    var error = CustomerClientHelper.GetError(getCustomerByIdResponse.Errors);

                    errors.Add(new ResponseError { Code = error.Code, Message = error.Message });
                    response = ResponseHelper.Error<GetBillResponse>(errors);

                    return await Task.FromResult(response);
                }
                else
                {
                    customer = getCustomerByIdResponse.Data;
                }

                // Get orders
                ICollection<Order.Client.GetOrderResponse> orders = new List<Order.Client.GetOrderResponse>();
                var getOrdersResponse = await _orderManager.GetOrderByBillIdAsync(query.Request, latestBill.Id, Order.Client.EnumOrderStatus.Unknown);
                if (!OrderClientHelper.IsSuccess(getOrdersResponse))
                {
                    var error = OrderClientHelper.GetError(getOrdersResponse.Errors);

                    errors.Add(new ResponseError { Code = error.Code, Message = error.Message });
                    response = ResponseHelper.Error<GetBillResponse>(errors);

                    return await Task.FromResult(response);
                }
                else
                {
                    orders = getOrdersResponse.Data;
                }

                // Get courier's orders
                var courierOrders = new List<Courier.Client.GetOrderResponse>();
                foreach (var parcel in latestBill.Parcels)
                {
                    var getCourierOrderResponse = await _courierManager.GetOrderByFilterAsync(query.Request, parcel.Id);
                    if (CourierClientHelper.IsSuccess(getCourierOrderResponse))
                    {
                        courierOrders.Add(getCourierOrderResponse.Data);
                    }
                }

                // Get preferences
                Setting.Client.GetPreferencesResponse preferences = null;
                var getPreferencesResponse = await _settingManager.GetPreferencesByChannelIdAsync(query.Request, latestBill.ChannelId);
                if (!SettingClientHelper.IsSuccess(getPreferencesResponse))
                {
                    var error = SettingClientHelper.GetError(getPreferencesResponse.Errors);

                    errors.Add(new ResponseError { Code = error.Code, Message = error.Message });
                    response = ResponseHelper.Error<GetBillResponse>(errors);

                    return await Task.FromResult(response);
                }
                else
                {
                    preferences = getPreferencesResponse.Data;
                }

                // Get buyer's base url
                var baseUrl = configuration.GetValue<string>(AppSettings.Applications.Buyer.BaseUrl);

                // Build response
                data = BillMapper.Map(baseUrl, latestBill, customer, contactArea, orders, courierOrders, preferences);
                response = ResponseHelper.Ok(data);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);

                errors.Add(new ResponseError { Code = null, Message = ex.Message });
                response = ResponseHelper.Error<GetBillResponse>(errors);
            }

            return await Task.FromResult(response);
        }

        public async Task<Response<List<GetBillManifestResponse>>> GetAsync(IConfiguration configuration, GetLatestBillManifestByFilterQuery query)
        {
            List<GetBillManifestResponse> data = null;
            List<ResponseError> errors = new List<ResponseError>();
            Response<List<GetBillManifestResponse>> response = null;

            try
            {
                // Get orders
                ICollection<Order.Client.GetOrderResponse> orders = new List<Order.Client.GetOrderResponse>();
                var getOrdersResponse = await _orderManager.GetOrderByLiveIdAsync(query.Request, query.Payload.LiveId, Order.Client.EnumOrderStatus.Unknown);
                if (!OrderClientHelper.IsSuccess(getOrdersResponse))
                {
                    var error = OrderClientHelper.GetError(getOrdersResponse.Errors);

                    errors.Add(new ResponseError { Code = error.Code, Message = error.Message });
                    response = ResponseHelper.Error<List<GetBillManifestResponse>>(errors);

                    return await Task.FromResult(response);
                }
                else
                {
                    orders = getOrdersResponse.Data;
                }

                // Get bills
                var billIds = orders
                    .Where(x => x.Bill != null && x.Bill.Id.HasValue)
                    .Select(x => x.Bill.Id.Value)
                    .Distinct()
                    .ToArray();
                var bills = Repository.GetByIds(billIds);

                // Build response
                data = BillManifestMapper.Map(bills, orders);
                response = ResponseHelper.Ok(data);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);

                errors.Add(new ResponseError { Code = null, Message = ex.Message });
                response = ResponseHelper.Error<List<GetBillManifestResponse>>(errors);
            }

            return await Task.FromResult(response);
        }

        public async Task<Response<SearchBillResponse>> GetAsync(IConfiguration configuration, SearchBillByFilterQuery query)
        {
            SearchBillResponse data = null;
            List<ResponseError> errors = new List<ResponseError>();
            Response<SearchBillResponse> response = null;

            try
            {
                // Validate
                await new SearchBillByFilterValidator().ValidateAndThrowAsync(query.Payload);

                // Get page
                var page = PageHelper.GetPage(query.Payload.Page);

                // Get page size
                var pageSize = PageHelper.GetPageSize(query.Payload.PageSize);

                // Get begin and end
                DateTime begin = DateTime.Now;
                DateTime end = DateTime.Now;

                if (query.Payload.Period == EnumPeriod.Recent)
                {
                    var latestBill = Repository.GetLatest(query.Payload.ChannelId, null, null, null);
                    if (latestBill != null)
                    {
                        begin = PeriodHelper.GetBegin(latestBill.CreatedOn);
                        end = PeriodHelper.GetEnd(latestBill.CreatedOn);
                    }
                }
                else if (query.Payload.Period == EnumPeriod.Custom)
                {
                    begin = PeriodHelper.GetBegin(query.Payload.Date);
                    end = PeriodHelper.GetEnd(query.Payload.Date);
                }
                else
                {
                    begin = PeriodHelper.GetBegin(query.Payload.Period);
                    end = PeriodHelper.GetEnd(query.Payload.Period);
                }

                // Get number of items
                var numberOfItems = Repository.Count(query.Payload.ChannelId, query.Payload.Status, begin, end, query.Payload.Keyword);

                // Get bills
                var bills = Repository.Search(query.Payload.ChannelId, query.Payload.Status, begin, end, query.Payload.Keyword, query.Payload.SortBy, query.Payload.SortDesc, page, pageSize);

                // Get buyer's base url
                var baseUrl = configuration.GetValue<string>(AppSettings.Applications.Buyer.BaseUrl);

                // Get areas
                ICollection<Area.Client.GetAreaResponse> areas = new List<Area.Client.GetAreaResponse>();
                var getAreasResponse = await _areaManager.GetAreasAsync(query.Request);
                if (AreaClientHelper.IsSuccess(getAreasResponse))
                {
                    areas = getAreasResponse.Data;
                }

                // Get preferences
                Setting.Client.GetPreferencesResponse preferences = null;
                var getPreferencesResponse = await _settingManager.GetPreferencesByChannelIdAsync(query.Request, query.Payload.ChannelId);
                if (!SettingClientHelper.IsSuccess(getPreferencesResponse))
                {
                    var error = SettingClientHelper.GetError(getPreferencesResponse.Errors);

                    errors.Add(new ResponseError { Code = error.Code, Message = error.Message });
                    response = ResponseHelper.Error<SearchBillResponse>(errors);

                    return await Task.FromResult(response);
                }
                else
                {
                    preferences = getPreferencesResponse.Data;
                }

                // Get items
                var items = new List<SearchBillItem>();
                foreach (var bill in bills)
                {
                    // Get customer
                    Customer.Client.GetCustomerResponse customer = null;
                    var getCustomerByIdResponse = await _customerManager.GetCustomerByIdAsync(query.Request, bill.BillRecipient.CustomerId);
                    if (!CustomerClientHelper.IsSuccess(getCustomerByIdResponse))
                    {
                        var error = CustomerClientHelper.GetError(getCustomerByIdResponse.Errors);

                        errors.Add(new ResponseError { Code = error.Code, Message = error.Message });
                        response = ResponseHelper.Error<SearchBillResponse>(errors);

                        return await Task.FromResult(response);
                    }
                    else
                    {
                        customer = getCustomerByIdResponse.Data;
                    }

                    // Get orders
                    ICollection<Order.Client.GetOrderResponse> orders = new List<Order.Client.GetOrderResponse>();
                    var getOrdersResponse = await _orderManager.GetOrderByBillIdAsync(query.Request, bill.Id, Order.Client.EnumOrderStatus.Unknown);
                    if (!OrderClientHelper.IsSuccess(getOrdersResponse))
                    {
                        var error = OrderClientHelper.GetError(getOrdersResponse.Errors);

                        errors.Add(new ResponseError { Code = error.Code, Message = error.Message });
                        response = ResponseHelper.Error<SearchBillResponse>(errors);

                        return await Task.FromResult(response);
                    }
                    else
                    {
                        orders = getOrdersResponse.Data;
                    }

                    // Get courier's orders
                    ICollection<Courier.Client.GetOrderResponse> courierOrders = new List<Courier.Client.GetOrderResponse>();
                    foreach (var parcel in bill.Parcels)
                    {
                        var getCourierOrderResponse = await _courierManager.GetOrderByFilterAsync(query.Request, parcel.Id);
                        if (CourierClientHelper.IsSuccess(getCourierOrderResponse))
                        {
                            courierOrders.Add(getCourierOrderResponse.Data);
                        }
                    }

                    // Map
                    var item = SearchBillMapper.Map(baseUrl, bill, customer, orders, courierOrders, areas, preferences);

                    items.Add(item);
                }

                // Search
                items = BillHelper.Search(items, query.Payload.Keyword);

                // Sorting
                items = BillHelper.Sort(items, query.Payload.SortBy, query.Payload.SortDesc);

                // Get number of pages
                numberOfItems = items.Count < pageSize ? items.Count : numberOfItems;
                var numberOfpages = PageHelper.GetPages(numberOfItems, pageSize);

                // Build response
                data = SearchBillMapper.Map(page, pageSize, numberOfItems, numberOfpages, items);
                response = ResponseHelper.Ok(data);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);

                errors.Add(new ResponseError { Code = null, Message = ex.Message });
                response = ResponseHelper.Error<SearchBillResponse>(errors);
            }

            return await Task.FromResult(response);
        }

        public async Task<Response<GetBillSummaryResponse>> GetAsync(IConfiguration configuration, GetBillSummaryByFilterQuery query)
        {
            GetBillSummaryResponse data = null;
            List<ResponseError> errors = new List<ResponseError>();
            Response<GetBillSummaryResponse> response = null;

            try
            {
                // Get begin and end
                DateTime begin = DateTime.Now;
                DateTime end = DateTime.Now;

                if (query.Payload.Period == EnumPeriod.Recent)
                {
                    var lastBill = Repository.GetLatest(query.Payload.ChannelId, null, null, null);
                    if (lastBill != null)
                    {
                        begin = PeriodHelper.GetBegin(lastBill.CreatedOn);
                        end = PeriodHelper.GetEnd(lastBill.CreatedOn);
                    }
                }
                else if (query.Payload.Period == EnumPeriod.Custom)
                {
                    begin = PeriodHelper.GetBegin(query.Payload.Date);
                    end = PeriodHelper.GetEnd(query.Payload.Date);
                }
                else
                {
                    begin = PeriodHelper.GetBegin(query.Payload.Period);
                    end = PeriodHelper.GetEnd(query.Payload.Period);
                }

                // Count
                var all = Repository.Count(query.Payload.ChannelId, EnumSearchBillStatus.Unknown, begin, end);
                var pending = Repository.Count(query.Payload.ChannelId, EnumSearchBillStatus.Pending, begin, end);
                var notified = Repository.Count(query.Payload.ChannelId, EnumSearchBillStatus.Notified, begin, end);
                var rejected = Repository.Count(query.Payload.ChannelId, EnumSearchBillStatus.Rejected, begin, end);
                var confirmed = Repository.Count(query.Payload.ChannelId, EnumSearchBillStatus.Confirmed, begin, end);
                var deposited = Repository.CountDeposit(query.Payload.ChannelId, begin, end);
                var cod = Repository.CountCod(query.Payload.ChannelId, begin, end);
                var cancelled = Repository.Count(query.Payload.ChannelId, EnumSearchBillStatus.Cancelled, begin, end);
                var done = Repository.Count(query.Payload.ChannelId, EnumSearchBillStatus.Done, begin, end);
                var waitPrinting = Repository.Count(query.Payload.ChannelId, EnumSearchBillStatus.Printing, begin, end);
                var printed = Repository.Count(query.Payload.ChannelId, EnumSearchBillStatus.Printed, begin, end);
                var archived = Repository.Count(query.Payload.ChannelId, EnumSearchBillStatus.Archived, begin, end);

                // Build response
                data = new GetBillSummaryResponse
                {
                    All = all,
                    Pending = pending,
                    Notified = notified,
                    Rejected = rejected,
                    Confirmed = confirmed,
                    Deposited = deposited,
                    Cod = cod,
                    Cancelled = cancelled,
                    Done = done,
                    Printing = waitPrinting,
                    Printed = printed,
                    Archived = archived
                };
                response = ResponseHelper.Ok(data);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);

                errors.Add(new ResponseError { Code = null, Message = ex.Message });
                response = ResponseHelper.Error<GetBillSummaryResponse>(errors);
            }

            return await Task.FromResult(response);
        }

        public async Task<Response<DepositBillResponse>> UpdateAsync(IConfiguration configuration, DepositBillCommand command)
        {
            DepositBillResponse data = null;
            List<ResponseError> errors = new List<ResponseError>();
            Response<DepositBillResponse> response = null;

            try
            {
                // Validate
                await new DepositBillValidator().ValidateAndThrowAsync(command.Payload);

                // Get bill
                var bill = Repository.GetById(command.Payload.Id);
                if (bill == null)
                {
                    errors.Add(new ResponseError { Code = EnumErrorCode._102080001.GetDescription(), Message = ErrorMessage._102080001 });
                    response = ResponseHelper.Error<DepositBillResponse>(errors);

                    return await Task.FromResult(response);
                }

                // Get latest payment's status
                var latestPayment = _paymentRepository.GetLatestByBilId(bill.Id);
                if (latestPayment != null &&
                    latestPayment.PaymentStatus.Code == EnumPaymentStatus.Accepted.GetDescription() &&
                    !command.Payload.IsDeposit)
                {
                    // Automatic confirm bill
                    var billStatus = _billStatusRepository.GetByCode(EnumBillStatus.Confirmed.GetDescription());

                    bill.BillStatusId = billStatus.Id;
                    bill.UpdatedBy = command.Payload.UserId;
                    bill.UpdatedOn = DateTime.Now;

                    Repository.Update(bill);
                }

                // Deposit bill
                bill.IsDeposit = command.Payload.IsDeposit;
                bill.UpdatedBy = command.Payload.UserId;
                bill.UpdatedOn = DateTime.Now;

                Repository.Update(bill);

                // Build response
                data = new DepositBillResponse();
                response = ResponseHelper.Ok(data);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);

                errors.Add(new ResponseError { Code = null, Message = ex.Message });
                response = ResponseHelper.Error<DepositBillResponse>(errors);
            }

            return await Task.FromResult(response);
        }

        public async Task<Response<ConfirmBillResponse>> UpdateAsync(IConfiguration configuration, ConfirmBillCommand command)
        {
            ConfirmBillResponse data = null;
            List<ResponseError> errors = new List<ResponseError>();
            Response<ConfirmBillResponse> response = null;

            try
            {
                // Validate
                await new ConfirmBillValidator().ValidateAndThrowAsync(command.Payload);

                // Get bill
                var bill = Repository.GetById(command.Payload.Id);
                if (bill == null)
                {
                    errors.Add(new ResponseError { Code = EnumErrorCode._102080001.GetDescription(), Message = ErrorMessage._102080001 });
                    response = ResponseHelper.Error<ConfirmBillResponse>(errors);

                    return await Task.FromResult(response);
                }

                // Get orders
                ICollection<Order.Client.GetOrderResponse> orders = new List<Order.Client.GetOrderResponse>();
                var getOrdersResponse = await _orderManager.GetOrderByBillIdAsync(command.Request, bill.Id, Order.Client.EnumOrderStatus.Pending);
                if (!OrderClientHelper.IsSuccess(getOrdersResponse))
                {
                    var error = OrderClientHelper.GetError(getOrdersResponse.Errors);

                    errors.Add(new ResponseError { Code = error.Code, Message = error.Message });
                    response = ResponseHelper.Error<ConfirmBillResponse>(errors);

                    return await Task.FromResult(response);
                }
                else
                {
                    orders = getOrdersResponse.Data;
                }

                // Update stock
                if (!orders.IsEmpty())
                {
                    var items = new List<Inventory.Client.UpdateStockItem>();
                    foreach (var order in orders)
                    {
                        var item = new Inventory.Client.UpdateStockItem
                        {
                            Id = order.Product.Id,
                            Amount = -order.Amount
                        };
                        items.Add(item);
                    }

                    var updateStockResposne = await _inventoryManager.UpdateStockAsync(command.Request, items);
                    if (!InventoryClientHelper.IsSuccess(updateStockResposne))
                    {
                        var error = InventoryClientHelper.GetError(updateStockResposne.Errors);

                        errors.Add(new ResponseError { Code = error.Code, Message = error.Message });
                        response = ResponseHelper.Error<ConfirmBillResponse>(errors);

                        return await Task.FromResult(response);
                    }
                }

                // Update customer to regular
                var updateCustomerToRegularResponse = await _customerManager.RegularCustomerAsync(command.Request, bill.BillRecipient.CustomerId);
                if (!CustomerClientHelper.IsSuccess(updateCustomerToRegularResponse))
                {
                    var error = CustomerClientHelper.GetError(updateCustomerToRegularResponse.Errors);

                    errors.Add(new ResponseError { Code = error.Code, Message = error.Message });
                    response = ResponseHelper.Error<ConfirmBillResponse>(errors);

                    return await Task.FromResult(response);
                }

                // Confirm order
                var confirmOrderResponse = await _orderManager.ConfirmOrderAsync(command.Request, command.Payload.Id, command.Payload.UserId);
                if (!OrderClientHelper.IsSuccess(confirmOrderResponse))
                {
                    var error = OrderClientHelper.GetError(confirmOrderResponse.Errors);

                    errors.Add(new ResponseError { Code = error.Code, Message = error.Message });
                    response = ResponseHelper.Error<ConfirmBillResponse>(errors);

                    return await Task.FromResult(response);
                }

                // Update bill's status to confirmed
                var billStatus = _billStatusRepository.GetByCode(EnumBillStatus.Confirmed.GetDescription());

                bill.BillStatusId = billStatus.Id;
                bill.UpdatedBy = command.Payload.UserId;
                bill.UpdatedOn = DateTime.Now;

                Repository.Update(bill);

                // Send response message
                await _queueManager.NotifyBillConfirmAsync(bill.Id);

                // Build response
                data = new ConfirmBillResponse();
                response = ResponseHelper.Ok(data);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);

                errors.Add(new ResponseError { Code = null, Message = ex.Message });
                response = ResponseHelper.Error<ConfirmBillResponse>(errors);
            }

            return await Task.FromResult(response);
        }

        public async Task<Response<CancelBillResponse>> UpdateAsync(IConfiguration configuration, CancelBillCommand command)
        {
            CancelBillResponse data = null;
            List<ResponseError> errors = new List<ResponseError>();
            Response<CancelBillResponse> response = null;

            try
            {
                // Validate
                await new CancelBillValidator().ValidateAndThrowAsync(command.Payload);

                // Get bill
                var bill = Repository.GetById(command.Payload.Id);
                if (bill == null)
                {
                    errors.Add(new ResponseError { Code = EnumErrorCode._102080001.GetDescription(), Message = ErrorMessage._102080001 });
                    response = ResponseHelper.Error<CancelBillResponse>(errors);

                    return await Task.FromResult(response);
                }

                // Cancel deposit
                // Update bill's status to cancelled
                var billStatus = _billStatusRepository.GetByCode(EnumBillStatus.Cancelled.GetDescription());

                bill.IsDeposit = false;
                bill.BillStatusId = billStatus.Id;
                bill.UpdatedBy = command.Payload.UserId;
                bill.UpdatedOn = DateTime.Now;

                Repository.Update(bill);

                // Cancel order
                var cancelOrderResponse = await _orderManager.CancelOrderAsync(command.Request, command.Payload.Id, command.Payload.UserId);
                if (!OrderClientHelper.IsSuccess(cancelOrderResponse))
                {
                    var error = OrderClientHelper.GetError(cancelOrderResponse.Errors);

                    errors.Add(new ResponseError { Code = error.Code, Message = error.Message });
                    response = ResponseHelper.Error<CancelBillResponse>(errors);

                    return await Task.FromResult(response);
                }

                // Send response message
                await _queueManager.NotifyBillCancelAsync(bill.Id);

                // Build response
                data = new CancelBillResponse();
                response = ResponseHelper.Ok(data);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);

                errors.Add(new ResponseError { Code = null, Message = ex.Message });
                response = ResponseHelper.Error<CancelBillResponse>(errors);
            }

            return await Task.FromResult(response);
        }

        public async Task<Response<CancelBillsResponse>> UpdateAsync(IConfiguration configuration, CancelBillsCommand command)
        {
            CancelBillsResponse data = null;
            List<ResponseError> errors = new List<ResponseError>();
            Response<CancelBillsResponse> response = null;

            try
            {
                // Validate
                await new CancelBillsValidator().ValidateAndThrowAsync(command.Payload);

                foreach (var id in command.Payload.Ids)
                {
                    // Get bill
                    var bill = Repository.GetById(id);
                    if (bill == null)
                    {
                        errors.Add(new ResponseError { Code = EnumErrorCode._102080001.GetDescription(), Message = ErrorMessage._102080001 });
                        response = ResponseHelper.Error<CancelBillsResponse>(errors);

                        return await Task.FromResult(response);
                    }

                    // Cancel deposit
                    // Update bill's status to cancelled
                    var billStatus = _billStatusRepository.GetByCode(EnumBillStatus.Cancelled.GetDescription());

                    bill.IsDeposit = false;
                    bill.BillStatusId = billStatus.Id;
                    bill.UpdatedBy = command.Payload.UserId;
                    bill.UpdatedOn = DateTime.Now;

                    Repository.Update(bill);

                    // Cancel order
                    var cancelOrderResponse = await _orderManager.CancelOrderAsync(command.Request, id, command.Payload.UserId);
                    if (!OrderClientHelper.IsSuccess(cancelOrderResponse))
                    {
                        var error = OrderClientHelper.GetError(cancelOrderResponse.Errors);

                        errors.Add(new ResponseError { Code = error.Code, Message = error.Message });
                        response = ResponseHelper.Error<CancelBillsResponse>(errors);

                        return await Task.FromResult(response);
                    }

                    // Send response message
                    await _queueManager.NotifyBillCancelAsync(bill.Id);
                }

                // Build response
                data = new CancelBillsResponse();
                response = ResponseHelper.Ok(data);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);

                errors.Add(new ResponseError { Code = null, Message = ex.Message });
                response = ResponseHelper.Error<CancelBillsResponse>(errors);
            }

            return await Task.FromResult(response);
        }

        public async Task<Response<RenewBillResponse>> UpdateAsync(IConfiguration configuration, RenewBillCommand command)
        {
            RenewBillResponse data = null;
            List<ResponseError> errors = new List<ResponseError>();
            Response<RenewBillResponse> response = null;

            try
            {
                // Validate
                await new RenewBillValidator().ValidateAndThrowAsync(command.Payload);

                // Get bill
                var bill = Repository.GetById(command.Payload.Id);
                if (bill == null)
                {
                    errors.Add(new ResponseError { Code = EnumErrorCode._102080001.GetDescription(), Message = ErrorMessage._102080001 });
                    response = ResponseHelper.Error<RenewBillResponse>(errors);

                    return await Task.FromResult(response);
                }

                // Update bill's status to cancelled
                var billStatus = _billStatusRepository.GetByCode(EnumBillStatus.Pending.GetDescription());

                bill.BillStatusId = billStatus.Id;
                bill.UpdatedBy = command.Payload.UserId;
                bill.UpdatedOn = DateTime.Now;

                Repository.Update(bill);

                // Pending order
                var pendingOrderResponse = await _orderManager.PendingOrderAsync(command.Request, command.Payload.Id, command.Payload.UserId);
                if (!OrderClientHelper.IsSuccess(pendingOrderResponse))
                {
                    var error = OrderClientHelper.GetError(pendingOrderResponse.Errors);

                    errors.Add(new ResponseError { Code = error.Code, Message = error.Message });
                    response = ResponseHelper.Error<RenewBillResponse>(errors);

                    return await Task.FromResult(response);
                }

                // Build response
                data = new RenewBillResponse();
                response = ResponseHelper.Ok(data);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);

                errors.Add(new ResponseError { Code = null, Message = ex.Message });
                response = ResponseHelper.Error<RenewBillResponse>(errors);
            }

            return await Task.FromResult(response);
        }

        public async Task<Response<RenewBillsResponse>> UpdateAsync(IConfiguration configuration, RenewBillsCommand command)
        {
            RenewBillsResponse data = null;
            List<ResponseError> errors = new List<ResponseError>();
            Response<RenewBillsResponse> response = null;

            try
            {
                // Validate
                await new RenewBillsValidator().ValidateAndThrowAsync(command.Payload);

                // Get bills
                var bills = Repository.GetByIds(command.Payload.Ids);

                // Get bill's status
                var billStatus = _billStatusRepository.GetByCode(EnumBillStatus.Pending.GetDescription());

                foreach (var bill in bills)
                {
                    // Update bill's status to pending
                    bill.BillStatusId = billStatus.Id;
                    bill.UpdatedBy = command.Payload.UserId;
                    bill.UpdatedOn = DateTime.Now;

                    Repository.Update(bill);

                    // Pending order
                    var pendingOrderResponse = await _orderManager.PendingOrderAsync(command.Request, bill.Id, command.Payload.UserId);
                    if (!OrderClientHelper.IsSuccess(pendingOrderResponse))
                    {
                        var error = OrderClientHelper.GetError(pendingOrderResponse.Errors);

                        errors.Add(new ResponseError { Code = error.Code, Message = error.Message });
                        response = ResponseHelper.Error<RenewBillsResponse>(errors);

                        return await Task.FromResult(response);
                    }
                }

                // Build response
                data = new RenewBillsResponse();
                response = ResponseHelper.Ok(data);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);

                errors.Add(new ResponseError { Code = null, Message = ex.Message });
                response = ResponseHelper.Error<RenewBillsResponse>(errors);
            }

            return await Task.FromResult(response);
        }

        public async Task<Response<DoneBillResponse>> UpdateAsync(IConfiguration configuration, DoneBillCommand command)
        {
            DoneBillResponse data = null;
            List<ResponseError> errors = new List<ResponseError>();
            Response<DoneBillResponse> response = null;

            try
            {
                // Validate
                await new DoneBillValidator().ValidateAndThrowAsync(command.Payload);

                // Get bill
                var bill = Repository.GetById(command.Payload.Id);
                if (bill == null)
                {
                    errors.Add(new ResponseError { Code = EnumErrorCode._102080001.GetDescription(), Message = ErrorMessage._102080001 });
                    response = ResponseHelper.Error<DoneBillResponse>(errors);

                    return await Task.FromResult(response);
                }

                // Get orders
                ICollection<Order.Client.GetOrderResponse> orders = new List<Order.Client.GetOrderResponse>();
                var getOrdersResponse = await _orderManager.GetOrderByBillIdAsync(command.Request, bill.Id, Order.Client.EnumOrderStatus.Pending);
                if (!OrderClientHelper.IsSuccess(getOrdersResponse))
                {
                    var error = OrderClientHelper.GetError(getOrdersResponse.Errors);

                    errors.Add(new ResponseError { Code = error.Code, Message = error.Message });
                    response = ResponseHelper.Error<DoneBillResponse>(errors);

                    return await Task.FromResult(response);
                }
                else
                {
                    orders = getOrdersResponse.Data;
                }

                // Update stock
                if (!orders.IsEmpty())
                {
                    var items = new List<Inventory.Client.UpdateStockItem>();
                    foreach (var order in orders)
                    {
                        var item = new Inventory.Client.UpdateStockItem
                        {
                            Id = order.Product.Id,
                            Amount = -order.Amount
                        };
                        items.Add(item);
                    }

                    var updateStockResposne = await _inventoryManager.UpdateStockAsync(command.Request, items);
                    if (!InventoryClientHelper.IsSuccess(updateStockResposne))
                    {
                        var error = InventoryClientHelper.GetError(updateStockResposne.Errors);

                        errors.Add(new ResponseError { Code = error.Code, Message = error.Message });
                        response = ResponseHelper.Error<DoneBillResponse>(errors);

                        return await Task.FromResult(response);
                    }
                }

                // Update customer to regular
                var updateCustomerToRegularResponse = await _customerManager.RegularCustomerAsync(command.Request, bill.BillRecipient.CustomerId);
                if (!CustomerClientHelper.IsSuccess(updateCustomerToRegularResponse))
                {
                    var error = CustomerClientHelper.GetError(updateCustomerToRegularResponse.Errors);

                    errors.Add(new ResponseError { Code = error.Code, Message = error.Message });
                    response = ResponseHelper.Error<DoneBillResponse>(errors);

                    return await Task.FromResult(response);
                }

                // Confirm order
                var confirmOrderResponse = await _orderManager.ConfirmOrderAsync(command.Request, command.Payload.Id, command.Payload.UserId);
                if (!OrderClientHelper.IsSuccess(confirmOrderResponse))
                {
                    var error = OrderClientHelper.GetError(confirmOrderResponse.Errors);

                    errors.Add(new ResponseError { Code = error.Code, Message = error.Message });
                    response = ResponseHelper.Error<DoneBillResponse>(errors);

                    return await Task.FromResult(response);
                }

                // Cancel deposit
                // Update bill's status to done
                var billStatus = _billStatusRepository.GetByCode(EnumBillStatus.Done.GetDescription());

                bill.IsDeposit = false;
                bill.BillStatusId = billStatus.Id;
                bill.UpdatedBy = command.Payload.UserId;
                bill.UpdatedOn = DateTime.Now;

                Repository.Update(bill);

                // Build response
                data = new DoneBillResponse();
                response = ResponseHelper.Ok(data);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);

                errors.Add(new ResponseError { Code = null, Message = ex.Message });
                response = ResponseHelper.Error<DoneBillResponse>(errors);
            }

            return await Task.FromResult(response);
        }

        public async Task<Response<ArchiveBillResponse>> UpdateAsync(IConfiguration configuration, ArchiveBillCommand command)
        {
            ArchiveBillResponse data = null;
            List<ResponseError> errors = new List<ResponseError>();
            Response<ArchiveBillResponse> response = null;

            try
            {
                // Validate
                await new ArchiveBillValidator().ValidateAndThrowAsync(command.Payload);

                // Get bill
                var bill = Repository.GetById(command.Payload.Id);
                if (bill == null)
                {
                    errors.Add(new ResponseError { Code = EnumErrorCode._102080001.GetDescription(), Message = ErrorMessage._102080001 });
                    response = ResponseHelper.Error<ArchiveBillResponse>(errors);

                    return await Task.FromResult(response);
                }

                // Update bill's status to archived
                var billStatus = _billStatusRepository.GetByCode(EnumBillStatus.Archived.GetDescription());

                bill.BillStatusId = billStatus.Id;
                bill.UpdatedBy = command.Payload.UserId;
                bill.UpdatedOn = DateTime.Now;

                Repository.Update(bill);

                // Build response
                data = new ArchiveBillResponse();
                response = ResponseHelper.Ok(data);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);

                errors.Add(new ResponseError { Code = null, Message = ex.Message });
                response = ResponseHelper.Error<ArchiveBillResponse>(errors);
            }

            return await Task.FromResult(response);
        }

        public async Task<Response<ArchiveBillsResponse>> UpdateAsync(IConfiguration configuration, ArchiveBillsCommand command)
        {
            ArchiveBillsResponse data = null;
            List<ResponseError> errors = new List<ResponseError>();
            Response<ArchiveBillsResponse> response = null;

            try
            {
                // Validate
                await new ArchiveBillsValidator().ValidateAndThrowAsync(command.Payload);

                // Get bills
                var bills = Repository.GetByIds(command.Payload.Ids);

                // Update bill's status to archived
                var billStatus = _billStatusRepository.GetByCode(EnumBillStatus.Archived.GetDescription());
                foreach (var bill in bills)
                {
                    bill.BillStatusId = billStatus.Id;
                    bill.UpdatedBy = command.Payload.UserId;
                    bill.UpdatedOn = DateTime.Now;
                }

                Repository.Updates(bills);

                // Build response
                data = new ArchiveBillsResponse();
                response = ResponseHelper.Ok(data);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);

                errors.Add(new ResponseError { Code = null, Message = ex.Message });
                response = ResponseHelper.Error<ArchiveBillsResponse>(errors);
            }

            return await Task.FromResult(response);
        }
    }
}