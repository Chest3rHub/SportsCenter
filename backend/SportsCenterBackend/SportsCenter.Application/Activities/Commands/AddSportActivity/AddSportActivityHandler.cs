using MediatR;
using SportsCenter.Application.Exceptions.CourtsExceptions;
using SportsCenter.Application.Exceptions.EmployeesException;
using SportsCenter.Application.Exceptions.EmployeesExceptions;
using SportsCenter.Application.Exceptions.ReservationExceptions;
using SportsCenter.Application.Exceptions.SportActivitiesExceptions;
using SportsCenter.Core.Entities;
using SportsCenter.Core.Repositories;
using static SportsCenter.Core.Enums.TrainerAvailiabilityStatus;


namespace SportsCenter.Application.Activities.Commands.AddSportActivity;

internal class AddSportActivityHandler : IRequestHandler<AddSportActivity, Unit>
{
    private readonly ISportActivityRepository _sportActivityRepository;
    private readonly ISportsCenterRepository _sportsCenterRepository;
    private readonly ICourtRepository _courtRepository;
    private readonly IEmployeeRepository _employeeRepository;

    public AddSportActivityHandler(ISportActivityRepository SportActivityRepository, ISportsCenterRepository sportsCenterRepository, IEmployeeRepository employeeRepository, ICourtRepository courtRepository)
    {
        _sportActivityRepository = SportActivityRepository;
        _sportsCenterRepository = sportsCenterRepository;
        _employeeRepository = employeeRepository;
        _courtRepository = courtRepository;
    }
    int GetTimeInMinutes(TimeOnly time)
    {
        return time.Hour * 60 + time.Minute;
    }

    public async Task<Unit> Handle(AddSportActivity request, CancellationToken cancellationToken)
    {
      
        //TO DO rozwiazac problem z godzina by nie trzeba bylo podawac tez sekund
        //a jedynie samo HH:MM
        var startHour = request.StartHour;
        var fullStartHour = startHour.ToString() + ":00";
        var startTimeSpan = TimeSpan.Parse(fullStartHour);

        var endHour = startHour.Add(TimeSpan.FromMinutes(request.DurationInMinutes));

        int reservationStartInMinutes = GetTimeInMinutes(request.StartHour);
        int reservationEndInMinutes = reservationStartInMinutes + request.DurationInMinutes;

        var specialWorkingHours = await _sportsCenterRepository.GetSpecialWorkingHoursByDateAsync(request.StartDate.ToDateTime(request.StartHour), cancellationToken);

        if (specialWorkingHours != null)
        {
            WyjatkoweGodzinyPracy workingHours = specialWorkingHours;

            int clubOpeningTimeInMinutes = GetTimeInMinutes(workingHours.GodzinaOtwarcia);
            int clubClosingTimeInMinutes = GetTimeInMinutes(workingHours.GodzinaZamkniecia);

            if (reservationStartInMinutes < clubOpeningTimeInMinutes || reservationEndInMinutes > clubClosingTimeInMinutes)
            {
                throw new ActivityOutsideWorkingHoursException();
            }
        }
        else
        {
            string dayOfWeek = request.DayOfWeek;
            var standardWorkingHours = await _sportsCenterRepository.GetWorkingHoursByDayAsync(dayOfWeek, cancellationToken);

            if (standardWorkingHours == null)
            {
                throw new ActivityOutsideWorkingHoursException();
            }

            GodzinyPracyKlubu workingHours = new GodzinyPracyKlubu
            {
                GodzinaOtwarcia = standardWorkingHours.GodzinaOtwarcia,
                GodzinaZamkniecia = standardWorkingHours.GodzinaZamkniecia
            };

            int clubOpeningTimeInMinutes = GetTimeInMinutes(workingHours.GodzinaOtwarcia);
            int clubClosingTimeInMinutes = GetTimeInMinutes(workingHours.GodzinaZamkniecia);

            if (reservationStartInMinutes < clubOpeningTimeInMinutes || reservationEndInMinutes > clubClosingTimeInMinutes)
            {
                throw new ActivityOutsideWorkingHoursException();
            }
        }

        int levelId = await _sportActivityRepository.EnsureLevelNameExistsAsync(request.LevelName, cancellationToken);

        bool courtExists = await _courtRepository.CheckIfCourtExists(request.CourtName, cancellationToken);
        if (!courtExists)
        {
            throw new WrongCourtNameException(request.CourtName);
        }

        var court = await _courtRepository.GetCourtIdByName(request.CourtName, cancellationToken);
        int courtId = court.Value;

        var daysOfWeekTranslation = new Dictionary<DayOfWeek, string>
        {
            { DayOfWeek.Monday, "poniedzialek" },
            { DayOfWeek.Tuesday, "wtorek" },
            { DayOfWeek.Wednesday, "sroda" },
            { DayOfWeek.Thursday, "czwartek" },
            { DayOfWeek.Friday, "piatek" },
            { DayOfWeek.Saturday, "sobota" },
            { DayOfWeek.Sunday, "niedziela" }
        };

        var startDayOfWeek = daysOfWeekTranslation[request.StartDate.DayOfWeek];

        if (startDayOfWeek != request.DayOfWeek.ToLower())
        {
            throw new InvalidDayOfWeekException(request.DayOfWeek, request.StartDate);
        }

        var startTime = request.StartDate.ToDateTime(request.StartHour);
        var endTime = startTime.AddMinutes(request.DurationInMinutes);

        bool isCourtAvailable = await _courtRepository.IsCourtAvailableAsync(courtId, startTime, endTime, cancellationToken);
        if (!isCourtAvailable)
            throw new CourtNotAvaliableException(courtId);

        var trainerPosition = await _employeeRepository.GetEmployeePositionNameByIdAsync(request.EmployeeId, cancellationToken);
        if (trainerPosition == null)
        {
            throw new EmployeeNotFoundException(request.EmployeeId);
        }

        if (trainerPosition != "Trener") //musi byæ Trener w bazie w tabeli TypPracownika
        {
            throw new NotTrainerEmployeeException(request.EmployeeId);
        }

        DateTime startDateTime = new DateTime(
            request.StartDate.Year,
            request.StartDate.Month,
            request.StartDate.Day,
            request.StartHour.Hour,
            request.StartHour.Minute,
            0
        );

        var availabilityStatus = await _employeeRepository.IsTrainerAvailableAsync(request.EmployeeId, startDateTime, reservationStartInMinutes, reservationEndInMinutes, cancellationToken);

        if (availabilityStatus == TrainerAvailabilityStatus.IsFired)
        {
            throw new EmployeeAlreadyDismissedException(request.EmployeeId);
        }

        if (availabilityStatus != TrainerAvailabilityStatus.Available)
        {
            throw new TrainerNotAvaliableException();
        }

        var newSportActivity = new Zajecium
        {
            Nazwa = request.SportActivityName,
            IdPoziomZajec = levelId
        };

        await _sportActivityRepository.AddSportActivityAsync(newSportActivity, cancellationToken);

        var newSchedule = new GrafikZajec
        {
            DzienTygodnia = request.DayOfWeek,
            DataStartuZajec = request.StartDate,
            GodzinaOd = startTimeSpan,
            CzasTrwania = request.DurationInMinutes,
            ZajeciaId = newSportActivity.ZajeciaId,
            PracownikId = request.EmployeeId,
            LimitOsob = request.ParticipantLimit,
            KortId = courtId,
            KosztBezSprzetu = request.CostWithoutEquipment,
            KosztZeSprzetem = request.CostWithEquipment
        };

        await _sportActivityRepository.AddScheduleAsync(newSchedule, cancellationToken);

        return Unit.Value;
    }

}
