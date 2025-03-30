using SportsCenter.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SportsCenter.Core.Enums.PaymentResult;

namespace SportsCenter.Core.Repositories
{
    public interface IClientRepository
    {
        Task AddClientAsync(Klient client, CancellationToken cancellationToken);
        Task<Klient?> GetClientByEmailAsync(string email, CancellationToken cancellationToken);
        Task<Klient?> GetClientByIdAsync(int id, CancellationToken cancellationToken);
        Task UpdateClientAsync(Klient client, CancellationToken cancellationToken);
        Task<Tag?> GetTagById(int id, CancellationToken cancellationToken);
        Task<bool> RemoveClientTagsAsync(int clientId, List<int> tagIds, CancellationToken cancellationToken);
        Task<int?> GetActivityDiscountForClientAsync(int clientId, CancellationToken cancellationToken);
        Task<int> GetProductDiscountForClientAsync(int clientId, CancellationToken cancellationToken);
        Task<PaymentResultEnum> PayForActivityAsync(int activityId, int clientId, CancellationToken cancellationToken);
        Task<PaymentResultEnum> PayForReservationAsync(int activityInstanceId, int clientId, CancellationToken cancellationToken);
        Task<List<Klient>> GetClientsWhoPaidForCancelledActivitiesAsync(int instanceOfActivityId, CancellationToken cancellationToken);
        Task<decimal> CalculateRefundAmountAsync(Klient client, int instanceOfActivityId, CancellationToken cancellationToken);
        Task<int> RefundClientAsync(int clientId, decimal amount, int activityId, CancellationToken cancellationToken);
        Task<decimal> GetClientBalanceAsync(int clientId, CancellationToken cancellationToken);
    }
}
