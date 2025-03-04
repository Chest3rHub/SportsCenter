using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Employees.Commands.AddAbsenceRequest
{
    public class AddAbsenceRequestValidator : AbstractValidator<AddAbsenceRequest>
    {
        public AddAbsenceRequestValidator()
        {
            RuleFor(x => x.Date)
               .NotEmpty().WithMessage("Date is required.")
               .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today)).WithMessage("Date must be today or from the future.");

            RuleFor(x => x.StartHour)
                .NotEmpty().WithMessage("Start hour is required.");

            RuleFor(x => x.EndHour)
                .NotEmpty().WithMessage("End hour is required.");      

            RuleFor(x => x.StartHour)
                .LessThan(x => x.EndHour).WithMessage("Start hour must be earlier than end hour.");
        }
    }
}
