using FluentValidation;
using SportsCenter.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Reviews.Commands.AddReview
{
    public class AddReviewValidator : AbstractValidator<AddReview>
    {
        public AddReviewValidator()
        {
        }

        public AddReviewValidator(IReviewRepository reviewRepository)
        {                   
            RuleFor(x => x.Stars)
                .InclusiveBetween(0, 10)
                .WithMessage("Ocena musi być w zakresie od 0 do 10.");
        }
    }
}
