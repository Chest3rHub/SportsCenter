using Microsoft.EntityFrameworkCore;
using SportsCenter.Core.Entities;
using SportsCenter.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Infrastructure.DAL.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private SportsCenterDbContext _dbContext;

        public EmployeeRepository(SportsCenterDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<TypPracownika?> GetTypeOfEmployeeIdAsync(string positionName, CancellationToken cancellationToken) {
            return _dbContext.TypPracownikas.Where(o => o.Nazwa == positionName).FirstOrDefaultAsync(cancellationToken: cancellationToken);
        }

        public async Task AddEmployeeAsync(Pracownik employee, CancellationToken cancellationToken) {
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
        public async Task<Pracownik> GetEmployeeWithLeastOrdersAsync(CancellationToken cancellationToken)
        {
            return await _dbContext.Pracowniks
                .OrderBy(p => _dbContext.Zamowienies.Count(z => z.PracownikId == p.PracownikId && z.Status != "Zamknięte"))
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
                .Where(tp => tp.IdTypPracownika== employee.IdTypPracownika)
                .Select(tp => tp.Nazwa) 
                .FirstOrDefaultAsync(cancellationToken);

            return position;
        }

        public async Task<Pracownik> GetEmployeeWithFewestOrdersAsync(CancellationToken cancellationToken)
        {
            return await _dbContext.Pracowniks
                .OrderBy(p => _dbContext.Zamowienies.Count(o => o.PracownikId == p.PracownikId))
                .FirstOrDefaultAsync(cancellationToken);
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

    }
}
