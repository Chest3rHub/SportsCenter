using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Employees.Commands.RegisterEmployee
{
    public class RegisterEmployeeValidator : AbstractValidator<RegisterEmployee>
    {
        public RegisterEmployeeValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Imie is required.")
                .Length(2, 50).WithMessage("Imie must be between 2 and 50 characters.");

            RuleFor(x => x.Surname)
                .NotEmpty().WithMessage("Nazwisko is required.")
                .Length(2, 50).WithMessage("Nazwisko must be between 2 and 50 characters.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Haslo is required.")
                .MinimumLength(6).WithMessage("Haslo must be at least 6 characters long.");

            RuleFor(x => x.BirthDate)
                .NotEmpty().WithMessage("DataUr is required.")
                .Must(BeAValidAge).WithMessage("Client must be at least 18 years old.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("NrTel is required.")
                .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Invalid phone number format.");

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Adres is required.")
                .Length(5, 100).WithMessage("Adres must be between 5 and 100 characters.");
        }

        private bool BeAValidAge(DateTime date)
        {
            var age = DateTime.Today.Year - date.Year;
            if (date.Date > DateTime.Today.AddYears(-age)) age--;

            return age >= 18;
        }

    }
}
