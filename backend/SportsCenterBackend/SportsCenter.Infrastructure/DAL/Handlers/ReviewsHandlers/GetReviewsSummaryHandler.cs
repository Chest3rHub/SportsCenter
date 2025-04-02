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
            int pageSize = 6;
            int numberPerPage = 7;

            var reviews = await _dbContext.Ocenas
                .Where(o =>
                    o.DataWystawienia >= DateOnly.FromDateTime(request.StartDate) && o.DataWystawienia <= DateOnly.FromDateTime(request.EndDate))
                .Include(o => o.InstancjaZajecKlient)
                    .ThenInclude(gzk => gzk.Klient)
                    .ThenInclude(k => k.KlientNavigation)
                .Include(o => o.InstancjaZajecKlient)
                    .ThenInclude(gzk => gzk.InstancjaZajec)
                    .ThenInclude(gz => gz.GrafikZajec)
                    .ThenInclude(gz => gz.Pracownik)
                    .ThenInclude(p => p.PracownikNavigation)
                .Include(o => o.InstancjaZajecKlient)
                    .ThenInclude(gzk => gzk.InstancjaZajec)
                    .ThenInclude(gz => gz.GrafikZajec)
                    .ThenInclude(gz => gz.Zajecia)
                    .ThenInclude(z => z.IdPoziomZajecNavigation)
                .OrderByDescending(o => o.DataWystawienia)
                .Skip(request.Offset * pageSize)
                .Take(numberPerPage)
                .Select(o => new ReviewsSummaryDto
                {
                    Description = o.Opis,
                    Stars = o.Gwiazdki,
                    Date = o.DataWystawienia.ToDateTime(TimeOnly.MinValue),
                    ClientName = o.InstancjaZajecKlient.Klient.KlientNavigation.Imie,
                    TrainerName = o.InstancjaZajecKlient.InstancjaZajec.GrafikZajec.Pracownik.PracownikNavigation.Imie,
                    ClientSurname = o.InstancjaZajecKlient.Klient.KlientNavigation.Nazwisko,
                    TrainerSurname = o.InstancjaZajecKlient.InstancjaZajec.GrafikZajec.Pracownik.PracownikNavigation.Nazwisko,
                    ActivityName = o.InstancjaZajecKlient.InstancjaZajec.GrafikZajec.Zajecia.Nazwa,
                    ActivityLevel = o.InstancjaZajecKlient.InstancjaZajec.GrafikZajec.Zajecia.IdPoziomZajecNavigation.Nazwa
                })
                .ToListAsync(cancellationToken);

            return reviews;
        }
    }
}
