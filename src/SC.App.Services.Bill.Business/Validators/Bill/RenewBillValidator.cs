using FluentValidation;
using SC.App.Services.Bill.Business.Commands.Bill;

namespace SC.App.Services.Bill.Business.Validators.Bill
{
    public class RenewBillValidator : AbstractValidator<RenewBill>
    {
        public RenewBillValidator()
        {
        }
    }
}