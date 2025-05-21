using SportsCenter.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SportsCenter.Core.Enums.PaymentResult;
using static SportsCenter.Core.Enums.TrainerAvailiabilityStatus;

namespace SportsCenter.Core.Repositories
{
    public interface IEmployeeRepository
    {
        Task<TypPracownika?> GetTypeOfEmployeeIdAsync(string positionName, CancellationToken cancellationToken);
        Task AddEmployeeAsync(Pracownik employee, CancellationToken cancellationToken);
        Task<Pracownik?> GetEmployeeByEmailAsync(string email, CancellationToken cancellationToken);
        Task<Pracownik?> GetEmployeeByIdAsync(int id, CancellationToken cancellationToken);
        Task DeleteEmployeeAsync(int id, DateTime dismissalDate, CancellationToken cancellationToken);
        Task AddTaskAsync(Zadanie task, CancellationToken cancellationToken);
        Task<Zadanie?> GetTaskByIdAsync(int id, CancellationToken cancellationToken);
        Task RemoveTaskAsync(Zadanie task, CancellationToken cancellationToken);
        Task UpdateTaskAsync(Zadanie task, CancellationToken cancellationToken);
        Task<Pracownik> GetEmployeeWithFewestOrdersAsync(CancellationToken cancellationToken);
        Task<int?> GetEmployeeTypeByNameAsync(string name, CancellationToken cancellationToken);
        Task<string> GetEmployeePositionNameByIdAsync(int employeeId, CancellationToken cancellationToken);
        Task AddTrainerCertificateAsync(TrenerCertyfikat trainerCertificate, CancellationToken cancellationToken);
        Task<TrenerCertyfikat?> GetTrainerCertificateByIdAsync(int trainerId, int certificateId, CancellationToken cancellationToken);
        Task DeleteTrainerCertificateAsync(TrenerCertyfikat certificate, CancellationToken cancellationToken);
        Task<TrenerCertyfikat?> GetTrainerCertificateWithDetailsByIdAsync(int trainerId, int certificateId, CancellationToken cancellationToken);
        Task UpdateTrainerCertificateAsync(TrenerCertyfikat trainerCertificate, CancellationToken cancellationToken);
        Task AddAbsenceRequestAsync(BrakDostepnosci absenceRequest, CancellationToken cancellationToken);
        Task<BrakDostepnosci?> GetAbsenceRequestAsync(int employeeId, DateOnly date, CancellationToken cancellationToken);
        Task<BrakDostepnosci?> GetAbsenceRequestAsync(int requestId, CancellationToken cancellationToken);
        Task UpdateAbsenceRequestAsync(BrakDostepnosci absenceRequest, CancellationToken cancellationToken);
        Task UpdateAbsenceRequestAsync(int requestId, CancellationToken cancellationToken);
        Task<bool> ExistsAbsenceRequestAsync(int requestId, CancellationToken cancellationToken);
        Task<bool> IsAbsenceRequestPendingAsync(int requestId, CancellationToken cancellationToken);
        Task<TrainerAvailabilityStatus> IsTrainerAvailableAsync(int trainerId, DateTime requestedStart, int startHourInMinutes, int endHourInMinutes, CancellationToken cancellationToken, int? reservationToExclude = null);
        Task<IEnumerable<Pracownik>> GetAvailableTrainersAsync(DateTime startTime, DateTime endTime, CancellationToken cancellationToken);
        Task<bool> IsEmployeeDismissedAsync(int employeeId, CancellationToken cancellationToken);
        Task<PaymentResultEnum> PayForActivityAsync(int activityInstanceId, string clientEmail, CancellationToken cancellationToken);
        Task<PaymentResultEnum> PayForClientReservationAsync(int activityInstanceId, string clientEmail, CancellationToken cancellationToken);

    }
}
