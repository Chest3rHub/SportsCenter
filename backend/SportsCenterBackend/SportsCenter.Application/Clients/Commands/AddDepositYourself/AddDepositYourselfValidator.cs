using FluentValidation;
using SportsCenter.Application.Clients.Commands.AddDeposit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Clients.Commands.AddDepositYourself
{
    internal class AddDepositYourselfValidator : AbstractValidator<AddDepositYourself>
    {
        public AddDepositYourselfValidator()
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
