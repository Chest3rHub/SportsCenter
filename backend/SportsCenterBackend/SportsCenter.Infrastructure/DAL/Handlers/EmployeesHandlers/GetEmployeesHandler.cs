using MediatR;
using Microsoft.EntityFrameworkCore;
using SportsCenter.Application.Employees.Queries.GetEmployees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Infrastructure.DAL.Handlers.EmployeesHandlers
{
    internal class GetEmployeesHandler : IRequestHandler<GetEmployees, IEnumerable<EmployeeDto>>
    {
        private readonly SportsCenterDbContext _dbContext;

        public GetEmployeesHandler(SportsCenterDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<EmployeeDto>> Handle(GetEmployees request, CancellationToken cancellationToken)
        {
            // zwraca 7 ale wyswietlanych jest tylko 6 zeby bylo wiadomo czy odpytywac ze zwiekszonym 
            // offsetem czy juz wiecej rekordow nie ma wiec sie nie da i tak
            int pageSize = 6;
            int numberPerPage = 7;
            return await _dbContext.Pracowniks.Include(x => x.PracownikNavigation)
                .Select(p => new EmployeeDto
                {
                    Id = p.PracownikNavigation.OsobaId,
                    FullName = p.PracownikNavigation.Imie + " " + p.PracownikNavigation.Nazwisko,
                    Email = p.PracownikNavigation.Email,
                    Role = p.IdTypPracownikaNavigation.Nazwa,
                    FireDate = p.DataZwolnienia,
                })
                .OrderByDescending(p => p.Id)//od najnowszych
                .Skip(request.Offset * pageSize)
                .Take(numberPerPage)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
    }
}
