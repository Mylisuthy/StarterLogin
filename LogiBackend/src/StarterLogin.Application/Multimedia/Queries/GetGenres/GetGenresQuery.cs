using MediatR;
using StarterLogin.Application.Multimedia.Common;
using StarterLogin.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StarterLogin.Application.Multimedia.Queries.GetGenres;

public record GetGenresQuery : IRequest<IEnumerable<GenreResponse>>;

public class GetGenresQueryHandler : IRequestHandler<GetGenresQuery, IEnumerable<GenreResponse>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetGenresQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<GenreResponse>> Handle(GetGenresQuery request, CancellationToken cancellationToken)
    {
        var genres = await _unitOfWork.Genres.GetAllAsync();
        return genres.Select(g => new GenreResponse(g.Id, g.Name, g.Description));
    }
}
