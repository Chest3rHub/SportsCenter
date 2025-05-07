using MediatR;
using Microsoft.EntityFrameworkCore;
using SportsCenter.Application.Employees.Queries.GetAvailableTrainers;
using SportsCenter.Core.Entities;
using SportsCenter.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SportsCenter.Core.Enums.TrainerAvailiabilityStatus;

namespace SportsCenter.Infrastructure.DAL.Handlers.EmployeesHandlers
{
    internal class GetAvailableTrainersHandler : IRequestHandler<GetAvailableTrainers, IEnumerable<TrainerDto>>
    {
        private readonly IEmployeeRepository _employeeService;

        public GetAvailableTrainersHandler(IEmployeeRepository employeeService)
        {
            _employeeService = employeeService;
        }

        public async Task<IEnumerable<TrainerDto>> Handle(GetAvailableTrainers request, CancellationToken cancellationToken)
        {
            var trainers = await _employeeService.GetAvailableTrainersAsync(request.StartTime, request.EndTime, cancellationToken);

            return trainers.Select(t => new TrainerDto
            {
                Id = t.PracownikId,
                FullName = $"{t.PracownikNavigation.Imie} {t.PracownikNavigation.Nazwisko}"
            });
        }
    }
}
