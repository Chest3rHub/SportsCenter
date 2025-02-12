﻿using FluentValidation;
using SportsCenter.Application.Reservations.Commands.AddReservation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Reservations.Commands.AddSingleReservationYourself
{
    public class AddSingleReservationYourselfValidator : AbstractValidator<AddSingleReservationYourself>
    {
        public AddSingleReservationYourselfValidator()
        {
           
            RuleFor(x => x.CourtId)
                .NotEmpty().WithMessage("Court id is required.");

            RuleFor(x => x.StartTime)
                .NotEmpty().WithMessage("Start time is required.");

            RuleFor(x => x.EndTime)
                .NotEmpty().WithMessage("End time is required.")
                .GreaterThan(x => x.StartTime).WithMessage("End time must be later than start time."); // Sprawdza, czy endTime > startTime

            RuleFor(x => x.ParticipantsCount)
                .NotEmpty().WithMessage("Participants count is required.")
                .GreaterThanOrEqualTo(1).WithMessage("Participants count must be at least 1."); ;

            //dodalam ograniczenie ze rezerwacja trwa maksymalnie 1 dzień (roboczy klubu)
            //bo inaczej by bylo troche bez sensu 

            RuleFor(x => x.EndTime)
                .LessThanOrEqualTo(x => x.StartTime.AddDays(1))
                .WithMessage("Reservation duration cannot exceed 1 day.");
        }
    }
}
