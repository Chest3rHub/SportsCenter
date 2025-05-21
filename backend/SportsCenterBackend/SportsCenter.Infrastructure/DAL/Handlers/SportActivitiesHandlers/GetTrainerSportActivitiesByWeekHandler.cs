using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SportsCenter.Application.Activities.Queries.GetTrainerSportActivitiesByWeeks;
using SportsCenter.Application.Activities.Queries.GetYourSportActivitiesByWeeks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Infrastructure.DAL.Handlers.SportActivitiesHandlers
{
    internal class GetTrainerSportActivitiesByWeekHandler : IRequestHandler<GetTrainerSportActivitiesByWeek, IEnumerable<TrainerSportActivityByWeeksDto>>
    {

        private readonly SportsCenterDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetTrainerSportActivitiesByWeekHandler(SportsCenterDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<TrainerSportActivityByWeeksDto>> Handle(GetTrainerSportActivitiesByWeek request, CancellationToken cancellationToken)
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("userId")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                throw new UnauthorizedAccessException("You cannot access your absence requests without being logged in on your account.");
            }

            DateOnly today = DateOnly.FromDateTime(DateTime.Now);
            int daysSinceMonday = ((int)today.DayOfWeek + 6) % 7;
            DateOnly startOfWeek = today.AddDays(-daysSinceMonday).AddDays(request.WeekOffset * 7);
            DateOnly endOfWeek = startOfWeek.AddDays(6);

            var startDateTime = startOfWeek.ToDateTime(TimeOnly.MinValue);
            var endDateTime = endOfWeek.ToDateTime(TimeOnly.MaxValue);

            DateOnly startDate = DateOnly.FromDateTime(startDateTime);
            DateOnly endDate = DateOnly.FromDateTime(endDateTime);

            var sportActivities = await _dbContext.InstancjaZajecKlients
                .Where(ik => ik.InstancjaZajec.GrafikZajec.PracownikId == userId)
                .Join(
                    _dbContext.InstancjaZajecs,
                    ik => ik.InstancjaZajecId,
                    iz => iz.InstancjaZajecId,
                    (ik, iz) => new { ik, iz }
                )
                .Where(combined => combined.iz.Data >= startDate && combined.iz.Data <= endDate)
                .Join(
                    _dbContext.GrafikZajecs,
                    combined => combined.iz.GrafikZajecId,
                    g => g.GrafikZajecId,
                    (combined, g) => new TrainerSportActivityByWeeksDto
                    {
                        InstanceOfActivityId = combined.iz.InstancjaZajecId,
                        SportActivityName = g.Zajecia.Nazwa,
                        DateOfActivity = combined.iz.Data,
                        DayOfWeek = combined.iz.Data.DayOfWeek.ToString(),
                        StartTime = g.GodzinaOd,
                        DurationInMinutes = g.CzasTrwania,
                        EndTime = g.GodzinaOd.Add(TimeSpan.FromMinutes(g.CzasTrwania)),
                        LevelName = g.Zajecia.IdPoziomZajecNavigation.Nazwa,
                        EmployeeId = g.PracownikId,
                        CourtName = g.Kort.Nazwa,
                        IsActivityCanceled = combined.iz.CzyOdwolane.HasValue
                            ? (combined.iz.CzyOdwolane.Value ? "Tak" : "Nie")
                            : "Nie"
                    })
                .OrderBy(activity => activity.DateOfActivity)
                .ThenBy(activity => activity.StartTime)
                .ToListAsync(cancellationToken);

            var reservations = await _dbContext.Rezerwacjas
                .Include(r => r.Kort)
                .Where(r => r.TrenerId == userId &&
                        r.DataOd >= startDateTime &&
                        r.DataDo <= endDateTime)
                .Select(r => new TrainerSportActivityByWeeksDto
                {
                    ReservationId = r.RezerwacjaId,
                    Type = "Rezerwacja",
                    SportActivityName = "Rezerwacja",
                    DateOfActivity = DateOnly.FromDateTime(r.DataOd),
                    DayOfWeek = r.DataOd.DayOfWeek.ToString(),
                    StartTime = r.DataOd.TimeOfDay,
                    DurationInMinutes = (int)(r.DataDo - r.DataOd).TotalMinutes,
                    EndTime = r.DataDo.TimeOfDay,
                    EmployeeId = userId,
                    ClientName = r.Klient.KlientNavigation.Imie,
                    ClientSurame = r.Klient.KlientNavigation.Nazwisko,
                    CourtName = r.Kort.Nazwa,                    
                    IsEquipmentReserved = r.CzyUwzglednicSprzet == true ? "Tak" : "Nie",                 
                    IsActivityCanceled = r.CzyOdwolana == true ? "Tak" : "Nie"

                })
                .OrderBy(reservation => reservation.DateOfActivity)
                .ThenBy(reservation => reservation.StartTime)
                .ToListAsync(cancellationToken);

            var allActivities = sportActivities.ToList();
            allActivities.AddRange(reservations);

            return allActivities
                .OrderBy(a => a.DateOfActivity)
                .ThenBy(a => a.StartTime)
                .ToList();
        }
    }
}
