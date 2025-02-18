using MediatR;
using SportsCenter.Core.Entities;
using SportsCenter.Core.Repositories;


namespace SportsCenter.Application.Activities.Commands.AddSportActivity;

public class AddSportActivityHandler : IRequestHandler<AddSportActivity, Unit>
{
    private readonly ISportActivityRepository _SportActivityRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private const int TrainerTypeId = 2; // Jesli uznamy ze ID Trenerow w bazie to 2

    public AddSportActivityHandler(ISportActivityRepository SportActivityRepository, IEmployeeRepository employeeRepository)
    {
        _SportActivityRepository = SportActivityRepository;
        _employeeRepository = employeeRepository;
    }

    public async Task<Unit> Handle(AddSportActivity request, CancellationToken cancellationToken)
    {
        var employee = await _employeeRepository.GetEmployeeByIdAsync(request.EmployeeId, cancellationToken);
        if (employee == null || employee.IdTypPracownika != TrainerTypeId)
        {
            throw new InvalidOperationException("Assigned employee must be a trainer.");
        }
        
        var newActivity = new Zajecium
        {
            Nazwa = request.SportActivityName,
            IdPoziomZajec = request.LevelId
        };

        await _SportActivityRepository.AddSportActivityAsync(newActivity, cancellationToken);
        
        var newSchedule = new GrafikZajec
        {
            ZajeciaId = newActivity.ZajeciaId,
            CzasTrwania = request.Duration,
            PracownikId = request.EmployeeId,
            LimitOsob = request.ParticipantLimit,
            KortId = request.CourtId,
            KoszBezSprzetu = request.CostWithoutEquipment,
            KoszZeSprzetem = request.CostWithEquipment
        };

        await _SportActivityRepository.AddScheduleAsync(newSchedule,cancellationToken);

        return Unit.Value;
    }
    
}
