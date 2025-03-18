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
    private readonly IOrderRepository _orderRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DismissEmployeeHandler(IEmployeeRepository employeeRepository, IReservationRepository reservationRepository,
            IOrderRepository orderRepository, IHttpContextAccessor httpContextAccessor)
    {
        _employeeRepository = employeeRepository;
        _reservationRepository = reservationRepository;
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
            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            var upcomingReservations = reservations
                .Where(r => DateOnly.FromDateTime(r.DataOd) >= today)
                .ToList();

            foreach (var reservation in upcomingReservations)
            {
                var startTime = reservation.DataOd;
                var endTime = reservation.DataDo;

                //na razie system sam znajduje dostepnego trenra i mu przydziela calosc zastepstw
                //po integracji z frontendem zostanie dodane okienko obslugujace interakcje
                //wyboru dostepnych trenerow na poszczegolne zajecia/rezerwacje
                var substituteTrainers = await _employeeRepository.GetAvailableTrainersAsync(startTime, endTime, cancellationToken);
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

            var currentOrders = orders.Where(o => o.Status != "Zrealizowane").ToList();

            if (currentOrders.Any())
            {
                //na razie system znajduje pracownika majacego najmniej zamowien i mu przydziela 
                //aktywne zamowienia pracownika zwalnianego 
                //w przyszlosci mozna zrobic jakas interakcje na frontendzie
                var employeeWithFewestOrders = await _employeeRepository.GetEmployeeWithFewestOrdersAsync(cancellationToken);
                foreach (var order in currentOrders)
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
