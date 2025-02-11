using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Employees.Commands.AddAdmTask
{
    public class AddTaskForAdmEmployeeValidator : AbstractValidator<AddAdmTask>
    {
        public AddTaskForAdmEmployeeValidator()
        {
            RuleFor(x => x.DataDo)
                .NotEmpty().WithMessage("Data is required.")
                .Must(BeFutureDate).WithMessage("Date must be from the future.");

        }
        private bool BeFutureDate(DateTime date)
        {
            return DateOnly.FromDateTime(date) >= DateOnly.FromDateTime(DateTime.Today);
        }

    }
}
