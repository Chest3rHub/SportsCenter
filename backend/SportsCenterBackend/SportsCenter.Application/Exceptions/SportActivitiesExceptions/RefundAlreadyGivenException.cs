using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Exceptions.SportActivitiesExceptions
{
    public class RefundAlreadyGivenException : Exception
    {
        public int ClientId { get; set; }
        public int InstanceOfActivityId { get; set; }

        public RefundAlreadyGivenException(int clientId, int instanceOfActivityId) : base($"Refund for activity with id: {instanceOfActivityId} already given to te client with id: {clientId}.")
        {
            ClientId = clientId;
            InstanceOfActivityId = instanceOfActivityId;
        }
    }
}
