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

namespace SportsCenter.Application.Employees.Commands.AddTask
{
    internal sealed class AddTaskHandler : IRequestHandler<AddTask, Unit>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AddTaskHandler(IEmployeeRepository employeeRepository, IHttpContextAccessor httpContextAccessor)
        {
            _employeeRepository = employeeRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Unit> Handle(AddTask request, CancellationToken cancellationToken)
        {
            var pracownik = await _employeeRepository.GetEmployeeByIdAsync(request.PracownikId, cancellationToken);

            if (pracownik == null)
            {
                throw new EmployeeNotFoundException(request.PracownikId);
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
