using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Exceptions.ReviewsExceptions
{
    public sealed class ReviewAlreadyExistException : Exception
    {
        public int UserId { get; set; }
        public int ActivityId { get; set; }
        public ReviewAlreadyExistException(int userId, int activityId) : base($"the user with id: {userId} has already rated activity with id: {activityId}")
        {
            UserId = userId;
            ActivityId = activityId;
        }
    }
}
