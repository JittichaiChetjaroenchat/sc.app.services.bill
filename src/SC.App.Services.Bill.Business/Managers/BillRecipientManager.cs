using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using SC.App.Services.Bill.Business.Commands.BillRecipient;
using SC.App.Services.Bill.Business.Enums;
using SC.App.Services.Bill.Business.Managers.Interface;
using SC.App.Services.Bill.Business.Mappers;
using SC.App.Services.Bill.Business.Repositories.Interface;
using SC.App.Services.Bill.Business.Resources;
using SC.App.Services.Bill.Business.Validators.BillRecipient;
using SC.App.Services.Bill.Common.Helpers;
using SC.App.Services.Bill.Common.Responses;
using SC.App.Services.Bill.Lib.Extensions;
using Serilog;

namespace SC.App.Services.Bill.Business.Managers
{
    public class BillRecipientManager : IBillRecipientManager
    {
        private readonly IBillRecipientRepository _billRecipientRepository;
        private readonly IBillRecipientContactRepository _billRecipientContactRepository;

        public BillRecipientManager(
            IBillRecipientRepository billRecipientRepository,
            IBillRecipientContactRepository billRecipientContactRepository)
        {
            _billRecipientRepository = billRecipientRepository;
            _billRecipientContactRepository = billRecipientContactRepository;
        }
        
        public async Task<Response<UpdateBillRecipientResponse>> UpdateAsync(IConfiguration configuration, UpdateBillRecipientCommand command)
        {
            UpdateBillRecipientResponse data = null;
            List<ResponseError> errors = new List<ResponseError>();
            Response<UpdateBillRecipientResponse> response = null;

            try
            {
                // Validate
                await new UpdateBillRecipientValidator().ValidateAndThrowAsync(command.Payload);

                // Get bill recipient
                var billRecipient = _billRecipientRepository.GetById(command.Payload.Id);
                if (billRecipient == null)
                {
                    errors.Add(new ResponseError { Code = EnumErrorCode._102080004.GetDescription(), Message = ErrorMessage._102080004 });
                    response = ResponseHelper.Error<UpdateBillRecipientResponse>(errors);

                    return await Task.FromResult(response);
                }

                // Update bill recipient
                billRecipient = BillRecipientMapper.Map(billRecipient, command.Payload);
                _billRecipientRepository.Update(billRecipient);

                // Get bill recipient's contact
                var billRecipientContact = _billRecipientContactRepository.GetByBillRecipientId(billRecipient.Id);
                if (billRecipientContact == null)
                {
                    // Create
                    billRecipientContact = BillRecipientContactMapper.Map(billRecipient.Id, command.Payload.Contact, command.Payload.UserId);
                    _billRecipientContactRepository.Add(billRecipientContact);
                }
                else
                {
                    // Update
                    billRecipientContact = BillRecipientContactMapper.Map(billRecipientContact, command.Payload.Contact, command.Payload.UserId);
                    _billRecipientContactRepository.Update(billRecipientContact);
                }

                // Build response
                data = new UpdateBillRecipientResponse();
                response = ResponseHelper.Ok(data);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);

                errors.Add(new ResponseError { Code = null, Message = ex.Message });
                response = ResponseHelper.Error<UpdateBillRecipientResponse>(errors);
            }

            return await Task.FromResult(response);
        }
    }
}