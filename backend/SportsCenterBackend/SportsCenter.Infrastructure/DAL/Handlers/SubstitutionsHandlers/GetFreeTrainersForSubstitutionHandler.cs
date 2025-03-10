using MediatR;
using Microsoft.EntityFrameworkCore;
using SportsCenter.Application.Substitutions.Queries.GetFreeTrainersForSubstitution;
using SportsCenter.Core.Entities;
using SportsCenter.Infrastructure.DAL;

internal sealed class GetFreeTrainersForSubstitutionHandler : IRequestHandler<GetFreeTrainersForSubstitution, IEnumerable<FreeTrainerForSubstitutionDto>>
{
    private readonly SportsCenterDbContext _dbContext;

    public GetFreeTrainersForSubstitutionHandler(SportsCenterDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    private bool IsTrainerBookedOrUnavailable(int trainerId, DateTime requestedStart, DateTime requestedEnd, int startHourInMinutes, int endHourInMinutes, CancellationToken cancellationToken)
    {
        var requestedStartDateTime = requestedStart;
        var requestedEndDateTime = requestedEnd;

        var isBookedInReservations = _dbContext.Rezerwacjas
            .Any(r =>
                r.TrenerId == trainerId &&
                ((requestedStartDateTime < r.DataDo && requestedEndDateTime > r.DataOd) ||
                 (requestedStartDateTime >= r.DataOd && requestedStartDateTime < r.DataDo) ||
                 (requestedEndDateTime > r.DataOd && requestedEndDateTime <= r.DataDo) ||
                 (requestedStartDateTime <= r.DataOd && requestedEndDateTime >= r.DataDo))
            );

        if (isBookedInReservations)
            return true;

        string dzienTygodnia = requestedStart.ToString("dddd", new System.Globalization.CultureInfo("pl-PL"));

        var isBookedInActivities = _dbContext.GrafikZajecs
            .AsEnumerable()
            .Any(gz =>
                gz.PracownikId == trainerId &&
                gz.DzienTygodnia == dzienTygodnia &&
                (startHourInMinutes < (gz.GodzinaOd.TotalMinutes + gz.CzasTrwania) &&
                 endHourInMinutes > gz.GodzinaOd.TotalMinutes)
            );

        if (isBookedInActivities)
            return true;

        var isUnavailable = _dbContext.BrakDostepnoscis
            .AsEnumerable()
            .Any(bd =>
                bd.PracownikId == trainerId &&
                (
                    (requestedStartDateTime >= bd.Data.ToDateTime(bd.GodzinaOd) && requestedStartDateTime < bd.Data.ToDateTime(bd.GodzinaDo)) ||
                    (requestedEndDateTime > bd.Data.ToDateTime(bd.GodzinaOd) && requestedEndDateTime <= bd.Data.ToDateTime(bd.GodzinaDo)) ||
                    (requestedStartDateTime <= bd.Data.ToDateTime(bd.GodzinaOd) && requestedEndDateTime >= bd.Data.ToDateTime(bd.GodzinaDo))
                )
            );

        return isUnavailable;
    }

    public async Task<IEnumerable<FreeTrainerForSubstitutionDto>> Handle(GetFreeTrainersForSubstitution request, CancellationToken cancellationToken)
    {
        //uwaga: typ pracownika 3 jako trener (mozna by bylo ewentualnie znajdowac id po nazwie Trener"
        //o ile to jest dużo lepszy pomysł bo po czesci tez zaklada potrzebe seedowania danych
        var allTrainers = await _dbContext.Pracowniks
            .Where(p => p.IdTypPracownika == 3 && p.DataZwolnienia == null)
            .Include(p => p.PracownikNavigation)
            .ToListAsync(cancellationToken);


        var availableTrainers = new List<FreeTrainerForSubstitutionDto>();

        foreach (var trainer in allTrainers)
        {          
            var requestedStart = request.Date.Add(request.StartHour);
            var requestedEnd = request.Date.Add(request.EndHour);

            if (!IsTrainerBookedOrUnavailable(trainer.PracownikId, requestedStart, requestedEnd, (int)request.StartHour.TotalMinutes, (int)request.EndHour.TotalMinutes, cancellationToken))
            {
                if (trainer.PracownikNavigation != null)
                {                   
                    availableTrainers.Add(new FreeTrainerForSubstitutionDto
                    {
                        EmployeeId = trainer.PracownikId,
                        Name = trainer.PracownikNavigation.Imie,
                        Surname = trainer.PracownikNavigation.Nazwisko
                    });
                }            
            }
        }

        return availableTrainers;
    }
}
