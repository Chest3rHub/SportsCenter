using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SportsCenter.Core.Entities;
using SportsCenter.Core.Enums;
using SportsCenter.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static SportsCenter.Core.Enums.PaymentResult;
using static SportsCenter.Core.Enums.TrainerAvailiabilityStatus;

namespace SportsCenter.Infrastructure.DAL.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private SportsCenterDbContext _dbContext;

        public EmployeeRepository(SportsCenterDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<TypPracownika?> GetTypeOfEmployeeIdAsync(string positionName, CancellationToken cancellationToken)
        {
            return _dbContext.TypPracownikas.Where(o => o.Nazwa == positionName).FirstOrDefaultAsync(cancellationToken: cancellationToken);
        }

        public async Task AddEmployeeAsync(Pracownik employee, CancellationToken cancellationToken)
        {
            await _dbContext.Pracowniks.AddAsync(employee, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<Pracownik?> GetEmployeeByEmailAsync(string email, CancellationToken cancellationToken)
        {
            return await _dbContext.Pracowniks
                .Include(p => p.PracownikNavigation)
                .FirstOrDefaultAsync(p => p.PracownikNavigation.Email == email, cancellationToken);
        }

        public async Task<Pracownik?> GetEmployeeByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _dbContext.Pracowniks
                .Include(p => p.PracownikNavigation)
                .FirstOrDefaultAsync(p => p.PracownikNavigation.OsobaId == id, cancellationToken);
        }
        public async Task DeleteEmployeeAsync(int id, DateTime dismissalDate, CancellationToken cancellationToken)
        {
            var pracownik = await _dbContext.Pracowniks
                .Where(p => p.PracownikId == id)
                .FirstOrDefaultAsync(cancellationToken);

            pracownik.DataZwolnienia = DateOnly.FromDateTime(dismissalDate);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task AddTaskAsync(Zadanie task, CancellationToken cancellationToken)
        {
            await _dbContext.Zadanies.AddAsync(task, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<Zadanie?> GetTaskByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _dbContext.Zadanies
                .FirstOrDefaultAsync(z => z.ZadanieId == id, cancellationToken);
        }

        public async Task RemoveTaskAsync(Zadanie task, CancellationToken cancellationToken)
        {
            _dbContext.Zadanies.Remove(task);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateTaskAsync(Zadanie task, CancellationToken cancellationToken)
        {
            _dbContext.Zadanies.Update(task);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        public async Task<Pracownik> GetEmployeeWithFewestOrdersAsync(CancellationToken cancellationToken)
        {
            return await _dbContext.Pracowniks
                .Include(p => p.IdTypPracownikaNavigation)
                .Where(p => p.IdTypPracownikaNavigation.Nazwa == "Pracownik administracyjny")
                .OrderBy(p => _dbContext.Zamowienies.Count(z => z.PracownikId == p.PracownikId && z.Status != "Zrealizowane"))
                .FirstOrDefaultAsync(cancellationToken);
        }


        public async Task<int?> GetEmployeeTypeByNameAsync(string name, CancellationToken cancellationToken)
        {
            return await _dbContext.TypPracownikas
                .Where(t => t.Nazwa == name)
                .Select(t => (int?)t.IdTypPracownika)
                .FirstOrDefaultAsync(cancellationToken);
        }
        public async Task<string> GetEmployeePositionNameByIdAsync(int employeeId, CancellationToken cancellationToken)
        {
            var employee = await _dbContext.Pracowniks
                .Where(p => p.PracownikId == employeeId)
                .FirstOrDefaultAsync(cancellationToken);

            if (employee == null)
            {
                return null;
            }

            var position = await _dbContext.TypPracownikas
                .Where(tp => tp.IdTypPracownika == employee.IdTypPracownika)
                .Select(tp => tp.Nazwa)
                .FirstOrDefaultAsync(cancellationToken);

            return position;
        }

        public async Task AddTrainerCertificateAsync(TrenerCertyfikat trainerCertificate, CancellationToken cancellationToken)
        {
            await _dbContext.TrenerCertyfikats.AddAsync(trainerCertificate, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        public async Task<TrenerCertyfikat?> GetTrainerCertificateByIdAsync(int trainerId, int certificateId, CancellationToken cancellationToken)
        {
            return await _dbContext.TrenerCertyfikats
                .FirstOrDefaultAsync(tc => tc.PracownikId == trainerId && tc.CertyfikatId == certificateId, cancellationToken);
        }

        public async Task DeleteTrainerCertificateAsync(TrenerCertyfikat certificate, CancellationToken cancellationToken)
        {
            _dbContext.TrenerCertyfikats.Remove(certificate);

            var certificat = await _dbContext.Certyfikats
                .FirstAsync(c => c.CertyfikatId == certificate.CertyfikatId, cancellationToken);
           
            _dbContext.Certyfikats.Remove(certificat);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        public async Task<TrenerCertyfikat?> GetTrainerCertificateWithDetailsByIdAsync(int trainerId, int certificateId, CancellationToken cancellationToken)
        {
            return await _dbContext.TrenerCertyfikats
                .Include(tc => tc.Certyfikat)
                .FirstOrDefaultAsync(tc => tc.PracownikId == trainerId && tc.CertyfikatId == certificateId, cancellationToken);
        }
        public async Task UpdateTrainerCertificateAsync(TrenerCertyfikat trainerCertificate, CancellationToken cancellationToken)
        {
            _dbContext.TrenerCertyfikats.Update(trainerCertificate);

            var certificate = await _dbContext.Certyfikats
                .FirstAsync(c => c.CertyfikatId == trainerCertificate.CertyfikatId, cancellationToken);

            certificate.Nazwa = trainerCertificate.Certyfikat.Nazwa;
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        public async Task AddAbsenceRequestAsync(BrakDostepnosci absenceRequest, CancellationToken cancellationToken)
        {
            await _dbContext.BrakDostepnoscis.AddAsync(absenceRequest);
            await _dbContext.SaveChangesAsync();
        }
        public async Task<BrakDostepnosci?> GetAbsenceRequestAsync(int employeeId, DateOnly date, CancellationToken cancellationToken)
        {
            return await _dbContext.BrakDostepnoscis
                .Where(a => a.PracownikId == employeeId && a.Data == date)
                .FirstOrDefaultAsync();
        }
        public async Task<BrakDostepnosci?> GetAbsenceRequestAsync(int requestId, CancellationToken cancellationToken)
        {
            return await _dbContext.BrakDostepnoscis
                .Where(a => a.BrakDostepnosciId == requestId)
                .FirstOrDefaultAsync();
        }
        public async Task UpdateAbsenceRequestAsync(BrakDostepnosci absenceRequest, CancellationToken cancellationToken)
        {
            _dbContext.BrakDostepnoscis.Update(absenceRequest);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        public async Task UpdateAbsenceRequestAsync(int requestId, CancellationToken cancellationToken)
        {
            var absence = await _dbContext.BrakDostepnoscis
           .FirstOrDefaultAsync(b => b.BrakDostepnosciId == requestId, cancellationToken);

            absence.CzyZatwierdzone = true;
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        public async Task<bool> ExistsAbsenceRequestAsync(int requestId, CancellationToken cancellationToken)
        {
            return await _dbContext.BrakDostepnoscis
                .AnyAsync(b => b.BrakDostepnosciId == requestId, cancellationToken);
        }

        public async Task<bool> IsAbsenceRequestPendingAsync(int requestId, CancellationToken cancellationToken)
        {
            return await _dbContext.BrakDostepnoscis
                .AnyAsync(b => b.BrakDostepnosciId == requestId && !b.CzyZatwierdzone, cancellationToken);
        }
        public async Task<IEnumerable<Pracownik>> GetAvailableTrainersAsync(DateTime startTime, DateTime endTime, CancellationToken cancellationToken)
        {
            int startHourInMinutes = (int)startTime.TimeOfDay.TotalMinutes;
            int endHourInMinutes = (int)endTime.TimeOfDay.TotalMinutes;

            var trainers = await _dbContext.Pracowniks
                   .Where(p => p.IdTypPracownikaNavigation.Nazwa == "Trener")
                   .ToListAsync(cancellationToken);

            var availableTrainers = new List<Pracownik>();

            foreach (var trainer in trainers)
            {

                var availabilityStatus = await IsTrainerAvailableAsync(trainer.PracownikId, startTime, startHourInMinutes, endHourInMinutes, cancellationToken);

                if (availabilityStatus == TrainerAvailabilityStatus.Available)
                {
                    availableTrainers.Add(trainer);
                }
            }
            return availableTrainers;
        }

        public async Task<TrainerAvailabilityStatus> IsTrainerAvailableAsync(int trainerId, DateTime requestedStart, int startHourInMinutes, int endHourInMinutes, CancellationToken cancellationToken)
        {
            DateTime requestedStartTime = requestedStart.Date.AddMinutes(startHourInMinutes);
            DateTime requestedEndTime = requestedStart.Date.AddMinutes(endHourInMinutes);

            var isFired = await _dbContext.Pracowniks
                .Where(p => p.PracownikId == trainerId)
                .Select(p => p.DataZwolnienia)
                .FirstOrDefaultAsync(cancellationToken);

            if (isFired != null) return TrainerAvailabilityStatus.IsFired;

            var hasReservations = await _dbContext.Rezerwacjas
                .AnyAsync(r =>
                    r.TrenerId == trainerId &&
                    r.CzyOdwolana == false &&
                    ((requestedStartTime >= r.DataOd && requestedStartTime < r.DataDo) ||
                     (requestedEndTime > r.DataOd && requestedEndTime <= r.DataDo) ||
                     (requestedStartTime <= r.DataOd && requestedEndTime >= r.DataDo)),
                    cancellationToken);

            if (hasReservations) return TrainerAvailabilityStatus.HasReservations;

            var dniTygodnia = new Dictionary<DayOfWeek, string>
            {
                { DayOfWeek.Monday, "poniedzialek" },
                { DayOfWeek.Tuesday, "wtorek" },
                { DayOfWeek.Wednesday, "sroda" },
                { DayOfWeek.Thursday, "czwartek" },
                { DayOfWeek.Friday, "piatek" },
                { DayOfWeek.Saturday, "sobota" },
                { DayOfWeek.Sunday, "niedziela" }
            };

            string dayOfWeek = dniTygodnia[requestedStart.DayOfWeek];

            var activitiesSchedule = await _dbContext.GrafikZajecs
                .Where(gz => gz.PracownikId == trainerId && gz.DzienTygodnia == dayOfWeek)
                .ToListAsync(cancellationToken);

            foreach (var grafik in activitiesSchedule)
            {
                int godzinaOdInMinutes = (int)grafik.GodzinaOd.TotalMinutes;
                int godzinaDoInMinutes = godzinaOdInMinutes + grafik.CzasTrwania;
              
                if ((startHourInMinutes < godzinaDoInMinutes && endHourInMinutes > godzinaOdInMinutes))
                {
                    return TrainerAvailabilityStatus.HasActivities;
                }
            }

            var unavailability = await _dbContext.BrakDostepnoscis
                .Where(bd => bd.PracownikId == trainerId)
                .ToListAsync(cancellationToken);

            foreach (var bd in unavailability)
            {
                DateTime startDateTime = bd.Data.ToDateTime(bd.GodzinaOd);
                DateTime endDateTime = bd.Data.ToDateTime(bd.GodzinaDo);

                if ((requestedStartTime >= startDateTime && requestedStartTime < endDateTime) ||
                    (requestedEndTime > startDateTime && requestedEndTime <= endDateTime) ||
                    (requestedStartTime <= startDateTime && requestedEndTime >= endDateTime))
                {
                    return TrainerAvailabilityStatus.IsUnavailable;
                }
            }
            return TrainerAvailabilityStatus.Available;
        }
        public async Task<bool> IsEmployeeDismissedAsync(int employeeId, CancellationToken cancellationToken)
        {
            return await _dbContext.Pracowniks
                .Where(p => p.PracownikId == employeeId)
                .Select(p => p.DataZwolnienia != null)
                .FirstOrDefaultAsync(cancellationToken);
        }
        public async Task<PaymentResultEnum> PayForActivityAsync(int activityInstanceId, string clientEmail, CancellationToken cancellationToken)
        {
            var activityInstance = await _dbContext.InstancjaZajecs
                .Include(i => i.InstancjaZajecKlients)
                .ThenInclude(ik => ik.Klient)
                .Include(i => i.GrafikZajec)
                .Where(i => i.InstancjaZajecId == activityInstanceId && i.InstancjaZajecKlients.Any(ik => ik.Klient.KlientNavigation.Email == clientEmail))
                .AsTracking()
                .FirstOrDefaultAsync(cancellationToken);

            if (activityInstance == null)
            {
                return PaymentResultEnum.ActivityInstanceNotFound;
            }

            if (activityInstance.CzyOdwolane.HasValue && activityInstance.CzyOdwolane == true)
            {
                return PaymentResultEnum.ActivityCanceled;
            }

            var activityInstanceClient = await _dbContext.InstancjaZajecKlients
              .Where(ai => ai.InstancjaZajecId == activityInstance.InstancjaZajecId)
              .FirstOrDefaultAsync(cancellationToken);

            if (activityInstanceClient.DataWypisu.HasValue)
            {
                return PaymentResultEnum.ClientWithdrawn;
            }

            if (activityInstanceClient.CzyOplacone == true)
            {
                return PaymentResultEnum.AlreadyPaid;
            }

            var activitySchedule = await _dbContext.GrafikZajecs
               .Where(asch => asch.GrafikZajecId == activityInstance.GrafikZajecId)
               .FirstOrDefaultAsync(cancellationToken);

            decimal cost = activityInstanceClient.CzyUwzglednicSprzet ? activitySchedule.KosztZeSprzetem : activitySchedule.KosztBezSprzetu;

            var client = await _dbContext.Klients
                .Where(c => c.KlientNavigation.Email == clientEmail)
                .FirstOrDefaultAsync(cancellationToken);

            if (client == null)
            {
                return PaymentResultEnum.ClientNotFound;
            }

            decimal discount = client.ZnizkaNaZajecia ?? 0m;
            if (discount > 0)
            {
                cost *= (1 - discount / 100m);
            }

            if (client.Saldo < cost)
            {
                return PaymentResultEnum.InsufficientFunds;
            }

            client.Saldo -= cost;
            _dbContext.Entry(client).State = EntityState.Modified;

            activityInstanceClient.CzyOplacone = true;
            _dbContext.Entry(activityInstanceClient).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                return PaymentResultEnum.InsufficientFunds;
            }

            return PaymentResultEnum.Success;
        }
        public async Task<PaymentResultEnum> PayForClientReservationAsync(int reservationId, string clientEmail, CancellationToken cancellationToken)
        {
            var reservation = await _dbContext.Rezerwacjas
         .Include(r => r.Klient)
             .ThenInclude(k => k.KlientNavigation)
         .FirstOrDefaultAsync(r => r.RezerwacjaId == reservationId, cancellationToken);

            if (reservation == null)
                return PaymentResultEnum.ActivityNotFound;

            if (reservation.Klient.KlientNavigation.Email != clientEmail)
                return PaymentResultEnum.ClientNotFound;

            if (reservation.CzyOplacona.HasValue && reservation.CzyOplacona.Value)
                return PaymentResultEnum.AlreadyPaid;

            var client = await _dbContext.Klients
                .FirstOrDefaultAsync(k => k.KlientNavigation.Email == clientEmail, cancellationToken);

            if (client == null)
                return PaymentResultEnum.ClientNotFound;

            decimal cost = reservation.Koszt;

            if (client.Saldo < cost)
                return PaymentResultEnum.InsufficientFunds;

            client.Saldo -= cost;
            reservation.CzyOplacona = true;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return PaymentResultEnum.Success;
        }
    }
}
