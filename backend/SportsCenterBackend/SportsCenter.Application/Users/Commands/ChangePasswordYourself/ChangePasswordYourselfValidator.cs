using FluentValidation;
using SportsCenter.Application.Users.Commands.ChangePassowrd;

namespace SportsCenter.Application.Users.Commands.ChangePassword
{
    public class ChangePasswordYourselfValidator : AbstractValidator<ChangePasswordYourself>
    {
        public ChangePasswordYourselfValidator()
        {           
            RuleFor(x => x.Value)
                .NotEmpty().WithMessage("Haslo is required.")
                .MinimumLength(6).WithMessage("Haslo must be at least 6 characters long.");
        }
    }
}
