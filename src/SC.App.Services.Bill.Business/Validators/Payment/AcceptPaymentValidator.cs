using FluentValidation;
using SC.App.Services.Bill.Business.Commands.Payment;

namespace SC.App.Services.Bill.Business.Validators.Payment
{
    public class AcceptPaymentValidator : AbstractValidator<AcceptPayment>
    {
        public AcceptPaymentValidator()
        {
        }
    }
}