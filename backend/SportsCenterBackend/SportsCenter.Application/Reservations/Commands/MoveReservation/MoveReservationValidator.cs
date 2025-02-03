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
            RuleFor(x => x.NewStartTime)
                .GreaterThanOrEqualTo(DateTime.UtcNow.AddDays(1))
                .WithMessage("Reservation can only be moved if the remaining time is greater than or equal to 24 hours.");

            RuleFor(x => x.NewEndTime)
                .GreaterThan(x => x.NewStartTime)
                .WithMessage("End time must be later than start time.");
        }
    }
}
