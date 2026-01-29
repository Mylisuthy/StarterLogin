using MediatR;
using StarterLogin.Application.Multimedia.Common;
using StarterLogin.Domain.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace StarterLogin.Application.Multimedia.Queries.GetMediaById;

public record GetMediaByIdQuery(Guid Id) : IRequest<MediaResponse?>;

public class GetMediaByIdQueryHandler : IRequestHandler<GetMediaByIdQuery, MediaResponse?>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetMediaByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<MediaResponse?> Handle(GetMediaByIdQuery request, CancellationToken cancellationToken)
    {
        var m = await _unitOfWork.Media.GetByIdAsync(request.Id);
        if (m == null) return null;

        return new MediaResponse(
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
        );
    }
}
