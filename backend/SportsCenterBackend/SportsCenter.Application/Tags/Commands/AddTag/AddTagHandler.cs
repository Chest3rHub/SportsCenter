using MediatR;
using SportsCenter.Application.Exceptions.TagsException;
using SportsCenter.Core.Repositories;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Tags.Commands.AddTag
{
    internal sealed class AddTagHandler : IRequestHandler<AddTag, Unit>
    {
        private readonly ITagRepository _tagRepository;

        public AddTagHandler(ITagRepository tagRepository)
        {         
            _tagRepository = tagRepository;
        }
        public async Task<Unit> Handle(AddTag request, CancellationToken cancellationToken)
        {
            bool isTagAddded =  await _tagRepository.AddTagAsync(request.TagName, cancellationToken);
            if (!isTagAddded)
            {
                throw new TagAlreadyExistException(request.TagName);
            }
            return Unit.Value;
        }
    }
}
