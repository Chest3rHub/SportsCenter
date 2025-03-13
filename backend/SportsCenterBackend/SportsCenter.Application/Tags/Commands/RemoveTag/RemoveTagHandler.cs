using MediatR;
using SportsCenter.Application.Exceptions.TagsException;
using SportsCenter.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Tags.Commands.RemoveTag
{
    internal sealed class RemoveTagHandler : IRequestHandler<RemoveTag, Unit>
    {
        private readonly ITagRepository _tagRepository;

        public RemoveTagHandler(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        public async Task<Unit> Handle(RemoveTag request, CancellationToken cancellationToken)
        {
            bool isTagRemoved = await _tagRepository.RemoveTagAsync(request.TagId, cancellationToken);
            if (!isTagRemoved) {
                throw new TagNotFoundException(request.TagId);
            }
            return Unit.Value;
        }
    }
}
