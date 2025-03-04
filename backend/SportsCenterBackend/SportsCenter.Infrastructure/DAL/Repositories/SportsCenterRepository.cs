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

    public async Task<bool> CheckIfDayExistsAsync(string dayOfWeek, CancellationToken cancellationToken)
    {
        return await _dbContext.GodzinyPracyKlubus
            .AnyAsync(d => d.DzienTygodnia == dayOfWeek, cancellationToken);
    }

    public async Task AddWorkingHoursForGivenDay(GodzinyPracyKlubu workingHoursOfDay, CancellationToken cancellationToken)
    {
        await _dbContext.GodzinyPracyKlubus.AddAsync(workingHoursOfDay, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<GodzinyPracyKlubu> GetWorkingHoursByDayAsync(string dayOfWeek, CancellationToken cancellationToken)
    {
        return await _dbContext.GodzinyPracyKlubus
                             .FirstOrDefaultAsync(g => g.DzienTygodnia == dayOfWeek, cancellationToken);
    }
    public async Task UpdateWorkingHours(GodzinyPracyKlubu workingHours, CancellationToken cancellationToken)
    {
        _dbContext.GodzinyPracyKlubus.Update(workingHours);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<WyjatkoweGodzinyPracy> GetWorkingHoursByDateAsync(DateTime date, CancellationToken cancellationToken)
    {
        return await _dbContext.WyjatkoweGodzinyPracies
                             .FirstOrDefaultAsync(g => g.Data == DateOnly.FromDateTime(date), cancellationToken);
    }

    public async Task UpdateDateWorkingHours(WyjatkoweGodzinyPracy workingHours, CancellationToken cancellationToken)
    {
        _dbContext.WyjatkoweGodzinyPracies.Update(workingHours);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task AddWorkingHoursForGivenDate(WyjatkoweGodzinyPracy workingHoursOfDay, CancellationToken cancellationToken)
    {
        await _dbContext.WyjatkoweGodzinyPracies.AddAsync(workingHoursOfDay, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
