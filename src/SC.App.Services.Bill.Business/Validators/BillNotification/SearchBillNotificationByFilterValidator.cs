using FluentValidation;
using SC.App.Services.Bill.Business.Queries.BillNotification;

namespace SC.App.Services.Bill.Business.Validators.BillNotification
{
    public class SearchBillNotificationByFilterValidator : AbstractValidator<SearchBillNotificationByFilter>
    {
        public SearchBillNotificationByFilterValidator()
        {
        }
    }
}