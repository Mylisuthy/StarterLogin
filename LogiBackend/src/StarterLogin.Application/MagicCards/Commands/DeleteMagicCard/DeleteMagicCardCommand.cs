using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using StarterLogin.Domain.Interfaces;

namespace StarterLogin.Application.MagicCards.Commands.DeleteMagicCard;

public record DeleteMagicCardCommand(Guid Id) : IRequest;

public class DeleteMagicCardCommandHandler : IRequestHandler<DeleteMagicCardCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteMagicCardCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteMagicCardCommand request, CancellationToken cancellationToken)
    {
        var card = await _unitOfWork.MagicCards.GetByIdAsync(request.Id);

        if (card == null)
        {
            throw new KeyNotFoundException($"Magic Card with ID {request.Id} not found.");
        }

        _unitOfWork.MagicCards.Remove(card);
        await _unitOfWork.SaveChangesAsync();
    }
}
