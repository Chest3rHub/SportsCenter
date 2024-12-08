using FluentValidation;

namespace SportsCenter.Application.Activities.Commands.RemoveSportActivity;

internal class RemoveSportActivityValidator : AbstractValidator<RemoveSportActivity>
{
    public RemoveSportActivityValidator()
    {
        RuleFor(x => x.SportActivityId)
            .GreaterThan(0).WithMessage("There is no such activity ID");
    }
}