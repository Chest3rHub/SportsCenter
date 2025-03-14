﻿using MediatR;
using Microsoft.AspNetCore.Http;
using SportsCenter.Application.Exceptions.ReviewsExceptions;
using SportsCenter.Core.Entities;
using SportsCenter.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static SportsCenter.Core.Enums.ClientReviewStatus;

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
            var user = _httpContextAccessor.HttpContext?.User;
            if (user == null)
            {
                throw new UnauthorizedAccessException("No user authorization.");
            }

            var userIdClaim = user.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            if (!int.TryParse(userIdClaim, out int clientId))
            {
                throw new UnauthorizedAccessException("Invalid user ID.");
            }

            if (!user.Claims.Any(c => c.Type == ClaimTypes.Role && c.Value == "Klient"))
            {
                throw new UnauthorizedAccessException("Only customers can make reservations.");
            }

            var reviewStatus = await _reviewRepository.CanUserReviewAsync(request.ActivityId, clientId, cancellationToken);

            if (reviewStatus.Equals(ReviewStatus.ReviewPeriodExpired))
            {
                throw new ReviewTimeExceededException();
            }

            if (reviewStatus.Equals(ReviewStatus.NotSignedUp))
            {
                throw new ClientNotSignedUpForActivityException(clientId, request.ActivityId);
            }

            var hasReviewed = await _reviewRepository.HasUserAlreadyReviewedAsync(request.ActivityId, clientId, cancellationToken);
            if (hasReviewed)
                throw new ReviewAlreadyExistException(clientId, request.ActivityId);

            var ocena = new Ocena
            {
                GrafikZajecKlientId = request.ActivityId,
                Opis = request.Description,
                Gwiazdki = request.Stars,
                DataWystawienia = DateOnly.FromDateTime(DateTime.UtcNow)
            };

            await _reviewRepository.AddReviewAsync(ocena, cancellationToken);
            return Unit.Value;
        }
    }
}
