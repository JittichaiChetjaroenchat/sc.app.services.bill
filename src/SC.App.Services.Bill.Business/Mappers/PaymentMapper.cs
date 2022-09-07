using System;
using System.Collections.Generic;
using System.Linq;
using SC.App.Services.Bill.Business.Queries.Payment;
using SC.App.Services.Bill.Database.Models;
using SC.App.Services.Bill.Lib.Helpers;

namespace SC.App.Services.Bill.Business.Mappers
{
    public class PaymentMapper
    {
        public static List<GetPaymentResponse> Map(List<Payment> payments, ICollection<Document.Client.GetDocumentResponse> evidences)
        {
            return payments
                .Select(x => Map(x, evidences))
                .ToList();
        }

        private static GetPaymentResponse Map(Payment payment, ICollection<Document.Client.GetDocumentResponse> evidences)
        {
            var evidence = evidences
                .FirstOrDefault(x => x.Id == payment.Id);

            return Map(payment, evidence);
        }

        public static GetPaymentResponse Map(Payment payment, Document.Client.GetDocumentResponse evidence)
        {
            return new GetPaymentResponse
            {
                Id = payment.Id,
                PaymentNo = payment.PaymentNo,
                Amount = payment.Amount,
                PayOn = Map(payment.PayOn),
                Remark = payment.Remark,
                Evidence = Map(evidence),
                Status = Map(payment.PaymentStatus),
                CreatedOn = Map(payment.CreatedOn)
            };
        }

        private static GetPaymentDate Map(DateTime dateTime)
        {
            return new GetPaymentDate
            {
                Date = dateTime,
                IsPresentDate = DateTimeHelper.IsPresentDate(dateTime),
                IsPresentMonth = DateTimeHelper.IsPresentMonth(dateTime),
                IsPresentYear = DateTimeHelper.IsPresentYear(dateTime)
            };
        }

        private static GetPaymentEvidence Map(Document.Client.GetDocumentResponse evidence)
        {
            if (evidence == null)
            {
                return null;
            }

            return new GetPaymentEvidence
            {
                Name = evidence.Name,
                Content = evidence.Content,
                ContentType = evidence.Content_type,
                ContentLength = evidence.Content_length
            };
        }

        private static GetPaymentStatus Map(PaymentStatus paymentStatus)
        {
            if (paymentStatus == null)
            {
                return null;
            }

            return new GetPaymentStatus
            {
                Code = paymentStatus.Code
            };
        }
    }
}