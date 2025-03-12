using MediatR;
using SportsCenter.Application.Exceptions;
using SportsCenter.Application.Exceptions.EmployeesException;
using SportsCenter.Application.Exceptions.UsersException;
using SportsCenter.Application.Security;
using SportsCenter.Core.Entities;
using SportsCenter.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Employees.Commands.RegisterEmployee
{
    internal class RegisterEmployeeHandler : IRequestHandler<RegisterEmployee, Unit>
    {

        private readonly IUserRepository _userRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IPasswordManager _passwordManager;

        public RegisterEmployeeHandler(IUserRepository userRepository, IEmployeeRepository employeeRepository, IPasswordManager passwordManager)
        {
            _userRepository = userRepository;
            _employeeRepository = employeeRepository;
            _passwordManager = passwordManager;
        }
        public async Task<Unit> Handle(RegisterEmployee request, CancellationToken cancellationToken)
        {

            var existingUser = await _userRepository.GetUserByEmailAsync(request.Email, cancellationToken);
            if (existingUser != null) throw new UserAlreadyExistsException(request.Email);

            var employeeType = await _employeeRepository.GetTypeOfEmployeeIdAsync(request.PositionName, cancellationToken);
            if (employeeType == null) throw new WrongPositionNameException(request.PositionName);

            var employeeTypeId = employeeType.IdTypPracownika;

            var securedPassword = _passwordManager.Secure(request.Password);

            var newOsoba = new Osoba
            {
                Imie = request.Name,
                Nazwisko = request.Surname,
                Adres = request.Address,
                DataUr = DateOnly.FromDateTime(request.BirthDate),
                Email = request.Email,
                Haslo = securedPassword,
                NrTel = request.PhoneNumber,
                
            };

            var newEmployee = new Pracownik
            {
                IdTypPracownika = employeeTypeId,
                DataZatrudnienia = DateOnly.FromDateTime(request.HireDate),
                PracownikNavigation = newOsoba
            };

            await _employeeRepository.AddEmployeeAsync(newEmployee, cancellationToken);

            return Unit.Value;

        }
    }
}
