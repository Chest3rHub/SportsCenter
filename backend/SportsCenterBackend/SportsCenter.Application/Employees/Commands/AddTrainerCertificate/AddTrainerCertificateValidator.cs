using FluentValidation;
using System;

namespace SportsCenter.Application.Employees.Commands.AddTrainerCertificate
{
    public class AddTrainerCertificateValidator : AbstractValidator<AddTrainerCertificate>
    {
        public AddTrainerCertificateValidator()
        {
            RuleFor(x => x.ReceivedDate)
                .NotEmpty().WithMessage("Data is required.")
                .Must(BeTodayOrPastDate).WithMessage("Date must be today or in the past.");
        }

        private bool BeTodayOrPastDate(DateOnly date)
        {
            return date <= DateOnly.FromDateTime(DateTime.Today);
        }
    }
}
