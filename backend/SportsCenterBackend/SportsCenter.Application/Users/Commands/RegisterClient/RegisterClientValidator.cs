using System;
using FluentValidation;

namespace SportsCenter.Application.Users.Commands.RegisterClient;

public class RegisterClientValidator : AbstractValidator<RegisterClient>
{
    public RegisterClientValidator()
    {
        RuleFor(x => x.Imie)
            .NotEmpty().WithMessage("Imie is required.")
            .Length(2, 50).WithMessage("Imie must be between 2 and 50 characters.");

        RuleFor(x => x.Nazwisko)
            .NotEmpty().WithMessage("Nazwisko is required.")
            .Length(2, 50).WithMessage("Nazwisko must be between 2 and 50 characters.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");

        RuleFor(x => x.Haslo)
            .NotEmpty().WithMessage("Haslo is required.")
            .MinimumLength(6).WithMessage("Haslo must be at least 6 characters long.");

        RuleFor(x => x.DataUr)
            .NotEmpty().WithMessage("DataUr is required.")
            .Must(BeAValidAge).WithMessage("Client must be at least 18 years old.");

        RuleFor(x => x.NrTel)
            .NotEmpty().WithMessage("NrTel is required.")
            .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Invalid phone number format.");

        RuleFor(x => x.Adres)
            .NotEmpty().WithMessage("Adres is required.")
            .Length(5, 100).WithMessage("Adres must be between 5 and 100 characters.");
    }

    private bool BeAValidAge(DateTime? date)
    {
        if (date == null)
            return false;

        var age = DateTime.Today.Year - date.Value.Year;
        if (date.Value.Date > DateTime.Today.AddYears(-age)) age--;

        return age >= 18;
    }
}