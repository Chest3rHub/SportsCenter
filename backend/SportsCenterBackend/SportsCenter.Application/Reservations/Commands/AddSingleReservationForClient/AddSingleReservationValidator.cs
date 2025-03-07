using FluentValidation;

namespace SportsCenter.Application.Reservations.Commands.AddReservation
{
    public class AddSingleReservationValidator : AbstractValidator<AddSingleReservation>
    {
        public AddSingleReservationValidator()
        {
            RuleFor(x => x.ClientId)
                .NotEmpty().WithMessage("Client id is required.");

            RuleFor(x => x.CourtId)
                .NotEmpty().WithMessage("Court id is required.");

            RuleFor(x => x.StartTime)
                .NotEmpty().WithMessage("Start time is required.");

            RuleFor(x => x.EndTime)
                .NotEmpty().WithMessage("End time is required.")
                .GreaterThan(x => x.StartTime).WithMessage("End time must be later than start time.") // Sprawdza, czy endTime > startTime
                .LessThanOrEqualTo(x => x.StartTime.AddDays(1)).WithMessage("Reservation duration cannot exceed 1 day."); // Maksymalnie 1 dzień

            RuleFor(x => x.ParticipantsCount)
                .NotEmpty().WithMessage("Participants count is required.")
                .GreaterThanOrEqualTo(1).WithMessage("Participants count must be at least 1.");

            RuleFor(x => x)
                .Must(x => (x.EndTime - x.StartTime).TotalHours >= 1)
                .WithMessage("Reservation must be at least 1 hour long.")
                .Must(x => (x.EndTime - x.StartTime).TotalHours <= 5)
                .WithMessage("Reservation cannot be longer than 5 hours.");
        }
    }
}
