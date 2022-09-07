using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using SC.App.Services.Bill.Business.Commands.BillTag;
using SC.App.Services.Bill.Business.Enums;
using SC.App.Services.Bill.Business.Managers.Interface;
using SC.App.Services.Bill.Business.Mappers;
using SC.App.Services.Bill.Business.Repositories.Interface;
using SC.App.Services.Bill.Business.Resources;
using SC.App.Services.Bill.Business.Validators.BillTag;
using SC.App.Services.Bill.Common.Helpers;
using SC.App.Services.Bill.Common.Managers;
using SC.App.Services.Bill.Common.Responses;
using SC.App.Services.Bill.Lib.Extensions;
using SC.App.Services.Bill.Lib.Helpers;
using Serilog;

namespace SC.App.Services.Bill.Business.Managers
{
    public class BillTagManager : BaseManager<IBillTagRepository>, IBillTagManager
    {
        public BillTagManager(IBillTagRepository repository)
            : base(repository)
        {
        }

        public async Task<Response<CreateBillTagResponse>> CreateAsync(IConfiguration configuration, CreateBillTagCommand command)
        {
            CreateBillTagResponse data = null;
            List<ResponseError> errors = new List<ResponseError>();
            Response<CreateBillTagResponse> response = null;

            try
            {
                // Validate
                await new CreateBillTagValidator().ValidateAndThrowAsync(command.Payload);

                // Get bill's tag
                var billTag = Repository.GetByUniqueKey(command.Payload.BillId, command.Payload.TagId);
                if (billTag != null)
                {
                    errors.Add(new ResponseError { Code = EnumErrorCode._102080044.GetDescription(), Message = ErrorMessage._102080044 });
                    response = ResponseHelper.Error<CreateBillTagResponse>(errors);

                    return await Task.FromResult(response);
                }

                // Save
                var newBillTagId = GuidHelper.Generate($"{command.Payload.BillId}_{command.Payload.TagId}");
                billTag = BillTagMapper.Create(newBillTagId, command.Payload);
                _ = Repository.Add(billTag);

                // Build response
                data = new CreateBillTagResponse(billTag.Id);
                response = ResponseHelper.Ok(data);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);

                errors.Add(new ResponseError { Code = null, Message = ex.Message });
                response = ResponseHelper.Error<CreateBillTagResponse>(errors);
            }

            return await Task.FromResult(response);
        }

        public async Task<Response<UpdateBillTagResponse>> UpdateAsync(IConfiguration configuration, UpdateBillTagCommand command)
        {
            UpdateBillTagResponse data = null;
            List<ResponseError> errors = new List<ResponseError>();
            Response<UpdateBillTagResponse> response = null;

            try
            {
                // Validate
                await new UpdateBillTagValidator().ValidateAndThrowAsync(command.Payload);

                // Get bill's tags
                var billTags = Repository.GetByBillId(command.Payload.BillId);

                // Delete
                var tagIds = command.Payload.TagIds;
                foreach (var billTag in billTags)
                {
                    var foundTag = tagIds
                        .Any(x => x == billTag.TagId);
                    if (!foundTag)
                    {
                        var exist = Repository.GetByUniqueKey(command.Payload.BillId, billTag.TagId);
                        Repository.Remove(exist);
                    }

                    tagIds.Remove(billTag.TagId);
                }

                // Add
                foreach (var tagId in tagIds)
                {
                    var newBillTagId = GuidHelper.Generate($"{command.Payload.BillId}_{tagId}");
                    var billTag = BillTagMapper.Update(newBillTagId, tagId, command.Payload);
                    _ = Repository.Add(billTag);
                }

                // Build response
                data = new UpdateBillTagResponse();
                response = ResponseHelper.Ok(data);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);

                errors.Add(new ResponseError { Code = null, Message = ex.Message });
                response = ResponseHelper.Error<UpdateBillTagResponse>(errors);
            }

            return await Task.FromResult(response);
        }

        public async Task<Response<DeleteBillTagResponse>> DeleteAsync(IConfiguration configuration, DeleteBillTagCommand command)
        {
            DeleteBillTagResponse data = null;
            List<ResponseError> errors = new List<ResponseError>();
            Response<DeleteBillTagResponse> response = null;

            try
            {
                // Get bill tag
                var billTag = Repository.GetById(command.Payload.Id);
                if (billTag == null)
                {
                    errors.Add(new ResponseError { Code = EnumErrorCode._102080043.GetDescription(), Message = ErrorMessage._102080043 });
                    response = ResponseHelper.Error<DeleteBillTagResponse>(errors);

                    return await Task.FromResult(response);
                }

                // Delete
                Repository.Remove(billTag);

                // Build response
                data = new DeleteBillTagResponse();
                response = ResponseHelper.Ok(data);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);

                errors.Add(new ResponseError { Code = null, Message = ex.Message });
                response = ResponseHelper.Error<DeleteBillTagResponse>(errors);
            }

            return await Task.FromResult(response);
        }
    }
}