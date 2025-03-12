using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Employees.Commands.EditTask
{
    public class EditTaskValidator : AbstractValidator<EditTask>
    {
        public EditTaskValidator()
        {
            RuleFor(x => x.DateTo)
                .NotEmpty().WithMessage("Data is required.")
                .Must(BeFutureDate).WithMessage("Date must be from the future.");

        }
        private bool BeFutureDate(DateTime date)
        {
            return DateOnly.FromDateTime(date) >= DateOnly.FromDateTime(DateTime.Today);
        }

    }
}
