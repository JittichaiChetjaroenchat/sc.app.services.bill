using FluentValidation;
using SC.App.Services.Bill.Business.Commands.Bill;

namespace SC.App.Services.Bill.Business.Validators.Bill
{
    public class CancelBillValidator : AbstractValidator<CancelBill>
    {
        public CancelBillValidator()
        {
        }
    }
}