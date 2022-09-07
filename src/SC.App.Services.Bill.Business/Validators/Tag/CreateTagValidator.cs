using FluentValidation;
using SC.App.Services.Bill.Business.Commands.Tag;

namespace SC.App.Services.Bill.Business.Validators.Tag
{
    public class CreateTagValidator : AbstractValidator<CreateTag>
    {
        public CreateTagValidator()
        {
        }
    }
}