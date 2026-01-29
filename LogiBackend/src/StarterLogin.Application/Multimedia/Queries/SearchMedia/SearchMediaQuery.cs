using MediatR;
using StarterLogin.Application.Multimedia.Common;
using StarterLogin.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StarterLogin.Application.Multimedia.Queries.SearchMedia;

public record SearchMediaQuery(string Query) : IRequest<IEnumerable<MediaResponse>>;

public class SearchMediaQueryHandler : IRequestHandler<SearchMediaQuery, IEnumerable<MediaResponse>>
{
    private readonly IUnitOfWork _unitOfWork;

    public SearchMediaQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<MediaResponse>> Handle(SearchMediaQuery request, CancellationToken cancellationToken)
    {
        var media = await _unitOfWork.Media.GetAllAsync();
        var filtered = media.Where(m => 
            m.Title.Contains(request.Query, StringComparison.OrdinalIgnoreCase) || 
            (m.Genre?.Name?.Contains(request.Query, StringComparison.OrdinalIgnoreCase) ?? false)
        );

        return filtered.Select(m => new MediaResponse(
            m.Id,
            m.Title,
            m.Description,
            m.ImageUrl,
            m.VideoUrl,
            m.Duration,
            m.ReleaseDate,
            m.Rating,
            m.GenreId,
            m.Genre?.Name ?? "Unknown",
            m.GetType().Name
        ));
    }
}
