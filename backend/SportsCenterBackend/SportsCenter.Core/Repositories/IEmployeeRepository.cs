﻿using SportsCenter.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        Task<Pracownik> GetEmployeeWithLeastOrdersAsync(CancellationToken cancellationToken);
        Task<int?> GetEmployeeTypeByNameAsync(string name, CancellationToken cancellationToken);
        Task<string> GetEmployeePositionNameByIdAsync(int employeeId, CancellationToken cancellationToken);
        Task<Pracownik> GetEmployeeWithFewestOrdersAsync(CancellationToken cancellationToken);
        Task AddTrainerCertificateAsync(TrenerCertifikat trainerCertificate, CancellationToken cancellationToken);
        Task<TrenerCertifikat?> GetTrainerCertificateByIdAsync(int trainerId, int certificateId, CancellationToken cancellationToken);
        Task DeleteTrainerCertificateAsync(TrenerCertifikat certificate, CancellationToken cancellationToken);
        Task<TrenerCertifikat?> GetTrainerCertificateWithDetailsByIdAsync(int trainerId, int certificateId, CancellationToken cancellationToken);
        Task UpdateTrainerCertificateAsync(TrenerCertifikat trainerCertificate, CancellationToken cancellationToken);
    }
}
