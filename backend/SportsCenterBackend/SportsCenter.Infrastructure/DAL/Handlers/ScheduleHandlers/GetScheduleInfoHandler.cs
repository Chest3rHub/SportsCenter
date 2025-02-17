using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SportsCenter.Application.Schedule.Queries.GetScheduleInfo;
using SportsCenter.Infrastructure.DAL;
using System.Security.Claims;
using Newtonsoft.Json;

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
     
        var reservations = await _dbContext.Rezerwacjas
            .Where(r => r.DataOd >= request.StartDate && r.DataDo <= request.EndDate)
            .Include(r => r.Klient)
            .ThenInclude(k => k.KlientNavigation)
            .Include(r => r.Kort)
            .Include(r => r.Trener)
            .ThenInclude(t => t.PracownikNavigation)
            .ToListAsync(cancellationToken);


        var scheduledClasses = await _dbContext.GrafikZajecs
            .Where(g => g.DataZajecs.Any(d => d.Date >= request.StartDate && d.Date <= request.EndDate))
            .Include(g => g.Zajecia)
            .ThenInclude(z => z.IdPoziomZajecNavigation)
            .Include(g => g.Pracownik)
            .ThenInclude(t => t.PracownikNavigation)
            .Include(g => g.Kort)
            .Include(g => g.DataZajecs)
            .Include(g => g.GrafikZajecKlients)
            .ToListAsync(cancellationToken);

        var scheduleInfoDtos = new List<ScheduleInfoBaseDto>();
      
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
                    ReservationCost = reservation.Koszt,
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

            if (isAdminRole)
            {
                dto = new ScheduleInfoAdminDto
                {
                    Type = "Admin",
                    Date = scheduledClass.DataZajecs.FirstOrDefault()?.Date ?? DateTime.MinValue,
                    StartTime = scheduledClass.DataZajecs.FirstOrDefault()?.Date.TimeOfDay ?? TimeSpan.Zero,
                    EndTime = scheduledClass.DataZajecs.FirstOrDefault()?.Date.AddMinutes(scheduledClass.CzasTrwania).TimeOfDay ?? TimeSpan.Zero,
                    CourtNumber = scheduledClass.KortId,
                    TrainerName = scheduledClass.Pracownik?.PracownikNavigation != null
                    ? $"{scheduledClass.Pracownik.PracownikNavigation.Imie} {scheduledClass.Pracownik.PracownikNavigation.Nazwisko}"
                    : "Brak trenera",
                    GroupName = scheduledClass.Zajecia?.Nazwa,
                    SkillLevel = scheduledClass.Zajecia?.IdPoziomZajecNavigation?.Nazwa,
                    ReservationCost = scheduledClass.KoszZeSprzetem > 0
                                      ? scheduledClass.KoszZeSprzetem
                                      : (decimal?)0,
                    Participants = scheduledClass.GrafikZajecKlients
                    .Select(g => $"{g.Klient.KlientNavigation.Imie} {g.Klient.KlientNavigation.Nazwisko}")
                    .ToList()
                };
            }else if (isTrainerRole)
            {
                dto = new ScheduleInfoTrainerDto
                {
                    Type = "Trainer",
                    Date = scheduledClass.DataZajecs.FirstOrDefault()?.Date ?? DateTime.MinValue,
                    StartTime = scheduledClass.DataZajecs.FirstOrDefault()?.Date.TimeOfDay ?? TimeSpan.Zero,
                    EndTime = scheduledClass.DataZajecs.FirstOrDefault()?.Date.AddMinutes(scheduledClass.CzasTrwania).TimeOfDay ?? TimeSpan.Zero,
                    CourtNumber = scheduledClass.KortId,
                    TrainerName = scheduledClass.Pracownik?.PracownikNavigation != null
                   ? $"{scheduledClass.Pracownik.PracownikNavigation.Imie} {scheduledClass.Pracownik.PracownikNavigation.Nazwisko}"
                   : "Brak trenera",
                    GroupName = scheduledClass.Zajecia?.Nazwa,
                    SkillLevel = scheduledClass.Zajecia?.IdPoziomZajecNavigation?.Nazwa,
                    Participants = scheduledClass.GrafikZajecKlients
                    .Select(g => $"{g.Klient.KlientNavigation.Imie} {g.Klient.KlientNavigation.Nazwisko}")
                    .ToList()
                };
            }
            else
            {
                dto = new ScheduleInfoBasicDto
                {
                    Type = "Basic",
                    Date = scheduledClass.DataZajecs.FirstOrDefault()?.Date ?? DateTime.MinValue,
                    StartTime = scheduledClass.DataZajecs.FirstOrDefault()?.Date.TimeOfDay ?? TimeSpan.Zero,
                    EndTime = scheduledClass.DataZajecs.FirstOrDefault()?.Date.AddMinutes(scheduledClass.CzasTrwania).TimeOfDay ?? TimeSpan.Zero,
                    CourtNumber = scheduledClass.KortId,
                };
            }

            scheduleInfoDtos.Add(dto);
        }

        var json = JsonConvert.SerializeObject(scheduleInfoDtos, Formatting.Indented);

        return scheduleInfoDtos;
    }
}
