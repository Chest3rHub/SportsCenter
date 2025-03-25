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

    public async Task<WyjatkoweGodzinyPracy> GetSpecialWorkingHoursByDateAsync(DateTime date, CancellationToken cancellationToken)
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
    public async Task<List<object>> GetConflictingReservationsOrActivities(DateOnly date, TimeOnly newOpenHour, TimeOnly newCloseHour, CancellationToken cancellationToken)
    {
        var newOpenTime = newOpenHour.ToTimeSpan();
        var newCloseTime = newCloseHour.ToTimeSpan();
        var dateStart = date.ToDateTime(newOpenHour);

        var conflictingReservations = await _dbContext.Rezerwacjas
            .Where(r => r.DataOd.Date == date.ToDateTime(new TimeOnly()).Date &&
                        (r.DataOd.TimeOfDay < newOpenTime || r.DataDo.TimeOfDay > newCloseTime))
            .ToListAsync(cancellationToken);

        var conflictingClasses = await _dbContext.InstancjaZajecs
            .Include(iz => iz.GrafikZajec)
            .ToListAsync(cancellationToken);

        var newOpenTimeSpan = newOpenHour.ToTimeSpan();
        var newCloseTimeSpan = newCloseHour.ToTimeSpan();

        var conflictingClassesInSpecifiedTime = conflictingClasses
            .Where(iz => iz.GrafikZajec != null &&
                  iz.GrafikZajec.GodzinaOd != null &&
                  (iz.GrafikZajec.GodzinaOd < newOpenTimeSpan ||
                   iz.GrafikZajec.GodzinaOd.Add(TimeSpan.FromMinutes(iz.GrafikZajec.CzasTrwania)) > newCloseTimeSpan))
             .Where(iz => iz.Data == date)
            .ToList();

        var conflicts = new List<object>();
        conflicts.AddRange(conflictingReservations);
        conflicts.AddRange(conflictingClassesInSpecifiedTime);

        return conflicts;
    }
    private static readonly Dictionary<DayOfWeek, string> DayOfWeekMapping = new Dictionary<DayOfWeek, string>
    {
        { DayOfWeek.Sunday, "niedziela" },
        { DayOfWeek.Monday, "poniedzialek" },
        { DayOfWeek.Tuesday, "wtorek" },
        { DayOfWeek.Wednesday, "sroda" },
        { DayOfWeek.Thursday, "czwartek" },
        { DayOfWeek.Friday, "piatek" },
        { DayOfWeek.Saturday, "sobota" }
    };
    public async Task<IEnumerable<object>> GetConflictingReservationsOrActivitiesByDayOfWeek(
    DayOfWeek dayOfWeek, TimeOnly newOpenHour, TimeOnly newCloseHour, CancellationToken cancellationToken)
    {
        string dayOfWeekInPolish = DayOfWeekMapping[dayOfWeek];
        var today = DateOnly.FromDateTime(DateTime.Today);

        var reservations = await _dbContext.Rezerwacjas
            .Where(r => (!r.CzyOdwolana.HasValue || r.CzyOdwolana == false) &&
                        DateOnly.FromDateTime(r.DataOd) >= today)
            .ToListAsync(cancellationToken);

        Console.WriteLine($"Znaleziono {reservations.Count} aktywnych przyszłych rezerwacji.");

        var conflictingReservations = reservations
            .Where(r => r.DataOd.DayOfWeek == dayOfWeek &&
                       (r.DataOd.TimeOfDay < newOpenHour.ToTimeSpan() ||
                        r.DataDo.TimeOfDay > newCloseHour.ToTimeSpan()))
            .ToList();

        Console.WriteLine($"Znaleziono {conflictingReservations.Count} kolidujących rezerwacji.");

        var conflictingClasses = await _dbContext.InstancjaZajecs
            .Where(i => (!i.CzyOdwolane.HasValue || i.CzyOdwolane == false) &&
                        i.Data >= today)
            .Where(i => i.GrafikZajec.DzienTygodnia == dayOfWeekInPolish)
            .Include(i => i.GrafikZajec)
            .ToListAsync(cancellationToken);

        var conflictingClassesWithAdjustedTime = conflictingClasses
            .Where(i => i.GrafikZajec.GodzinaOd < newOpenHour.ToTimeSpan() ||
                        i.GrafikZajec.GodzinaOd.Add(TimeSpan.FromMinutes(i.GrafikZajec.CzasTrwania)) > newCloseHour.ToTimeSpan())
            .ToList();

        foreach (var activity in conflictingClassesWithAdjustedTime)
        {
            Console.WriteLine($"Zajęcia ID: {activity.GrafikZajecId} | Data: {activity.Data} | Od: {activity.GrafikZajec.GodzinaOd} | Czas trwania: {activity.GrafikZajec.CzasTrwania} min");
        }

        var conflicts = new List<object>();
        conflicts.AddRange(conflictingReservations);
        conflicts.AddRange(conflictingClassesWithAdjustedTime);

        return conflicts;
    }
}
