using MediatR;
using Microsoft.EntityFrameworkCore;
using SportsCenter.Application.SportsCenterManagement.Queries.GetSportsCenterWorkingHours;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsCenter.Infrastructure.DAL.Handlers.SportsCenterInfoHandler
{
    internal sealed class GetSportsCenterWorkingHoursHandler : IRequestHandler<GetSportsCenterWorkingHours, IEnumerable<SportsCenterWorkingHoursDto>>
    {
        private readonly SportsCenterDbContext _dbContext;

        public GetSportsCenterWorkingHoursHandler(SportsCenterDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<SportsCenterWorkingHoursDto>> Handle(GetSportsCenterWorkingHours request, CancellationToken cancellationToken)
        {
            DateOnly startDate = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(request.WeekOffset * 7);
            DateOnly monday = startDate.AddDays(-(int)startDate.DayOfWeek + 1);
            DateOnly sunday = monday.AddDays(6);

            var dniTygodnia = new Dictionary<DayOfWeek, string>
            {
                { DayOfWeek.Monday, "poniedzialek" },
                { DayOfWeek.Tuesday, "wtorek" },
                { DayOfWeek.Wednesday, "sroda" },
                { DayOfWeek.Thursday, "czwartek" },
                { DayOfWeek.Friday, "piatek" },
                { DayOfWeek.Saturday, "sobota" },
                { DayOfWeek.Sunday, "niedziela" }
            };

            var standardHours = await _dbContext.GodzinyPracyKlubus
                .ToDictionaryAsync(h => h.DzienTygodnia.ToLower(), h => new { h.GodzinaOtwarcia, h.GodzinaZamkniecia });

            var specialHours = await _dbContext.WyjatkoweGodzinyPracies
                .Where(w => w.Data >= monday && w.Data <= sunday)
                .ToDictionaryAsync(w => w.Data, w => new { w.GodzinaOtwarcia, w.GodzinaZamkniecia });

            var workingHours = new List<SportsCenterWorkingHoursDto>();

            for (int i = 0; i < 7; i++)
            {
                DateOnly date = monday.AddDays(i);
                DayOfWeek dayOfWeekEnum = date.DayOfWeek;
                string dayOfWeekString = dniTygodnia[dayOfWeekEnum];

                if (specialHours.ContainsKey(date))
                {
                    var specialHour = specialHours[date];
                    workingHours.Add(new SportsCenterWorkingHoursDto
                    {
                        Date = date,
                        DayOfWeek = dayOfWeekString,
                        OpenHour = specialHour.GodzinaOtwarcia.ToTimeSpan(),
                        CloseHour = specialHour.GodzinaZamkniecia.ToTimeSpan(),
                    });
                }
                else
                {
                    var standardHour = standardHours[dayOfWeekString];
                    workingHours.Add(new SportsCenterWorkingHoursDto
                    {
                        Date = date,
                        DayOfWeek = dayOfWeekString,
                        OpenHour = standardHour.GodzinaOtwarcia.ToTimeSpan(),
                        CloseHour = standardHour.GodzinaZamkniecia.ToTimeSpan(),
                    });
                }
            }

            return workingHours;
        }
    }
}
