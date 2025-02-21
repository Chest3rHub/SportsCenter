using MediatR;
using Microsoft.AspNetCore.Http;
using SportsCenter.Application.Exceptions.EmployeesException;
using SportsCenter.Application.Exceptions.EmployeesExceptions;
using SportsCenter.Core.Entities;
using SportsCenter.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Employees.Commands.AddTrainerCertificate
{
    internal sealed class AddTrainerCertificateHandler : IRequestHandler<AddTrainerCertificate, Unit>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AddTrainerCertificateHandler(IEmployeeRepository employeeRepository, IHttpContextAccessor httpContextAccessor)
        {
            _employeeRepository = employeeRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Unit> Handle(AddTrainerCertificate request, CancellationToken cancellationToken)
        {
            var trainer = await _employeeRepository.GetEmployeeByIdAsync(request.TrainerId, cancellationToken);
            if (trainer == null)
            {
                throw new EmployeeNotFoundException(request.TrainerId);
            }

            int trainerEmployeeId = (int)await _employeeRepository.GetEmployeeTypeByNameAsync("Trener", cancellationToken);

            if (trainer.IdTypPracownika != trainerEmployeeId)
            {
                throw new NotTrainerEmployeeException(trainer.PracownikId);
            }

            var newCertificate = new Certyfikat
            {
                Nazwa = request.CertificateName
            };

            var newTrainerCertificate = new TrenerCertifikat
            {
                Certyfikat = newCertificate,
                PracownikId = request.TrainerId,
                DataOtrzymania = request.ReceivedDate
            };

            await _employeeRepository.AddTrainerCertificateAsync(newTrainerCertificate, cancellationToken);
            return Unit.Value;
        }
    }
}
