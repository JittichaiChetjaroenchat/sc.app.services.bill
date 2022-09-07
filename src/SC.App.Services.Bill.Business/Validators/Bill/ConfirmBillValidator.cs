using FluentValidation;
using SC.App.Services.Bill.Business.Commands.Bill;

namespace SC.App.Services.Bill.Business.Validators.Bill
{
    public class ConfirmBillValidator : AbstractValidator<ConfirmBill>
    {
        public ConfirmBillValidator()
        {
        }
    }
}