using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Core.Enums
{
    public class ClientReviewStatus
    {
        public enum ReviewStatus
        {
            CanReview,
            NotSignedUp,
            ReviewPeriodExpired
        }
    }
}
