using MediatR;
using StarterLogin.Application.Multimedia.Common;
using StarterLogin.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StarterLogin.Application.Multimedia.Queries.GetMediaList;

public record GetMediaListQuery : IRequest<IEnumerable<MediaResponse>>;

public class GetMediaListQueryHandler : IRequestHandler<GetMediaListQuery, IEnumerable<MediaResponse>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetMediaListQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<MediaResponse>> Handle(GetMediaListQuery request, CancellationToken cancellationToken)
    {
        var media = await _unitOfWork.Media.GetAllAsync();
        return media.Select(m => new MediaResponse(
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
