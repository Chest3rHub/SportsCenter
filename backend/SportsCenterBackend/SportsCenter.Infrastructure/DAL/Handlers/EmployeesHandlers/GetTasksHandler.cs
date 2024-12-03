using MediatR;
using Microsoft.EntityFrameworkCore;
using SportsCenter.Application.Employees.Queries.GetEmployees;
using SportsCenter.Application.Employees.Queries.GetTasks;
using SportsCenter.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Infrastructure.DAL.Handlers.EmployeesHandlers
{
    internal class GetTasksHandler : IRequestHandler<GetTasks, IEnumerable<TaskDto>>
    {

    private readonly SportsCenterDbContext _dbContext;

    public GetTasksHandler(SportsCenterDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<TaskDto>> Handle(GetTasks request, CancellationToken cancellationToken)
    {
            return await _dbContext.Zadanies
                     .Where(t => t.PracownikId == request.PracownikId)
                     .Select(t => new TaskDto
                     {            
                         Description = t.Opis,
                         //DateTo = t.DataDo
                     })
                     .AsNoTracking()  
                     .ToListAsync(cancellationToken); 
        }
}
}
