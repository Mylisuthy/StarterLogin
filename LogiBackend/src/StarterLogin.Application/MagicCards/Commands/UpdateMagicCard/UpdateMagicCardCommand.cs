using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using StarterLogin.Application.Common.Interfaces;
using StarterLogin.Domain.Interfaces;

namespace StarterLogin.Application.MagicCards.Commands.UpdateMagicCard;

public record UpdateMagicCardCommand(
    Guid Id,
    string Title,
    string Description,
    Stream? ImageStream, // Optional, if null, don't update image
    string? FileName
) : IRequest;

public class UpdateMagicCardCommandHandler : IRequestHandler<UpdateMagicCardCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IImageService _imageService;

    public UpdateMagicCardCommandHandler(IUnitOfWork unitOfWork, IImageService imageService)
    {
        _unitOfWork = unitOfWork;
        _imageService = imageService;
    }

    public async Task Handle(UpdateMagicCardCommand request, CancellationToken cancellationToken)
    {
        var card = await _unitOfWork.MagicCards.GetByIdAsync(request.Id);
        
        if (card == null)
        {
            throw new KeyNotFoundException($"Magic Card with ID {request.Id} not found.");
        }

        string imageUrl = card.ImageUrl;
        
        if (request.ImageStream != null && request.FileName != null)
        {
            imageUrl = await _imageService.UploadImageAsync(request.ImageStream, request.FileName);
        }

        card.Update(
            request.Title,
            request.Description,
            imageUrl,
            card.IsPublished // Keep existing status
        );

        _unitOfWork.MagicCards.Update(card);
        await _unitOfWork.SaveChangesAsync();
    }
}
