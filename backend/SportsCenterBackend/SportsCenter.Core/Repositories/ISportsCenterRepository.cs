﻿using SportsCenter.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Core.Repositories
{
    public interface ISportsCenterRepository
    {
        Task<bool> CheckIfDayExistsAsync(string dayOfWeek, CancellationToken cancellationToken);
        Task AddWorkingHoursForGivenDay(GodzinyPracyKlubu workingHoursOfDay, CancellationToken cancellationToken);
        Task UpdateWorkingHours(GodzinyPracyKlubu workingHours, CancellationToken cancellationToken);
        Task<GodzinyPracyKlubu> GetWorkingHoursByDayAsync(string dayOfWeek, CancellationToken cancellationToken);
        Task<WyjatkoweGodzinyPracy> GetSpecialWorkingHoursByDateAsync(DateTime date, CancellationToken cancellationToken);
        Task UpdateDateWorkingHours(WyjatkoweGodzinyPracy workingHours, CancellationToken cancellationToken);
        Task AddWorkingHoursForGivenDate(WyjatkoweGodzinyPracy workingHoursOfDay, CancellationToken cancellationToken);
        Task<List<object>> GetConflictingReservationsOrActivities(DateOnly date, TimeOnly newOpenHour, TimeOnly newCloseHour, CancellationToken cancellationToken);
        Task<IEnumerable<object>> GetConflictingReservationsOrActivitiesByDayOfWeek(DayOfWeek dayOfWeek, TimeOnly newOpenHour, TimeOnly newCloseHour, CancellationToken cancellationToken);
    }
}
