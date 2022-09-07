using FluentValidation;
using SC.App.Services.Bill.Business.Commands.BillRecipient;

namespace SC.App.Services.Bill.Business.Validators.BillRecipient
{
    public class UpdateBillRecipientValidator : AbstractValidator<UpdateBillRecipient>
    {
        public UpdateBillRecipientValidator()
        {
        }
    }
}