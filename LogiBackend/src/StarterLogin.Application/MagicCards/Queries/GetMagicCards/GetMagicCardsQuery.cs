using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using StarterLogin.Domain.Interfaces;
using StarterLogin.Application.MagicCards.Queries;

namespace StarterLogin.Application.MagicCards.Queries.GetMagicCards;

public record GetMagicCardsQuery(bool IncludeDrafts) : IRequest<List<MagicCardDto>>;

public class GetMagicCardsQueryHandler : IRequestHandler<GetMagicCardsQuery, List<MagicCardDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetMagicCardsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<MagicCardDto>> Handle(GetMagicCardsQuery request, CancellationToken cancellationToken)
    {
        var cards = request.IncludeDrafts 
            ? await _unitOfWork.MagicCards.GetAllAsync()
            : await _unitOfWork.MagicCards.GetPublishedAsync();

        return cards.Select(c => new MagicCardDto(
            c.Id,
            c.Title,
            c.Description,
            c.ImageUrl,
            c.IsPublished,
            c.CreatedAt
        )).ToList();
    }
}
