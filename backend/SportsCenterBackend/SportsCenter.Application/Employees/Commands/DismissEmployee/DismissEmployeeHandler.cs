using MediatR;
using Microsoft.AspNetCore.Http;
using SportsCenter.Application.Employees.Commands.DismissEmployee;
using SportsCenter.Application.Exceptions.EmployeesException;
using SportsCenter.Application.Exceptions.EmployeesExceptions;
using SportsCenter.Core.Repositories;

internal sealed class DismissEmployeeHandler : IRequestHandler<DismissEmployee, ReservationFailureResponse>
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IReservationRepository _reservationRepository;
    private readonly ISportActivityRepository _sportActivityRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DismissEmployeeHandler(IEmployeeRepository employeeRepository, IReservationRepository reservationRepository,
         ISportActivityRepository sportActivityRepository, IOrderRepository orderRepository, IHttpContextAccessor httpContextAccessor)
    {
        _employeeRepository = employeeRepository;
        _reservationRepository = reservationRepository;
        _sportActivityRepository = sportActivityRepository;
        _orderRepository = orderRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ReservationFailureResponse> Handle(DismissEmployee request, CancellationToken cancellationToken)
    {
        var employee = await _employeeRepository.GetEmployeeByIdAsync(request.DismissedEmployeeId, cancellationToken);
        if (employee == null)
        {
            throw new EmployeeNotFoundException(request.DismissedEmployeeId);
        }

        if (employee.DataZwolnienia != null)
        {
            throw new EmployeeAlreadyDismissedException(request.DismissedEmployeeId);
        }

        var position = await _employeeRepository.GetEmployeePositionNameByIdAsync(request.DismissedEmployeeId, cancellationToken);

        List<int> failedReservations = new List<int>();

        if (position == "Trener")
        {
            var reservations = await _reservationRepository.GetReservationsByTrainerIdAsync(request.DismissedEmployeeId, cancellationToken);

            foreach (var reservation in reservations)
            {
                var startTime = reservation.DataOd;
                var endTime = reservation.DataDo;

                var substituteTrainers = await _reservationRepository.GetAvailableTrainersAsync(startTime, endTime, cancellationToken);
                if (substituteTrainers == null || !substituteTrainers.Any())
                {
                    failedReservations.Add(reservation.RezerwacjaId);
                }
                else
                {
                    var trainerToAssign = substituteTrainers.FirstOrDefault();
                    if (trainerToAssign != null)
                    {
                        reservation.TrenerId = trainerToAssign.PracownikId;
                        await _reservationRepository.UpdateReservationAsync(reservation, cancellationToken);
                    }
                }
            }
        }
        else if (position == "Pracownik administracyjny")
        {
            var orders = await _orderRepository.GetOrdersByEmployeeIdAsync(request.DismissedEmployeeId, cancellationToken);
            if (orders.Any())
            {
                var employeeWithFewestOrders = await _employeeRepository.GetEmployeeWithFewestOrdersAsync(cancellationToken);
                foreach (var order in orders)
                {
                    order.PracownikId = employeeWithFewestOrders.PracownikId;
                    await _orderRepository.UpdateOrderAsync(order, cancellationToken);
                }
            }
        }
        else
        {
            await _employeeRepository.DeleteEmployeeAsync(request.DismissedEmployeeId, request.DismissalDate, cancellationToken);
        }

        await _employeeRepository.DeleteEmployeeAsync(request.DismissedEmployeeId, request.DismissalDate, cancellationToken);

        return new ReservationFailureResponse(failedReservations);
    }
}
