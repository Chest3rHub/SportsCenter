using MediatR;
using SportsCenter.Application.SportsCenterManagement.Queries.GetSportsCenterWorkingHours;
using SportsCenter.Infrastructure.DAL;
using System.Linq; 
using Microsoft.EntityFrameworkCore; 


internal sealed class GetSportsCenterWorkingHoursHandler : IRequestHandler<GetSportsCenterWorkingHours, SportsCenterWorkingHoursDto>
{
    private readonly SportsCenterDbContext _dbContext;

    public GetSportsCenterWorkingHoursHandler(SportsCenterDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<SportsCenterWorkingHoursDto> Handle(GetSportsCenterWorkingHours request, CancellationToken cancellationToken)
    {
        DateOnly targetDate = DateOnly.FromDateTime(request.TargetDate);
        string dayOfWeekString = targetDate.DayOfWeek.ToString().ToLower().Trim();

        var dniTygodniaMap = new Dictionary<string, string>
        {
            { "monday", "poniedzialek" },
            { "tuesday", "wtorek" },
            { "wednesday", "sroda" },
            { "thursday", "czwartek" },
            { "friday", "piatek" },
            { "saturday", "sobota" },
            { "sunday", "niedziela" }
        };

        string dzienPoPolsku = dniTygodniaMap.ContainsKey(dayOfWeekString)
            ? dniTygodniaMap[dayOfWeekString]
            : dayOfWeekString;

        Console.WriteLine("AAAAAAA dzien po polsku: " + dzienPoPolsku);
        Console.WriteLine("AAAAAA data po ktorej szukamy " + targetDate);

        var specialHours = await _dbContext.WyjatkoweGodzinyPracies
            .Where(w => w.Data == targetDate)
            .FirstOrDefaultAsync();

        if (specialHours != null)
        {
            return new SportsCenterWorkingHoursDto
            {
                Date = targetDate,
                DayOfWeek = dzienPoPolsku,
                OpenHour = specialHours.GodzinaOtwarcia.ToTimeSpan(),
                CloseHour = specialHours.GodzinaZamkniecia.ToTimeSpan()
            };
        }

        var standardHours = await _dbContext.GodzinyPracyKlubus
            .Where(h => h.DzienTygodnia == dzienPoPolsku)
            .FirstOrDefaultAsync();

        if (standardHours != null)
        {
            Console.WriteLine("AAAAAAA sa standard hours dla " + dzienPoPolsku);
            Console.WriteLine("AAAAAAA Open hour: " + standardHours.GodzinaOtwarcia);
            Console.WriteLine("AAAAAAA Close hour: " + standardHours.GodzinaZamkniecia);
            return new SportsCenterWorkingHoursDto
            {
                Date = targetDate,
                DayOfWeek = dzienPoPolsku,
                OpenHour = standardHours.GodzinaOtwarcia.ToTimeSpan(),
                CloseHour = standardHours.GodzinaZamkniecia.ToTimeSpan(),
            };
           
        }

        Console.WriteLine("Błąd: Nie znaleziono godzin dla podanej daty.");
        return null;
    }
}
