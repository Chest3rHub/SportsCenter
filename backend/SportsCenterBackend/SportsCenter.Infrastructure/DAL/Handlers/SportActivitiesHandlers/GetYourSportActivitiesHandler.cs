using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SportsCenter.Application.Activities.Queries.GetYourSportActivities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Infrastructure.DAL.Handlers.SportActivitiesHandlers
{
    internal class GetYourSportActivitiesHandler : IRequestHandler<GetYourSportActivities, IEnumerable<YourSportActivityDto>>
    {
        private readonly SportsCenterDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetYourSportActivitiesHandler(SportsCenterDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<IEnumerable<YourSportActivityDto>> Handle(GetYourSportActivities request, CancellationToken cancellationToken)
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("userId")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                throw new UnauthorizedAccessException("You cannot access your absence requests without being logged in on your account.");
            }

            int pageSize = 6;
            int numberPerPage = 7;

            var sportActivities = await _dbContext.InstancjaZajecKlients
            .Where(ik => ik.KlientId == userId)
            .Join(
                _dbContext.InstancjaZajecs,
                ik => ik.InstancjaZajecId,
                iz => iz.InstancjaZajecId,
                (ik, iz) => new { ik, iz }
            )
            .Join(
                _dbContext.GrafikZajecs,
                combined => combined.iz.GrafikZajecId,
                g => g.GrafikZajecId,
                (combined, g) => new YourSportActivityDto
                {
                    InstanceOfActivityId = combined.iz.InstancjaZajecId,
                    SportActivityName = g.Zajecia.Nazwa,
                    DateOfActivity = combined.iz.Data,
                    DayOfWeek = combined.iz.Data.DayOfWeek.ToString(),
                    StartHour = g.GodzinaOd,
                    DurationInMinutes = g.CzasTrwania,
                    LevelName = g.Zajecia.IdPoziomZajecNavigation.Nazwa,
                    EmployeeId = g.PracownikId,
                    CourtName = g.Kort.Nazwa,
                    CostWithoutEquipment = g.KosztBezSprzetu,
                    CostWithEquipment = g.KosztZeSprzetem,
                    IsEquipmentReserved = combined.ik.CzyUwzglednicSprzet ? "Tak" : "Nie",

                    IsActivityPaid = combined.ik.CzyOplacone.HasValue
                        ? (combined.ik.CzyOplacone.Value ? "Tak" : "Nie")
                        : "Nie",
                    IsActivityCanceled = combined.iz.CzyOdwolane.HasValue
                        ? (combined.iz.CzyOdwolane.Value ? "Tak" : "Nie")
                        : "Nie"
                })
            .OrderByDescending(activity => activity.DateOfActivity)
            .ThenBy(activity => activity.StartHour)
            .Skip(request.Offset * pageSize)
            .Take(numberPerPage)
            .ToListAsync(cancellationToken);

            return sportActivities;
        }
    }
}
