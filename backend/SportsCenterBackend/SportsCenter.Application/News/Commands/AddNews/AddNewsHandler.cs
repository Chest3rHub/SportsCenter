using MediatR;
using SportsCenter.Core.Entities;
using SportsCenter.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SportsCenter.Application.News.Commands.AddNews
{
    internal sealed class AddNewsHandler : IRequestHandler<AddNews, Unit>
    {
        private readonly INewsRepository _newsRepository;

        public AddNewsHandler(INewsRepository newsRepository, ITagRepository tagRepository)
        {
            _newsRepository = newsRepository;
        }
        public async Task<Unit> Handle(AddNews request, CancellationToken cancellationToken)
        {
            var news = new Aktualnosci
            {
                Nazwa = request.Title,
                Opis = request.Content,
                WazneOd = request.ValidFrom,
                WazneDo = request.ValidUntil
            };

            await _newsRepository.AddNewsAsync(news, cancellationToken);
            
            return Unit.Value;
        }
    }
}
