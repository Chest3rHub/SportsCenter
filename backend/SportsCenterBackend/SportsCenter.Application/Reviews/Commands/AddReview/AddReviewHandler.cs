using MediatR;
using Microsoft.AspNetCore.Http;
using SportsCenter.Application.Exceptions.ReviewsExceptions;
using SportsCenter.Core.Entities;
using SportsCenter.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Reviews.Commands.AddReview
{
    internal sealed class AddReviewHandler : IRequestHandler<AddReview, Unit>
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AddReviewHandler(IReviewRepository reviewRepository, IHttpContextAccessor httpContextAccessor)
        {
            _reviewRepository = reviewRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Unit> Handle(AddReview request, CancellationToken cancellationToken)
        {          
            var isEligible = await _reviewRepository.CanUserReviewAsync(request.ScheduleActivitiesClientId, request.ClientId, cancellationToken);
            if (!isEligible)
                throw new ReviewTimeExceededException();

            var hasReviewed = await _reviewRepository.HasUserAlreadyReviewedAsync(request.ScheduleActivitiesClientId, request.ClientId, cancellationToken);
            if (hasReviewed)
                throw new ReviewAlreadyExistException(request.ClientId, request.ScheduleActivitiesClientId);

            var ocena = new Ocena
            {
                GrafikZajecKlientId = request.ScheduleActivitiesClientId,
                Opis = request.Description,
                Gwiazdki = request.Stars,
                DataWystawienia = DateTime.UtcNow
            };

            await _reviewRepository.AddReviewAsync(ocena, cancellationToken);
            return Unit.Value;
        }
    }
}
