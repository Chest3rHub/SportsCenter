using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Exceptions.ReviewsExceptions
{
    public sealed class ReviewTimeExceededException : Exception
    {
        public ReviewTimeExceededException() : base("the time for submitting a rating has passed")
        {
        }
    }
}
