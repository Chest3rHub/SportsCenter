using MediatR;
using Microsoft.AspNetCore.Http;
using SportsCenter.Application.Exceptions.EmployeesExceptions;
using SportsCenter.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Employees.Commands.AcceptAbsenceRequest
{
    internal sealed class AcceptAbsenceRequestHandler : IRequestHandler<AcceptAbsenceRequest, Unit>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AcceptAbsenceRequestHandler(IEmployeeRepository employeeRepository, IHttpContextAccessor httpContextAccessor)
        {
            _employeeRepository = employeeRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<Unit> Handle(AcceptAbsenceRequest request, CancellationToken cancellationToken)
        {
            var requestExist =  await _employeeRepository.ExistsAbsenceRequestAsync(request.RequestId, cancellationToken);

            if (!requestExist)
            {
               throw new AbsenceRequestNotFoundException(request.RequestId);
            }

            var isPendingRequest = await _employeeRepository.IsAbsenceRequestPendingAsync(request.RequestId, cancellationToken);

            if (!isPendingRequest)
            {
                throw new AbsenceRequestAlreadyAcceptedException(request.RequestId);
            }

            await _employeeRepository.UpdateAbsenceRequestAsync(request.RequestId, cancellationToken);

            return Unit.Value;
        }
    }
}
