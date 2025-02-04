using FluentValidation;
using SportsCenter.Application.Reservations.Commands.AddReservation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Reservations.Commands.AddRecurringReservation
{
    public class AddRecurringReservationValidator : AbstractValidator<AddRecurringReservation>
    {
        public AddRecurringReservationValidator()
        {            
            RuleFor(x => x.ClientId)
                .NotEmpty().WithMessage("Client id is required.");

            RuleFor(x => x.CourtId)
                .NotEmpty().WithMessage("Court id is required.");

            RuleFor(x => x.StartTime)
                .NotEmpty().WithMessage("Start time is required.");

            RuleFor(x => x.EndTime)
                .NotEmpty().WithMessage("End time is required.")
                .GreaterThan(x => x.StartTime).WithMessage("End time must be later than start time.");

            RuleFor(x => x.RecurrenceEndDate)
                .NotEmpty().WithMessage("Recurrence end date is required.")
                .GreaterThanOrEqualTo(x => x.StartTime).WithMessage("Recurrence end date must be after the start time.");

            RuleFor(x => x.Recurrence)
                .NotEmpty().WithMessage("Recurrence type is required.")
                .Must(x => x == "Daily" || x == "Weekly" || x == "BiWeekly" || x == "Monthly")
                .WithMessage("Recurrence type must be 'Daily', 'Weekly', 'BiWeekly', or 'Monthly'.");

            RuleFor(x => x.ParticipantsCount)
                .GreaterThanOrEqualTo(1).WithMessage("Participants count must be at least 1.");

            RuleFor(x => x.EndTime)
                .LessThanOrEqualTo(x => x.StartTime.AddDays(1))
                .WithMessage("Reservation duration cannot exceed 1 day.");
        }
    }
}
