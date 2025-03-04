using Microsoft.EntityFrameworkCore;
using SportsCenter.Core.Entities;
using SportsCenter.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Infrastructure.DAL.Repositories;

public class SportsCenterRepository : ISportsCenterRepository
{
    private SportsCenterDbContext _dbContext;

    public SportsCenterRepository(SportsCenterDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    //public async Task<bool> CheckIfDayExistsAsync(int dzienTygodniaId, CancellationToken cancellationToken)
    //{    
    //    return await _dbContext.DzienTygodnium
    //        .AnyAsync(d => d.DzienTygodniaId == dzienTygodniaId, cancellationToken);
    //}

    //public async Task AddWorkingHoursForGivenDay(GodzinyPracyKlubu workingHoursOfDay, CancellationToken cancellationToken)
    //{
    //    await _dbContext.GodzinyPracyKlubu.AddAsync(workingHoursOfDay, cancellationToken);
    //    await _dbContext.SaveChangesAsync(cancellationToken);
    //}

    //public async Task<GodzinyPracyKlubu> GetWorkingHoursByDayAsync(int dayOfWeekId, CancellationToken cancellationToken)
    //{
    //    return await _dbContext.GodzinyPracyKlubu
    //                         .FirstOrDefaultAsync(g => g.DzienTygodniaId == dayOfWeekId, cancellationToken);
   // }
    //public async Task UpdateWorkingHours(GodzinyPracyKlubu godzinyPracy, CancellationToken cancellationToken)
    //{
    //    _dbContext.GodzinyPracyKlubu.Update(godzinyPracy);
    //    await _dbContext.SaveChangesAsync(cancellationToken);
    //}
}
