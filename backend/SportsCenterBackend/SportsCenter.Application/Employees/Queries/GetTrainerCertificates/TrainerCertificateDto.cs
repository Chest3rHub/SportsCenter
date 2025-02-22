using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Employees.Queries.GetTrainerCertificates
{
    public class TrainerCertificateDto 
    {
        public string CertificateName { get; set; }
        public DateOnly ReceivedDate { get; set; }
    }
}
