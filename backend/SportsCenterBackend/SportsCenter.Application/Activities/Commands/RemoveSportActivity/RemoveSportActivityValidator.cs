using FluentValidation;

namespace SportsCenter.Application.Activities.Commands.RemoveSportActivity
{

    public class RemoveSportActivityValidator : AbstractValidator<RemoveSportActivity>
    {
        public RemoveSportActivityValidator()
        {
            RuleFor(x => x.SportActivityId)
                .NotEmpty().WithMessage("Sport Activity Id is required.");
        }
    }
}