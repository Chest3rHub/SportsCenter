using FluentValidation;

namespace SportsCenter.Application.Activities.Commands.AddSportActivity;

public class AddSportActivityValidator : AbstractValidator<AddSportActivity>
{
    public AddSportActivityValidator()
    {
        RuleFor(x => x.SportActivityName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.LevelId).GreaterThan(0);
        RuleFor(x => x.Duration).GreaterThan(0);
        RuleFor(x => x.EmployeeId).GreaterThan(0).WithMessage("Employee ID must be valid.");
        RuleFor(x => x.ParticipantLimit).GreaterThan(0);
        RuleFor(x => x.CourtId).GreaterThan(0).WithMessage("Court ID must be valid.");
        RuleFor(x => x.CostWithoutEquipment).GreaterThan(0);
        RuleFor(x => x.CostWithEquipment).GreaterThan(0);
    }
}
