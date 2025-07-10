using Bank.Domain.Entities;
using FluentValidation;

namespace Bank.Application.Validators
{
    public class OperationValidator : AbstractValidator<Operation>
    {
        public OperationValidator()
        {
            RuleFor(x => x.Date).Must(IsValidDate).WithMessage("{PropertyName} cannot be in the future");
        }

        private bool IsValidDate(DateTime date)
        {
            return date <= DateTime.Now;
        }
    }
}
