using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using SportsCenter.Application.Exceptions.SportActivitiesException;
using SportsCenter.Core.Entities;
using SportsCenter.Core.Repositories;

namespace SportsCenter.Infrastructure.DAL.Repositories;

public class SportActivityRepository : ISportActivityRepository
{
    private SportsCenterDbContext _dbContext;

    public SportActivityRepository(SportsCenterDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddSportActivityAsync(Zajecium sportActivity, CancellationToken cancellationToken)
    {
        _dbContext.Zajecia.Add(sportActivity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task AddScheduleAsync(GrafikZajec schedule, CancellationToken cancellationToken)
    {
        _dbContext.GrafikZajecs.Add(schedule);
        await _dbContext.SaveChangesAsync();
    }
    
    public async Task<Zajecium> GetSportActivityByIdAsync(int sportActivityId, CancellationToken cancellationToken)
    {
        return await _dbContext.Zajecia
            .Include(sa => sa.IdPoziomZajecNavigation)
            .Include(sa => sa.GrafikZajecs)
            .FirstOrDefaultAsync(sa => sa.ZajeciaId == sportActivityId, cancellationToken);
    }
    public async Task<IEnumerable<Zajecium>> GetAllSportActivitiesAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.Zajecia
            .Include(sa => sa.IdPoziomZajec)
            .Include(sa => sa.GrafikZajecs)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
    //public async Task RemoveSportActivityAsync(Zajecium sportActivity, CancellationToken cancellationToken)
    //{
    //    var grafikZajec = await _dbContext.GrafikZajecs
    //        .Where(g => g.ZajeciaId == sportActivity.ZajeciaId)
    //        .ToListAsync(cancellationToken);

    //    if (grafikZajec.Any())
    //    {
    //        _dbContext.GrafikZajecs.RemoveRange(grafikZajec);
    //    }

    //    _dbContext.Zajecia.Remove(sportActivity);
    //    await _dbContext.SaveChangesAsync(cancellationToken);
    //}

    public async Task RemoveSportActivityAsync(Zajecium sportActivity, CancellationToken cancellationToken)
    {
        //garfik
        var grafikZajecList = await _dbContext.GrafikZajecs
            .Where(g => g.ZajeciaId == sportActivity.ZajeciaId)
            .ToListAsync(cancellationToken);

        foreach (var grafik in grafikZajecList)
        {
            //instancja
            var instancje = await _dbContext.InstancjaZajecs
                .Where(i => i.GrafikZajecId == grafik.GrafikZajecId)
                .ToListAsync(cancellationToken);

            foreach (var instancja in instancje)
            {
                //klienci zapisani na instancje
                var klienci = await _dbContext.InstancjaZajecKlients
                    .Where(k => k.InstancjaZajecId == instancja.InstancjaZajecId)
                    .ToListAsync(cancellationToken);

                foreach (var klient in klienci)
                {
                    //oceny instancji
                    var oceny = await _dbContext.Ocenas
                        .Where(o => o.InstancjaZajecKlientId == klient.InstancjaZajecKlientId)
                        .ToListAsync(cancellationToken);

                    _dbContext.Ocenas.RemoveRange(oceny);
                }

                _dbContext.InstancjaZajecKlients.RemoveRange(klienci);
            }

            _dbContext.InstancjaZajecs.RemoveRange(instancje);
        }

        _dbContext.GrafikZajecs.RemoveRange(grafikZajecList);
        _dbContext.Zajecia.Remove(sportActivity);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }



    public async Task<IEnumerable<GrafikZajec>> GetSchedulesByTrainerIdAsync(int trainerId, CancellationToken cancellationToken)
    {
        return await _dbContext.GrafikZajecs
            .Where(gz => gz.PracownikId == trainerId)
            .ToListAsync(cancellationToken);
    }

    public async Task<(DateTime? date, int? duration)> GetActivityDetailsByIdAsync(int activitiesId)
    {
        var activityDetails = await _dbContext.GrafikZajecs
            .Where(gz => gz.ZajeciaId == activitiesId)
            .Select(gz => new
            {
                gz.CzasTrwania,
                gz.DzienTygodnia,
                gz.GodzinaOd
            })
            .FirstOrDefaultAsync();

        if (activityDetails == null)
        {
            return (null, null);
        }

        string dzienTygodnia = activityDetails.DzienTygodnia;
        TimeSpan godzinaOd = activityDetails.GodzinaOd;

        var currentDate = DateTime.Now;

        var dayOfWeek = Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>()
            .FirstOrDefault(d => d.ToString() == dzienTygodnia);

        DateTime activityDate = currentDate.AddDays((int)dayOfWeek - (int)currentDate.DayOfWeek);
        if ((int)dayOfWeek <= (int)currentDate.DayOfWeek)
        {
            activityDate = activityDate.AddDays(7);
        }

        DateTime finalDate = activityDate.Add(godzinaOd);

        return (finalDate, activityDetails.CzasTrwania);
    }
    public async Task<bool> IsTrainerAssignedToActivityAsync(int activityId, int trainerId)
    {
        return await _dbContext.GrafikZajecs
            .Where(gz => gz.ZajeciaId == activityId && gz.PracownikId == trainerId)
            .AnyAsync();
    }
    public async Task<(DateTime date, TimeSpan startTime, TimeSpan endTime)?> GetActivityDetailsAsync(int activityId, CancellationToken cancellationToken)
    {

        var activity = await _dbContext.GrafikZajecs
            .Where(g => g.ZajeciaId == activityId)
            .Select(g => new { g.DzienTygodnia, g.GodzinaOd, g.CzasTrwania })
            .FirstOrDefaultAsync(cancellationToken);

        if (activity == null)
            return null;

        var currentDate = DateTime.Now;

        var dayOfWeek = Enum.GetValues(typeof(DayOfWeek))
            .Cast<DayOfWeek>()
            .FirstOrDefault(d => d.ToString() == activity.DzienTygodnia);

        DateTime activityDate = currentDate.AddDays((int)dayOfWeek - (int)currentDate.DayOfWeek);

        if ((int)dayOfWeek <= (int)currentDate.DayOfWeek)
        {
            activityDate = activityDate.AddDays(7);
        }

        TimeSpan startTime = activityDate.Date.Add(activity.GodzinaOd).TimeOfDay;
        TimeSpan endTime = startTime + TimeSpan.FromMinutes(activity.CzasTrwania);

        return (activityDate.Date, startTime, endTime);
    }
    public async Task<int> EnsureLevelNameExistsAsync(string levelName, CancellationToken cancellationToken = default)
    {
        var poziomZajec = await _dbContext.PoziomZajecs
                                       .FirstOrDefaultAsync(pz => pz.Nazwa == levelName, cancellationToken);

        if (poziomZajec != null)
        {         
            return poziomZajec.IdPoziomZajec;
        }

        var nowyPoziom = new PoziomZajec { Nazwa = levelName };
        _dbContext.PoziomZajecs.Add(nowyPoziom);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return nowyPoziom.IdPoziomZajec;
    }
    public async Task<InstancjaZajec> GetInstanceByScheduleAndDateAsync(GrafikZajec activitySchedule, DateOnly selectedDate, CancellationToken cancellationToken)
    {
       var selectedDateTime = selectedDate.ToDateTime(TimeOnly.MinValue);
        return await _dbContext.InstancjaZajecs
            .Where(i => i.GrafikZajecId == activitySchedule.GrafikZajecId && i.Data == DateOnly.FromDateTime(selectedDateTime.Date))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task AddInstanceAsync(InstancjaZajec instancjaZajec, CancellationToken cancellationToken)
    {
        await _dbContext.InstancjaZajecs.AddAsync(instancjaZajec, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
    public async Task<GrafikZajec?> GetScheduleByActivityIdAsync(int activityId, CancellationToken cancellationToken)
    {
        return await _dbContext.GrafikZajecs
            .Where(g => g.ZajeciaId == activityId)
            .FirstOrDefaultAsync(cancellationToken);
    }
    public async Task<bool> IsClientSignedUpAsync(int clientId, int instantionOfActivity, CancellationToken cancellationToken)
    {
        return await _dbContext.InstancjaZajecKlients
            .AnyAsync(i => i.KlientId == clientId && i.InstancjaZajecId == instantionOfActivity);
    }
    public async Task AddClientToInstanceAsync(InstancjaZajecKlient signUp, CancellationToken cancellationToken)
    {
        await _dbContext.InstancjaZajecKlients.AddAsync(signUp, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> CancelInstanceOfActivityAsync(int instantionOfActivityId, CancellationToken cancellationToken)
    {
        var instanceOfActivity = await _dbContext.InstancjaZajecs
            .FirstOrDefaultAsync(i => i.InstancjaZajecId == instantionOfActivityId);

        if (instanceOfActivity.CzyOdwolane == true)
        {
            return 0;
        }

            instanceOfActivity.CzyOdwolane = true;
            await _dbContext.SaveChangesAsync(cancellationToken);
        return 1;
    }
    public async Task<InstancjaZajec> GetInstanceOfActivityAsync(int instanceOfActivity, CancellationToken cancellationToken)
    {
        var instance = await _dbContext.InstancjaZajecs
            .Include(i => i.InstancjaZajecKlients)
            .ThenInclude(ik => ik.Klient)
            .Include(i => i.GrafikZajec)
            .Where(i => i.InstancjaZajecId == instanceOfActivity)
            .AsTracking()
            .FirstOrDefaultAsync(cancellationToken);

        return instance;
    }
    public async Task<InstancjaZajecKlient> GetInstanceOfActivityClientAsync(int instanceOfActivityId, CancellationToken cancellationToken)
    {
        return await _dbContext.InstancjaZajecKlients
            .Where(gzk => gzk.InstancjaZajecId == instanceOfActivityId)
            .FirstOrDefaultAsync(cancellationToken);
    }
    public async Task<Zajecium> GetActivityByInstanceOfActivityIdAsync(int instanceOfActivityId, CancellationToken cancellationToken)
    {
        var instanceOfActivity = await _dbContext.InstancjaZajecs
            .Where(i => i.InstancjaZajecId == instanceOfActivityId)
            .FirstOrDefaultAsync(cancellationToken);

        var activitySchedule = await _dbContext.GrafikZajecs
            .Where(gz => gz.GrafikZajecId == instanceOfActivity.GrafikZajecId)
            .FirstOrDefaultAsync(cancellationToken);

        var activity = await _dbContext.Zajecia
            .Where(z => z.ZajeciaId == activitySchedule.ZajeciaId)
            .FirstOrDefaultAsync(cancellationToken);

        return activity;
    }
    public async Task<InstancjaZajec?> GetInstanceOfActivityAsync(int activityId, DateOnly date, CancellationToken cancellationToken)
    {
        return await _dbContext.InstancjaZajecs
            .Where(i => i.GrafikZajec.ZajeciaId == activityId && i.Data == date)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> IsClientAvailableForActivityAsync(int clientId, int activityId, DateOnly selectedDate, CancellationToken cancellationToken)
    {
        //grafik zajec na podstawie przekazanego id zajec 
        var schedule = await _dbContext.GrafikZajecs
            .Where(g => g.ZajeciaId == activityId)
            .FirstOrDefaultAsync(cancellationToken);

        if (schedule == null)
            throw new SportActivityNotFoundException(activityId);

        //Obliczenie czasu godzina od i do
        var startDateTime = selectedDate.ToDateTime(TimeOnly.FromTimeSpan(schedule.GodzinaOd));
        var endDateTime = startDateTime.AddMinutes(schedule.CzasTrwania);

        //Czy w tym czasie jest kolizja z innymi zaj
        var conflictList = _dbContext.InstancjaZajecKlients
            .Where(ik => ik.KlientId == clientId)
            .Join(_dbContext.InstancjaZajecs,
                ik => ik.InstancjaZajecId,
                iz => iz.InstancjaZajecId,
                (ik, iz) => new { iz.Data, iz.GrafikZajecId })
            .Join(_dbContext.GrafikZajecs,
                joined => joined.GrafikZajecId,
                g => g.GrafikZajecId,
                (joined, g) => new { joined.Data, g.GodzinaOd, g.CzasTrwania })
            .ToList();

        var hasActivityConflict = conflictList.Any(x =>
        {
            var start = x.Data.ToDateTime(TimeOnly.FromTimeSpan(x.GodzinaOd));
            var end = start.AddMinutes(x.CzasTrwania);
            return start < endDateTime && end > startDateTime;
        });

        if (hasActivityConflict)
            return false;

        //Czy jest kolizja w tym czasie z jakas rezerwacja
        var hasReservationConflict = await _dbContext.Rezerwacjas
            .AnyAsync(r =>
                r.KlientId == clientId &&
                r.DataOd < endDateTime &&
                r.DataDo > startDateTime,
                cancellationToken);

        return !hasReservationConflict;
    }


}