using MediatR;
using Microsoft.AspNetCore.Http;
using SportsCenter.Application.Employees.Commands.RemoveTask;
using SportsCenter.Application.Exceptions.ReservationExceptions;
using SportsCenter.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Reservations.Commands.RemoveReservation
{
    internal sealed class RemoveReservationHandler : IRequestHandler<RemoveReservation, Unit>
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RemoveReservationHandler(IReservationRepository reservationRepository, IHttpContextAccessor httpContextAccessor)
        {
            _reservationRepository = reservationRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Unit> Handle(RemoveReservation request, CancellationToken cancellationToken)
        {         
            var reservation = await _reservationRepository.GetReservationByIdAsync(request.Id, cancellationToken);

            if (reservation == null)
            {
                throw new ReservationNotFoundException(request.Id);
            }

            var user = _httpContextAccessor.HttpContext?.User;
            if (user == null)
                throw new UnauthorizedAccessException("No user authorization.");

            var userRoles = user.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList();

            bool isOwner = userRoles.Contains("Wlasciciel");
            bool isClient = userRoles.Contains("Klient");

            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst("userId")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                throw new UnauthorizedAccessException("No permission to remove reservation.");
            }

            var hasClientReservation = await _reservationRepository.HasClientReservation(request.Id, userId, cancellationToken);

            if (!hasClientReservation)
            {
                throw new NotThatClientReservationException(userId, request.Id);
            }

            var remainingTime = reservation.DataOd - DateTime.UtcNow;

            if (isClient && !isOwner && remainingTime.TotalHours < 24)
            {
                throw new InvalidOperationException("The client can only cancel the reservation up to 24 hours before the start.");
            }

            await _reservationRepository.DeleteReservationAsync(reservation, cancellationToken);

            return Unit.Value;
        }
    }
}
