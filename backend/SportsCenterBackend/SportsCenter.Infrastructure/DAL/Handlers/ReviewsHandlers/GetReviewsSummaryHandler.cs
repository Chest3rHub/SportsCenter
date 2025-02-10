using MediatR;
using Microsoft.EntityFrameworkCore;
using SportsCenter.Application.Reviews.Queries;
using SportsCenter.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Infrastructure.DAL.Handlers.ReviewsHandler
{
    internal class GetReviewsSummaryHandler : IRequestHandler<GetReviewsSummary, IEnumerable<ReviewsSummaryDto>>
    {
        private readonly SportsCenterDbContext _dbContext;

        public GetReviewsSummaryHandler(SportsCenterDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<ReviewsSummaryDto>> Handle(GetReviewsSummary request, CancellationToken cancellationToken)
        {
            var reviews = await _dbContext.Ocenas
    .Where(o =>
        o.DataWystawienia >= request.StartDate && o.DataWystawienia <= request.EndDate)
    .Include(o => o.GrafikZajecKlient)  
        .ThenInclude(gzk => gzk.Klient)
        .ThenInclude(k => k.KlientNavigation) 
    .Include(o => o.GrafikZajecKlient)
        .ThenInclude(gzk => gzk.SportActivitySchedule)
        .ThenInclude(gz => gz.Pracownik)
        .ThenInclude(p => p.PracownikNavigation)
    .Include(o => o.GrafikZajecKlient)
        .ThenInclude(gzk => gzk.SportActivitySchedule)
        .ThenInclude(gz => gz.Zajecia)
        .ThenInclude(z => z.IdPoziomZajecNavigation)
    .Select(o => new ReviewsSummaryDto
    {
        Description = o.Opis,
        Stars = o.Gwiazdki,
        Date = o.DataWystawienia,
        ClientName = o.GrafikZajecKlient.Klient.KlientNavigation.Imie, 
        TrainerName = o.GrafikZajecKlient.SportActivitySchedule.Pracownik.PracownikNavigation.Imie,
        ClientSurname = o.GrafikZajecKlient.Klient.KlientNavigation.Nazwisko,
        TrainerSurname = o.GrafikZajecKlient.SportActivitySchedule.Pracownik.PracownikNavigation.Nazwisko,
        ActivityName = o.GrafikZajecKlient.SportActivitySchedule.Zajecia.Nazwa,
        ActivityLevel = o.GrafikZajecKlient.SportActivitySchedule.Zajecia.IdPoziomZajecNavigation.Nazwa
    })
    .ToListAsync(cancellationToken);

            return reviews;
        }
    }
}
