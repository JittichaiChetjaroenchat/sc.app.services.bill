using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using SC.App.Services.Bill.Business.Commands.BillConfiguration;
using SC.App.Services.Bill.Business.Enums;
using SC.App.Services.Bill.Business.Managers.Interface;
using SC.App.Services.Bill.Business.Mappers;
using SC.App.Services.Bill.Business.Queries.BillConfiguration;
using SC.App.Services.Bill.Business.Repositories.Interface;
using SC.App.Services.Bill.Business.Resources;
using SC.App.Services.Bill.Business.Validators.BillConfiguration;
using SC.App.Services.Bill.Common.Helpers;
using SC.App.Services.Bill.Common.Managers;
using SC.App.Services.Bill.Common.Responses;
using SC.App.Services.Bill.Database.Models;
using SC.App.Services.Bill.Lib.Extensions;
using Serilog;

namespace SC.App.Services.Bill.Business.Managers
{
    public class BillConfigurationManager : BaseManager<IBillConfigurationRepository>, IBillConfigurationManager
    {
        public BillConfigurationManager(IBillConfigurationRepository repository)
            : base(repository)
        {
        }

        public async Task<Response<CreateBillConfigurationResponse>> CreateAsync(IConfiguration configuration, CreateBillConfigurationCommand command)
        {
            CreateBillConfigurationResponse data = null;
            List<ResponseError> errors = new List<ResponseError>();
            Response<CreateBillConfigurationResponse> response = null;

            try
            {
                // Validate
                await new CreateBillConfigurationValidator().ValidateAndThrowAsync(command.Payload);

                // Get bill's configuration
                var billConfiguration = Repository.GetByChannelId(command.Payload.ChannelId);
                if (billConfiguration == null)
                {
                    // Create
                    billConfiguration = new BillConfiguration
                    {
                        ChannelId = command.Payload.ChannelId,
                        CurrentNo = 0,
                        CreatedBy = command.Payload.UserId
                    };

                    _ = Repository.Add(billConfiguration);
                }

                // Build response
                data = new CreateBillConfigurationResponse(billConfiguration.Id);
                response = ResponseHelper.Ok(data);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);

                errors.Add(new ResponseError { Code = null, Message = ex.Message });
                response = ResponseHelper.Error<CreateBillConfigurationResponse>(errors);
            }

            return await Task.FromResult(response);
        }

        public async Task<Response<GetBillConfigurationResponse>> GetAsync(IConfiguration configuration, GetBillConfigurationByFilterQuery query)
        {
            GetBillConfigurationResponse data = null;
            List<ResponseError> errors = new List<ResponseError>();
            Response<GetBillConfigurationResponse> response = null;

            try
            {
                var billConfiuration = Repository.GetByChannelId(query.Payload.ChannelId);
                if (billConfiuration == null)
                {
                    errors.Add(new ResponseError { Code = EnumErrorCode._102080006.GetDescription(), Message = ErrorMessage._102080006 });
                    response = ResponseHelper.Error<GetBillConfigurationResponse>(errors);

                    return await Task.FromResult(response);
                }

                // Build response
                data = BillConfigurationMapper.Map(billConfiuration);
                response = ResponseHelper.Ok(data);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);

                errors.Add(new ResponseError { Code = null, Message = ex.Message });
                response = ResponseHelper.Error<GetBillConfigurationResponse>(errors);
            }

            return await Task.FromResult(response);
        }

        public void IncreaseCurrentNo(Guid channelId)
        {
            var billConfiguration = Repository.GetByChannelId(channelId);
            if (billConfiguration != null)
            {
                billConfiguration.CurrentNo = billConfiguration.CurrentNo + 1;
                Repository.Update(billConfiguration);
            }
        }
    }
}