using FluentValidation;

namespace SportsCenter.Application.Users.Commands.RemoveClientTags;

public class RemoveClientTagsValidator : AbstractValidator<RemoveClientTags>
{
    public RemoveClientTagsValidator()
    {
        RuleFor(x => x.TagIds)
            .NotEmpty().WithMessage("Tag list can not be empty");
    }
}