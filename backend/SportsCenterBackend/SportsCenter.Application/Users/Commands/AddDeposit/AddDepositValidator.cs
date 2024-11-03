using FluentValidation;

namespace SportsCenter.Application.Users.Commands.AccountDeposit
{
    internal class AddDepositValidator : AbstractValidator<AddDeposit>
    {
        public AddDepositValidator()
        {
            RuleFor(x => x.Deposit)
                .Must(BeAValidAmount)
                .WithMessage("The deposit must be greater than zero and a maximum of 5000.");
        }

        private bool BeAValidAmount(decimal amount)
        {
            return amount > 0 && amount <= 5000;
        }
    }
}
