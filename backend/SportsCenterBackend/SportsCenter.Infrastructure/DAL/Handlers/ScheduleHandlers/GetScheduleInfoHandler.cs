using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SportsCenter.Application.Schedule.Queries.GetScheduleInfo;
using SportsCenter.Infrastructure.DAL;
using System.Security.Claims;

internal class GetScheduleInfoHandler : IRequestHandler<GetScheduleInfo, List<ScheduleInfoBaseDto>>
{
    private readonly SportsCenterDbContext _dbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetScheduleInfoHandler(SportsCenterDbContext dbContext, IHttpContextAccessor httpContextAccessor)
    {
        _dbContext = dbContext;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<List<ScheduleInfoBaseDto>> Handle(GetScheduleInfo request, CancellationToken cancellationToken)
    {
        var user = _httpContextAccessor.HttpContext.User;
        var userRole = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value
                        ?? user.Claims.FirstOrDefault(c => c.Type == "role")?.Value
                        ?? user.Claims.FirstOrDefault(c => c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role")?.Value;

        bool isAdminRole = userRole == "Wlasciciel" || userRole == "Pracownik administracyjny";
        bool isTrainerRole = userRole == "Trener";
        bool isBasicRole = userRole == "Klient" || userRole == "Pomoc sprzatajaca" || string.IsNullOrEmpty(userRole);

        DateOnly today = DateOnly.FromDateTime(DateTime.Now);
        int daysSinceMonday = ((int)today.DayOfWeek + 6) % 7;
        DateOnly startOfWeek = today.AddDays(-daysSinceMonday).AddDays(request.WeekOffset * 7);
        DateOnly endOfWeek = startOfWeek.AddDays(6);

        var reservations = await _dbContext.Rezerwacjas
            .Where(r => r.DataOd.Date >= startOfWeek.ToDateTime(TimeOnly.MinValue) && r.DataDo.Date <= endOfWeek.ToDateTime(TimeOnly.MinValue))
            .Include(r => r.Klient)
            .ThenInclude(k => k.KlientNavigation)
            .Include(r => r.Kort)
            .Include(r => r.Trener)
            .ThenInclude(t => t.PracownikNavigation)
            .ToListAsync(cancellationToken);

        var scheduledClasses = await _dbContext.GrafikZajecs
            .Where(g => g.DzienTygodnia != null)
            .Include(g => g.Zajecia)
            .ThenInclude(z => z.IdPoziomZajecNavigation)
            .Include(g => g.Pracownik)
            .ThenInclude(t => t.PracownikNavigation)
            .Include(g => g.Kort)
            .Include(g => g.InstancjaZajec)
            .ThenInclude(i => i.InstancjaZajecKlients)
            .ThenInclude(ik => ik.Klient)
            .ThenInclude(k => k.KlientNavigation)
            .ToListAsync(cancellationToken);

        var scheduleInfoDtos = new List<ScheduleInfoBaseDto>();

        var dayOfWeekMap = new Dictionary<string, DayOfWeek>
        {
            { "poniedzialek", DayOfWeek.Monday },
            { "wtorek", DayOfWeek.Tuesday },
            { "sroda", DayOfWeek.Wednesday },
            { "czwartek", DayOfWeek.Thursday },
            { "piatek", DayOfWeek.Friday },
            { "sobota", DayOfWeek.Saturday },
            { "niedziela", DayOfWeek.Sunday }
        };
      
        foreach (var reservation in reservations)
        {
            ScheduleInfoBaseDto dto;

            if (isAdminRole)
            {
                dto = new ScheduleInfoAdminDto
                {
                    Type = "Admin",
                    Description = "Rezerwacja",
                    Id = reservation.RezerwacjaId,
                    Date = reservation.DataOd,
                    StartTime = reservation.DataOd.TimeOfDay,
                    EndTime = reservation.DataDo.TimeOfDay,
                    CourtName = reservation.Kort.Nazwa,
                    TrainerName = reservation.Trener?.PracownikNavigation != null
                        ? $"{reservation.Trener.PracownikNavigation.Imie} {reservation.Trener.PracownikNavigation.Nazwisko}"
                        : "Brak trenera",
                    ReservationCost = reservation.Koszt,
                    Discount = reservation.Klient?.ZnizkaNaZajecia ?? 0,
                    Participants = new List<ParticipantDto>
                    {
                        new ParticipantDto
                        {
                        FirstName = reservation.Klient?.KlientNavigation?.Imie,
                        LastName = reservation.Klient?.KlientNavigation?.Nazwisko,
                        IsPaid = reservation.CzyOplacona
                        }
                    },
                    IsEquipmentReserved = reservation.CzyUwzglednicSprzet,
                    IsCanceled = (bool)reservation.CzyOdwolana
                };
            }
            else if (isTrainerRole)
            {
                dto = new ScheduleInfoTrainerDto
                {
                    Type = "Trainer",
                    Description = "Rezerwacja",
                    Id = reservation.RezerwacjaId,
                    Date = reservation.DataOd,
                    StartTime = reservation.DataOd.TimeOfDay,
                    EndTime = reservation.DataDo.TimeOfDay,
                    CourtName = reservation.Kort.Nazwa,
                    TrainerName = reservation.Trener?.PracownikNavigation != null
                        ? $"{reservation.Trener.PracownikNavigation.Imie} {reservation.Trener.PracownikNavigation.Nazwisko}"
                        : "Brak trenera",
                    Participants = new List<ParticipantDto>
                    {
                        new ParticipantDto
                        {
                        FirstName = reservation.Klient?.KlientNavigation?.Imie,
                        LastName = reservation.Klient?.KlientNavigation?.Nazwisko,
                        IsPaid = null //trener nie powinien widziec tej informacji
                        }
                    },
                };
            }
            else
            {
                dto = new ScheduleInfoBasicDto
                {
                    Type = "Basic",
                    Description = "Rezerwacja",
                    Id = reservation.RezerwacjaId,
                    Date = reservation.DataOd,
                    StartTime = reservation.DataOd.TimeOfDay,
                    EndTime = reservation.DataDo.TimeOfDay,
                    CourtName = reservation.Kort.Nazwa,
                };
            }

            scheduleInfoDtos.Add(dto);
        }

        foreach (var scheduledClass in scheduledClasses)
        {
            ScheduleInfoBaseDto dto;

            DateTime startDate, startTime, endTime;

            var dayOfWeekString = scheduledClass.DzienTygodnia;

            if (dayOfWeekMap.TryGetValue(dayOfWeekString, out DayOfWeek dayOfWeek))
            {
                startDate = startOfWeek.ToDateTime(TimeOnly.MinValue).AddDays((int)dayOfWeek - (int)startOfWeek.DayOfWeek);

                if (dayOfWeek == DayOfWeek.Sunday)
                {
                    startDate = startDate.AddDays(7);
                }

                startTime = startDate.AddMinutes(scheduledClass.GodzinaOd.TotalMinutes);
                endTime = startTime.AddMinutes(scheduledClass.CzasTrwania);
            }
            else
            {
                throw new InvalidOperationException($"Unknown day of week: {dayOfWeekString}");
            }

            if (isAdminRole)
            {
                dto = new ScheduleInfoAdminDto
                {
                    Type = "Admin",
                    Description = "Zajęcia",
                    Id = scheduledClass.ZajeciaId,
                    Date = startDate,
                    StartTime = startTime.TimeOfDay,
                    EndTime = endTime.TimeOfDay,
                    CourtName = scheduledClass.Kort.Nazwa,
                    TrainerName = scheduledClass.Pracownik?.PracownikNavigation != null
                            ? $"{scheduledClass.Pracownik.PracownikNavigation.Imie} {scheduledClass.Pracownik.PracownikNavigation.Nazwisko}"
                            : "Brak trenera",
                    GroupName = scheduledClass.Zajecia?.Nazwa,
                    SkillLevel = scheduledClass.Zajecia?.IdPoziomZajecNavigation?.Nazwa,
                    CostWithEquipment = scheduledClass.KosztZeSprzetem,
                    CostWithoutEquipment = scheduledClass.KosztBezSprzetu,
                    Participants = scheduledClass.InstancjaZajec
                        .Where(i =>
                        {
                            var instDate = i.Data.ToDateTime(TimeOnly.MinValue);
                            return instDate >= startDate && instDate <= endTime;
                        })
                        .SelectMany(i => i.InstancjaZajecKlients
                        .Where(ik => {
                            var activityDateTime = i.Data.ToDateTime(TimeOnly.MinValue);
                            var signUpDateTime = ik.DataZapisu.ToDateTime(TimeOnly.MinValue);
                            var diff = activityDateTime - signUpDateTime;
                            //Console.WriteLine($"AAAAAAAAAAAAsignUpdate{signUpDateTime}ActivityDate{activityDateTime}odstep{diff}");
                            return diff.TotalHours <= 48 && diff.TotalHours >= 0;
                        }))
                         .Select(ik => new ParticipantDto
                         {
                            FirstName = ik.Klient.KlientNavigation.Imie,
                            LastName = ik.Klient.KlientNavigation.Nazwisko,
                            IsPaid = ik.CzyOplacone
                         })
                        .ToList()
                };

            }
            else if (isTrainerRole)
            {
                dto = new ScheduleInfoTrainerDto
                {
                    Type = "Trainer",
                    Description = "Zajęcia",
                    Id = scheduledClass.ZajeciaId,
                    Date = startDate,
                    StartTime = startTime.TimeOfDay,
                    EndTime = endTime.TimeOfDay,
                    CourtName = scheduledClass.Kort.Nazwa,
                    TrainerName = scheduledClass.Pracownik?.PracownikNavigation != null
                            ? $"{scheduledClass.Pracownik.PracownikNavigation.Imie} {scheduledClass.Pracownik.PracownikNavigation.Nazwisko}"
                            : "Brak trenera",
                    GroupName = scheduledClass.Zajecia?.Nazwa,
                    SkillLevel = scheduledClass.Zajecia?.IdPoziomZajecNavigation?.Nazwa,
                    Participants = scheduledClass.InstancjaZajec
                        .Where(i =>
                        {
                            var instDate = i.Data.ToDateTime(TimeOnly.MinValue);
                            return instDate >= startDate && instDate <= endTime;
                        })
                        .SelectMany(i => i.InstancjaZajecKlients
                        .Where(ik => {
                            var activityDateTime = i.Data.ToDateTime(TimeOnly.MinValue);
                            var signUpDateTime = ik.DataZapisu.ToDateTime(TimeOnly.MinValue);
                            var diff = activityDateTime - signUpDateTime;
                            //Console.WriteLine($"AAAAAAAAAAAAsignUpdate{signUpDateTime}ActivityDate{activityDateTime}odstep{diff}");
                            return diff.TotalHours <= 48 && diff.TotalHours >= 0;
                        }))
                        .Select(ik => new ParticipantDto
                        {
                            FirstName = ik.Klient.KlientNavigation.Imie,
                            LastName = ik.Klient.KlientNavigation.Nazwisko,
                            IsPaid = null //trener tego nie powinien widziec
                        })
                        .ToList()
                };
            }
            else if (isBasicRole)
            {
                dto = new ScheduleInfoBasicDto
                {
                    Type = "Basic",
                    Description = "Zajęcia",
                    Id = scheduledClass.ZajeciaId,
                    Date = startDate,
                    StartTime = startTime.TimeOfDay,
                    EndTime = endTime.TimeOfDay,
                    CourtName = scheduledClass.Kort.Nazwa,
                    TrainerName = scheduledClass.Pracownik?.PracownikNavigation != null
                        ? $"{scheduledClass.Pracownik.PracownikNavigation.Imie} {scheduledClass.Pracownik.PracownikNavigation.Nazwisko}"
                        : "Brak trenera",
                    GroupName = scheduledClass.Zajecia?.Nazwa,
                    SkillLevel = scheduledClass.Zajecia?.IdPoziomZajecNavigation?.Nazwa,
                    CostWithEquipment = scheduledClass.KosztZeSprzetem,
                    CostWithoutEquipment = scheduledClass.KosztBezSprzetu,
                };
            }
            else
            {
                throw new UnauthorizedAccessException("Unknown user role.");
            }

            scheduleInfoDtos.Add(dto);
        }

        return scheduleInfoDtos
            .OrderBy(dto => new DateTime(dto.Date.Year, dto.Date.Month, dto.Date.Day)
            .Add(dto.StartTime))
            .ToList();
    }
}
