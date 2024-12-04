using MediatR;
using Microsoft.AspNetCore.Http;
using SportsCenter.Application.Exceptions.EmployeesException;
using SportsCenter.Core.Entities;
using SportsCenter.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//klasa przeznaczona do realizacji przypadku "Dodawanie zadań to-do wybranemu pracownikowi administracyjnemu"

namespace SportsCenter.Application.Employees.Commands.AddTask
{
    internal sealed class AddTaskForAdmEmployeeHandler : IRequestHandler<AddTask, Unit>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AddTaskForAdmEmployeeHandler(IEmployeeRepository employeeRepository, IHttpContextAccessor httpContextAccessor)
        {
            _employeeRepository = employeeRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Unit> Handle(AddTask request, CancellationToken cancellationToken)
        {
            var employee = await _employeeRepository.GetEmployeeByIdAsync(request.PracownikId, cancellationToken);

            if (employee == null)
            {
                throw new EmployeeNotFoundException(request.PracownikId);
            }

            int employeeTypeId = employee.IdTypPracownika;

            //2 to id zarezerowane w tebeli slownikowej TypPracownika dla pracownika Administracyjnego
            if (employeeTypeId != 2)
            {
                throw new NotAdmEmployeeException(request.PracownikId);
            }

            var newTask = new Zadanie
            {
                Opis = request.Opis,
                DataDo = DateOnly.FromDateTime(request.DataDo),
                PracownikId = request.PracownikId,
                PracownikZlecajacyId = request.PracownikZlecajacyId,
            };

            await _employeeRepository.AddTaskAsync(newTask, cancellationToken);

            return Unit.Value;
        }
    }

}
