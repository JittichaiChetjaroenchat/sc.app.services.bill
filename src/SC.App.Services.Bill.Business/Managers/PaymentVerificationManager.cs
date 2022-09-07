using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SC.App.Services.Bill.Business.Commands.PaymentVerification;
using SC.App.Services.Bill.Business.Enums;
using SC.App.Services.Bill.Business.Managers.Interface;
using SC.App.Services.Bill.Business.Mappers;
using SC.App.Services.Bill.Business.Queries.PaymentVerification;
using SC.App.Services.Bill.Business.Repositories.Interface;
using SC.App.Services.Bill.Business.Resources;
using SC.App.Services.Bill.Client.Setting;
using SC.App.Services.Bill.Common.Helpers;
using SC.App.Services.Bill.Common.Managers;
using SC.App.Services.Bill.Common.Responses;
using SC.App.Services.Bill.Lib.Extensions;
using SC.App.Services.Bill.Queue.Managers.Interface;
using Serilog;

namespace SC.App.Services.Bill.Business.Managers
{
    public class PaymentVerificationManager : BaseManager<IPaymentVerificationRepository>, IPaymentVerificationManager
    {
        private readonly ISettingManager _settingManager;
        private readonly IQueueManager _queueManager;

        public PaymentVerificationManager(
            IPaymentVerificationRepository repository,

            ISettingManager settingManager,
            IQueueManager queueManager)
            : base(repository)
        {
            _settingManager = settingManager;
            _queueManager = queueManager;
        }

        public async Task<Response<GetPaymentVerificationResponse>> GetAsync(IConfiguration configuration, GetPaymentVerificationByIdQuery query)
        {
            GetPaymentVerificationResponse data = null;
            List<ResponseError> errors = new List<ResponseError>();
            Response<GetPaymentVerificationResponse> response = null;

            try
            {
                // Get payment verification
                var paymentVerification = Repository.GetById(query.Payload.Id);
                if (paymentVerification == null)
                {
                    errors.Add(new ResponseError { Code = EnumErrorCode._102080045.GetDescription(), Message = ErrorMessage._102080045 });
                    response = ResponseHelper.Error<GetPaymentVerificationResponse>(errors);

                    return await Task.FromResult(response);
                }

                // Build response
                data = PaymentVerificationMapper.Map(paymentVerification);
                response = ResponseHelper.Ok(data);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);

                errors.Add(new ResponseError { Code = null, Message = ex.Message });
                response = ResponseHelper.Error<GetPaymentVerificationResponse>(errors);
            }

            return await Task.FromResult(response);
        }

        public async Task<Response<VerifyPaymentResponse>> CreateAsync(IConfiguration configuration, VerifyPaymentCommand command)
        {
            VerifyPaymentResponse data = null;
            List<ResponseError> errors = new List<ResponseError>();
            Response<VerifyPaymentResponse> response = null;

            try
            {
                // Get payment verification
                var paymentVerification = Repository.GetById(command.Payload.PaymentVerificationId);
                if (paymentVerification == null)
                {
                    errors.Add(new ResponseError { Code = EnumErrorCode._102080045.GetDescription(), Message = ErrorMessage._102080045 });
                    response = ResponseHelper.Error<VerifyPaymentResponse>(errors);

                    return await Task.FromResult(response);
                }

                // Get billing
                Setting.Client.GetBillingResponse billing = null;
                var getBillingResponse = await _settingManager.GetBillingByChannelIdAsync(command.Request, paymentVerification.Payment.Bill.ChannelId);
                if (!SettingClientHelper.IsSuccess(getBillingResponse))
                {
                    var error = SettingClientHelper.GetError(getBillingResponse.Errors);

                    errors.Add(new ResponseError { Code = error.Code, Message = error.Message });
                    response = ResponseHelper.Error<VerifyPaymentResponse>(errors);

                    return await Task.FromResult(response);
                }
                else
                {
                    billing = getBillingResponse.Data;
                }

                // Queue for verify slip
                await _queueManager.VerifyPaymentAsync(paymentVerification.PaymentId);

                // Build response
                data = new VerifyPaymentResponse();
                response = ResponseHelper.Ok(data);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);

                errors.Add(new ResponseError { Code = null, Message = ex.Message });
                response = ResponseHelper.Error<VerifyPaymentResponse>(errors);
            }

            return await Task.FromResult(response);
        }
    }
}