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
        public async Task DeleteEmployeeAsync(Pracownik employee, CancellationToken cancellationToken)
        {
            _dbContext.Pracowniks.Remove(employee);
            await _dbContext.SaveChangesAsync();
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
    }
}
