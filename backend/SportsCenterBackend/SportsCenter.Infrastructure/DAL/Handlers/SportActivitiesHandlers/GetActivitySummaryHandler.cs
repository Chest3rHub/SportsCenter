using MediatR;
using Microsoft.EntityFrameworkCore;
using SportsCenter.Application.Activities.Queries.GetActivitySummary;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Infrastructure.DAL.Handlers.SportActivitiesHandlers
{
    internal class GetActivitySummaryHandler : IRequestHandler<GetActivitySummary, ActivitySummaryDto>
    {
        private readonly SportsCenterDbContext _dbContext;

        public GetActivitySummaryHandler(SportsCenterDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<ActivitySummaryDto> Handle(GetActivitySummary request, CancellationToken cancellationToken)
        {

            int pageSize = 5;
            int numberPerPage = 6;

            var startDate = DateOnly.FromDateTime(request.StartDate);
            var endDate = DateOnly.FromDateTime(request.EndDate);

            var summaryData = await (
                from instancja in _dbContext.InstancjaZajecs
                join grafik in _dbContext.GrafikZajecs on instancja.GrafikZajecId equals grafik.GrafikZajecId
                join zajecia in _dbContext.Zajecia on grafik.ZajeciaId equals zajecia.ZajeciaId
                where instancja.Data >= startDate && instancja.Data <= endDate
                group instancja by new { zajecia.ZajeciaId, zajecia.Nazwa } into g
                select new ActivityGroupSummaryDto
                {
                    ZajeciaNazwa = g.Key.Nazwa,
                    CompletedActivities = g.Count(x => x.CzyOdwolane == false),
                    CancelledActivities = g.Count(x => x.CzyOdwolane == true),
                    TotalRevenue = (
                        from klient in _dbContext.InstancjaZajecKlients
                        join i in _dbContext.InstancjaZajecs on klient.InstancjaZajecId equals i.InstancjaZajecId
                        join gr in _dbContext.GrafikZajecs on i.GrafikZajecId equals gr.GrafikZajecId
                        where i.Data >= startDate && i.Data <= endDate
                              && gr.ZajeciaId == g.Key.ZajeciaId
                              && klient.CzyOplacone == true
                              && klient.CzyZwroconoPieniadze == false
                        select klient.CzyUwzglednicSprzet ? gr.KosztZeSprzetem : gr.KosztBezSprzetu
                    ).Sum()
                }
            ).Skip(request.Offset * pageSize)
             .Take(numberPerPage)
            .ToListAsync(cancellationToken);

            return new ActivitySummaryDto
            {
                SummariesByZajecia = summaryData
            };
        }
    }
}
