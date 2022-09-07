using System;
using SC.App.Services.Bill.Business.Queries.PaymentVerification;
using SC.App.Services.Bill.Database.Models;
using SC.App.Services.Bill.Lib.Extensions;
using SC.App.Services.Bill.Lib.Helpers;

namespace SC.App.Services.Bill.Business.Mappers
{
    public class PaymentVerificationMapper
    {
        public static GetPaymentVerificationResponse Map(PaymentVerification paymentVerification)
        {
            if (paymentVerification == null)
            {
                return null;
            }

            return new GetPaymentVerificationResponse
            {
                Id = paymentVerification.Id,
                IsProceed = paymentVerification.IsProceed,
                CanVerify = paymentVerification.CanVerify,
                IsUnique = paymentVerification.IsUnique,
                DuplicateTo = paymentVerification.DuplicateTo,
                IsCorrectBankAccountNumber = paymentVerification.IsCorrectBankAccountNumber,
                IsCorrectBankAccountName = paymentVerification.IsCorrectBankAccountName,
                IsCorrectAmount = paymentVerification.IsCorrectAmount,
                UnBalanceAmount = paymentVerification.UnBalanceAmount,
                Remark = paymentVerification.Remark,
                Detail = PaymentVerificationDetailMapper.Map(paymentVerification.PaymentVerificationDetail),
                Status = PaymentVerificationStatusMapper.Map(paymentVerification.PaymentVerificationStatus)
            };
        }

        private class PaymentVerificationDetailMapper
        {
            public static GetPaymentVerificationDetail Map(PaymentVerificationDetail paymentVerificationDetail)
            {
                if (paymentVerificationDetail == null)
                {
                    return null;
                }

                return new GetPaymentVerificationDetail
                {
                    Source = PaymentVerificationBankAccountMapper.Map(paymentVerificationDetail.SourceBankCode, paymentVerificationDetail.SourceBankAccountType, paymentVerificationDetail.SourceBankAccountNumber, paymentVerificationDetail.SourceBankAccountName, paymentVerificationDetail.SourceBankAccountDisplayName),
                    Destination = PaymentVerificationBankAccountMapper.Map(paymentVerificationDetail.DestinationBankCode, paymentVerificationDetail.DestinationBankAccountType, paymentVerificationDetail.DestinationBankAccountNumber, paymentVerificationDetail.DestinationBankAccountName, paymentVerificationDetail.DestinationBankAccountDisplayName),
                    Amount = paymentVerificationDetail.Amount,
                    TransactionRefNo = paymentVerificationDetail.TransactionRefNo,
                    TransactionDate = PaymentVerificationDateMapper.Map(paymentVerificationDetail.TransactionDate)
                };
            }

            private class PaymentVerificationBankAccountMapper
            {
                public static GetPaymentVerificationBankAccount Map(string bankCode, string bankAccountType, string bankAccountNumber, string bankAccountName, string bankAccountDisplayName)
                {
                    return new GetPaymentVerificationBankAccount
                    {
                        Bank = PaymentVerificationBankMapper.Map(bankCode),
                        Type = bankAccountType,
                        Number = bankAccountNumber,
                        Name = Map(bankAccountName, bankAccountDisplayName)
                    };
                }

                private static string Map(string name, string displayName)
                {
                    if (!name.IsEmpty())
                    {
                        return name;
                    }

                    return displayName;
                }
            }

            private class PaymentVerificationBankMapper
            {
                public static GetPaymentVerificationBank Map(string code)
                {
                    if (code.IsEmpty())
                    {
                        return null;
                    }

                    return new GetPaymentVerificationBank
                    {
                        Code = code
                    };
                }
            }

            private class PaymentVerificationDateMapper
            {
                public static GetPaymentVerificationDate Map(DateTime dateTime)
                {
                    return new GetPaymentVerificationDate
                    {
                        Date = dateTime,
                        IsPresentDate = DateTimeHelper.IsPresentDate(dateTime),
                        IsPresentMonth = DateTimeHelper.IsPresentMonth(dateTime),
                        IsPresentYear = DateTimeHelper.IsPresentYear(dateTime)
                    };
                }
            }
        }

        private class PaymentVerificationStatusMapper
        {
            public static GetPaymentVerificationStatus Map(PaymentVerificationStatus status)
            {
                if (status == null)
                {
                    return null;
                }

                return new GetPaymentVerificationStatus
                {
                    Code = status.Code
                };
            }
        }
    }
}