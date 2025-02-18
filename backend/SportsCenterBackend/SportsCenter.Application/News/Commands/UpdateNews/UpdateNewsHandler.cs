using MediatR;
using SportsCenter.Application.Exceptions.NewsExceptions;
using SportsCenter.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.News.Commands.UpdateNews
{
    internal class UpdateNewsHandler : IRequestHandler<UpdateNews, Unit>
    {
        private readonly INewsRepository _newsRepository;

        public UpdateNewsHandler(INewsRepository newsRepository, ITagRepository tagRepository)
        {
            _newsRepository = newsRepository;
        }

        public async Task<Unit> Handle(UpdateNews request, CancellationToken cancellationToken)
        {
            var news = await _newsRepository.GetNewsByIdAsync(request.NewsId, cancellationToken);
            if (news == null)
            {
                throw new NewsNotFoundException(request.NewsId);
            }

            news.Nazwa = request.Title;
            news.Opis = request.Content;
            news.WazneOd = request.ValidFrom;
            news.WazneDo = request.ValidUntil;

            await _newsRepository.UpdateNewsAsync(news, cancellationToken);

            return Unit.Value;
        }
    }
}
