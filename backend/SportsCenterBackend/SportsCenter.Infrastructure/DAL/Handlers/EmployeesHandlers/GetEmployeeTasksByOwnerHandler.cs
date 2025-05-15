using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SportsCenter.Application.Employees.Queries.GetEmployeeTasksByOwner;
using SportsCenter.Application.Employees.Queries.GetTasks;

namespace SportsCenter.Infrastructure.DAL.Handlers.EmployeesHandlers
{
    internal class GetEmployeeTasksByOwnerHandler : IRequestHandler<GetEmployeeTasksByOwner, IEnumerable<TaskDto>>
    {
        private readonly SportsCenterDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetEmployeeTasksByOwnerHandler(SportsCenterDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<TaskDto>> Handle(GetEmployeeTasksByOwner request, CancellationToken cancellationToken)
        {
            var ownerId = await _dbContext.Pracowniks
                .Where(p => p.IdTypPracownika == 1)
                .Select(p => p.PracownikId)
                .FirstOrDefaultAsync(cancellationToken);

            if (ownerId == 0)
            {
                throw new Exception("Owner not found");
            }

            var tasks = await _dbContext.Zadanies
                .Where(t => t.PracownikId == request.EmployeeId && t.PracownikZlecajacyId == ownerId)
                .OrderByDescending(t => t.DataDo)
                .Select(t => new TaskDto
                {
                    TaskId = t.ZadanieId,
                    Description = t.Opis,
                    DateTo = (DateOnly)t.DataDo,
                    EmpId = t.PracownikId,
                    AssigningEmpId = t.PracownikZlecajacyId
                })
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return tasks;
        }
    }
}