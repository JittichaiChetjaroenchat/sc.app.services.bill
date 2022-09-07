using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using SC.App.Services.Bill.Business.Commands.BillNotification;
using SC.App.Services.Bill.Business.Enums;
using SC.App.Services.Bill.Business.Helpers;
using SC.App.Services.Bill.Business.Managers.Interface;
using SC.App.Services.Bill.Business.Mappers;
using SC.App.Services.Bill.Business.Queries.BillNotification;
using SC.App.Services.Bill.Business.Repositories.Interface;
using SC.App.Services.Bill.Business.Resources;
using SC.App.Services.Bill.Business.Validators.BillNotification;
using SC.App.Services.Bill.Client.Customer;
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
    public class BillNotificationManager : BaseManager<IBillNotificationRepository>, IBillNotificationManager
    {
        private readonly ICustomerManager _customerManager;
        private readonly ISettingManager _settingManager;
        private readonly IQueueManager _queueManager;

        public BillNotificationManager(
            IBillNotificationRepository repository,
            ICustomerManager customerManager,
            ISettingManager settingManager,
            IQueueManager queueManager)
            : base(repository)
        {
            _customerManager = customerManager;
            _settingManager = settingManager;
            _queueManager = queueManager;
        }

        public async Task<Response<GetBillNotificationResponse>> GetAsync(IConfiguration configuration, GetBillNotificationByIdQuery query)
        {
            GetBillNotificationResponse data = null;
            List<ResponseError> errors = new List<ResponseError>();
            Response<GetBillNotificationResponse> response = null;

            try
            {
                // Get bill notification
                var billNotification = Repository.GetById(query.Payload.Id);
                if (billNotification == null)
                {
                    errors.Add(new ResponseError { Code = EnumErrorCode._102080049.GetDescription(), Message = ErrorMessage._102080049 });
                    response = ResponseHelper.Error<GetBillNotificationResponse>(errors);

                    return await Task.FromResult(response);
                }

                // Build response
                data = BillNotificationMapper.Map(billNotification);
                response = ResponseHelper.Ok(data);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);

                errors.Add(new ResponseError { Code = null, Message = ex.Message });
                response = ResponseHelper.Error<GetBillNotificationResponse>(errors);
            }

            return await Task.FromResult(response);
        }

        public async Task<Response<SearchBillNotificationResponse>> GetAsync(IConfiguration configuration, SearchBillNotificationByFilterQuery query)
        {
            SearchBillNotificationResponse data = null;
            List<ResponseError> errors = new List<ResponseError>();
            Response<SearchBillNotificationResponse> response = null;

            try
            {
                // Validate
                await new SearchBillNotificationByFilterValidator().ValidateAndThrowAsync(query.Payload);

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

                // Get bill's notifications
                var billNotifications = Repository.Search(query.Payload.ChannelId, query.Payload.Status, begin, end, query.Payload.Keyword, query.Payload.SortBy, query.Payload.SortDesc, page, pageSize);

                // Get buyer's base url
                var baseUrl = configuration.GetValue<string>(AppSettings.Applications.Buyer.BaseUrl);

                // Get items
                var items = new List<SearchBillNotificationItem>();
                foreach (var billNotification in billNotifications)
                {
                    // Get customer
                    Customer.Client.GetCustomerResponse customer = null;
                    var getCustomerByIdResponse = await _customerManager.GetCustomerByIdAsync(query.Request, billNotification.Bill.BillRecipient.CustomerId);
                    if (!CustomerClientHelper.IsSuccess(getCustomerByIdResponse))
                    {
                        var error = CustomerClientHelper.GetError(getCustomerByIdResponse.Errors);

                        errors.Add(new ResponseError { Code = error.Code, Message = error.Message });
                        response = ResponseHelper.Error<SearchBillNotificationResponse>(errors);

                        return await Task.FromResult(response);
                    }
                    else
                    {
                        customer = getCustomerByIdResponse.Data;
                    }

                    // Get preferences
                    Setting.Client.GetPreferencesResponse preferences = null;
                    var getPreferencesResponse = await _settingManager.GetPreferencesByChannelIdAsync(query.Request, billNotification.Bill.ChannelId);
                    if (!SettingClientHelper.IsSuccess(getPreferencesResponse))
                    {
                        var error = SettingClientHelper.GetError(getPreferencesResponse.Errors);

                        errors.Add(new ResponseError { Code = error.Code, Message = error.Message });
                        response = ResponseHelper.Error<SearchBillNotificationResponse>(errors);

                        return await Task.FromResult(response);
                    }
                    else
                    {
                        preferences = getPreferencesResponse.Data;
                    }

                    // Map
                    var item = SearchBillNotificationMapper.Map(baseUrl, billNotification, customer, preferences);

                    items.Add(item);
                }

                // Search
                items = BillNotificationHelper.Search(items, query.Payload.Keyword);

                // Sorting
                items = BillNotificationHelper.Sort(items, query.Payload.SortBy, query.Payload.SortDesc);

                // Get number of pages
                numberOfItems = items.Count < pageSize ? items.Count : numberOfItems;
                var numberOfpages = PageHelper.GetPages(numberOfItems, pageSize);

                // Build response
                data = SearchBillNotificationMapper.Map(page, pageSize, numberOfItems, numberOfpages, items);
                response = ResponseHelper.Ok(data);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);

                errors.Add(new ResponseError { Code = null, Message = ex.Message });
                response = ResponseHelper.Error<SearchBillNotificationResponse>(errors);
            }

            return await Task.FromResult(response);
        }

        public async Task<Response<GetBillNotificationSummaryResponse>> GetAsync(IConfiguration configuration, GetBillNotificationSummaryByFilterQuery query)
        {
            GetBillNotificationSummaryResponse data = null;
            List<ResponseError> errors = new List<ResponseError>();
            Response<GetBillNotificationSummaryResponse> response = null;

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
                var all = Repository.Count(query.Payload.ChannelId, EnumSearchBillNotificationStatus.Unknown, begin, end);
                var sentSummary = Repository.Count(query.Payload.ChannelId, EnumSearchBillNotificationStatus.SentSummary, begin, end);
                var unsentSummary = Repository.Count(query.Payload.ChannelId, EnumSearchBillNotificationStatus.UnsentSummary, begin, end);

                // Build response
                data = new GetBillNotificationSummaryResponse
                {
                    All = all,
                    SentSummary = sentSummary,
                    UnsentSummary = unsentSummary
                };
                response = ResponseHelper.Ok(data);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);

                errors.Add(new ResponseError { Code = null, Message = ex.Message });
                response = ResponseHelper.Error<GetBillNotificationSummaryResponse>(errors);
            }

            return await Task.FromResult(response);
        }

        public async Task<Response<NotifyBillSummaryResponse>> CreateAsync(IConfiguration configuration, NotifyBillSummaryCommand command)
        {
            NotifyBillSummaryResponse data = null;
            List<ResponseError> errors = new List<ResponseError>();
            Response<NotifyBillSummaryResponse> response = null;

            try
            {
                foreach (var id in command.Payload.Id)
                {
                    // Get bill notification
                    var billNotification = Repository.GetById(id);
                    if (billNotification == null)
                    {
                        errors.Add(new ResponseError { Code = EnumErrorCode._102080049.GetDescription(), Message = ErrorMessage._102080049 });
                        response = ResponseHelper.Error<NotifyBillSummaryResponse>(errors);

                        return await Task.FromResult(response);
                    }

                    // Send response message
                    await _queueManager.NotifyBillSummaryAsync(billNotification.BillId);
                }

                // Build response
                data = new NotifyBillSummaryResponse();
                response = ResponseHelper.Ok(data);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);

                errors.Add(new ResponseError { Code = null, Message = ex.Message });
                response = ResponseHelper.Error<NotifyBillSummaryResponse>(errors);
            }

            return await Task.FromResult(response);
        }
    }
}