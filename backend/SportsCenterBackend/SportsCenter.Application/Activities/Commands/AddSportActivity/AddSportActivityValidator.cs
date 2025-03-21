using FluentValidation;

namespace SportsCenter.Application.Activities.Commands.AddSportActivity;

public class AddSportActivityValidator : AbstractValidator<AddSportActivity>
{
    public AddSportActivityValidator()
    {
        RuleFor(x => x.SportActivityName)
    .NotEmpty().WithMessage("Sport Activity Name is required.")
    .MaximumLength(100).WithMessage("Sport Activity Name cannot exceed 100 characters.");

        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("Start Date is required.");

        RuleFor(x => x.DayOfWeek)
            .NotEmpty().WithMessage("Day of Week is required.");

        RuleFor(x => x.StartHour)
            .NotEmpty().WithMessage("Start Hour is required.");

        RuleFor(x => x.DurationInMinutes)
            .GreaterThan(0).WithMessage("Duration must be greater than 0.");

        RuleFor(x => x.LevelName)
            .NotEmpty().WithMessage("Level Name is required.");

        RuleFor(x => x.EmployeeId)
            .GreaterThan(0).WithMessage("Employee ID must be valid.");

        RuleFor(x => x.ParticipantLimit)
            .GreaterThan(0).WithMessage("Participant Limit must be greater than 0.");

        RuleFor(x => x.CourtName)
            .NotEmpty().WithMessage("Court Name is required.");

        RuleFor(x => x.CostWithoutEquipment)
            .GreaterThan(0).WithMessage("Cost without Equipment must be greater than 0.");

        RuleFor(x => x.CostWithEquipment)
            .GreaterThan(0).WithMessage("Cost with Equipment must be greater than 0.");

    }
}
