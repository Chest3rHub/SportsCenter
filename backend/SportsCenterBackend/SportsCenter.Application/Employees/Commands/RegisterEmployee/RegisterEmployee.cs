using MediatR;
using SportsCenter.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Employees.Commands.RegisterEmployee
{
    public sealed record RegisterEmployee : ICommand<Unit>
    {
        public string Imie { get; } = null!;
        public string Nazwisko { get; } = null!;
        public string Email { get; } = null!;
        public string Haslo { get; } = null!;
        public DateTime DataUr { get; }
        public string NrTel { get; } = null!;
        public string Adres { get; } = null!;
        public string PositionName { get; } = null!;
        public DateTime HireDate { get; set; }

        public RegisterEmployee(string imie, string nazwisko, string email, string haslo, DateTime dataUr, string nrTel,
       string adres, string positionName, DateTime hireDate)
        {
            Imie = imie;
            Nazwisko = nazwisko;
            Email = email;
            Haslo = haslo;
            DataUr = dataUr;
            NrTel = nrTel;
            Adres = adres;
            PositionName = positionName;
            HireDate = hireDate;
        }
    }
}
