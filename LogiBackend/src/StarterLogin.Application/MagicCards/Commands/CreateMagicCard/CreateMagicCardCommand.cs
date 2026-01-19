using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using StarterLogin.Application.Common.Interfaces;
using StarterLogin.Domain.Entities;
using StarterLogin.Domain.Interfaces;

namespace StarterLogin.Application.MagicCards.Commands.CreateMagicCard;

public record CreateMagicCardCommand(
    string Title,
    string Description,
    Stream ImageStream,
    string FileName
) : IRequest<Guid>;

public class CreateMagicCardCommandHandler : IRequestHandler<CreateMagicCardCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IImageService _imageService;

    public CreateMagicCardCommandHandler(IUnitOfWork unitOfWork, IImageService imageService)
    {
        _unitOfWork = unitOfWork;
        _imageService = imageService;
    }

    public async Task<Guid> Handle(CreateMagicCardCommand request, CancellationToken cancellationToken)
    {
        var imageUrl = await _imageService.UploadImageAsync(request.ImageStream, request.FileName);

        var magicCard = new MagicCard(
            request.Title,
            request.Description,
            imageUrl,
            isPublished: false // Default to draft
        );

        await _unitOfWork.MagicCards.AddAsync(magicCard);
        await _unitOfWork.SaveChangesAsync();
        
        return magicCard.Id;
    }
}
