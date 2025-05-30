﻿using MediatR;
using Microsoft.AspNetCore.Http;
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
    internal class GetEmployeesTasksHandler : IRequestHandler<GetTasks, IEnumerable<TaskDto>>
    {

    private readonly SportsCenterDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetEmployeesTasksHandler(SportsCenterDbContext dbContext, IHttpContextAccessor httpContextAccessor)
    {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
    }

    public async Task<IEnumerable<TaskDto>> Handle(GetTasks request, CancellationToken cancellationToken)
    {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("userId")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int employeeId))
            {
                throw new UnauthorizedAccessException("You cannot access your tasks without being logged in on your account.");
            }

            int PageSize = 7;
            int NumberPerPage = 8;

            var tasks = await _dbContext.Zadanies
           .Where(t => t.PracownikId == employeeId)
           .OrderByDescending(t => t.DataDo) //Sortowane od najbliższego terminu
           .Skip(request.Offset * PageSize)
           .Take(NumberPerPage)
           .Select(t => new TaskDto
           {
               TaskId=t.ZadanieId,
               Description = t.Opis,
               DateTo = (DateOnly)t.DataDo,
               EmpId =t.PracownikId,
               AssigningEmpId= t.PracownikZlecajacyId
           })
           .AsNoTracking()
           .ToListAsync(cancellationToken);
            return tasks;
        }
    }
}
