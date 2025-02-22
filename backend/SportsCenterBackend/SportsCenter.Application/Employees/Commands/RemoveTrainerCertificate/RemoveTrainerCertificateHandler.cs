using MediatR;
using Microsoft.AspNetCore.Http;
using SportsCenter.Application.Exceptions.EmployeesException;
using SportsCenter.Application.Exceptions.EmployeesExceptions;
using SportsCenter.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Employees.Commands.DeleteTrainerCertificate
{
    internal sealed class RemoveTrainerCertificateHandler : IRequestHandler<RemoveTrainerCertificate, Unit>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RemoveTrainerCertificateHandler(IEmployeeRepository employeeRepository, IHttpContextAccessor httpContextAccessor)
        {
            _employeeRepository = employeeRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Unit> Handle(RemoveTrainerCertificate request, CancellationToken cancellationToken)
        {
            var employee = await _employeeRepository.GetEmployeeByIdAsync(request.TrainerId, cancellationToken);
            if (employee == null) { 
                throw new EmployeeNotFoundException(request.TrainerId);
            }

            int trainerEmployeeId = (int)await _employeeRepository.GetEmployeeTypeByNameAsync("Trener", cancellationToken);

            if (employee.IdTypPracownika != trainerEmployeeId)
            {
                throw new NotTrainerEmployeeException(employee.PracownikId);
            }

            var certificate = await _employeeRepository.GetTrainerCertificateByIdAsync(request.TrainerId, request.CertyficateId, cancellationToken);

            if (certificate == null)
            {
                throw new TrainerCertificateNotFoundException(request.CertyficateId, request.TrainerId);
            }

            await _employeeRepository.DeleteTrainerCertificateAsync(certificate, cancellationToken);

            return Unit.Value;
        }
    }
}
