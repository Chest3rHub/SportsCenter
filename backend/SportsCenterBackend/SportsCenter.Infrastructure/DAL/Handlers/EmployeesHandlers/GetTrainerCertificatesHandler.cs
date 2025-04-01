using MediatR;
using Microsoft.EntityFrameworkCore;
using SportsCenter.Application.Employees.Queries.GetTrainerCertificates;
using SportsCenter.Application.Exceptions.EmployeesException;
using SportsCenter.Application.Exceptions.EmployeesExceptions;
using SportsCenter.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Infrastructure.DAL.Handlers.EmployeesHandlers
{
    internal class GetTrainerCertificatesHandler : IRequestHandler<GetTrainerCertificates, IEnumerable<TrainerCertificateDto>>
    {
        private readonly SportsCenterDbContext _dbContext;

        public GetTrainerCertificatesHandler(SportsCenterDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<TrainerCertificateDto>> Handle(GetTrainerCertificates request, CancellationToken cancellationToken)
        {

            var employee = await GetEmployeeByIdAsync(request.TrainerId, cancellationToken);
            if (employee == null)
            {
                throw new EmployeeNotFoundException(request.TrainerId);
            }

            var trainerEmployeeTypeId = await GetEmployeeTypeByNameAsync("Trener", cancellationToken);

            var trainer = await _dbContext.Pracowniks
                .Where(p => p.PracownikId == request.TrainerId && p.IdTypPracownika == trainerEmployeeTypeId)
                .FirstOrDefaultAsync(cancellationToken);

            if (trainer == null)
            {
                throw new NotTrainerEmployeeException(request.TrainerId);
            }

            int PageSize = 6;
            int NumberPerPage = 7;

            var trainerCertificatesQuery = _dbContext.TrenerCertyfikats
                .Where(tc => tc.PracownikId == request.TrainerId)
                .OrderByDescending(tc => tc.DataOtrzymania)
                .Skip(request.Offset * PageSize)
                .Take(NumberPerPage)
                .AsNoTracking();

            var trainerCertificates = await trainerCertificatesQuery
                .Join(_dbContext.Certyfikats,
                    tc => tc.CertyfikatId,
                    c => c.CertyfikatId,
                    (tc, c) => new TrainerCertificateDto
                    {
                        CertificateName = c.Nazwa,
                        ReceivedDate = tc.DataOtrzymania
                    })
                .ToListAsync(cancellationToken);

            return trainerCertificates;

        }

        private async Task<int?> GetEmployeeTypeByNameAsync(string name, CancellationToken cancellationToken)
        {
            return await _dbContext.TypPracownikas
                .Where(t => t.Nazwa == name)
                .Select(t => (int?)t.IdTypPracownika)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<Pracownik?> GetEmployeeByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _dbContext.Pracowniks
                .Include(p => p.PracownikNavigation)
                .FirstOrDefaultAsync(p => p.PracownikNavigation.OsobaId == id, cancellationToken);
        }
    }
}
