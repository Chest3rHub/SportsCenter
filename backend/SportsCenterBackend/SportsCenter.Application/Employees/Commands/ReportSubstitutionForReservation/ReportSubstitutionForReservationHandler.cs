using MediatR;
using Microsoft.AspNetCore.Http;
using SportsCenter.Application.Exceptions.ReservationExceptions;
using SportsCenter.Core.Entities;
using SportsCenter.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SportsCenter.Application.Employees.Commands.ReportSubstitutionForReservation
{
    internal sealed class ReportSubstitutionForReservationHandler : IRequestHandler<ReportSubstitutionForReservation, Unit>
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ReportSubstitutionForReservationHandler(IReservationRepository reservationRepository, IHttpContextAccessor httpContextAccessor)
        {
            _reservationRepository = reservationRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Unit> Handle(ReportSubstitutionForReservation request, CancellationToken cancellationToken)
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst("userId")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                throw new UnauthorizedAccessException("No permission to report need for reservations.");
            }
            int absentEmployee = userId;

            var isTrainerAssignedToActivity = await _reservationRepository.IsTrainerAssignedToReservationAsync(request.ReservationId, userId);

            if (!isTrainerAssignedToActivity)
            {
                throw new SubstitutionForReservationNotAllowedException(request.ReservationId);
            }

            var alreadyRequested = await _reservationRepository.HasEmployeeAlreadyRequestedSubstitutionForReservationAsync(request.ReservationId, userId);
            if (alreadyRequested)
            {
                throw new DuplicateSubstitutionReservationRequestException(request.ReservationId);
            }

            var (startDate, endDate) = await _reservationRepository.GetReservationDetailsByIdAsync(request.ReservationId);

            if (startDate == DateTime.MinValue || endDate == DateTime.MinValue)
            {
                throw new ReservationNotFoundException(request.ReservationId);
            }

            TimeOnly startHourTimeOnly = TimeOnly.FromDateTime(startDate);
            TimeOnly endHourTimeOnly = TimeOnly.FromDateTime(endDate);

            var substitution = new Zastepstwo
            {
                Data = DateOnly.FromDateTime(startDate),
                GodzinaOd = startHourTimeOnly,
                GodzinaDo = endHourTimeOnly,
                ZajeciaId = null,
                RezerwacjaId = request.ReservationId,
                PracownikNieobecnyId = absentEmployee,
                PracownikZastepujacy = null,
                PracownikZatwierdzajacy = null
            };

            await _reservationRepository.AddSubstitutionForReservationAsync(substitution);

            return Unit.Value;
        }
    }
}
