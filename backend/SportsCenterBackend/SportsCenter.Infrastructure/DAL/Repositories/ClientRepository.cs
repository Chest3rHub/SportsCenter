﻿using Azure.Core;
using Microsoft.EntityFrameworkCore;
using SportsCenter.Core.Entities;
using SportsCenter.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SportsCenter.Core.Enums.PaymentResult;

namespace SportsCenter.Infrastructure.DAL.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private SportsCenterDbContext _dbContext;

        public ClientRepository(SportsCenterDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddClientAsync(Klient client, CancellationToken cancellationToken)
        {
            await _dbContext.Klients.AddAsync(client, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<Klient?> GetClientByEmailAsync(string email, CancellationToken cancellationToken)
        {
            return await _dbContext.Klients
                .Include(k => k.KlientNavigation)
                .FirstOrDefaultAsync(k => k.KlientNavigation.Email == email, cancellationToken);
        }

        public async Task<Klient?> GetClientByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _dbContext.Klients
                .Include(k => k.KlientNavigation)
                .FirstOrDefaultAsync(k => k.KlientNavigation.OsobaId == id, cancellationToken);
        }

        public async Task UpdateClientAsync(Klient client, CancellationToken cancellationToken)
        {
            _dbContext.Klients.Update(client);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<Tag?> GetTagById(int id, CancellationToken cancellationToken)
        {
            return await _dbContext.Tags.Where(o => o.TagId == id).FirstOrDefaultAsync(cancellationToken: cancellationToken);
        }

        public async Task<bool> RemoveClientTagsAsync(int clientId, List<int> tagIds, CancellationToken cancellationToken)
        {
            var client = await _dbContext.Klients
                .Include(c => c.Tags)
                .FirstOrDefaultAsync(c => c.KlientId == clientId, cancellationToken);

            if (client == null)
                return false;

            var tagsToRemove = client.Tags.Where(t => tagIds.Contains(t.TagId)).ToList();
            if (!tagsToRemove.Any())
                return false;

            foreach (var tag in tagsToRemove)
            {
                client.Tags.Remove(tag);
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }
        public async Task<int?> GetActivityDiscountForClientAsync(int clientId, CancellationToken cancellationToken)
        {
            var client = await _dbContext.Klients
                .Where(c => c.KlientId == clientId)
                .FirstOrDefaultAsync(cancellationToken);

            return client?.ZnizkaNaZajecia;
        }
        public async Task<int> GetProductDiscountForClientAsync(int clientId, CancellationToken cancellationToken)
        {
            var client = await _dbContext.Klients
                .Where(c => c.KlientId == clientId)
                .FirstOrDefaultAsync(cancellationToken);

            return client?.ZnizkaNaProdukty ?? 0;
        }
        public async Task<PaymentResultEnum> PayForActivityAsync(int activityInstanceId, int clientId, CancellationToken cancellationToken)
        {
            var activityInstance = await _dbContext.InstancjaZajecs
                .Include(i => i.InstancjaZajecKlients)
                .ThenInclude(ik => ik.Klient)
                .Include(i => i.GrafikZajec)
                .Where(i => i.InstancjaZajecId == activityInstanceId && i.InstancjaZajecKlients.Any(ik => ik.Klient.KlientId == clientId))
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
                .Where(c => c.KlientId == clientId)
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
        public async Task<PaymentResultEnum> PayForReservationAsync(int reservationId, int clientId, CancellationToken cancellationToken)
        {
            var reservation = await _dbContext.Rezerwacjas
                .FirstOrDefaultAsync(r => r.RezerwacjaId == reservationId, cancellationToken);

            if (reservation == null)
                return PaymentResultEnum.ActivityNotFound;

            if (reservation.KlientId != clientId)
                return PaymentResultEnum.ClientNotFound;

            if (reservation.CzyOplacona.HasValue && reservation.CzyOplacona.Value)
                return PaymentResultEnum.AlreadyPaid;

            var client = await _dbContext.Klients
                .FirstOrDefaultAsync(k => k.KlientId == clientId, cancellationToken);

            if (client == null)
                return PaymentResultEnum.ClientNotFound;

            decimal cost = reservation.Koszt;

            decimal discount = client.ZnizkaNaZajecia ?? 0m;
            if (discount > 0)
            {
                cost *= (1 - discount / 100m);
            }

            if (client.Saldo < cost)
                return PaymentResultEnum.InsufficientFunds;

            client.Saldo -= cost;
            reservation.CzyOplacona = true;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return PaymentResultEnum.Success;
        }

    }
}
