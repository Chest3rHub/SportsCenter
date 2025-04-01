using SportsCenter.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Employees.Queries.GetTrainerCertificates
{
    public class GetTrainerCertificates : IQuery<IEnumerable<TrainerCertificateDto>>
    {
        public int TrainerId { get; set; }
        public int Offset { get; set; } = 0;

        public GetTrainerCertificates(int trainerId, int offset)
        {
            TrainerId = trainerId;
            Offset = offset;
        }
    }
}
