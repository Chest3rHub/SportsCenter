using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Employees.Queries.GetTasks
{
    public class TaskDto
    {
        public int TaskId { get; set; }
        public string Description { get; set; }
        public DateOnly DateTo { get; set; }
        public int EmpId { get; set; }
        public int AssigningEmpId { get; set; }
        
    }
}
