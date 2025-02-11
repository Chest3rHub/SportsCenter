using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SportsCenter.Application.Schedule.Queries.GetScheduleInfo;
using SportsCenter.Infrastructure.DAL;
using System.Security.Claims;

internal class GetScheduleInfoHandler : IRequestHandler<GetScheduleInfo, List<ScheduleInfoDto>>
{
    private readonly SportsCenterDbContext _dbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetScheduleInfoHandler(SportsCenterDbContext dbContext, IHttpContextAccessor httpContextAccessor)
    {
        _dbContext = dbContext;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<List<ScheduleInfoDto>> Handle(GetScheduleInfo request, CancellationToken cancellationToken)
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

        var scheduleInfoDtos = new List<ScheduleInfoDto>();

        foreach (var reservation in reservations)
        {
            var dto = new ScheduleInfoDto
            {
                Date = reservation.DataOd,
                StartTime = reservation.DataOd.TimeOfDay,
                EndTime = reservation.DataDo.TimeOfDay,
                CourtNumber = reservation.KortId,
            };
        
            if (isTrainerRole || isAdminRole)
            {
                dto.TrainerName = reservation.Trener?.PracownikNavigation != null
                    ? $"{reservation.Trener.PracownikNavigation.Imie} {reservation.Trener.PracownikNavigation.Nazwisko}"
                    : "Brak trenera";
            }

            if (isTrainerRole || isAdminRole)
            {
                dto.Participants = new List<string> { $"{reservation.Klient.KlientNavigation.Imie} {reservation.Klient.KlientNavigation.Nazwisko}" };
            }

            if (isAdminRole)
            {                
                dto.ReservationCost = reservation.Koszt;
            }

            if (isAdminRole)
            {
                dto.Discount = reservation.Klient?.ZnizkaNaZajecia ?? null;
            }

            scheduleInfoDtos.Add(dto);
        }


        foreach (var scheduledClass in scheduledClasses)
        {
            var dto = new ScheduleInfoDto
            {
                Date = scheduledClass.DataZajecs.FirstOrDefault()?.Date ?? DateTime.MinValue,
                StartTime = scheduledClass.DataZajecs.FirstOrDefault()?.Date.TimeOfDay ?? TimeSpan.Zero,
                EndTime = scheduledClass.DataZajecs.FirstOrDefault()?.Date.AddMinutes(scheduledClass.CzasTrwania).TimeOfDay ?? TimeSpan.Zero,
                CourtNumber = scheduledClass.KortId,
            };

            if (isTrainerRole || isAdminRole)
            {
                dto.TrainerName = scheduledClass.Pracownik?.PracownikNavigation != null
                    ? $"{scheduledClass.Pracownik.PracownikNavigation.Imie} {scheduledClass.Pracownik.PracownikNavigation.Nazwisko}"
                    : "Brak trenera";
            }
         
            if (isTrainerRole || isAdminRole)
            {
                dto.GroupName = scheduledClass.Zajecia?.Nazwa;
                dto.SkillLevel = scheduledClass.Zajecia?.IdPoziomZajecNavigation?.Nazwa;
            }

            if (isAdminRole)
            {                
                dto.ReservationCost = scheduledClass.KoszZeSprzetem > 0
                                      ? scheduledClass.KoszZeSprzetem
                                      : (decimal?)null;
            }

            if (isTrainerRole || isAdminRole)
            {
                dto.Participants = scheduledClass.GrafikZajecKlients
                    .Select(g => $"{g.Klient.KlientNavigation.Imie} {g.Klient.KlientNavigation.Nazwisko}")
                    .ToList();
            }

            scheduleInfoDtos.Add(dto);
        }
        return scheduleInfoDtos;
    }
}
