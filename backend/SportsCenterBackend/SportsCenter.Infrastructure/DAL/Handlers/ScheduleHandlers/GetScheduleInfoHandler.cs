﻿using MediatR;
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

        DateOnly today = DateOnly.FromDateTime(DateTime.UtcNow);
        DateOnly startOfWeek = today.AddDays(-(int)today.DayOfWeek + 1).AddDays(request.WeekOffset * 7);
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
                    Date = reservation.DataOd,
                    StartTime = reservation.DataOd.TimeOfDay,
                    EndTime = reservation.DataDo.TimeOfDay,
                    CourtNumber = reservation.KortId,
                    TrainerName = reservation.Trener?.PracownikNavigation != null
                        ? $"{reservation.Trener.PracownikNavigation.Imie} {reservation.Trener.PracownikNavigation.Nazwisko}"
                        : "Brak trenera",
                    Participants = new List<string> { $"{reservation.Klient.KlientNavigation.Imie} {reservation.Klient.KlientNavigation.Nazwisko}" },
                    Cost = reservation.Koszt,
                    Discount = reservation.Klient?.ZnizkaNaZajecia ?? 0
                };
            }
            else if (isTrainerRole)
            {
                dto = new ScheduleInfoTrainerDto
                {
                    Type = "Trainer",
                    Date = reservation.DataOd,
                    StartTime = reservation.DataOd.TimeOfDay,
                    EndTime = reservation.DataDo.TimeOfDay,
                    CourtNumber = reservation.KortId,
                    TrainerName = reservation.Trener?.PracownikNavigation != null
                        ? $"{reservation.Trener.PracownikNavigation.Imie} {reservation.Trener.PracownikNavigation.Nazwisko}"
                        : "Brak trenera",
                    Participants = new List<string> { $"{reservation.Klient.KlientNavigation.Imie} {reservation.Klient.KlientNavigation.Nazwisko}" }
                };
            }
            else
            {
                dto = new ScheduleInfoBasicDto
                {
                    Type = "Basic",
                    Date = reservation.DataOd,
                    StartTime = reservation.DataOd.TimeOfDay,
                    EndTime = reservation.DataDo.TimeOfDay,
                    CourtNumber = reservation.KortId,
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
                startTime = startDate.AddMinutes(scheduledClass.GodzinaOd.TotalMinutes);
                endTime = startTime.AddMinutes(scheduledClass.CzasTrwania);
            }
            else
            {
                throw new InvalidOperationException($"Nieznany dzień tygodnia: {dayOfWeekString}");
            }

            if (isAdminRole)
            {
                dto = new ScheduleInfoAdminDto
                {
                    Type = "Admin",
                    Date = startDate,
                    StartTime = startTime.TimeOfDay,
                    EndTime = endTime.TimeOfDay,
                    CourtNumber = scheduledClass.KortId,
                    TrainerName = scheduledClass.Pracownik?.PracownikNavigation != null
                            ? $"{scheduledClass.Pracownik.PracownikNavigation.Imie} {scheduledClass.Pracownik.PracownikNavigation.Nazwisko}"
                            : "Brak trenera",
                    GroupName = scheduledClass.Zajecia?.Nazwa,
                    SkillLevel = scheduledClass.Zajecia?.IdPoziomZajecNavigation?.Nazwa,
                    Cost = scheduledClass.KosztZeSprzetem > 0
                            ? scheduledClass.KosztZeSprzetem
                            : (decimal?)0,
                    Participants = scheduledClass.InstancjaZajec
                        .SelectMany(i => i.InstancjaZajecKlients)
                        .Select(ik => $"{ik.Klient.KlientNavigation.Imie} {ik.Klient.KlientNavigation.Nazwisko}")
                        .ToList()
                };

            }
            else if (isTrainerRole)
            {
                dto = new ScheduleInfoTrainerDto
                {
                    Type = "Trainer",
                    Date = startDate,
                    StartTime = startTime.TimeOfDay,
                    EndTime = endTime.TimeOfDay,
                    CourtNumber = scheduledClass.KortId,
                    TrainerName = scheduledClass.Pracownik?.PracownikNavigation != null
                            ? $"{scheduledClass.Pracownik.PracownikNavigation.Imie} {scheduledClass.Pracownik.PracownikNavigation.Nazwisko}"
                            : "Brak trenera",
                    GroupName = scheduledClass.Zajecia?.Nazwa,
                    SkillLevel = scheduledClass.Zajecia?.IdPoziomZajecNavigation?.Nazwa,
                    Participants = scheduledClass.InstancjaZajec
                        .SelectMany(i => i.InstancjaZajecKlients)
                        .Select(ik => $"{ik.Klient.KlientNavigation.Imie} {ik.Klient.KlientNavigation.Nazwisko}")
                        .ToList()
                };
            }
            else
            {
                dto = new ScheduleInfoBasicDto
                {
                    Type = "Basic",
                    Date = startDate,
                    StartTime = startTime.TimeOfDay,
                    EndTime = endTime.TimeOfDay,
                    CourtNumber = scheduledClass.KortId,
                };
            }

            scheduleInfoDtos.Add(dto);
        }

        return scheduleInfoDtos;
    }

}
