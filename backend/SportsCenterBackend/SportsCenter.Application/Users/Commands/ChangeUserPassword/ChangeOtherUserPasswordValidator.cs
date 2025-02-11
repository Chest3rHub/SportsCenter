using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Users.Commands.ChangeUserPassword
{
    public class ChangeOtherUserPasswordValidator : AbstractValidator<ChangeOtherUserPassword>
    {
        public ChangeOtherUserPasswordValidator()
        {
            RuleFor(x => x.Value)
                .NotEmpty().WithMessage("Haslo is required.")
                .MinimumLength(6).WithMessage("Haslo must be at least 6 characters long.");
        }
    }
}
