using SportsCenter.Core.Entities;
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
        Task AddTaskAsync(Zadanie task, CancellationToken cancellationToken);
        Task<Zadanie?> GetTaskByIdAsync(int id, CancellationToken cancellationToken);
        Task RemoveTaskAsync(Zadanie task, CancellationToken cancellationToken);
    }
}
