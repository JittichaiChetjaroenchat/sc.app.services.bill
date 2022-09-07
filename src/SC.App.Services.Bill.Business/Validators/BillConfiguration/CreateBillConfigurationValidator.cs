using FluentValidation;
using SC.App.Services.Bill.Business.Commands.BillConfiguration;

namespace SC.App.Services.Bill.Business.Validators.BillConfiguration
{
    public class CreateBillConfigurationValidator : AbstractValidator<CreateBillConfiguration>
    {
        public CreateBillConfigurationValidator()
        {
        }
    }
}