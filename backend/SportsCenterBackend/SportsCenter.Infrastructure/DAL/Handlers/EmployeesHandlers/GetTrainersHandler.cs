using MediatR;
using Microsoft.EntityFrameworkCore;
using SportsCenter.Application.Employees.Queries.GetEmployees;
using SportsCenter.Application.Employees.Queries.GetTrainers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Infrastructure.DAL.Handlers.EmployeesHandlers
{
    internal class GetTrainersHandler : IRequestHandler<GetTrainers, IEnumerable<TrainerDto>>
    {

        private readonly SportsCenterDbContext _dbContext;

        public GetTrainersHandler(SportsCenterDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<TrainerDto>> Handle(GetTrainers request, CancellationToken cancellationToken)
        {
            int pageSize = 6;
            int numberPerPage = 7;

            return await _dbContext.Pracowniks
               .Include(x => x.PracownikNavigation)
               .ThenInclude(pn => pn.Pracownik.IdTypPracownikaNavigation)
               .Where(p => p.PracownikNavigation.Pracownik.IdTypPracownikaNavigation.Nazwa == "Trener")
               .Select(p => new TrainerDto
               {
                   Id = p.PracownikNavigation.OsobaId,
                   FullName = p.PracownikNavigation.Imie + " " + p.PracownikNavigation.Nazwisko,
               })
               .OrderByDescending(p => p.Id)
               .Skip(request.Offset * pageSize)
               .Take(numberPerPage)
               .AsNoTracking()
               .ToListAsync(cancellationToken);
        }
    }
}
