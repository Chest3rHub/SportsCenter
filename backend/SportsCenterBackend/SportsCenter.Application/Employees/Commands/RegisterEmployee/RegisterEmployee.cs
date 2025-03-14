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
        public string Name { get; } = null!;
        public string Surname { get; } = null!;
        public string Email { get; } = null!;
        public string Password { get; } = null!;
        public DateTime BirthDate { get; }
        public string PhoneNumber { get; } = null!;
        public string Address { get; } = null!;
        public string PositionName { get; } = null!;
        public DateTime HireDate { get; set; }

        public RegisterEmployee(string name, string surname, string email, string password, DateTime birthDate, string phoneNumber,
       string address, string positionName, DateTime hireDate)
        {
            Name = name;
            Surname = surname;
            Email = email;
            Password = password;
            BirthDate = birthDate;
            PhoneNumber = phoneNumber;
            Address = address;
            PositionName = positionName;
            HireDate = hireDate;
        }
    }
}
