using SportsCenter.Application.Abstractions;
using SportsCenter.Application.Employees.Queries.GetTasks;

namespace SportsCenter.Application.Employees.Queries.GetEmployeeTasksByOwner
{
    public class GetEmployeeTasksByOwner : IQuery<IEnumerable<TaskDto>>
    {
        public int EmployeeId { get; set; }
        
        public GetEmployeeTasksByOwner(int employeeId)
        {
            EmployeeId = employeeId;
        }
    }
}