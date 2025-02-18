using MediatR;
using SportsCenter.Application.Exceptions.NewsExceptions;
using SportsCenter.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.News.Commands.RemoveNews
{
    internal sealed class RemoveNewsHandler : IRequestHandler<RemoveNews, Unit>
    {
        private readonly INewsRepository _newsRepository;

        public RemoveNewsHandler(INewsRepository newsRepository, ITagRepository tagRepository)
        {
            _newsRepository = newsRepository;
        }
        public async Task<Unit> Handle(RemoveNews request, CancellationToken cancellationToken)
        {
            var news = await _newsRepository.GetNewsByIdAsync(request.NewsId, cancellationToken);
            if (news == null)
            {
                throw new NewsNotFoundException(request.NewsId);
            }

            await _newsRepository.RemoveNewsAsync(news, cancellationToken);
            return Unit.Value;
        }
    }
}
