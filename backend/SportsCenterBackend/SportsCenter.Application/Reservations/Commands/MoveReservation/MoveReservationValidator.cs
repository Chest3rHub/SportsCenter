using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Reservations.Commands.MoveReservation
{
    public class MoveReservationValidator : AbstractValidator<MoveReservation>
    {
        public MoveReservationValidator()
        {
            RuleFor(x => DateTime.Parse(x.NewStartTime))
                .GreaterThanOrEqualTo(DateTime.UtcNow.AddDays(1))
                .WithMessage("Reservation can only be moved if the remaining time is greater than or equal to 24 hours.");

            RuleFor(x => DateTime.Parse(x.NewEndTime))
                .GreaterThan(x => DateTime.Parse(x.NewStartTime))
                .WithMessage("End time must be later than start time.");

            RuleFor(x => x)
               .Must(x => (DateTime.Parse(x.NewEndTime) - DateTime.Parse(x.NewStartTime)).TotalHours >= 1)
               .WithMessage("Reservation must be at least 1 hour long.")
               .Must(x => (DateTime.Parse(x.NewEndTime) - DateTime.Parse(x.NewStartTime)).TotalHours <= 5)
               .WithMessage("Reservation cannot be longer than 5 hours.");
        }
    }
}
