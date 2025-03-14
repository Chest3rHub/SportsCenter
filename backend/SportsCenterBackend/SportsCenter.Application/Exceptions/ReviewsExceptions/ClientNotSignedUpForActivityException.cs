using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Exceptions.ReviewsExceptions
{
    public class ClientNotSignedUpForActivityException : Exception
    {
        public int UserId { get; set; }
        public int ActivityId { get; set; }
        public ClientNotSignedUpForActivityException(int userId, int activityId) : base($"client with id: {userId} is not signed up for activity with id: {activityId}")
        {
            UserId = userId;
            ActivityId = activityId;
        }
    }
}
