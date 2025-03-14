using MediatR;
using Microsoft.AspNetCore.Http;
using SportsCenter.Application.Exceptions.ClientsExceptions;
using SportsCenter.Application.Exceptions.EmployeesExceptions;
using SportsCenter.Application.Exceptions.ReservationExceptions;
using SportsCenter.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SportsCenter.Core.Enums.PaymentResult;

namespace SportsCenter.Application.Reservations.Commands.PayForClientReservation
{
    internal class PayForClientReservationHandler : IRequestHandler<PayForClientReservation, Unit>
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PayForClientReservationHandler(IReservationRepository reservationRepository, IEmployeeRepository employeeRepository, IHttpContextAccessor httpContextAccessor)
        {
            _reservationRepository = reservationRepository;
            _employeeRepository = employeeRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<Unit> Handle(PayForClientReservation request, CancellationToken cancellationToken)
        {
          
            var reservationToPay = await _reservationRepository.GetReservationByIdAsync(request.ReservationId, cancellationToken);
            if (reservationToPay == null)
            {
                throw new ReservationNotFoundException(request.ReservationId);
            }

            var paymentResult = await _employeeRepository.PayForClientReservationAsync(request.ReservationId, request.ClientEmail, cancellationToken);

            switch (paymentResult)
            {
                case PaymentResultEnum.Success:

                    return Unit.Value;

                case PaymentResultEnum.InsufficientFunds:

                    throw new PaymentFailedException();

                case PaymentResultEnum.ActivityInstanceNotFound:

                    throw new ReservationNotFoundException(request.ReservationId);

                case PaymentResultEnum.ClientNotFound:

                    throw new ClientNotFoundException(request.ClientEmail);

                case PaymentResultEnum.AlreadyPaid:

                    throw new ReservationAlreadyPaidException(request.ReservationId);
            }

            return Unit.Value;
        }
    }
}
