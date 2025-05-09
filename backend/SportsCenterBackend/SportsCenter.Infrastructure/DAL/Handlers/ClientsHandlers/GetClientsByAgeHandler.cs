﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using SportsCenter.Core.Abstractions;
using SportsCenter.Application.Clients.Queries.GetClientsByAge;

namespace SportsCenter.Infrastructure.DAL.Handlers.ClientsHandlers
{
    internal class GetClientsByAgeHandler : IRequestHandler<GetClientsByAge, IEnumerable<ClientByAgeDto>>
    {
        private readonly SportsCenterDbContext _dbContext;
        private readonly IClock _clock;

        public GetClientsByAgeHandler(SportsCenterDbContext dbContext, IClock clock)
        {
            _dbContext = dbContext;
            _clock = clock;
        }

        public async Task<IEnumerable<ClientByAgeDto>> Handle(GetClientsByAge request, CancellationToken cancellationToken)
        {
            var today = DateOnly.FromDateTime(_clock.Current());

            int PageSize = 6;
            int NumberPerPage = 7;

            return await _dbContext.Klients.Include(x => x.KlientNavigation)
                .Select(k => new
                {
                    k.KlientId,
                    k.KlientNavigation.Imie,
                    k.KlientNavigation.Nazwisko,
                    k.KlientNavigation.Email,
                    k.KlientNavigation.DataUr,

                    Age = today.Year - k.KlientNavigation.DataUr.Value.Year -
                    (today.Month < k.KlientNavigation.DataUr.Value.Month
                    || today.Month == k.KlientNavigation.DataUr.Value.Month && today.Day < k.KlientNavigation.DataUr.Value.Day ? 1 : 0)
                })
                .Where(k => k.Age >= request.MinAge && k.Age <= request.MaxAge)
                .OrderBy(k => k.Age)
                .Skip(request.Offset * PageSize)
                .Take(NumberPerPage)
                .Select(k => new ClientByAgeDto
                {
                    Id = k.KlientId,
                    Name = k.Imie,
                    Surname = k.Nazwisko,
                    Email = k.Email,
                    DateOfBirth = k.DataUr
                })
                .AsNoTracking().ToListAsync(cancellationToken);
        }
    }
}
