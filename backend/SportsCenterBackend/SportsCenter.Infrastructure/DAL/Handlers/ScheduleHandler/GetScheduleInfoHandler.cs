using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using SportsCenter.Application.Schedule.Queries.GetScheduleInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsCenter.Infrastructure.DAL.Handlers.ScheduleHandler
{
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
            
            var userRole = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "role")?.Value;

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
                    TrainerName = reservation.Trener != null
                        ? $"{reservation.Trener.PracownikNavigation.Imie} {reservation.Trener.PracownikNavigation.Nazwisko}"
                        : "Brak trenera",
                    Participants = userRole == "Klient" || userRole == "Guest" || userRole == "Cleaner"
                        ? new List<string> { $"{reservation.Klient.KlientNavigation.Imie} {reservation.Klient.KlientNavigation.Nazwisko}" }
                        : new List<string>(),
                    ReservationCost = reservation.Koszt,
                    Discount = reservation.Klient?.ZnizkaNaZajecia ?? 0,
                };

                
                if (userRole == "Owner")
                {
                    scheduleInfoDtos.Add(dto);
                }
                else if (userRole == "Klient" || userRole == "Guest" || userRole == "Cleaner")
                {
                    
                    scheduleInfoDtos.Add(dto);
                }
            }

            foreach (var scheduledClass in scheduledClasses)
            {
                var dto = new ScheduleInfoDto
                {
                    Date = scheduledClass.DataZajecs.FirstOrDefault()?.Date ?? DateTime.MinValue,
                    StartTime = scheduledClass.DataZajecs.FirstOrDefault()?.Date.TimeOfDay ?? TimeSpan.Zero,
                    EndTime = scheduledClass.DataZajecs.FirstOrDefault()?.Date.AddMinutes(scheduledClass.CzasTrwania).TimeOfDay ?? TimeSpan.Zero,
                    CourtNumber = scheduledClass.KortId,
                    TrainerName = $"{scheduledClass.Pracownik.PracownikNavigation.Imie} {scheduledClass.Pracownik.PracownikNavigation.Nazwisko}",
                    GroupName = userRole == "Owner" ? scheduledClass.Zajecia.Nazwa : null, 
                    SkillLevel = userRole == "Owner" ? scheduledClass.Zajecia.IdPoziomZajecNavigation.Nazwa : null,
                    ReservationCost = scheduledClass.KoszZeSprzetem,
                    Participants = userRole == "Klient" || userRole == "Guest" || userRole == "Cleaner"
                        ? scheduledClass.GrafikZajecKlients.Select(g => $"{g.Klient.KlientNavigation.Imie} {g.Klient.KlientNavigation.Nazwisko}").ToList()
                        : new List<string>(), 
                };

                if (userRole == "Owner")
                {
                    scheduleInfoDtos.Add(dto);
                }
                else if (userRole == "Klient" || userRole == "Guest" || userRole == "Cleaner")
                {
                    scheduleInfoDtos.Add(dto);
                }
            }

            return scheduleInfoDtos;
        }
    }
}
