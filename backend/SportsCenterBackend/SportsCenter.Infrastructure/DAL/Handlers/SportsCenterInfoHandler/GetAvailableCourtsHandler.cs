using MediatR;
using SportsCenter.Application.SportsCenterManagement.Queries.GetAvailableCourts;
using SportsCenter.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Infrastructure.DAL.Handlers.SportsCenterInfoHandler
{
    internal sealed class GetAvailableCourtsHandler : IRequestHandler<GetAvailableCourts, IEnumerable<CourtDto>>
    {
        private readonly ICourtRepository _courtRepository;

        public GetAvailableCourtsHandler(ICourtRepository courtRepository)
        {
           _courtRepository = courtRepository;
        }

        public async Task<IEnumerable<CourtDto>> Handle(GetAvailableCourts request, CancellationToken cancellationToken)
        {
            var courts = await _courtRepository.GetAvailableCourtsAsync(request.StartTime, request.EndTime, cancellationToken);

            return courts.Select(t => new CourtDto
            {
                Id = t.KortId,
                Name = t.Nazwa,
            });
        }
    }
}
