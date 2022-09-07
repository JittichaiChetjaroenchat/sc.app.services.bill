using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using SC.App.Services.Bill.Business.Commands.Tag;
using SC.App.Services.Bill.Business.Enums;
using SC.App.Services.Bill.Business.Managers.Interface;
using SC.App.Services.Bill.Business.Mappers;
using SC.App.Services.Bill.Business.Queries.Tag;
using SC.App.Services.Bill.Business.Repositories.Interface;
using SC.App.Services.Bill.Business.Resources;
using SC.App.Services.Bill.Business.Validators.Tag;
using SC.App.Services.Bill.Common.Helpers;
using SC.App.Services.Bill.Common.Managers;
using SC.App.Services.Bill.Common.Responses;
using SC.App.Services.Bill.Lib.Extensions;
using SC.App.Services.Bill.Lib.Helpers;
using Serilog;

namespace SC.App.Services.Bill.Business.Managers
{
    public class TagManager : BaseManager<ITagRepository>, ITagManager
    {
        public TagManager(ITagRepository repository)
            : base(repository)
        {
        }

        public async Task<Response<GetTagResponse>> GetAsync(IConfiguration configuration, GetTagByIdQuery query)
        {
            GetTagResponse data = null;
            List<ResponseError> errors = new List<ResponseError>();
            Response<GetTagResponse> response = null;

            try
            {
                // Get tag
                var tag = Repository.GetById(query.Payload.Id);
                if (tag == null)
                {
                    errors.Add(new ResponseError { Code = EnumErrorCode._102080041.GetDescription(), Message = ErrorMessage._102080041 });
                    response = ResponseHelper.Error<GetTagResponse>(errors);

                    return await Task.FromResult(response);
                }

                // Build response
                data = TagMapper.Map(tag);
                response = ResponseHelper.Ok(data);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);

                errors.Add(new ResponseError { Code = null, Message = ex.Message });
                response = ResponseHelper.Error<GetTagResponse>(errors);
            }

            return await Task.FromResult(response);
        }

        public async Task<Response<List<GetTagResponse>>> GetAsync(IConfiguration configuration, GetTagByFilterQuery query)
        {
            List<GetTagResponse> data = null;
            List<ResponseError> errors = new List<ResponseError>();
            Response<List<GetTagResponse>> response = null;

            try
            {
                // Get tags
                var tags = Repository.GetByChannelId(query.Payload.ChannelId);

                // Build response
                data = TagMapper.Map(tags);
                response = ResponseHelper.Ok(data);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);

                errors.Add(new ResponseError { Code = null, Message = ex.Message });
                response = ResponseHelper.Error<List<GetTagResponse>>(errors);
            }

            return await Task.FromResult(response);
        }

        public async Task<Response<CreateTagResponse>> CreateAsync(IConfiguration configuration, CreateTagCommand command)
        {
            CreateTagResponse data = null;
            List<ResponseError> errors = new List<ResponseError>();
            Response<CreateTagResponse> response = null;

            try
            {
                // Validate
                await new CreateTagValidator().ValidateAndThrowAsync(command.Payload);

                // Get tag
                var tag = Repository.GetByUniqueKey(command.Payload.ChannelId, command.Payload.Name);
                if (tag != null)
                {
                    errors.Add(new ResponseError { Code = EnumErrorCode._102080042.GetDescription(), Message = ErrorMessage._102080042 });
                    response = ResponseHelper.Error<CreateTagResponse>(errors);

                    return await Task.FromResult(response);
                }

                // Save
                var newTagId = GuidHelper.Generate($"{command.Payload.ChannelId}_{command.Payload.Name}");
                tag = TagMapper.Create(newTagId, command.Payload);
                _ = Repository.Add(tag);

                // Build response
                data = new CreateTagResponse(tag.Id);
                response = ResponseHelper.Ok(data);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);

                errors.Add(new ResponseError { Code = null, Message = ex.Message });
                response = ResponseHelper.Error<CreateTagResponse>(errors);
            }

            return await Task.FromResult(response);
        }

        public async Task<Response<UpdateTagResponse>> UpdateAsync(IConfiguration configuration, UpdateTagCommand command)
        {
            UpdateTagResponse data = null;
            List<ResponseError> errors = new List<ResponseError>();
            Response<UpdateTagResponse> response = null;

            try
            {
                // Validate
                await new UpdateTagValidator().ValidateAndThrowAsync(command.Payload);

                // Get tag
                var tag = Repository.GetById(command.Payload.Id);
                if (tag == null)
                {
                    errors.Add(new ResponseError { Code = EnumErrorCode._102080041.GetDescription(), Message = ErrorMessage._102080041 });
                    response = ResponseHelper.Error<UpdateTagResponse>(errors);

                    return await Task.FromResult(response);
                }

                // Update
                TagMapper.Update(command.Payload, tag);
                Repository.Update(tag);

                // Build response
                data = new UpdateTagResponse();
                response = ResponseHelper.Ok(data);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);

                errors.Add(new ResponseError { Code = null, Message = ex.Message });
                response = ResponseHelper.Error<UpdateTagResponse>(errors);
            }

            return await Task.FromResult(response);
        }

        public async Task<Response<DeleteTagByIdResponse>> DeleteAsync(IConfiguration configuration, DeleteTagByIdCommand command)
        {
            DeleteTagByIdResponse data = null;
            List<ResponseError> errors = new List<ResponseError>();
            Response<DeleteTagByIdResponse> response = null;

            try
            {
                // Validate
                await new DeleteTagByIdValidator().ValidateAndThrowAsync(command.Payload);

                // Get tag
                var tag = Repository.GetById(command.Payload.Id);
                if (tag == null)
                {
                    errors.Add(new ResponseError { Code = EnumErrorCode._102080041.GetDescription(), Message = ErrorMessage._102080041 });
                    response = ResponseHelper.Error<DeleteTagByIdResponse>(errors);

                    return await Task.FromResult(response);
                }

                // Delete
                Repository.Remove(tag);

                // Build response
                data = new DeleteTagByIdResponse();
                response = ResponseHelper.Ok(data);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);

                errors.Add(new ResponseError { Code = null, Message = ex.Message });
                response = ResponseHelper.Error<DeleteTagByIdResponse>(errors);
            }

            return await Task.FromResult(response);
        }

        public async Task<Response<DeleteTagByIdsResponse>> DeleteAsync(IConfiguration configuration, DeleteTagByIdsCommand command)
        {
            DeleteTagByIdsResponse data = null;
            List<ResponseError> errors = new List<ResponseError>();
            Response<DeleteTagByIdsResponse> response = null;

            try
            {
                // Validate
                await new DeleteTagByIdsValidator().ValidateAndThrowAsync(command.Payload);

                // Get tags
                var tags = Repository.GetByIds(command.Payload.Ids);
                if (tags.IsEmpty())
                {
                    errors.Add(new ResponseError { Code = EnumErrorCode._102080041.GetDescription(), Message = ErrorMessage._102080041 });
                    response = ResponseHelper.Error<DeleteTagByIdsResponse>(errors);

                    return await Task.FromResult(response);
                }

                // Delete
                Repository.Removes(tags.ToArray());

                // Build response
                data = new DeleteTagByIdsResponse();
                response = ResponseHelper.Ok(data);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);

                errors.Add(new ResponseError { Code = null, Message = ex.Message });
                response = ResponseHelper.Error<DeleteTagByIdsResponse>(errors);
            }

            return await Task.FromResult(response);
        }
    }
}