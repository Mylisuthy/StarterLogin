using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using StarterLogin.Domain.Interfaces;

namespace StarterLogin.Application.MagicCards.Commands.PublishMagicCard;

public record PublishMagicCardCommand(Guid Id, bool IsPublished) : IRequest;

public class PublishMagicCardCommandHandler : IRequestHandler<PublishMagicCardCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public PublishMagicCardCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(PublishMagicCardCommand request, CancellationToken cancellationToken)
    {
        var card = await _unitOfWork.MagicCards.GetByIdAsync(request.Id);

        if (card == null)
        {
            throw new KeyNotFoundException($"Magic Card with ID {request.Id} not found.");
        }

        card.SetPublished(request.IsPublished);
        
        _unitOfWork.MagicCards.Update(card);
        await _unitOfWork.SaveChangesAsync();
    }
}
