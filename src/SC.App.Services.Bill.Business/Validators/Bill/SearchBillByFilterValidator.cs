using FluentValidation;
using SC.App.Services.Bill.Business.Queries.Bill;

namespace SC.App.Services.Bill.Business.Validators.Bill
{
    public class SearchBillByFilterValidator : AbstractValidator<SearchBillByFilter>
    {
        public SearchBillByFilterValidator()
        {
        }
    }
}