using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using SC.App.Services.Bill.Business.Commands.Bill;
using SC.App.Services.Bill.Business.Commands.Payment;
using SC.App.Services.Bill.Business.Enums;
using SC.App.Services.Bill.Business.Helpers;
using SC.App.Services.Bill.Business.Managers.Interface;
using SC.App.Services.Bill.Business.Mappers;
using SC.App.Services.Bill.Business.Queries.Payment;
using SC.App.Services.Bill.Business.Repositories.Interface;
using SC.App.Services.Bill.Business.Resources;
using SC.App.Services.Bill.Business.Validators.Payment;
using SC.App.Services.Bill.Client.Customer;
using SC.App.Services.Bill.Client.Document;
using SC.App.Services.Bill.Client.Setting;
using SC.App.Services.Bill.Common.Helpers;
using SC.App.Services.Bill.Common.Managers;
using SC.App.Services.Bill.Common.Responses;
using SC.App.Services.Bill.Lib.Extensions;
using SC.App.Services.Bill.Queue.Managers.Interface;
using Serilog;

namespace SC.App.Services.Bill.Business.Managers
{
    public class PaymentManager : BaseManager<IPaymentRepository>, IPaymentManager
    {
        private readonly IBillRepository _billRepository;
        private readonly IBillStatusRepository _billStatusRepository;
        private readonly IPaymentStatusRepository _paymentStatusRepository;

        private readonly IBillManager _billManager;
        private readonly IDocumentManager _documentManager;
        private readonly ICustomerManager _customerManager;
        private readonly IStreamingManager _streamingManager;
        private readonly ISettingManager _settingManager;
        private readonly IQueueManager _queueManager;

        public PaymentManager(
            IPaymentRepository repository,
            IBillRepository billRepository,
            IBillStatusRepository billStatusRepository,
            IPaymentStatusRepository paymentStatusRepository,

            IBillManager billManager,
            IDocumentManager documentManager,
            ICustomerManager customerManager,
            IStreamingManager streamingManager,
            ISettingManager settingManager,
            IQueueManager queueManager)
            : base(repository)
        {
            _billRepository = billRepository;
            _billStatusRepository = billStatusRepository;
            _paymentStatusRepository = paymentStatusRepository;

            _billManager = billManager;
            _documentManager = documentManager;
            _customerManager = customerManager;
            _streamingManager = streamingManager;
            _settingManager = settingManager;
            _queueManager = queueManager;
        }

        public async Task<Response<GetPaymentResponse>> GetAsync(IConfiguration configuration, GetPaymentByIdQuery query)
        {
            GetPaymentResponse data = null;
            List<ResponseError> errors = new List<ResponseError>();
            Response<GetPaymentResponse> response = null;

            try
            {
                // Get payment
                var payment = Repository.GetById(query.Payload.Id);
                if (payment == null)
                {
                    errors.Add(new ResponseError { Code = EnumErrorCode._102080007.GetDescription(), Message = ErrorMessage._102080007 });
                    response = ResponseHelper.Error<GetPaymentResponse>(errors);

                    return await Task.FromResult(response);
                }

                // Get evidence
                Document.Client.GetDocumentResponse evidence = null;
                if (payment.EvidenceId.HasValue)
                {
                    var getDoucmentByIdResponse = await _documentManager.GetDocumentByIdAsync(query.Request, payment.EvidenceId.Value);
                    if (!DocumentClientHelper.IsSuccess(getDoucmentByIdResponse))
                    {
                        var error = DocumentClientHelper.GetError(getDoucmentByIdResponse.Errors);

                        errors.Add(new ResponseError { Code = error.Code, Message = error.Message });
                        response = ResponseHelper.Error<GetPaymentResponse>(errors);

                        return await Task.FromResult(response);
                    }
                    else
                    {
                        evidence = getDoucmentByIdResponse.Data;
                    }
                }

                // Build response
                data = PaymentMapper.Map(payment, evidence);
                response = ResponseHelper.Ok(data);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);

                errors.Add(new ResponseError { Code = null, Message = ex.Message });
                response = ResponseHelper.Error<GetPaymentResponse>(errors);
            }

            return await Task.FromResult(response);
        }

        public async Task<Response<List<GetPaymentResponse>>> GetAsync(IConfiguration configuration, GetPaymentByFilterQuery query)
        {
            List<GetPaymentResponse> data = null;
            List<ResponseError> errors = new List<ResponseError>();
            Response<List<GetPaymentResponse>> response = null;

            try
            {
                // Get payments
                var payments = Repository.GetByBilId(query.Payload.BillId);

                // Get evidences
                ICollection<Document.Client.GetDocumentResponse> evidences = new List<Document.Client.GetDocumentResponse>();
                var evidenceIds = payments
                    .Where(x => x.EvidenceId.HasValue)
                    .Select(x => x.EvidenceId.Value)
                    .ToArray();
                var getDoucmentByIdsResponse = await _documentManager.GetDocumentByIdsAsync(query.Request, evidenceIds);
                if (!DocumentClientHelper.IsSuccess(getDoucmentByIdsResponse))
                {
                    var error = DocumentClientHelper.GetError(getDoucmentByIdsResponse.Errors);

                    errors.Add(new ResponseError { Code = error.Code, Message = error.Message });
                    response = ResponseHelper.Error<List<GetPaymentResponse>>(errors);

                    return await Task.FromResult(response);
                }
                else
                {
                    evidences = getDoucmentByIdsResponse.Data;
                }

                // Build response
                data = PaymentMapper.Map(payments, evidences);
                response = ResponseHelper.Ok(data);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);

                errors.Add(new ResponseError { Code = null, Message = ex.Message });
                response = ResponseHelper.Error<List<GetPaymentResponse>>(errors);
            }

            return await Task.FromResult(response);
        }

        public async Task<Response<OpenPaymentResponse>> UpdateAsync(IConfiguration configuration, OpenPaymentCommand command)
        {
            OpenPaymentResponse data = null;
            List<ResponseError> errors = new List<ResponseError>();
            Response<OpenPaymentResponse> response = null;

            try
            {
                // Validate
                await new CreatePaymentValidator().ValidateAndThrowAsync(command.Payload);

                // Get bill
                var bill = _billRepository.GetById(command.Payload.BillId);
                if (bill == null)
                {
                    errors.Add(new ResponseError { Code = EnumErrorCode._102080001.GetDescription(), Message = ErrorMessage._102080001 });
                    response = ResponseHelper.Error<OpenPaymentResponse>(errors);

                    return await Task.FromResult(response);
                }

                // Get latest payment
                var payment = Repository.GetLatestByBilId(command.Payload.BillId);
                if (payment != null && payment.PaymentStatus.Code == EnumPaymentStatus.Pending.GetDescription())
                {
                    errors.Add(new ResponseError { Code = EnumErrorCode._102080009.GetDescription(), Message = ErrorMessage._102080009 });
                    response = ResponseHelper.Error<OpenPaymentResponse>(errors);

                    return await Task.FromResult(response);
                }

                // Update bill's status to pending
                var billStatus = _billStatusRepository.GetByCode(EnumBillStatus.Pending.GetDescription());

                bill.BillStatusId = billStatus.Id;
                bill.UpdatedBy = command.Payload.UserId;
                bill.UpdatedOn = DateTime.Now;

                _billRepository.Update(bill);

                // Build response
                data = new OpenPaymentResponse(payment.Id);
                response = ResponseHelper.Ok(data);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);

                errors.Add(new ResponseError { Code = null, Message = ex.Message });
                response = ResponseHelper.Error<OpenPaymentResponse>(errors);
            }

            return await Task.FromResult(response);
        }

        public async Task<Response<AcceptPaymentResponse>> UpdateAsync(IConfiguration configuration, AcceptPaymentCommand command)
        {
            List<ResponseError> errors = new List<ResponseError>();
            Response<AcceptPaymentResponse> response = null;

            try
            {
                // Validate
                await new AcceptPaymentValidator().ValidateAndThrowAsync(command.Payload);

                // Get bill
                var bill = _billRepository.GetById(command.Payload.BillId);
                if (bill == null)
                {
                    errors.Add(new ResponseError { Code = EnumErrorCode._102080001.GetDescription(), Message = ErrorMessage._102080001 });
                    response = ResponseHelper.Error<AcceptPaymentResponse>(errors);

                    return await Task.FromResult(response);
                }

                // Get billing
                Setting.Client.GetBillingResponse billing = null;
                var getBillingResponse = await _settingManager.GetBillingByChannelIdAsync(command.Request, bill.ChannelId);
                if (!SettingClientHelper.IsSuccess(getBillingResponse))
                {
                    var error = SettingClientHelper.GetError(getBillingResponse.Errors);

                    errors.Add(new ResponseError { Code = error.Code, Message = error.Message });
                    response = ResponseHelper.Error<AcceptPaymentResponse>(errors);

                    return await Task.FromResult(response);
                }
                else
                {
                    billing = getBillingResponse.Data;
                }

                // Accept payment
                var billPaymentType = BillPaymentTypeHelper.GetByCode(bill.BillPayment.BillPaymentType.Code);
                switch (billPaymentType)
                {
                    case EnumBillPaymentType.PrePaid:
                        response = await AcceptTransferAsync(configuration, command, bill);
                        break;
                    case EnumBillPaymentType.PostPaid:
                        response = await AcceptCodAsync(configuration, command, bill);
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);

                errors.Add(new ResponseError { Code = null, Message = ex.Message });
                response = ResponseHelper.Error<AcceptPaymentResponse>(errors);
            }

            return await Task.FromResult(response);
        }

        public async Task<Response<RejectPaymentResponse>> UpdateAsync(IConfiguration configuration, RejectPaymentCommand command)
        {
            List<ResponseError> errors = new List<ResponseError>();
            Response<RejectPaymentResponse> response = null;

            try
            {
                // Validate
                await new RejectPaymentValidator().ValidateAndThrowAsync(command.Payload);

                // Get bill
                var bill = _billRepository.GetById(command.Payload.BillId);
                if (bill == null)
                {
                    errors.Add(new ResponseError { Code = EnumErrorCode._102080001.GetDescription(), Message = ErrorMessage._102080001 });
                    response = ResponseHelper.Error<RejectPaymentResponse>(errors);

                    return await Task.FromResult(response);
                }

                var billPaymentType = BillPaymentTypeHelper.GetByCode(bill.BillPayment.BillPaymentType.Code);
                switch (billPaymentType)
                {
                    case EnumBillPaymentType.PrePaid:
                        response = await RejectTransferAsync(configuration, command, bill);
                        break;
                    case EnumBillPaymentType.PostPaid:
                        response = await RejectCodAsync(configuration, command, bill);
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);

                errors.Add(new ResponseError { Code = null, Message = ex.Message });
                response = ResponseHelper.Error<RejectPaymentResponse>(errors);
            }

            return await Task.FromResult(response);
        }

        private async Task<Response<AcceptPaymentResponse>> AcceptTransferAsync(IConfiguration configuration, AcceptPaymentCommand command, Database.Models.Bill bill)
        {
            AcceptPaymentResponse data = null;
            List<ResponseError> errors = new List<ResponseError>();
            Response<AcceptPaymentResponse> response = null;

            try
            {
                // Get latest payment
                var payment = Repository.GetLatestByBilId(command.Payload.BillId);
                if (payment == null)
                {
                    errors.Add(new ResponseError { Code = EnumErrorCode._102080007.GetDescription(), Message = ErrorMessage._102080007 });
                    response = ResponseHelper.Error<AcceptPaymentResponse>(errors);

                    return await Task.FromResult(response);
                }

                // Update customer to regular
                var updateCustomerToRegularResponse = await _customerManager.RegularCustomerAsync(command.Request, bill.BillRecipient.CustomerId);
                if (!CustomerClientHelper.IsSuccess(updateCustomerToRegularResponse))
                {
                    var error = CustomerClientHelper.GetError(updateCustomerToRegularResponse.Errors);

                    errors.Add(new ResponseError { Code = error.Code, Message = error.Message });
                    response = ResponseHelper.Error<AcceptPaymentResponse>(errors);

                    return await Task.FromResult(response);
                }

                // Update payment to accepted
                var paymentStatus = _paymentStatusRepository.GetByCode(EnumPaymentStatus.Accepted.GetDescription());
                payment.PaymentStatusId = paymentStatus.Id;
                Repository.Update(payment);

                // Send response message
                await _queueManager.NotifyPaymentAcceptAsync(payment.Id);

                // Update bill to comfirmed
                var comfirmBillPayload = new ConfirmBill { Id = command.Payload.BillId, UserId = command.Payload.UserId };
                var confirmBillCommand = new ConfirmBillCommand(command.Request, comfirmBillPayload);
                _ = await _billManager.UpdateAsync(configuration, confirmBillCommand);

                // Unlock booking
                await _streamingManager.UnlockBooking(command.Request, bill.ChannelId, bill.BillRecipient.AliasName);

                // Build response
                data = new AcceptPaymentResponse();
                response = ResponseHelper.Ok(data);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);

                errors.Add(new ResponseError { Code = null, Message = ex.Message });
                response = ResponseHelper.Error<AcceptPaymentResponse>(errors);
            }

            return await Task.FromResult(response);
        }

        private async Task<Response<AcceptPaymentResponse>> AcceptCodAsync(IConfiguration configuration, AcceptPaymentCommand command, Database.Models.Bill bill)
        {
            AcceptPaymentResponse data = null;
            List<ResponseError> errors = new List<ResponseError>();
            Response<AcceptPaymentResponse> response = null;

            try
            {
                // Update customer to regular
                var updateCustomerToRegularResponse = await _customerManager.RegularCustomerAsync(command.Request, bill.BillRecipient.CustomerId);
                if (!CustomerClientHelper.IsSuccess(updateCustomerToRegularResponse))
                {
                    var error = CustomerClientHelper.GetError(updateCustomerToRegularResponse.Errors);

                    errors.Add(new ResponseError { Code = error.Code, Message = error.Message });
                    response = ResponseHelper.Error<AcceptPaymentResponse>(errors);

                    return await Task.FromResult(response);
                }

                // Send response message
                await _queueManager.NotifyDeliveryAddressAcceptAsync(bill.Id);

                // Update bill to comfirmed
                var comfirmBillPayload = new ConfirmBill { Id = command.Payload.BillId, UserId = command.Payload.UserId };
                var confirmBillCommand = new ConfirmBillCommand(command.Request, comfirmBillPayload);
                _ = await _billManager.UpdateAsync(configuration, confirmBillCommand);

                // Unlock booking
                await _streamingManager.UnlockBooking(command.Request, bill.ChannelId, bill.BillRecipient.AliasName);

                // Build response
                data = new AcceptPaymentResponse();
                response = ResponseHelper.Ok(data);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);

                errors.Add(new ResponseError { Code = null, Message = ex.Message });
                response = ResponseHelper.Error<AcceptPaymentResponse>(errors);
            }

            return await Task.FromResult(response);
        }

        private async Task<Response<RejectPaymentResponse>> RejectTransferAsync(IConfiguration configuration, RejectPaymentCommand command, Database.Models.Bill bill)
        {
            RejectPaymentResponse data = null;
            List<ResponseError> errors = new List<ResponseError>();
            Response<RejectPaymentResponse> response = null;

            try
            {
                // Get latest payment
                var payment = Repository.GetLatestByBilId(command.Payload.BillId);
                if (payment == null)
                {
                    errors.Add(new ResponseError { Code = EnumErrorCode._102080007.GetDescription(), Message = ErrorMessage._102080007 });
                    response = ResponseHelper.Error<RejectPaymentResponse>(errors);

                    return await Task.FromResult(response);
                }

                // Update bill's status to rejected
                var billStatus = _billStatusRepository.GetByCode(EnumBillStatus.Rejected.GetDescription());
                bill.BillStatusId = billStatus.Id;
                bill.UpdatedBy = command.Payload.UserId;
                bill.UpdatedOn = DateTime.Now;
                _billRepository.Update(bill);

                // Update payment's status to rejected
                var paymentStatus = _paymentStatusRepository.GetByCode(EnumPaymentStatus.Rejected.GetDescription());
                payment.PaymentStatusId = paymentStatus.Id;
                Repository.Update(payment);

                // Send response message
                await _queueManager.NotifyPaymentRejectAsync(payment.Id);

                // Build response
                data = new RejectPaymentResponse();
                response = ResponseHelper.Ok(data);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);

                errors.Add(new ResponseError { Code = null, Message = ex.Message });
                response = ResponseHelper.Error<RejectPaymentResponse>(errors);
            }

            return await Task.FromResult(response);
        }

        private async Task<Response<RejectPaymentResponse>> RejectCodAsync(IConfiguration configuration, RejectPaymentCommand command, Database.Models.Bill bill)
        {
            RejectPaymentResponse data = null;
            List<ResponseError> errors = new List<ResponseError>();
            Response<RejectPaymentResponse> response = null;

            try
            {
                // Update bill's status to rejected
                var billStatus = _billStatusRepository.GetByCode(EnumBillStatus.Rejected.GetDescription());
                bill.BillStatusId = billStatus.Id;
                bill.UpdatedBy = command.Payload.UserId;
                bill.UpdatedOn = DateTime.Now;
                _billRepository.Update(bill);

                // Send response message
                await _queueManager.NotifyDeliveryAddressRejectAsync(bill.Id);

                // Build response
                data = new RejectPaymentResponse();
                response = ResponseHelper.Ok(data);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);

                errors.Add(new ResponseError { Code = null, Message = ex.Message });
                response = ResponseHelper.Error<RejectPaymentResponse>(errors);
            }

            return await Task.FromResult(response);
        }
    }
}