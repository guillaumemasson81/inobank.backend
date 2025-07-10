using Bank.Domain.Entities;
using FluentValidation;

namespace Bank.Application.Validators
{
    public class AccountValidator : AbstractValidator<Account>
    {
        public AccountValidator()
        {
            RuleFor(x => x.AccountType).NotEmpty();
            RuleFor(x => x.AllowedOverdraft).GreaterThan(0);
            RuleFor(x => x.Currency).Equal("euro");
        }
    }
}
